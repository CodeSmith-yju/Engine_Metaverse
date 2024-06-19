using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ui_Main;


    // ��ü���� UI ������ ���⼭ �ϸ� ������? �̹����� �ؽ�Ʈ �� Ű����ũ UI�� ����� �ű�� ������ (GameMgr���� ����� ���� ��, ��ư�� ��Ŭ�� �̺�Ʈ �߰��� �� UIManager ������Ʈ ���)
    [Header("Popup")]
    public GameObject alert_Popup;

    [Header("Setting")]
    public GameObject setting_UI;
    public GameObject content_Info_UI;
    public GameObject keyset_Info_UI;

    [Header("Resume")]
    public GameObject job_Opening_UI;
    public GameObject resume_UI;

    [Header("POS")]
    public GameObject pos_Menu_UI;
    public GameObject pos_Menu_Order_UI;
    public GameObject pos_Menu_Resume_UI;
    public GameObject pos_Menu_Sales_UI;
    public GameObject pos_Menu_Crew_UI;
    public GameObject pos_Menu_Access_Popup;

    [Header("ScreenUI")]
    public GameObject cup_List_BG;
    public GameObject cup_List;
    public List<GameObject> cup_Icon_List;
    public GameObject coin_UI;

    [Header("KitchenUI")]
    public GameObject water_dispenser_UI;

    private void Update()
    {
        // UI ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �ٸ� UI â�� �ȿ��� ���� �� ����â ������ ��.
            if (!setting_UI.activeSelf && !job_Opening_UI.activeSelf && !pos_Menu_UI.activeSelf)
            {
                setting_UI.SetActive(true);
            }
            else if (setting_UI.activeSelf && !job_Opening_UI.activeSelf && !pos_Menu_UI.activeSelf)
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
            if (pos_Menu_UI.activeSelf)
            {
                /*if (pos_Menu_Order_UI.activeSelf)
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
                else
                {
                    pos_Menu_UI.SetActive(false);
                }*/
                pos_Menu_UI.SetActive(false);
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

    public void CancelPopup(GameObject popup)
    {
        if (popup.activeSelf) 
        {
            popup.SetActive(false);
        }
    }

    

}
