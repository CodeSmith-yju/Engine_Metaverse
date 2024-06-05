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

    private int order_Index;

    [SerializeField] private Role role;

    // �׽�Ʈ�� ���� start �޼��带 �̿��ؼ� ������ ���� �ֱ�
    private void Start()
    {
        gameObject.GetComponent<Players>();
        recipe = FindObjectOfType<RecipeManager>();
        cur_IngrList.Clear(); // �ʱ�ȭ
    }

    private void Update()
    {
        // UI ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �ٸ� UI â�� �ȿ��� ���� �� ����â ������ ��.
            if (!GameMgr.Instance.ui.setting_Ui.activeSelf && !GameMgr.Instance.ui.job_Opening_Ui.activeSelf)
            {
                GameMgr.Instance.ui.setting_Ui.SetActive(true);
            } 
            else if (GameMgr.Instance.ui.setting_Ui.activeSelf)
            {
                // ESC�� ������ ���� â�� �ڽ����� �ִ� ������ �˾��̳� Ű���� �ȳ� �˾��� ���������� �̰� ���� ���� �ݵ��� ��.
                if (GameMgr.Instance.ui.content_Info_Ui.activeSelf)
                {
                    GameMgr.Instance.ui.content_Info_Ui.SetActive(false);
                }
                else if (GameMgr.Instance.ui.keyset_Info_Ui.activeSelf)
                {
                    GameMgr.Instance.ui.keyset_Info_Ui.SetActive(false);
                }
                else
                {
                    GameMgr.Instance.ui.setting_Ui.SetActive(false);
                }
            }

            // �̷¼� ���� UI�� ���� ���� �� ESCŰ�� ������ �ݵ��� ��.
            if (GameMgr.Instance.ui.job_Opening_Ui.activeSelf)
            {
                if (GameMgr.Instance.ui.resume_Ui.activeSelf) 
                {
                    GameMgr.Instance.ui.resume_Ui.SetActive(false);
                }
                else
                {
                    GameMgr.Instance.ui.job_Opening_Ui.SetActive(false);
                }
            }
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

    // �ϼ��� �丮�� ��� ������Ʈ ��ȣ�ۿ� �� ����
    public void Done()
    {
        if (done) // �ֱٿ� �ֹ��� �޴��� ��ġ�ϸ�
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
