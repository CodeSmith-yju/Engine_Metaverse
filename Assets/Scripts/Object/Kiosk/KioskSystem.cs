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
    [SerializeField] private List<Slot> poolSlot = new List<Slot>(); // 수정된 부분
    [SerializeField] private List<SelectedMenu> listSelectedMenus = new List<SelectedMenu>(); // 수정된 부분

    public GameObject slotPrefab; // 새로운 슬롯을 생성할 때 사용할 프리팹

    //06-19 Add
    public List<ConsumSlot> commitOrderList = new();//주문완료된 메뉴 정보를 가지고있다가 해당 정보를 통해 오브젝트를 제거하는데 사용함
    public List<ConsumSlot> waitOrderList = new();

    public ConsumSlot objNowActiveConsumerOrder;//최초 주문 완료 시, 좌측상단의 노란색 큰 글씨를 얘가 들고있음
    public GameObject conSumePrefab;// 생성/파괴에 사용될 메뉴정보를 담은 오브젝트

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

    [HideInInspector] public Slot selectedSlot = null;// 얘를 통해서 이제 상단의 목록을통해 클릭해서 상세(Desc)를 보는중인 오브젝트의 상호작용여부가 어떻게될지 결정, 해당버튼컴포넌트들은 slot.desc로 가야할 것.

    //player move
    public bool kiosck;
    [Header("#Menu Rank")]
    public GameObject obj_managerOrderView;//화면에 표시될 UI

    public Dictionary <string, SelectedMenu> dict_managerOrderView = new();// 점장 바뀌면 초기화되는 데이터, 얘를바탕으로 list에 Prefab 삽입하고 갱신할 것.
    public List<SelectedMenu> list_ManagerOrderView = new();// 실제 화면 키면 보일거
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

        // 슬롯을 추가한 후에 정렬합니다.
        SortSlots();
        //재사용된 후 정렬 필요



        KioskUpdate();
        //SellerDisplayUpdate();
        //ConsumerDisplayUpdate();이제 안 씀

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

/*    private void SellerDisplayUpdate()
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
        // 활성화된 슬롯을 비활성화
        foreach (var slot in poolSlot)
        {
            slot.gameObject.SetActive(false);
        }
        // 리스트2개랑 commitText 비워줌.
        // listSelectedMenus의 각 요소에 대해 슬롯을 생성 또는 재사용
        foreach (var selectedMenu in listSelectedMenus)
        {
            CreateOrReuseSlot(selectedMenu);
            //재사용코드 필요//06-17 Add
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
        //비활성화 찾아서 제거하는코드 필요 
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
        //지운 후 정렬 필요

        KioskUpdate();
        //SellerDisplayUpdate();
        //ConsumerDisplayUpdate();이제 안 씀
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
        //06-20 Add
        if (_slot.selectedMenu.oneTimeCall)
            btn_CallConsumer.interactable = true;
        else
            btn_CallConsumer.interactable= false;


        Debug.Log("Run PassData MenuName: "+_slot.textName);
        selectedSlot = _slot;//선택중인 슬롯..얘를통해 상호작용해서 고객호출, 주문완료, 주문취소 가 실행될 것. 

        Desc.SetActive(true);
        descImg.sprite = _slot.imgIcon.sprite;
        textdescName.text = _slot.textName.text;
        textdescIndex.text = _slot.textIndex.text;
    }

    public void OnClickCommitOrder()//시발이게왜 돼냐? -> 이게 왜 되냐면 _slot으로 받아오는게 새로운 slot을 만드는게아니라 slot넘겨준 오브젝트의 주소값을 참조하고있는 상황이라 지울때도 접근이 가능한거.
    {
        //주문취소랑 주문 완료는 동일하게줘도될듯? 그냥 여기 if문넣어서 주문취소이면  false하고 돈 돌려주고 return, || 주문완료이면 판매 목록에 오브젝트 생성~ 같은식으로 
        if (false)//cancelOrder
        {
        }

        RemoveCommitOrder(selectedSlot.selectedMenu);//06-20 Add
        RemoveSlot(selectedSlot.selectedMenu);//그래서 어디로 빼냄? 

        Debug.Log("OnClick Slot Index: " + selectedSlot.selectedMenu.GetIndex());
        selectedSlot.gameObject.SetActive(false);
        Desc.SetActive(false);
    }
    public void OnClickCallConsumer()
    {
        StartCoroutine("CallOderFade");//코루틴때문에 여기 안 넣으면 연속클릭되버림

        Debug.Log("Call Consumer");
        //imgOrder활성 TextOrder에 내 index 표시됨, 활성이미지는 코루틴으로 넣어주면될거같은데 사운드 활성화하고
        if (commitOrderList.Count == 0 && string.IsNullOrEmpty(objNowActiveConsumerOrder.selectedText.text))//&& string.IsNullOrEmpty(txtNowCommitOrder.text))
        {
            SetActiveConsumerData();
        }
        else if (!string.IsNullOrEmpty(objNowActiveConsumerOrder.selectedText.text))
        {
            AddCommitOrder(objNowActiveConsumerOrder.selectedMenu);//좌측 상단의데이터 하단으로 Pass
            SetActiveConsumerData();
            Debug.Log("Call Consumer is false");
        }
    }
    private void SetActiveConsumerData()// 중복되는 코드 메서드로 따로 뺌 06-20
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
        GameObject go = Instantiate(conSumePrefab, tr_waitTxt);//실제 생성된 오브젝트를 지정된 위치에 대입

        ConsumSlot consumSlot = go.GetComponent<ConsumSlot>();
        waitOrderList.Add(consumSlot);
        consumSlot.Init(_selectedMenu);
    }
    public void AddCommitOrder(SelectedMenu _selectedMenu)
    {
        GameObject go = Instantiate(conSumePrefab, tr_commitTxt);//실제 생성된 오브젝트를 지정된 위치에 대입

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

        // alpha 값이 0이 되면 이미지 비활성화
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
        /*GameObject go = Instantiate(conSumePrefab, tr_commitTxt);//실제 생성된 오브젝트를 지정된 위치에 대입

        ConsumSlot consumSlot = go.GetComponent<ConsumSlot>();
        consumSlot.Init(_selectedMenu);
        commitOrderList.Add(consumSlot);*/
    }
}
