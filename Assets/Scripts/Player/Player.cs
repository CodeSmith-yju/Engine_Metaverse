using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<string> cur_IngrList = new List<string>(); // �ֹ濡�� �ſ� ��� ���� ����Ʈ
    public string cur_Ordered_Menu = "";
    public bool cup = false; // �� ������Ʈ�� ��ȣ�ۿ� �ϸ� true�� �ǵ��� ���س��� ����
    public bool done = false; // ���� �Ϸ� 
    public bool resume_Done = false;
    private RecipeManager recipe;

    private int order_Index;

    [SerializeField] private Role role;

    // �׽�Ʈ�� ���� start �޼��带 �̿��ؼ� ������ ���� �ֱ�
    private void Start()
    {
        gameObject.GetComponent<Player>();

        recipe = FindObjectOfType<RecipeManager>();

        cur_IngrList.Clear(); // �ʱ�ȭ

        SetRole(Role.Manager);

        if (role == Role.Manager || role == Role.Empolyee)
        {
            order_Index = 0;
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
