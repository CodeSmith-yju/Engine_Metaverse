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

    [SerializeField] private List<Slot> poolSlot = new List<Slot>(); // ������ �κ�
    [SerializeField] private List<SelectedMenu> listSelectedMenus = new List<SelectedMenu>(); // ������ �κ�

    public GameObject slotPrefab; // ���ο� ������ ������ �� ����� ������

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

        // ������ �߰��� �Ŀ� �����մϴ�.
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
                kioskText += "�ֹ��Ͻ� �޴�: \n" + menus[ticketNum] + "\n �ֹ� ��ȣ: \n 00" + ticketNum.ToString();
            }
            else if (ticketNum < 100)
            {
                kioskText += "�ֹ��Ͻ� �޴�: \n" + menus[ticketNum] + "\n �ֹ� ��ȣ: \n 0" + ticketNum.ToString();
            }
            else
            {
                kioskText += "�ֹ��Ͻ� �޴�: \n" + menus[ticketNum] + "\n �ֹ� ��ȣ: \n" + ticketNum.ToString();
            }
        }
        else
        {
            kioskText += "�ֹ��Ͻ� �޴�: \n" + menuName + "\n �ֹ� ��ȣ: \n" + ticketNum.ToString();
        }

        textNumberKiosk.text = kioskText;
    }

    private void SellerDisplayUpdate()
    {
        string displayText = "";

        List<int> sortedTicketNumbers = ticketNumbers.OrderBy(num => num).ToList();

        for (int i = 0; i < sortedTicketNumbers.Count; i++)
        {
            if (menus.ContainsKey(sortedTicketNumbers[i])) // ��ųʸ��� �ش� Ű�� �ִ��� Ȯ��
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
        // Ȱ��ȭ�� ������ ��Ȱ��ȭ
        foreach (var slot in poolSlot)
        {
            slot.gameObject.SetActive(false);
        }

        // listSelectedMenus�� �� ��ҿ� ���� ������ ���� �Ǵ� ����
        foreach (var selectedMenu in listSelectedMenus)
        {
            CreateOrReuseSlot(selectedMenu);
        }
    }

    private void CreateOrReuseSlot(SelectedMenu _newMenu)
    {
        Slot slot = poolSlot.Find(s => !s.gameObject.activeSelf); // ��Ȱ��ȭ�� ���� ã��
        if (slot == null)
        {
            // ��Ȱ��ȭ�� ������ ������ ���� ����
            GameObject go = Instantiate(slotPrefab, poolSlot[0].transform.parent);
            slot = go.GetComponent<Slot>();
            poolSlot.Add(slot); // ������Ʈ Ǯ�� ���� �߰�
        }

        // ������ �ε����� ã�Ƽ� �� �տ� ������ ����
        int newIndex = listSelectedMenus.IndexOf(_newMenu);
        if (newIndex < 0)
        {
            Debug.LogError("Failed to find index of the new menu.");
            return;
        }
        poolSlot.Insert(newIndex, slot);

        slot.Init(_newMenu); // ���� �ʱ�ȭ
        slot.gameObject.SetActive(true); // ���� Ȱ��ȭ
    }


    public void RemoveSlot(SelectedMenu _nowMenu)
    {
        // �ش��ϴ� �޴��� ������ ã���ϴ�.
        Slot slotToRemove = poolSlot.Find(slot => slot.selectedMenu == _nowMenu);
        if (slotToRemove != null)
        {
            // ������ ��Ȱ��ȭ�Ͽ� Ǯ�� ��ȯ�մϴ�.
            slotToRemove.gameObject.SetActive(false);
        }

        // �޴��� Ƽ�� ������ �����մϴ�.
        listSelectedMenus.Remove(_nowMenu);
        ticketNumbers.Remove(_nowMenu.GetIndex());
        menus.Remove(_nowMenu.GetIndex());

        // �����մϴ�.
        SortSlots();

        KioskUpdate();
        SellerDisplayUpdate();
        ConsumerDisplayUpdate();
    }


    public void SortSlots()
    {
        // ��� ������ ������ ����Ʈ�� ����ϴ�.
        List<Slot> allSlots = new List<Slot>(poolSlot);
        foreach (var slot in poolSlot)
        {
            // Ȱ��ȭ�� �����̶�� �ش� ������ �߰��մϴ�.
            if (slot.gameObject.activeSelf)
            {
                allSlots.Add(slot);
            }
        }

        // ��� ������ �����մϴ�.
        allSlots.Sort((a, b) => a.selectedMenu.intSlotIndex.CompareTo(b.selectedMenu.intSlotIndex));

        // ���ĵ� ������� ������ ������ �����մϴ�.
        for (int i = 0; i < allSlots.Count; i++)
        {
            allSlots[i].transform.SetSiblingIndex(i);
        }
    }


}
