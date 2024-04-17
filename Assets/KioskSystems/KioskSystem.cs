using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KioskSystem : MonoBehaviour
{
    public static KioskSystem single;
    [Header("Kiosk Object")]
    public List<GameObject> kioskScene;//키오스크에 표시될 화면, 리스트로 관리하면 편할듯?

    public Dictionary<int, string> menus = new();// 주문받은 메뉴 정보를 보관하게될 사전, player스크립트에서 다른 오브젝트와의 상호작용을통해 내부의 값을 변경 할 수있어야 하므로 public
    [SerializeField] private List<int> ticketNumbers = new();//주문받은 번호들을 보관, 
    [SerializeField] private int ticketNum = 0;//주문받은 메뉴 인덱스, 사전의 Key값, 얘는 쉽게 바뀌어서는 안 됨, private
    public string menuName = "";// 사전의 Value값, 버튼상호작용을 통하여 해당 버튼오브젝트의 이름을 대입할것이므로 public

    // 결제 화면
    public Image kioskBuyPanel;

    // 티켓 출력 화면
    [SerializeField] private Image tiketIssuance;// 결제 화면의 OK버튼 클릭 시, 출력될 주문번호 출력 화면
    [SerializeField] private TextMeshProUGUI textNumberKiosk;// 키오스크에 표시될 디스플레이 텍스트

    [Header("SellerDisplay Object")]
    public TextMeshProUGUI textNumberellerDisPlay;// 판매자 전광판에 표시될 디스플레이 텍스트

    [Header("ConsumerDisplay Object")]
    public TextMeshProUGUI textNumberConsumerDisPlay;// 소비자 전광판에 표시될 디스플레이 텍스트

    public int kioIndex = 0;
    public bool buyCheck = false;

    public Button btnBuyOK;

    //주문한 물품 제작되었을때 주문한 손님 호출
    [Header("Order")]
    public Image imgOrder;
    public TextMeshProUGUI textOrder;
    private int removeCount = 0;

    public Button btnQuiteKiosk;
    
    private void Awake()
    {
        single = this;
        KioskStart();

    }
    public void KioskSceneChange()
    {
        switch (kioIndex)
        {
            case 0:
                Debug.Log("case 0");
                Debug.Log(kioIndex);
                kioskScene[0].SetActive(false);
                kioskScene[1].SetActive(true);
                kioIndex++;
                Debug.Log(kioIndex);
                break;
            case 1:
                if (buyCheck == true)
                {
                    Debug.Log("case 1");
                    Debug.Log(kioIndex);
                    kioskScene[0].SetActive(false);
                    kioskScene[1].SetActive(false);
                    //IssuanceOK()
                }
                break;
            default:
                break;
        }

    }
    public void TakeTicket()
    {
        if (ticketNum >= 999) // 번호표 초기화
        {
            ticketNumbers.Clear();
            ticketNum = 0;
        }
        ticketNumbers.Add(++ticketNum);
        menus.Add(ticketNum, menuName);

        //키오스크에 표시될 숫자가 Update됨.
        KioskUpdate();

        //Update된 숫자가 전광판에 표시됨
        SellerDisplayUpdate();
        ConsumerDisplayUpdate();
    }

    private void KioskUpdate()
    {
        string kioskText = "";
        if (menus.ContainsKey(ticketNum))
        {
            if (ticketNum < 10)
            {
                kioskText += "주문하신 메뉴: " + menus[ticketNum] + "\n 주문 번호: \n 00" + ticketNum.ToString();
            }
            else if (ticketNum < 100)
            {
                kioskText += "주문하신 메뉴: " + menus[ticketNum] + "\n 주문 번호: \n 0" + ticketNum.ToString();
            }
            else
            {
                kioskText += "주문하신 메뉴: " + menus[ticketNum] + "\n 주문 번호: \n" + ticketNum.ToString();
            }
        }
        else
        {
            kioskText += "주문하신 메뉴: " + menuName + "\n 주문 번호: \n" + ticketNum.ToString();
        }

        textNumberKiosk.text = kioskText;
    }

    private void SellerDisplayUpdate()
    {
        string displayText = "";

        List<int> sortedTicketNumbers = ticketNumbers.OrderBy(num => num).ToList();

        for (int i = 0; i < sortedTicketNumbers.Count; i++)
        {
            displayText += menus[sortedTicketNumbers[i]] + sortedTicketNumbers[i];
            if (sortedTicketNumbers.Count > 1 && i < sortedTicketNumbers.Count - 1)
            {
                displayText += ", ";
            }
        }

        textNumberellerDisPlay.text = displayText;
    }

    private void ConsumerDisplayUpdate()
    {
        string displayText = string.Join(", ", ticketNumbers);
        textNumberConsumerDisPlay.text = displayText;
    }

    public void RemoveNum()
    {
        if (removeCount == 0)
        {
            imgOrder.gameObject.SetActive(true);

            if (menus.Count > 0)
            {
                if (ticketNumbers.Count > 0)
                {
                    textOrder.text = ticketNumbers[0].ToString();
                }
            }
            removeCount = 1;
        }
        else
        {
            imgOrder.gameObject.SetActive(false);
            if (menus.Count > 0)
            {
                if (ticketNumbers.Count > 0)
                {
                    int ticketToRemove = ticketNumbers[0]; // 첫 번째 요소 가져오기
                    ticketNumbers.RemoveAt(0); // 리스트에서 첫 번째 요소 제거
                    menus.Remove(ticketToRemove); // 사전에서도 해당 티켓 제거

                    //키오스크에 표시될 숫자가 Update됨.
                    KioskUpdate();

                    //Update된 숫자가 전광판에 표시됨
                    SellerDisplayUpdate();
                    ConsumerDisplayUpdate();
                    removeCount = 0;
                }
            }
        }

    }

    private void KioskStart()
    {
        //씬이 최초로 시작되었을때 키오스크는 기본화면으로 초기화되어야 한다. 이때 다른 키오스크의 옵션들도 같이 초기화해 준다면 좋을 듯.
        for (int i = 0; i < kioskScene.Count; i++)
        {
            kioskScene[i].SetActive(false);
            btnQuiteKiosk.gameObject.SetActive(false);
        }


    }

    public void OnBuyMenue()
    {
        buyCheck = true;
        if (buyCheck == true)
        {
            Debug.Log("BuyChek OK");
            TakeTicket();
            tiketIssuance.gameObject.SetActive(buyCheck);
            KioskSceneChange();
            kioIndex++;
        }
    }
    public void IssuanceOK()
    {
        buyCheck = false;
        tiketIssuance.gameObject.SetActive(buyCheck);
        kioIndex = 0;
        btnQuiteKiosk.gameObject.SetActive(false);
    }
    public void OnQuiteKiosk()
    {
        for (int i = 0; i < kioskScene.Count; i++)
        {
            kioskScene[i].SetActive(false);
        }

        kioskBuyPanel.gameObject.SetActive(false);
        tiketIssuance.gameObject.SetActive(false);
        
        btnQuiteKiosk.gameObject.SetActive(false);
        kioIndex = 0;
    }

}
