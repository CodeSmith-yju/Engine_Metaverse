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
                Debug.Log("충돌한 오브젝트: " + parent_Tag);
                break;
            case "Grinder":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.coffee)
                    {
                        Debug.Log("이미 커피 가루를 가지고 있습니다.");
                        return;
                    }
                    player.coffee = true;
                    Debug.Log("커피 가루 얻기");
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
                        Debug.Log("에스프레소가 담긴 컵 가져가기");
                        foreach (string ingr in temp_List)
                        {
                            player.cur_IngrList.Add(ingr);
                        }
                        player.cur_IngrList.Add("에스프레소");
                        return;
                    }

                    if (player.coffee && !coffee_Check)
                    {
                        Debug.Log("커피 가루를 커피 머신에 넣었습니다.");
                        coffee_Check = true;
                        player.coffee = false;
                    }
                    else if (player.coffee && coffee_Check)
                    {
                        Debug.Log("커피 머신에 이미 커피 가루가 들어 가 있습니다.");
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
                        Debug.Log("음료 제작 완료" + player.cur_Ordered_Menu);
                        player.Done();
                    }
                    else
                    {
                        Debug.Log("음료 제작이 완료 되지 않았습니다.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("권한이 없습니다");
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
                KioskSystem.single.OnQuiteKiosk();// 키오스크 상호작용화면 off
                break;
            default:
                Debug.Log("상호작용 트리거에서 벗어남");
                break;
        }

        KioskSystem.single.announce.SetActive(false);
    }

    private IEnumerator CoffeeRoutine()
    {
        yield return StartCoroutine(Espresso());

        Debug.Log("커피 내리기 완료");
        coffee_Done = true;
    }


    private IEnumerator Espresso()
    {
        yield return new WaitForSeconds(30);
    }
}
