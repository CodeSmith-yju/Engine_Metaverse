using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Role
{
    Manager,
    Empolyee,
    Customer
}


public class GameMgr : MonoBehaviour
{
    private static GameMgr instance = null;
    //public string order; // �ӽ÷� �ֹ��� �޴��� ����� ���� ���߿� Ű����ũ���� �ֹ��� �ֹ���ȣ�� �޴� �̸��� ��ųʸ����� ������ ����

    //public Dictionary<int, string> order_List = new Dictionary<int, string>(); // �ֹ���ȣ�� �ֹ� �޴��� �����ϴ� ��ųʸ�
    public List<GameObject> player_List = new List<GameObject>(); // ������ �÷��̾� ����Ʈ
    public RecipeManager recipe;

    public static GameMgr Instance 
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject players in player) 
        {
            if (player != null) 
            {
                player_List.Add(players);
            }
        }
    }
}
