using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum Role
{
    Manager,
    Employee,
    Customer
}


public class GameMgr : MonoBehaviour
{
    private static GameMgr instance = null;
    //public string order; // �ӽ÷� �ֹ��� �޴��� ����� ���� ���߿� Ű����ũ���� �ֹ��� �ֹ���ȣ�� �޴� �̸��� ��ųʸ����� ������ ����
    public UIManager ui;
    public List<GameObject> player_List = new List<GameObject>();
    //public Dictionary<int, string> order_List = new Dictionary<int, string>(); // �ֹ���ȣ�� �ֹ� �޴��� �����ϴ� ��ųʸ�
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
        //player_Receipt.SetActive(false);
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
