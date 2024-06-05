using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public UIManager ui;
    //public Dictionary<int, string> order_List = new Dictionary<int, string>(); // �ֹ���ȣ�� �ֹ� �޴��� �����ϴ� ��ųʸ�
    public List<GameObject> player_List = new List<GameObject>(); // ������ �÷��̾� ����Ʈ
    public RecipeManager recipe;

    [Header("Receipt")]
    public GameObject player_Receipt;
    public TextMeshPro text_Receipt;

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

        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject players in player) 
        {
            if (player != null) 
            {
                player_List.Add(players);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player_Receipt.activeSelf)
            {
                player_Receipt.SetActive(false);
            }
            else
            {
                player_Receipt.SetActive(true);
            }
        }
    }
}
