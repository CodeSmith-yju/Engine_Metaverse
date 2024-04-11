using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<string> cur_IngrList = new List<string>(); // �ֹ濡�� �ſ� ��� ���� ����Ʈ
    public bool cup = false; // �� ������Ʈ�� ��ȣ�ۿ� �ϸ� true�� �ǵ��� ���س��� ����
    public bool done = false; // ���� �Ϸ� 
    private RecipeManager recipe;


    [SerializeField] private Role role;

    // �׽�Ʈ�� ���� start �޼��带 �̿��ؼ� ������ ���� �ֱ�
    private void Start()
    {
        gameObject.GetComponent<Player>();

        recipe = FindObjectOfType<RecipeManager>();

        cur_IngrList.Clear(); // �ʱ�ȭ


        SetRole(Role.Manager);
    }


    public void SetRole(Role newRole) // ������ ������ �� �� �޼��带 �̿��ؼ� �÷��̾�鿡�� ���� �ֱ�
    {
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
            bool isdone = recipe.Cook(GameMgr.Instance.order_List[0], cur_IngrList);
            cur_IngrList.Clear();

            if (isdone)
            {
                done = true;
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
        if (done)
        {
            done = false;
            cup = false;
        }
    }

}
