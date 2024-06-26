using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ui_Main;
    public GameObject ui_Connect;

    // 전체적인 UI 관리를 여기서 하면 좋을듯? 이미지나 텍스트 등 키오스크 UI도 여기로 옮기면 좋을듯 (GameMgr에서 끌어다 쓰면 됨, 버튼에 온클릭 이벤트 추가할 땐 UIManager 오브젝트 사용)
    [Header("Popup")]
    public GameObject alert_Popup;
    public GameObject check_Popup;
    public GameObject fire_Popup; 
    public GameObject master_Popup;
    public GameObject nonAccept_Popup;
    public GameObject accept_Popup;

    [Header("Setting")]
    public GameObject setting_UI;
    public GameObject content_Info_UI;
    public GameObject keyset_Info_UI;

    [Header("Resume")]
    public GameObject job_Opening_UI;
    public GameObject resume_UI;
    public GameObject resume_Info;
    public Transform resume_List_Pos;
    public GameObject resume_List_Prefabs;
    public GameObject resume_Write_UI;

    [Header("POS")]
    public GameObject pos_Menu_UI_Bg;
    public GameObject pos_Menu_UI;
    public GameObject pos_Menu_Order_UI;
    public GameObject pos_Menu_Resume_UI;
    public GameObject pos_Menu_Sales_UI;
    public GameObject pos_Menu_Crew_UI;
    public Transform pos_Crew_List_Pos;
    public GameObject crew_Info;

    [Header("ScreenUI")]
    public GameObject cup_List_BG;
    public GameObject cup_List;
    public List<GameObject> cup_Icon_List;
    public GameObject coin_UI;
    public GameObject recipeInfo;

    [Header("KitchenUI")]
    public GameObject water_dispenser_UI;
    public GameObject refrigerator_UI;
    private void Update()
    {
        // UI 관련
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 다른 UI 창이 안열려 있을 때 설정창 열도록 함.
            if (!setting_UI.activeSelf 
                && !job_Opening_UI.activeSelf 
                && !pos_Menu_UI_Bg.activeSelf 
                && !alert_Popup.activeSelf 
                && !check_Popup.activeSelf 
                && !water_dispenser_UI.activeSelf 
                && !refrigerator_UI.activeSelf 
                && !accept_Popup.activeSelf 
                && !nonAccept_Popup.activeSelf 
                && !fire_Popup.activeSelf
                && !master_Popup.activeSelf
                && !resume_Info.activeSelf)
            {
                setting_UI.SetActive(true);
            }
            else if (setting_UI.activeSelf && !job_Opening_UI.activeSelf && !pos_Menu_UI_Bg.activeSelf)
            {
                // ESC를 누르면 셋팅 창의 자식으로 있는 컨텐츠 팝업이나 키셋팅 안내 팝업이 열려있으면 이거 부터 먼저 닫도록 함.
                if (content_Info_UI.activeSelf)
                {
                    content_Info_UI.SetActive(false);
                }
                else if (keyset_Info_UI.activeSelf)
                {
                    keyset_Info_UI.SetActive(false);
                }
                else
                {
                    setting_UI.SetActive(false);
                }
            }

            // 이력서 관련 UI가 열려 있을 때 ESC키를 누르면 닫도록 함.
            if (job_Opening_UI.activeSelf)
            {
                if (resume_UI.activeSelf)
                {
                    resume_UI.SetActive(false);
                }
                else
                {
                    job_Opening_UI.SetActive(false);
                }
            }

            // 포스메뉴 UI가 열려 있을 때 ESC키 누르면 닫도록 함.
            if (pos_Menu_UI_Bg.activeSelf)
            {
                if (pos_Menu_Order_UI.activeSelf)
                {
                    pos_Menu_Order_UI.SetActive(false);
                }
                else if (pos_Menu_Resume_UI.activeSelf)
                {
                    pos_Menu_Resume_UI.SetActive(false);
                }
                else if (pos_Menu_Sales_UI.activeSelf)
                {
                    pos_Menu_Sales_UI.SetActive(false);
                }
                else if (pos_Menu_Crew_UI.activeSelf)
                {
                    pos_Menu_Crew_UI.SetActive(false);
                }
                else if (pos_Menu_UI.activeSelf)
                {
                    pos_Menu_UI.SetActive(false);
                }
                else
                {
                    pos_Menu_UI_Bg.SetActive(false);
                }
               
            }

            // 알림창 팝업이 켜져있을때 닫도록함.
            if (alert_Popup.activeSelf)
            {
                alert_Popup.SetActive(false);
            } 

            if (check_Popup.activeSelf)
            {
                check_Popup.SetActive(false);
            }

            if (water_dispenser_UI.activeSelf)
            {
                water_dispenser_UI.SetActive(false);
            }

            if (refrigerator_UI.activeSelf)
            {
                refrigerator_UI.SetActive(false);
            }

            if (fire_Popup.activeSelf)
            {
                fire_Popup.SetActive(false);
            }
            
            if (master_Popup.activeSelf)
            {
                master_Popup.SetActive(false);
            }

            if (nonAccept_Popup.activeSelf)
            {
                nonAccept_Popup.SetActive(false);
            }

            if(resume_Info.activeSelf)
            {
                resume_Info.SetActive(false);
            }

            if (recipeInfo.activeSelf)
            {
                recipeInfo.SetActive(false);
            }

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (recipeInfo.activeSelf)
            {
                recipeInfo.SetActive(false);
            }
            else
            {
                recipeInfo.SetActive(true);
            }
        }
    }




    public void OnPopup(GameObject popup)
    {
        if (!popup.activeSelf)
        {
            popup.SetActive(true);
        }
    }

    public void OnAlertPopup(string alert)
    {
        alert_Popup.SetActive(true);
        alert_Popup.GetComponent<AlertInit>().TextInit(alert);
    }

    public void CancelPopup(GameObject popup)
    {
        if (popup.activeSelf) 
        {
            popup.SetActive(false);
        }
    }

    public void POSOnPopup(GameObject popup)
    {
        foreach (GameObject players in GameMgr.Instance.player_List)
        {
            PhotonView player_View = players.GetComponent<PhotonView>();

            if (player_View != null && player_View.IsMine)
            {
                Players player_Obj = player_View.GetComponent<Players>();

                if (player_Obj.GetRole() == Role.Manager)
                {
                    popup.SetActive(true);
                }
                else
                {
                    OnAlertPopup("권한이 없습니다.");
                }
            }
        }
    } 

    // 컵에 들어 있는 재료 아이콘 생성
    public void CupIcon(string tag)
    {
        foreach (GameObject icon_List in cup_Icon_List)
        {
            if (icon_List.tag == tag)
            {
                if (cup_List.transform.childCount > 5)
                {
                    OnAlertPopup("더 이상 컵에 재료를 \n넣을 수 없습니다.");
                    return;
                }
                GameObject icon = Instantiate(icon_List, GameMgr.Instance.ui.cup_List.transform);
            }
        }
    }

    // 컵에 들어있는 재료 전체 아이콘 삭제
    public void DeleteCupIcon()
    {
        foreach (Transform icon_List in GameMgr.Instance.ui.cup_List.transform)
        {
            if (icon_List != null)
                Destroy(icon_List.gameObject);
        }
    }

    // 요리 상호작용 팝업
    public void CheckPopup(string tag, Players player)
    {
        check_Popup.SetActive(true);
        string alert = "";

        switch (tag)
        {
            case "Ice":
                alert = "컵에 얼음을 넣으시겠습니까?";
                break;
            case "Cup":
                alert = "컵을 손에 들겠습니까?";
                break;
            case "Espresso":
                alert = "컵에 에스프레소를 넣으시겠습니까?";
                break;
            case "Mixer":
                alert = "현재까지 컵에 있는 재료들을 \n믹서기에 갈겠습니까?";
                break;
            case "Grinder":
                alert = "커피가루를 손에 드시겠습니까?";
                break;
            case "Dish":
                alert = "컵을 씻으시겠습니까?";
                break;
            case "Water_Ice":
                alert = "컵에 냉수를 넣으시겠습니까?";
                break;
            case "Water_Hot":
                alert = "컵에 온수를 넣으시겠습니까?";
                break;
            case "Strawberry":
                alert = "컵에 딸기를 넣으시겠습니까?";
                break;
            case "Milk":
                alert = "컵에 우유를 넣으시겠습니까?";
                break;
            case "Chocolate":
                alert = "컵에 초콜릿를 넣으시겠습니까?";
                break;
            case "Yogurt":
                alert = "컵에 요거트를 넣으시겠습니까?";
                break;
        }

        check_Popup.GetComponent<CheckPopupInit>().Init(alert, tag, player);
    }

    public void ResumeSubmitPopup(GameObject popup)
    {

        foreach (GameObject player in GameMgr.Instance.player_List)
        {
            PhotonView player_view = player.GetComponent<PhotonView>();

            if (player_view != null && player_view.IsMine)
            {
                Players player_Obj = player.GetComponent<Players>();
                if (player_Obj.resume_Done)
                {
                    Debug.Log("Resume already submitted.");
                    OnAlertPopup("이미 이력서를 제출했습니다.");
                    return;
                }
                else
                {
                    popup.SetActive(true);
                }
            }

        }
    }

    public void ResumeWirting()
    {
        ResumeView resume = resume_Write_UI.GetComponent<ResumeView>();

        foreach (GameObject player in GameMgr.Instance.player_List)
        {
            PhotonView player_view = player.GetComponent<PhotonView>();

            if (player_view != null && player_view.IsMine)
            {
                Players player_Obj = player.GetComponent<Players>();
                if (player_Obj.resume_Done)
                {
                    Debug.Log("Resume already submitted.");
                    return;
                }
                else
                {
                    resume.NameGenderInit(player_Obj.nickName, player_Obj.gender);
                }
            }

        }
    }

    


}
