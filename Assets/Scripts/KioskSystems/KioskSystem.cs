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
    [SerializeField] private List<Slot> poolSlot = new List<Slot>(); // 수정된 부분
    [SerializeField] private List<SelectedMenu> listSelectedMenus = new List<SelectedMenu>(); // 수정된 부분

    public GameObject slotPrefab; // 새로운 슬롯을 생성할 때 사용할 프리팹

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

    public Slot selectedSlot = null;// 얘를 통해서 이제 상단의 목록을통해 클릭해서 상세(Desc)를 보는중인 오브젝트의 상호작용여부가 어떻게될지 결정, 해당버튼컴포넌트들은 slot.desc로 가야할 것.

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

        // 슬롯을 추가한 후에 정렬합니다.
        SortSlots();

        KioskUpdate();
        SellerDisplayUpdate();
        ConsumerDisplayUpdate();

        menuName = null;
        menuSp = null;
    }


    private void KioskUpdate()
    {
        string kioskText = "영수증 수령";
        /*if (order_List.ContainsKey(ticketNum))
        {
            if (ticketNum < 10)
            {
                kioskText += "주문하신 메뉴: \n" + order_List[ticketNum] + "\n 주문 번호: \n 00" + ticketNum.ToString();
            }
            else if (ticketNum < 100)
            {
                kioskText += "주문하신 메뉴: \n" + order_List[ticketNum] + "\n 주문 번호: \n 0" + ticketNum.ToString();
            }
            else
            {
                kioskText += "주문하신 메뉴: \n" + order_List[ticketNum] + "\n 주문 번호: \n" + ticketNum.ToString();
            }
        }
        else
        {
            kioskText += "주문하신 메뉴: \n" + menuName + "\n 주문 번호: \n" + ticketNum.ToString();
        }*/

        textNumberKiosk.text = kioskText;
    }

    private void SellerDisplayUpdate()
    {
        string displayText = "";

        List<int> sortedTicketNumbers = ticketNumbers.OrderBy(num => num).ToList();

        for (int i = 0; i < sortedTicketNumbers.Count; i++)
        {
            if (order_List.ContainsKey(sortedTicketNumbers[i])) // 딕셔너리에 해당 키가 있는지 확인
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
        order_List.Remove(_nowMenu.GetIndex());

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

    public void PassMenuData(Slot _slot)
    {
        /*if ( 환불 == true)
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
        selectedSlot = _slot;//선택중인 슬롯..얘를통해 상호작용해서 고객호출, 주문완료, 주문취소 가 실행될 것. 

        Desc.SetActive(true);
        descImg.sprite = _slot.imgIcon.sprite;
        textdescName.text = _slot.textName.text;
        textdescIndex.text = _slot.textIndex.text;
    }

    public void OnClickCommitOrder()//시발이게왜 돼냐? -> 이게 왜 되냐면 _slot으로 받아오는게 새로운 slot을 만드는게아니라 slot넘겨준 오브젝트의 주소값을 참조하고있는 상황이라 지울때도 접근이 가능한거.
    {
        //주문취소랑 주문 완료는 동일하게줘도될듯? 그냥 여기 if문넣어서 주문취소이면  false하고 돈 돌려주고 return, 주문완료이면 오브젝트 생성~ 같은식으로 
        RemoveSlot(selectedSlot.selectedMenu);//그래서 어디로 빼냄? 
        Debug.Log("OnClick Slot Index: " + selectedSlot.selectedMenu.GetIndex());
        selectedSlot.gameObject.SetActive(false);
        Desc.SetActive(false);
    }
}
