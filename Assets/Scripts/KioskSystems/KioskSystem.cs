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

    public Dictionary<int, string> order_List = new Dictionary<int, string>();
    [SerializeField] private List<int> ticketNumbers = new List<int>();
    [SerializeField] private int ticketNum = 0;
    public string menuName = "";
    public Sprite menuSp = null;

    public Image kioskBuyPanel;

    [SerializeField] private Image tiketIssuance;
    [SerializeField] private TextMeshProUGUI textNumberKiosk;

    [Header("SellerDisplay Object")]
    public TextMeshProUGUI textNumberellerDisPlay;
    public Image sellerImg;
    [SerializeField] private List<Slot> poolSlot = new List<Slot>(); // ������ �κ�
    [SerializeField] private List<SelectedMenu> listSelectedMenus = new List<SelectedMenu>(); // ������ �κ�

    public GameObject slotPrefab; // ���ο� ������ ������ �� ����� ������

    //06-11 PosMachin Img Add
    [Header("Pos Machine")]
    public GameObject objPosMachin;

    [Header("ConsumerDisplay Object")]
    public TextMeshProUGUI textNumberConsumerDisPlay;

    public int kioIndex = 0;
    public bool buyCheck = false;

    public Button btnBuyOK;

    public TextMeshProUGUI textBuyDesc;

    [Header("Order")]
    public Image imgOrder;
    public TextMeshProUGUI textOrder;

    //public Button btnQuiteKiosk;
    public GameObject announce;
    public TextMeshProUGUI textannounce;

    [Header("Desc")]
    public GameObject Desc;
    public Image descImg;
    public TextMeshProUGUI textdescName;
    public TextMeshProUGUI textdescIndex;

    public Slot selectedSlot = null;// �긦 ���ؼ� ���� ����� ��������� Ŭ���ؼ� ��(Desc)�� �������� ������Ʈ�� ��ȣ�ۿ뿩�ΰ� ��Ե��� ����, �ش��ư������Ʈ���� slot.desc�� ������ ��.

    //player move
    public bool kiosck;

    private void Awake()
    {
        single = this;
        KioskStart();
    }
    private void Start()
    {
        announce.SetActive(false);
        textannounce.gameObject.SetActive(false);
        kioskBuyPanel.gameObject.SetActive(false);
        tiketIssuance.gameObject.SetActive(false);
        sellerImg.gameObject.SetActive(false);

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
        order_List.Add(ticketNum, menuName);
        SelectedMenu newMenu = new SelectedMenu(menuName, ticketNum, menuSp);
        listSelectedMenus.Add(newMenu);
        newMenu.intSlotIndex = ticketNum;
        CreateOrReuseSlot(newMenu);

        // ������ �߰��� �Ŀ� �����մϴ�.
        SortSlots();

        KioskUpdate();
        SellerDisplayUpdate();
        ConsumerDisplayUpdate();

        menuName = null;
        menuSp = null;
    }


    private void KioskUpdate()
    {
        string kioskText = "������ ����";
        /*if (order_List.ContainsKey(ticketNum))
        {
            if (ticketNum < 10)
            {
                kioskText += "�ֹ��Ͻ� �޴�: \n" + order_List[ticketNum] + "\n �ֹ� ��ȣ: \n 00" + ticketNum.ToString();
            }
            else if (ticketNum < 100)
            {
                kioskText += "�ֹ��Ͻ� �޴�: \n" + order_List[ticketNum] + "\n �ֹ� ��ȣ: \n 0" + ticketNum.ToString();
            }
            else
            {
                kioskText += "�ֹ��Ͻ� �޴�: \n" + order_List[ticketNum] + "\n �ֹ� ��ȣ: \n" + ticketNum.ToString();
            }
        }
        else
        {
            kioskText += "�ֹ��Ͻ� �޴�: \n" + menuName + "\n �ֹ� ��ȣ: \n" + ticketNum.ToString();
        }*/

        textNumberKiosk.text = kioskText;
    }

    private void SellerDisplayUpdate()
    {
        string displayText = "";

        List<int> sortedTicketNumbers = ticketNumbers.OrderBy(num => num).ToList();

        for (int i = 0; i < sortedTicketNumbers.Count; i++)
        {
            if (order_List.ContainsKey(sortedTicketNumbers[i])) // ��ųʸ��� �ش� Ű�� �ִ��� Ȯ��
            {
                displayText += order_List[sortedTicketNumbers[i]] + sortedTicketNumbers[i];
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
            //btnQuiteKiosk.gameObject.SetActive(false);
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
            
            /*foreach (GameObject players in GameMgr.Instance.player_List)
            {
                if (players.GetComponent<PhotonView>().IsMine)
                {
                    players.GetComponent<Players>().coin--;
                } 
            }*/

            tiketIssuance.gameObject.SetActive(buyCheck);
            //btnQuiteKiosk.gameObject .SetActive(false);
            KioskSceneChange();
            kioIndex++;
        }
    }
    public void IssuanceOK()
    {
        kiosck = false;
        buyCheck = false;
        tiketIssuance.gameObject.SetActive(buyCheck);
        kioIndex = 0;
        textBuyDesc.text = "";
        //btnQuiteKiosk.gameObject.SetActive(false);
    }
    public void OnQuiteKiosk()
    {
        kiosck = false;
        for (int i = 0; i < kioskScene.Count; i++)
        {
            kioskScene[i].SetActive(false);
        }

        kioskBuyPanel.gameObject.SetActive(false);
        tiketIssuance.gameObject.SetActive(false);

        //btnQuiteKiosk.gameObject.SetActive(false);
        kioIndex = 0;
    }

    public void KioskUsing()
    {
        kiosck = true;
        //btnQuiteKiosk.gameObject.SetActive(true);
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
        order_List.Remove(_nowMenu.GetIndex());

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

    public void PassMenuData(Slot _slot)
    {
        /*if ( ȯ�� == true)
        {
            foreach (GameObject players in GameMgr.Instance.player_List)
            {
                if (_slot.selectedMenu.intSlotIndex == players.GetComponent<Player>().nowMenuIndex)
                {
                    players.GetComponent<Players>().coin++;
                } 
            }
        }*/
        Debug.Log("Run PassData MenuName: "+_slot.textName);
        selectedSlot = _slot;//�������� ����..�긦���� ��ȣ�ۿ��ؼ� ��ȣ��, �ֹ��Ϸ�, �ֹ���� �� ����� ��. 

        Desc.SetActive(true);
        descImg.sprite = _slot.imgIcon.sprite;
        textdescName.text = _slot.textName.text;
        textdescIndex.text = _slot.textIndex.text;
    }

    public void OnClickCommitOrder()//�ù��̰Կ� �ų�? -> �̰� �� �ǳĸ� _slot���� �޾ƿ��°� ���ο� slot�� ����°Ծƴ϶� slot�Ѱ��� ������Ʈ�� �ּҰ��� �����ϰ��ִ� ��Ȳ�̶� ���ﶧ�� ������ �����Ѱ�.
    {
        //�ֹ���Ҷ� �ֹ� �Ϸ�� �����ϰ��൵�ɵ�? �׳� ���� if���־ �ֹ�����̸�  false�ϰ� �� �����ְ� return, �ֹ��Ϸ��̸� ������Ʈ ����~ ���������� 
        RemoveSlot(selectedSlot.selectedMenu);//�׷��� ���� ����? 
        Debug.Log("OnClick Slot Index: " + selectedSlot.selectedMenu.GetIndex());
        selectedSlot.gameObject.SetActive(false);
        Desc.SetActive(false);
    }
}
