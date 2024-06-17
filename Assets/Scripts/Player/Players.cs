using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public List<string> cur_IngrList = new List<string>(); // �ֹ濡�� �ſ� ��� ���� ����Ʈ
    public string cur_Ordered_Menu = "";
    public bool cup = false; // �� ������Ʈ�� ��ȣ�ۿ� �ϸ� true�� �ǵ��� ���س��� ����
    public bool done = false; // ���� �Ϸ� 
    public bool resume_Done = false;
    public bool coffee = false;
    private RecipeManager recipe;
    public bool ui_Opened = false;
    public int coin;

    private int order_Index;

    [SerializeField] private Role role;


    private void Awake()
    {
        coin = 3;
        GameMgr.Instance.ui.coin_UI.GetComponent<Coin_Init>().Init(coin);
    }

    // �׽�Ʈ�� ���� start �޼��带 �̿��ؼ� ������ ���� �ֱ�
    private void Start()
    {
        gameObject.GetComponent<Players>();
        recipe = FindObjectOfType<RecipeManager>();
        cur_IngrList.Clear(); // �ʱ�ȭ
    }

    private void Update()
    {
        if (GameMgr.Instance.ui.setting_UI.activeSelf || GameMgr.Instance.ui.job_Opening_UI.activeSelf || GameMgr.Instance.ui.pos_Menu_UI.activeSelf)
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

    /*public void Add(string ingredient)
    {
        cur_IngrList.Add(ingredient);

        switch (ingredient) 
        {
            case "����":
                Instantiate(GameMgr.Instance.ui.cup_Icon_List[0], GameMgr.Instance.ui.cup_List.transform);
                break;
            case "����������":
                break;
            case "�ü�":
                break;
            case "�¼�":
                break;
            case "����":
                break;
        }

    }*/

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

            if (KioskSystem.single.order_List.Count > 0)
            {
                ++order_Index;
            }
        }
    }
}