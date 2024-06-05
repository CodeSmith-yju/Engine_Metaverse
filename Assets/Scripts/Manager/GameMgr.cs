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
    //public string order; // 임시로 주문한 메뉴를 적어둘 변수 나중에 키오스크에서 주문한 주문번호와 메뉴 이름은 딕셔너리에서 관리할 예정
    public UIManager ui;
    //public Dictionary<int, string> order_List = new Dictionary<int, string>(); // 주문번호와 주문 메뉴를 저장하는 딕셔너리
    public List<GameObject> player_List = new List<GameObject>(); // 접속한 플레이어 리스트
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
