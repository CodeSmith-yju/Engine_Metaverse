using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KioskSystem : MonoBehaviour
{
    public static KioskSystem single;
    [Header("Kiosk Object")]
    public List<GameObject> kioskScene;

    public Dictionary<int, string> menus = new Dictionary<int, string>();
    [SerializeField] private List<int> ticketNumbers = new List<int>();
    [SerializeField] private int ticketNum = 0;
    public string menuName = "";

    public Image kioskBuyPanel;

    [SerializeField] private Image tiketIssuance;
    [SerializeField] private TextMeshProUGUI textNumberKiosk;

    [Header("SellerDisplay Object")]
    public TextMeshProUGUI textNumberellerDisPlay;

    [SerializeField] private List<Slot> poolSlot = new List<Slot>(); // 수정된 부분
    [SerializeField] private List<SelectedMenu> listSelectedMenus = new List<SelectedMenu>(); // 수정된 부분

    public GameObject slotPrefab; // 새로운 슬롯을 생성할 때 사용할 프리팹

    [Header("ConsumerDisplay Object")]
    public TextMeshProUGUI textNumberConsumerDisPlay;

    public int kioIndex = 0;
    public bool buyCheck = false;

    public Button btnBuyOK;

    [Header("Order")]
    public Image imgOrder;
    public TextMeshProUGUI textOrder;

    public Button btnQuiteKiosk;
    [SerializeField] ObjectInteractive kiosk;
    public GameObject announce;

    private void Awake()
    {
        single = this;
        KioskStart();
    }
    private void Start()
    {
        announce.SetActive(false);
    }

    public void KioskSceneChange()
    {
        switch (kioIndex)
        {
            case 0:
                Debug.Log("case 0");
                Debug.Log(kioIndex);
                kioskScene[0].SetActive(true);
                kioIndex++;
                Debug.Log(kioIndex);
                break;
            case 1:
                if (buyCheck == true)
                {
                    Debug.Log("case 1");
                    Debug.Log(kioIndex);
                    kioskScene[0].SetActive(false);
                }
                break;
            default:
                break;
        }

    }

    public void TakeTicket()
    {
        if (ticketNum >= 999)
        {
            ticketNumbers.Clear();
            ticketNum = 0;
        }

        ticketNumbers.Add(++ticketNum);
        menus.Add(ticketNum, menuName);
        SelectedMenu newMenu = new SelectedMenu(menuName, ticketNum);
        listSelectedMenus.Add(newMenu);
        newMenu.intSlotIndex = ticketNum;
        CreateOrReuseSlot(newMenu);

        // 슬롯을 추가한 후에 정렬합니다.
        SortSlots();

        KioskUpdate();
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
                kioskText += "주문하신 메뉴: \n" + menus[ticketNum] + "\n 주문 번호: \n 00" + ticketNum.ToString();
            }
            else if (ticketNum < 100)
            {
                kioskText += "주문하신 메뉴: \n" + menus[ticketNum] + "\n 주문 번호: \n 0" + ticketNum.ToString();
            }
            else
            {
                kioskText += "주문하신 메뉴: \n" + menus[ticketNum] + "\n 주문 번호: \n" + ticketNum.ToString();
            }
        }
        else
        {
            kioskText += "주문하신 메뉴: \n" + menuName + "\n 주문 번호: \n" + ticketNum.ToString();
        }

        textNumberKiosk.text = kioskText;
    }

    private void SellerDisplayUpdate()
    {
        string displayText = "";

        List<int> sortedTicketNumbers = ticketNumbers.OrderBy(num => num).ToList();

        for (int i = 0; i < sortedTicketNumbers.Count; i++)
        {
            if (menus.ContainsKey(sortedTicketNumbers[i])) // 딕셔너리에 해당 키가 있는지 확인
            {
                displayText += menus[sortedTicketNumbers[i]] + sortedTicketNumbers[i];
                if (sortedTicketNumbers.Count > 1 && i < sortedTicketNumbers.Count - 1)
                {
                    displayText += ", ";
                }
            }
        }

        textNumberellerDisPlay.text = displayText;
    }

    private void ConsumerDisplayUpdate()
    {
        string displayText = string.Join(", ", ticketNumbers);
        textNumberConsumerDisPlay.text = displayText;
    }

    private void KioskStart()
    {
        for (int i = 0; i < kioskScene.Count; i++)
        {
            kioskScene[i].SetActive(false);
            btnQuiteKiosk.gameObject.SetActive(false);
        }

        RefreshSlotList();
    }

    public void OnBuyMenue()
    {
        buyCheck = true;
        if (buyCheck == true)
        {
            Debug.Log("BuyChek OK");
            TakeTicket();
            tiketIssuance.gameObject.SetActive(buyCheck);
            btnQuiteKiosk.gameObject .SetActive(false);
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

        kiosk.PreOverlapKiosk();
    }

    public void KioskUsing()
    {
        btnQuiteKiosk.gameObject.SetActive(true);
        if (buyCheck == false)
        {
            KioskSceneChange();
        }
    }

    public void RefreshSlotList()
    {
        // 활성화된 슬롯을 비활성화
        foreach (var slot in poolSlot)
        {
            slot.gameObject.SetActive(false);
        }

        // listSelectedMenus의 각 요소에 대해 슬롯을 생성 또는 재사용
        foreach (var selectedMenu in listSelectedMenus)
        {
            CreateOrReuseSlot(selectedMenu);
        }
    }

    private void CreateOrReuseSlot(SelectedMenu _newMenu)
    {
        Slot slot = poolSlot.Find(s => !s.gameObject.activeSelf); // 비활성화된 슬롯 찾기
        if (slot == null)
        {
            // 비활성화된 슬롯이 없으면 새로 생성
            GameObject go = Instantiate(slotPrefab, poolSlot[0].transform.parent);
            slot = go.GetComponent<Slot>();
            poolSlot.Add(slot); // 오브젝트 풀에 슬롯 추가
        }

        // 슬롯의 인덱스를 찾아서 그 앞에 슬롯을 삽입
        int newIndex = listSelectedMenus.IndexOf(_newMenu);
        if (newIndex < 0)
        {
            Debug.LogError("Failed to find index of the new menu.");
            return;
        }
        poolSlot.Insert(newIndex, slot);

        slot.Init(_newMenu); // 슬롯 초기화
        slot.gameObject.SetActive(true); // 슬롯 활성화
    }


    public void RemoveSlot(SelectedMenu _nowMenu)
    {
        // 해당하는 메뉴의 슬롯을 찾습니다.
        Slot slotToRemove = poolSlot.Find(slot => slot.selectedMenu == _nowMenu);
        if (slotToRemove != null)
        {
            // 슬롯을 비활성화하여 풀에 반환합니다.
            slotToRemove.gameObject.SetActive(false);
        }

        // 메뉴와 티켓 정보를 제거합니다.
        listSelectedMenus.Remove(_nowMenu);
        ticketNumbers.Remove(_nowMenu.GetIndex());
        menus.Remove(_nowMenu.GetIndex());

        // 정렬합니다.
        SortSlots();

        KioskUpdate();
        SellerDisplayUpdate();
        ConsumerDisplayUpdate();
    }


    public void SortSlots()
    {
        // 모든 슬롯을 포함한 리스트를 만듭니다.
        List<Slot> allSlots = new List<Slot>(poolSlot);
        foreach (var slot in poolSlot)
        {
            // 활성화된 슬롯이라면 해당 슬롯을 추가합니다.
            if (slot.gameObject.activeSelf)
            {
                allSlots.Add(slot);
            }
        }

        // 모든 슬롯을 정렬합니다.
        allSlots.Sort((a, b) => a.selectedMenu.intSlotIndex.CompareTo(b.selectedMenu.intSlotIndex));

        // 정렬된 순서대로 슬롯의 순서를 갱신합니다.
        for (int i = 0; i < allSlots.Count; i++)
        {
            allSlots[i].transform.SetSiblingIndex(i);
        }
    }


}
