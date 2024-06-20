using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    public Image sellerImg;
    [SerializeField] private List<Slot> poolSlot = new List<Slot>(); // ������ �κ�
    [SerializeField] private List<SelectedMenu> listSelectedMenus = new List<SelectedMenu>(); // ������ �κ�

    public GameObject slotPrefab; // ���ο� ������ ������ �� ����� ������

    //06-19 Add
    public List<ConsumSlot> commitOrderList = new();//�ֹ��Ϸ�� �޴� ������ �������ִٰ� �ش� ������ ���� ������Ʈ�� �����ϴµ� �����
    public List<ConsumSlot> waitOrderList = new();

    public ConsumSlot objNowActiveConsumerOrder;//���� �ֹ� �Ϸ� ��, ��������� ����� ū �۾��� �갡 �������
    public GameObject conSumePrefab;// ����/�ı��� ���� �޴������� ���� ������Ʈ

    [SerializeField] private Transform tr_commitTxt;
    [SerializeField] private Transform tr_waitTxt;

    //06-11 PosMachin Img Add
    [Header("Pos Machine")]
    public Image management_Display;

    [Header("ConsumerDisplay Object")]
    public int kioIndex = 0;
    public bool buyCheck = false;

    public Button btnBuyOK;
    public TextMeshProUGUI textBuyDesc;

    //06-20 Add
    public Button btn_CallConsumer;

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

    [HideInInspector] public Slot selectedSlot = null;// �긦 ���ؼ� ���� ����� ��������� Ŭ���ؼ� ��(Desc)�� �������� ������Ʈ�� ��ȣ�ۿ뿩�ΰ� ��Ե��� ����, �ش��ư������Ʈ���� slot.desc�� ������ ��.

    //player move
    public bool kiosck;
    [Header("#Menu Rank")]
    public GameObject obj_managerOrderView;//ȭ�鿡 ǥ�õ� UI

    public Dictionary <string, SelectedMenu> dict_managerOrderView = new();// ���� �ٲ�� �ʱ�ȭ�Ǵ� ������, �긦�������� list�� Prefab �����ϰ� ������ ��.
    public List<SelectedMenu> list_ManagerOrderView = new();// ���� ȭ�� Ű�� ���ϰ�
    public GameObject obj_ManagerOrderView;

    private void Awake()
    {
        single = this;
        KioskStart();

        commitOrderList.Clear();
        waitOrderList.Clear();
    }
    private void Start()
    {
        announce.SetActive(false);
        textannounce.gameObject.SetActive(false);
        kioskBuyPanel.gameObject.SetActive(false);
        tiketIssuance.gameObject.SetActive(false);
        sellerImg.gameObject.SetActive(false);
        Desc.SetActive(false);
        imgOrder.gameObject.SetActive(false);
        management_Display.gameObject.SetActive(false);
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

        //06-19 ConsumDisplay
        AddWaitOrder(newMenu);

        // ������ �߰��� �Ŀ� �����մϴ�.
        SortSlots();
        //����� �� ���� �ʿ�



        KioskUpdate();
        //SellerDisplayUpdate();
        //ConsumerDisplayUpdate();���� �� ��

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

/*    private void SellerDisplayUpdate()
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
        //Add ConsumerSlot(ticketNumbers);
        string displayText = string.Join(", ", ticketNumbers);
        textNumberConsumerDisPlay.text = displayText;
    }*/

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
        Debug.Log("KioUsing");
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
        // ����Ʈ2���� commitText �����.
        // listSelectedMenus�� �� ��ҿ� ���� ������ ���� �Ǵ� ����
        foreach (var selectedMenu in listSelectedMenus)
        {
            CreateOrReuseSlot(selectedMenu);
            //�����ڵ� �ʿ�//06-17 Add
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
        //��Ȱ��ȭ ã�Ƽ� �����ϴ��ڵ� �ʿ� 
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
        //���� �� ���� �ʿ�

        KioskUpdate();
        //SellerDisplayUpdate();
        //ConsumerDisplayUpdate();���� �� ��
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
        //06-20 Add
        if (_slot.selectedMenu.oneTimeCall)
            btn_CallConsumer.interactable = true;
        else
            btn_CallConsumer.interactable= false;


        Debug.Log("Run PassData MenuName: "+_slot.textName);
        selectedSlot = _slot;//�������� ����..�긦���� ��ȣ�ۿ��ؼ� ��ȣ��, �ֹ��Ϸ�, �ֹ���� �� ����� ��. 

        Desc.SetActive(true);
        descImg.sprite = _slot.imgIcon.sprite;
        textdescName.text = _slot.textName.text;
        textdescIndex.text = _slot.textIndex.text;
    }

    public void OnClickCommitOrder()//�ù��̰Կ� �ų�? -> �̰� �� �ǳĸ� _slot���� �޾ƿ��°� ���ο� slot�� ����°Ծƴ϶� slot�Ѱ��� ������Ʈ�� �ּҰ��� �����ϰ��ִ� ��Ȳ�̶� ���ﶧ�� ������ �����Ѱ�.
    {
        //�ֹ���Ҷ� �ֹ� �Ϸ�� �����ϰ��൵�ɵ�? �׳� ���� if���־ �ֹ�����̸�  false�ϰ� �� �����ְ� return, || �ֹ��Ϸ��̸� �Ǹ� ��Ͽ� ������Ʈ ����~ ���������� 
        if (false)//cancelOrder
        {
        }

        RemoveCommitOrder(selectedSlot.selectedMenu);//06-20 Add
        RemoveSlot(selectedSlot.selectedMenu);//�׷��� ���� ����? 

        Debug.Log("OnClick Slot Index: " + selectedSlot.selectedMenu.GetIndex());
        selectedSlot.gameObject.SetActive(false);
        Desc.SetActive(false);
    }
    public void OnClickCallConsumer()
    {
        StartCoroutine("CallOderFade");//�ڷ�ƾ������ ���� �� ������ ����Ŭ���ǹ���

        Debug.Log("Call Consumer");
        //imgOrderȰ�� TextOrder�� �� index ǥ�õ�, Ȱ���̹����� �ڷ�ƾ���� �־��ָ�ɰŰ����� ���� Ȱ��ȭ�ϰ�
        if (commitOrderList.Count == 0 && string.IsNullOrEmpty(objNowActiveConsumerOrder.selectedText.text))//&& string.IsNullOrEmpty(txtNowCommitOrder.text))
        {
            SetActiveConsumerData();
        }
        else if (!string.IsNullOrEmpty(objNowActiveConsumerOrder.selectedText.text))
        {
            AddCommitOrder(objNowActiveConsumerOrder.selectedMenu);//���� ����ǵ����� �ϴ����� Pass
            SetActiveConsumerData();
            Debug.Log("Call Consumer is false");
        }
    }
    private void SetActiveConsumerData()// �ߺ��Ǵ� �ڵ� �޼���� ���� �� 06-20
    {
        objNowActiveConsumerOrder.selectedMenu = selectedSlot.selectedMenu;
        objNowActiveConsumerOrder.selectedText.text = objNowActiveConsumerOrder.selectedMenu.GetIndex().ToString();
        
        selectedSlot.selectedMenu.oneTimeCall = false;
        btn_CallConsumer.interactable = false;

        for (int i = 0; i < waitOrderList.Count; i++)
        {
            if (waitOrderList[i].selectedMenu == selectedSlot.selectedMenu)
            {
                waitOrderList[i].GetOutWaitSlot();
                return;
            }
        }
    }
    public void OpenReceiptMenu()
    {
        //management_Display.gameObject.SetActive(false);
        sellerImg.gameObject.SetActive(true);
    }

    public void OffManagemetMenu()
    {
        management_Display.gameObject.SetActive(false);
        kiosck = false;
    }
    //06-19
    public void AddWaitOrder(SelectedMenu _selectedMenu)
    {
        //ConsumSlot consumSlot = new();
        GameObject go = Instantiate(conSumePrefab, tr_waitTxt);//���� ������ ������Ʈ�� ������ ��ġ�� ����

        ConsumSlot consumSlot = go.GetComponent<ConsumSlot>();
        waitOrderList.Add(consumSlot);
        consumSlot.Init(_selectedMenu);
    }
    public void AddCommitOrder(SelectedMenu _selectedMenu)
    {
        GameObject go = Instantiate(conSumePrefab, tr_commitTxt);//���� ������ ������Ʈ�� ������ ��ġ�� ����

        ConsumSlot consumSlot = go.GetComponent<ConsumSlot>();
        consumSlot.Init(_selectedMenu);
        commitOrderList.Add(consumSlot);
    }
    public void RemoveCommitOrder(SelectedMenu _selectedMenu)
    {
        foreach (ConsumSlot c_Slot in waitOrderList)
        {
            if (c_Slot.selectedMenu == _selectedMenu)
            {
                c_Slot.GetOutWaitSlot();
                return;
            }
        }

        if (objNowActiveConsumerOrder.selectedMenu == selectedSlot.selectedMenu)
        {
            if (commitOrderList.Count > 0)
            {
                Debug.Log("commitList is Not null");
                Debug.Log("Cnt CommitList: " + commitOrderList.Count);
                objNowActiveConsumerOrder.selectedMenu = commitOrderList[(commitOrderList.Count - 1)].selectedMenu;
                objNowActiveConsumerOrder.selectedText.text = objNowActiveConsumerOrder.selectedMenu.GetIndex().ToString();

                commitOrderList[commitOrderList.Count - 1].GetOutWaitSlot();
                commitOrderList.Remove(commitOrderList[commitOrderList.Count - 1]);
                return;
            }
            else
            {
                Debug.Log("Active Text is true");
                objNowActiveConsumerOrder.selectedMenu = null;
                objNowActiveConsumerOrder.selectedText.text = null;
                return;
            }
        }

        foreach (ConsumSlot c_Slot in commitOrderList)
        {
            if (_selectedMenu == c_Slot.selectedMenu)
            {
                Debug.Log("after Active Text is true");
                commitOrderList.Remove(c_Slot);
                c_Slot.GetOutWaitSlot();
                return;
            }
        }
    }


    private IEnumerator CallOderFade()
    {
        foreach (var _slot in poolSlot)
        {
            _slot.GetComponent<Button>().interactable = false;
        }
        btn_CallConsumer.interactable = false;

        Color color = imgOrder.color;
        imgOrder.gameObject.SetActive(true);
        color.a = 1f;
        imgOrder.color = color;

        textOrder.text = selectedSlot.selectedMenu.GetIndex().ToString();

        yield return new WaitForSeconds(2f);

        float fadeDuration = 0.5f;
        float endDuration = 0f;

        while (endDuration < fadeDuration)
        {
            endDuration += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, endDuration / fadeDuration);
            imgOrder.color = color;
            yield return null;
        }

        // alpha ���� 0�� �Ǹ� �̹��� ��Ȱ��ȭ
        color.a = 0f;
        imgOrder.color = color;
        imgOrder.gameObject.SetActive(false);

        foreach (var _slot in poolSlot)
        {
            _slot.GetComponent<Button>().interactable = true;
        }
    }


    //manager Order View
    public void AddmanagerOrderView( )
    {
        /*GameObject go = Instantiate(conSumePrefab, tr_commitTxt);//���� ������ ������Ʈ�� ������ ��ġ�� ����

        ConsumSlot consumSlot = go.GetComponent<ConsumSlot>();
        consumSlot.Init(_selectedMenu);
        commitOrderList.Add(consumSlot);*/
    }
}
