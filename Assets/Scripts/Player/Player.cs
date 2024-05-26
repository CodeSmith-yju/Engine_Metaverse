using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<string> cur_IngrList = new List<string>(); // 주방에서 컵에 재료 넣은 리스트
    public string cur_Ordered_Menu = "";
    public bool cup = false; // 컵 오브젝트를 상호작용 하면 true가 되도록 정해놓은 변수
    public bool done = false; // 제작 완료 
    public bool resume_Done = false;
    private RecipeManager recipe;

    private int order_Index;

    [SerializeField] private Role role;

    // 테스트를 위해 start 메서드를 이용해서 관리자 권한 주기
    private void Start()
    {
        gameObject.GetComponent<Player>();

        recipe = FindObjectOfType<RecipeManager>();

        cur_IngrList.Clear(); // 초기화

        SetRole(Role.Manager);

        if (role == Role.Manager || role == Role.Empolyee)
        {
            order_Index = 0;
        }

    }


    public void SetRole(Role newRole) // 서버에 접속할 때 이 메서드를 이용해서 플레이어들에게 권한 주기
    {
        Debug.Log("권한 설정 : " + name + "님의 권한이 설정되었습니다. ( " + GetRole() + " -> " + newRole + " )");
        role = newRole;
    }

    public Role GetRole()
    {
        return role;
    }


    public void Create()
    {
        if (cup)
        {
            bool isdone = recipe.Cook(KioskSystem.single.order_List[order_Index], cur_IngrList);
            cur_IngrList.Clear();

            if (isdone)
            {
                done = true;
                cur_Ordered_Menu = KioskSystem.single.order_List[order_Index];
            }
            else
            {
                done = false;
            }
        }
    }

    // 완성된 요리를 들고 오브젝트 상호작용 시 제출
    public void Done()
    {
        if (done) // 최근에 주문된 메뉴와 일치하면
        {
            done = false;
            cup = false;
            cur_Ordered_Menu = "";

            if (KioskSystem.single.order_List.Count > 0)
            {
                ++order_Index;
            }
        }
    }
}
