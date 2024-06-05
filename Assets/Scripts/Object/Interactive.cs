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
    Player player;
    bool isCoffee = false;

    private void Awake()
    {
        my = this.gameObject;
    }

    // 지금은 오브젝트 이름으로 관리 더 좋은 방법이 있으면 그걸로 바꿀 예정
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
        InteractExitPlayer(parent_Tag); // player 매개변수가 필요하면 추가 예정
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
                KioskSystem.single.kiosck = true;
                Debug.Log("충돌한 오브젝트: " + parent_Tag);
                break;
            case "Espresso":
                Debug.Log("커피머쉰");//상호작용했으니까
                CoffeMachine coffemachine = my.GetComponent<CoffeMachine>();
                coffemachine.StartTimer(30f);

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
                KioskSystem.single.OnQuiteKiosk();// 키오스크 상호작용화면 off
                break;
            case "POS":
                KioskSystem.single.sellerImg.gameObject.SetActive(false);
                KioskSystem.single.kiosck = false;
                break;
            default:
                Debug.Log("상호작용 트리거에서 벗어남");
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
    
}
