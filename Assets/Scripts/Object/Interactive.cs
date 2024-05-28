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
    bool coffee_Check = false;
    bool coffee_Done = false;
    Player player;
    List<string> temp_List = new List<string>();

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
            ActiveObjectName(parent_Tag);
            if (Input.GetKeyDown(KeyCode.F))
            {
                InteractWithPlayer(player, parent_Tag);
            }
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        my.GetComponent<Interactive>().enabled = true;
        KioskSystem.single.announce.SetActive(true);
        if (other.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Player>();
            isEnter = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCheck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractExitPlayer(parent_Tag); // player �Ű������� �ʿ��ϸ� �߰� ����
        player = null;
        my.GetComponent<Interactive>().enabled = false;
    }

    private void InteractWithPlayer(Player player, string obj_Tag)
    {
        switch (obj_Tag)
        {
            case "Kiosk":
                Debug.Log("키오스크 실행");
                KioskSystem.single.KioskUsing();
                break;
            case "Cup":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.cup)
                    {
                        Debug.Log("이미 컵을 들고 있습니다.");
                        return;
                    }
                    player.cup = true;
                    Debug.Log("컵을 듦");
                }
                else
                {
                    Debug.Log("권한이 없습니다.");
                    return;
                }
                break;
            case "POS":
                KioskSystem.single.sellerImg.gameObject.SetActive(true);
                Debug.Log("접촉한 오브젝트 : " + parent_Tag);
                break;
            case "Grinder":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.coffee)
                    {
                        Debug.Log("이미 커피 가루를 들고 있습니다.");
                        return;
                    }
                    player.coffee = true;
                    Debug.Log("커피가루를 듦");
                }
                else
                {
                    Debug.Log("권한이 없습니다.");
                    return;
                }
                break;
            case "Espresso":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (coffee_Check && player.cup)
                    {
                        Debug.Log("커피를 내립니다. (30초)");
                        coffee_Check = false;

                        foreach (string ingr in player.cur_IngrList)
                        {
                            temp_List.Add(ingr);
                        }

                        player.cur_IngrList.Clear();
                        player.cup = false;

                        StartCoroutine(CoffeeRoutine());
                        return;
                    }

                    if (coffee_Done && !player.cup) 
                    {
                        player.cup = true;
                        Debug.Log("에스프레소 내린 커피 컵 들기");
                        foreach (string ingr in temp_List)
                        {
                            player.cur_IngrList.Add(ingr);
                        }
                        player.cur_IngrList.Add("에스프레소");
                        return;
                    }

                    if (player.coffee && !coffee_Check)
                    {
                        Debug.Log("커피 가루를 넣었습니다.");
                        coffee_Check = true;
                        player.coffee = false;
                    }
                    else if (player.coffee && coffee_Check)
                    {
                        Debug.Log("이미 해당 커피머신에 커피 가루가 들어 있습니다.");
                        return;
                    }
                    else
                    {
                        Debug.Log("커피를 내리고 있거나 커피 가루를 가지고 있지 않습니다.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("권한이 없습니다.");
                    return;
                }
                break;
            case "Ice":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.cup)
                    {
                        player.cur_IngrList.Add("얼음");
                        Debug.Log("컵에 얼음 넣기");
                    }
                    else
                    {
                        Debug.Log("컵을 들고 있지 않습니다.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("권한이 없습니다.");
                    return;
                }
                break;
            case "Done":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.done)
                    {
                        Debug.Log("음료 제작 완료 : " + player.cur_Ordered_Menu);
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
                    Debug.Log("권한이 없습니다.");
                    return;
                }
                break;
            case "Dish":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.cup)
                    {
                        Debug.Log("컵 씻기");
                        player.cur_IngrList.Clear();
                    }
                    else
                    {
                        Debug.Log("컵을 들고 있어야 합니다.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("권한이 없습니다.");
                    return;
                }
                break;
            default:
                return;
        }
    }

    private void InteractExitPlayer(string obj_Tag)
    {
        isEnter = false;
        isCheck = false;

        switch (obj_Tag) 
        {
            case "Kiosk":
                KioskSystem.single.OnQuiteKiosk();// 키오스크 off
                break;
            case "POS":
                KioskSystem.single.sellerImg.gameObject.SetActive(false);
                break;
            default:
                Debug.Log("상호작용 범위에서 나감");
                break;
        }

        KioskSystem.single.announce.SetActive(false);
        KioskSystem.single.textannounce.gameObject.SetActive(false);
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
            default:
                KioskSystem.single.textannounce.text = _str;
                break;
        }
        KioskSystem.single.textannounce.gameObject.SetActive(true);
    }
    
    private IEnumerator CoffeeRoutine()
    {
        yield return StartCoroutine(Espresso());

        Debug.Log("커피가 다 내려졌습니다");
        coffee_Done = true;
    }


    private IEnumerator Espresso()
    {
        CoffeMachine coffemachine = my.GetComponent<CoffeMachine>();
        coffemachine.StartTimer(30f);
        yield return new WaitForSeconds(30);
    }
}
