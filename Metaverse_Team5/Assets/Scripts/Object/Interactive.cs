using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    bool isEnter = false;
    string ingredient = null;
    bool isHot = false;

    // 지금은 오브젝트 이름으로 관리 더 좋은 방법이 있으면 그걸로 바꿀 예정
    private void Start()
    {
        if (name == "Water" && tag == "Hot")
        {
            isHot = true;
        }

        switch(name)
        {
            case "Espresso":
                ingredient = "에스프레소";
                break;
            case "Strawberry":
                ingredient = "딸기";
                break;
            case "Chocolate":
                ingredient = "초콜릿";
                break;
            case "Milk":
                ingredient = "우유";
                break;
            case "Mixer":
                ingredient = "믹서기";
                break;
            case "Water":
                if (isHot)
                {
                    ingredient = "온수";
                }
                else
                {
                    ingredient = "냉수";
                }
                break;
            case "Ice":
                ingredient = "얼음";
                break;
        }
    }

    // 플레이어 태그를 가진 오브젝트와 부딪치면 오브젝트와 충돌 체크 테스트용 ( 나중에는 레이캐스트를 이용해서 사물을 바라보고 있을 때 쓸 할 예정 )
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isEnter = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (isEnter && collision.collider.CompareTag("Player") && player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
        {
            

            if (Input.GetKey(KeyCode.Space))
            {
                if (name == "Done" && player.cup && player.done)
                {
                    Debug.Log("제작 완료 손님 전달");
                    player.Done();
                }
                else if (name == "Create" && player.cup)
                {
                    Debug.Log("제작 시도");
                    player.Create();
                }
                else if (name == "Role")
                {
                    if (player.GetRole() == Role.Manager) // 권한이 매니저일 경우에만 사용가능하게
                    {
                        Debug.Log("권한 설정");
                        GameMgr.Instance.player_List[0].gameObject.GetComponent<Player>().SetRole(Role.Empolyee); 
                        // 이 코드를 플레이어 리스트를 가져와서 UI에서 선택하는 걸로 바꿀 예정
                    }
                }
                else if (name == "Cup")
                {
                    if (player.cup == false)
                    {
                        Debug.Log("손에 컵 듦");
                        player.cup = true;
                    }
                    else
                    {
                        Debug.Log("이미 컵을 들고 있음");
                    }
                }
                else
                {
                    if (player.done == false && player.cup == true)
                    {
                        player.cur_IngrList.Add(ingredient);
                        Debug.Log(ingredient + " 재료 추가");
                    }
                }

                isEnter = false;
            }

        }
    }
}
