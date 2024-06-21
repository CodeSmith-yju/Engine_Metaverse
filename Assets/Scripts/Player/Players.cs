using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public List<string> cur_IngrList = new List<string>(); // �ֹ濡�� �ſ� ��� ���� ����Ʈ
    public string cur_Ordered_Menu = "";
    public bool cup = false; // �� ������Ʈ�� ��ȣ�ۿ� �ϸ� true�� �ǵ��� ���س��� ����
    public bool done = false; // ���� �Ϸ� 
    public bool resume_Done = false; // �̷¼� �����ϸ� üũ�ϴ� ����
    public bool coffee = false; // Ŀ�ǰ��縦 ������ �ִ��� ������
    private RecipeManager recipe;
    public bool ui_Opened = false; // ui�� ���� ������ �������� �ʵ��� ��
    public int coin; // ������ ����
    public int gender;   // 0 : ����, 1: ����
    public string nickName = ""; // ĳ���� ���� �� �г����� �޾ƿ�

    private int order_Index; // �ֹ���ȣ�� ���Ƶδ� ����
     
    [SerializeField] private Role role; // ���� ���� ������ ����
    public string nowMakeMenu;

    private void Awake()
    {
        coin = 3;
        
    }

    // �׽�Ʈ�� ���� start �޼��带 �̿��ؼ� ������ ���� �ֱ�
    private void Start()
    {
        GameMgr.Instance.ui.coin_UI.GetComponent<Coin_Init>().Init(coin);
        gameObject.GetComponent<Players>();
        recipe = FindObjectOfType<RecipeManager>();
        cur_IngrList.Clear(); // �ʱ�ȭ
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


    public void SetRole(Role newRole) // ������ ������ �� �� �޼��带 �̿��ؼ� �÷��̾�鿡�� ���� �ֱ�
    {
        Debug.Log("���� ���� : " + name + "���� ������ �����Ǿ����ϴ�. ( " + GetRole() + " -> " + newRole + " )");
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

    // �ϼ��� �丮�� ��� ������Ʈ ��ȣ�ۿ� �� ����
    public void Done()
    {
        if (done) // �ֱٿ� �ֹ��� �޴��� ��ġ�ϸ�
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
