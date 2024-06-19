using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ui_Main;


    // 전체적인 UI 관리를 여기서 하면 좋을듯? 이미지나 텍스트 등 키오스크 UI도 여기로 옮기면 좋을듯 (GameMgr에서 끌어다 쓰면 됨, 버튼에 온클릭 이벤트 추가할 땐 UIManager 오브젝트 사용)
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
        // UI 관련
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 다른 UI 창이 안열려 있을 때 설정창 열도록 함.
            if (!setting_UI.activeSelf && !job_Opening_UI.activeSelf && !pos_Menu_UI.activeSelf)
            {
                setting_UI.SetActive(true);
            }
            else if (setting_UI.activeSelf && !job_Opening_UI.activeSelf && !pos_Menu_UI.activeSelf)
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
