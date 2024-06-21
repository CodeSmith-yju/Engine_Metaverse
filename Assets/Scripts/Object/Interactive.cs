using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    GameObject my;
    string parent_Tag;
    bool isEnter = false;
    bool isCheck = false;
    Players player;

    private void Awake()
    {
        my = this.gameObject;
    }
    private void Start()
    {
        parent_Tag = my.transform.parent.tag;
        my.GetComponent<Interactive>().enabled = false;
    }

    private void Update()
    {
        if (isCheck && isEnter) 
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (player.GetComponent<PhotonView>().IsMine) 
                {
                    InteractWithPlayer(player, parent_Tag);
                    KioskSystem.single.SetNowPlayer(this.player);
                }
                
            }
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            my.GetComponent<Interactive>().enabled = true;
            player = other.gameObject.GetComponent<Players>();
            ActiveObjectName(parent_Tag);
            isEnter = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            isCheck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            KioskSystem.single.announce.SetActive(false);
            KioskSystem.single.textannounce.gameObject.SetActive(false);
            player = null;
            my.GetComponent<Interactive>().enabled = false;
        }
    }

    private void InteractWithPlayer(Players player, string obj_Tag)
    {
        switch (obj_Tag)
        {
            case "Kiosk":
                Debug.Log("키오스크 실행");
                KioskSystem.single.KioskUsing();
                break;
            case "Cup":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
                {
                    if (player.cup)
                    {
                        GameMgr.Instance.ui.OnAlertPopup("이미 컵을 가지고 있습니다.");
                        return;
                    }
                    GameMgr.Instance.ui.CheckPopup(obj_Tag, player); // 팝업창
                    Debug.Log("컵을 듦");
                }
                else
                {
                    GameMgr.Instance.ui.OnAlertPopup("권한이 없습니다.");
                    return;
                }
                break;
            case "POS":
                //GameMgr.Instance.ui.pos_Menu_UI.SetActive(true);
                KioskSystem.single.sellerImg.gameObject.SetActive(true);
                break;
            case "Grinder":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
                {
                    if (player.coffee)
                    {
                        GameMgr.Instance.ui.OnAlertPopup("이미 커피 가루를 들고 있습니다.");
                        return;
                    }
                    GameMgr.Instance.ui.CheckPopup(obj_Tag, player); // 팝업창
                }
                else
                {
                    GameMgr.Instance.ui.OnAlertPopup("권한이 없습니다.");
                    return;
                }
                break;
            case "Espresso":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
                {
                    CoffeeMachine coffeemachine = my.GetComponent<CoffeeMachine>();
                    if (coffeemachine.coffee_Check && player.cup)
                    {
                        GameMgr.Instance.ui.CheckPopup(obj_Tag, player); // 팝업창
                        return;
                    }
                    else if (coffeemachine.coffee_Check && !player.cup)
                    {
                        GameMgr.Instance.ui.OnAlertPopup("컵을 들고 있지 않습니다.");
                        return;
                    }

                    if (coffeemachine.coffee_Done && !player.cup) 
                    {
                        player.cup = true;
                        coffeemachine.bg.SetActive(false);
                        Debug.Log("에스프레소 내린 커피 컵 들기");

                        player.cur_IngrList.Add("에스프레소");

                        GameMgr.Instance.ui.CupIcon(obj_Tag);
                        GameMgr.Instance.ui.OnAlertPopup("에스프레소 내린 컵을 들었습니다.");

                        return;
                    }
                    else if (!coffeemachine.coffee_Done && coffeemachine.coffee_Ing)
                    {
                        GameMgr.Instance.ui.OnAlertPopup("커피를 다 내리지 않았습니다.");
                        return;
                    }

                    if (player.coffee && !coffeemachine.coffee_Check)
                    {
                        GameMgr.Instance.ui.OnAlertPopup("커피 가루를 넣었습니다.");
                        coffeemachine.coffee_Check = true;
                        player.coffee = false;
                        coffeemachine.coffee_Icon.gameObject.SetActive(true);
                    }
                    else if (player.coffee && coffeemachine.coffee_Check)
                    {
                        GameMgr.Instance.ui.OnAlertPopup("이미 커피가루가 들어 있습니다.");
                        return;
                    }
                    else
                    {
                        GameMgr.Instance.ui.OnAlertPopup("커피가루를 먼저 넣어야 합니다.");
                        return;
                    }
                }
                else
                {
                    GameMgr.Instance.ui.OnAlertPopup("권한이 없습니다.");
                    return;
                }
                break;
            case "Ice":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
                {
                    if (player.cup)
                    {
                        GameMgr.Instance.ui.CheckPopup(obj_Tag, player);
                    }
                    else
                    {
                        GameMgr.Instance.ui.OnAlertPopup("컵을 들고 있지 않습니다.");
                        return;
                    }
                }
                else
                {
                    GameMgr.Instance.ui.OnAlertPopup("권한이 없습니다.");
                    return;
                }
                break;
            case "Done":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
                {
                    player.Create(); // 메뉴 제작 체크

                    if (player.done)
                    {
                        Debug.Log("음료 제작 완료 : " + player.cur_Ordered_Menu);
                        GameMgr.Instance.ui.DeleteCupIcon();
                        player.Done();
                        
                    }
                    else
                    {
                        Debug.Log("완성된 음료를 만들지 않았습니다.");
                        return;
                    }
                }
                else
                {
                    GameMgr.Instance.ui.OnAlertPopup("권한이 없습니다.");
                    return;
                }
                break;
            case "Dish":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
                {
                    if (player.cup)
                    {
                        GameMgr.Instance.ui.CheckPopup(obj_Tag, player);
                    }
                    else
                    {
                        GameMgr.Instance.ui.OnAlertPopup("컵을 들고 있지 않습니다.");
                        return;
                    }
                }
                else
                {
                    GameMgr.Instance.ui.OnAlertPopup("권한이 없습니다.");
                    return;
                }
                break;
            case "Water":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
                {
                    WaterDispenser water = my.GetComponent<WaterDispenser>();
                    water.Init(player);
                    if (player.cup)
                    {
                        //GameMgr.Instance.ui.water_dispenser_UI.SetActive(true);
                        Debug.Log("정수기 UI 열기");
                    }
                    else
                    {
                        GameMgr.Instance.ui.OnAlertPopup("컵을 들고 있지 않습니다.");
                        return;
                    }
                }
                else
                {
                    GameMgr.Instance.ui.OnAlertPopup("권한이 없습니다.");
                    return;
                }
                break;
            case "Mixer":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
                {
                    Debug.Log("믹서기 진입 테스트");
                    if (player.cur_IngrList.Count != 0 && player.cup)
                    {
                        GameMgr.Instance.ui.CheckPopup(obj_Tag, player);
                    }
                    else if (player.cur_IngrList.Count == 0 && player.cup)
                    {
                        GameMgr.Instance.ui.OnAlertPopup("믹서기에 갈 재료가 컵에 \n들어 있지 않습니다.");
                    }
                    else
                    {
                        GameMgr.Instance.ui.OnAlertPopup("컵을 들고 있지 않습니다.");
                    }
                }
                else
                {
                    GameMgr.Instance.ui.OnAlertPopup("권한이 없습니다.");
                    return;
                }
                break;
            default:
                return;
        }
    }
    private void ActiveObjectName(string _str)
    {
        switch (_str)
        {
            case "Kiosk":
                KioskSystem.single.textannounce.text = "키오스크";
                break;
            case "POS":
                KioskSystem.single.textannounce.text = "포스기";
                break;
            case "Ice":
                KioskSystem.single.textannounce.text = "얼음";
                break;
            case "Grinder":
                KioskSystem.single.textannounce.text = "커피가루";
                break;
            case "Espresso":
                CoffeeMachine coffeemachine = my.GetComponent<CoffeeMachine>();

                if(!coffeemachine.coffee_Check)
                {
                    KioskSystem.single.textannounce.text = "커피가루 넣기";
                }
                else
                {
                    KioskSystem.single.textannounce.text = "커피 내리기";
                }

                break;
            case "Mixer":
                KioskSystem.single.textannounce.text = "믹서기";
                break;
            case "Done":
                KioskSystem.single.textannounce.text = "음료 제작 및 완료";
                break;
            case "Water":
                KioskSystem.single.textannounce.text = "정수기";
                break;
            case "Dish":
                KioskSystem.single.textannounce.text = "씻기";
                break;
            case "Refrigerator":
                KioskSystem.single.textannounce.text = "냉장고";
                break;
            case "Cup":
                KioskSystem.single.textannounce.text = "컵";
                break;
            case "PickUp":
                KioskSystem.single.textannounce.text = "픽업";
                break;
            default:
                KioskSystem.single.textannounce.text = _str;
                break;
        }
        KioskSystem.single.announce.SetActive(true);
        KioskSystem.single.textannounce.gameObject.SetActive(true);
    }



}
