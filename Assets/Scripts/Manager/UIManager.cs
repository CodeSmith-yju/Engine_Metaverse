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

    // ��ü���� UI ������ ���⼭ �ϸ� ������? �̹����� �ؽ�Ʈ �� Ű����ũ UI�� ����� �ű�� ������ (GameMgr���� ����� ���� ��, ��ư�� ��Ŭ�� �̺�Ʈ �߰��� �� UIManager ������Ʈ ���)
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
        // UI ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �ٸ� UI â�� �ȿ��� ���� �� ����â ������ ��.
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
                // ESC�� ������ ���� â�� �ڽ����� �ִ� ������ �˾��̳� Ű���� �ȳ� �˾��� ���������� �̰� ���� ���� �ݵ��� ��.
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

            // �̷¼� ���� UI�� ���� ���� �� ESCŰ�� ������ �ݵ��� ��.
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

            // �����޴� UI�� ���� ���� �� ESCŰ ������ �ݵ��� ��.
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

            // �˸�â �˾��� ���������� �ݵ�����.
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
                    OnAlertPopup("������ �����ϴ�.");
                }
            }
        }
    } 

    // �ſ� ��� �ִ� ��� ������ ����
    public void CupIcon(string tag)
    {
        foreach (GameObject icon_List in cup_Icon_List)
        {
            if (icon_List.tag == tag)
            {
                if (cup_List.transform.childCount > 5)
                {
                    OnAlertPopup("�� �̻� �ſ� ��Ḧ \n���� �� �����ϴ�.");
                    return;
                }
                GameObject icon = Instantiate(icon_List, GameMgr.Instance.ui.cup_List.transform);
            }
        }
    }

    // �ſ� ����ִ� ��� ��ü ������ ����
    public void DeleteCupIcon()
    {
        foreach (Transform icon_List in GameMgr.Instance.ui.cup_List.transform)
        {
            if (icon_List != null)
                Destroy(icon_List.gameObject);
        }
    }

    // �丮 ��ȣ�ۿ� �˾�
    public void CheckPopup(string tag, Players player)
    {
        check_Popup.SetActive(true);
        string alert = "";

        switch (tag)
        {
            case "Ice":
                alert = "�ſ� ������ �����ðڽ��ϱ�?";
                break;
            case "Cup":
                alert = "���� �տ� ��ڽ��ϱ�?";
                break;
            case "Espresso":
                alert = "�ſ� ���������Ҹ� �����ðڽ��ϱ�?";
                break;
            case "Mixer":
                alert = "������� �ſ� �ִ� ������ \n�ͼ��⿡ ���ڽ��ϱ�?";
                break;
            case "Grinder":
                alert = "Ŀ�ǰ��縦 �տ� ��ðڽ��ϱ�?";
                break;
            case "Dish":
                alert = "���� �����ðڽ��ϱ�?";
                break;
            case "Water_Ice":
                alert = "�ſ� �ü��� �����ðڽ��ϱ�?";
                break;
            case "Water_Hot":
                alert = "�ſ� �¼��� �����ðڽ��ϱ�?";
                break;
            case "Strawberry":
                alert = "�ſ� ���⸦ �����ðڽ��ϱ�?";
                break;
            case "Milk":
                alert = "�ſ� ������ �����ðڽ��ϱ�?";
                break;
            case "Chocolate":
                alert = "�ſ� ���ݸ��� �����ðڽ��ϱ�?";
                break;
            case "Yogurt":
                alert = "�ſ� ���Ʈ�� �����ðڽ��ϱ�?";
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
                    OnAlertPopup("�̹� �̷¼��� �����߽��ϴ�.");
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
