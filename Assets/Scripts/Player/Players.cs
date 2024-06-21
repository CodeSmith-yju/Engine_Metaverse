using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public List<string> cur_IngrList = new List<string>(); // 주방에서 컵에 재료 넣은 리스트
    public string cur_Ordered_Menu = "";
    public bool cup = false; // 컵 오브젝트를 상호작용 하면 true가 되도록 정해놓은 변수
    public bool done = false; // 제작 완료 
    public bool resume_Done = false; // 이력서 제출하면 체크하는 변수
    public bool coffee = false; // 커피가루를 가지고 있는지 없는지
    private RecipeManager recipe;
    public bool ui_Opened = false; // ui를 열고 있으면 움직이지 않도록 함
    public int coin; // 구매할 코인
    public int gender;   // 0 : 남자, 1: 여자
    public string nickName = ""; // 캐릭터 생성 시 닉네임을 받아옴

    private int order_Index; // 주문번호를 남아두는 변수
     
    [SerializeField] private Role role; // 권한 관련 나열형 변수
    public string nowMakeMenu;

    private void Awake()
    {
        coin = 3;
        
    }

    // 테스트를 위해 start 메서드를 이용해서 관리자 권한 주기
    private void Start()
    {
        GameMgr.Instance.ui.coin_UI.GetComponent<Coin_Init>().Init(coin);
        gameObject.GetComponent<Players>();
        recipe = FindObjectOfType<RecipeManager>();
        cur_IngrList.Clear(); // 초기화
    }

    private void Update()
    {
        if (GameMgr.Instance.ui.setting_UI.activeSelf 
            || GameMgr.Instance.ui.job_Opening_UI.activeSelf 
            || GameMgr.Instance.ui.pos_Menu_UI_Bg.activeSelf 
            || GameMgr.Instance.ui.alert_Popup.activeSelf 
            || GameMgr.Instance.ui.check_Popup.activeSelf
            || GameMgr.Instance.ui.master_Popup.activeSelf
            || GameMgr.Instance.ui.fire_Popup.activeSelf
            || GameMgr.Instance.ui.nonAccept_Popup.activeSelf
            || GameMgr.Instance.ui.accept_Popup.activeSelf
            || GameMgr.Instance.ui.water_dispenser_UI.activeSelf
            || GameMgr.Instance.ui.refrigerator_UI.activeSelf)
        {
            ui_Opened = true;
        }
        else
        {
            ui_Opened = false;
        }

        if (cup && !GameMgr.Instance.ui.cup_List_BG.activeSelf)
        {
            GameMgr.Instance.ui.cup_List_BG.SetActive(cup);
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
            
            if (isdone)
            {
                done = true;
                cur_Ordered_Menu = KioskSystem.single.order_List[order_Index];
                cur_IngrList.Clear();
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
            
            if (GameMgr.Instance.ui.cup_List_BG.activeSelf)
            {
                GameMgr.Instance.ui.cup_List_BG.SetActive(cup);
            }
        }
    }
}
