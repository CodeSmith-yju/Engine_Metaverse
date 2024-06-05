using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 전체적인 UI 관리를 여기서 하면 좋을듯? 이미지나 텍스트 등 키오스크 UI도 여기로 옮기면 좋을듯 (GameMgr에서 끌어다 쓰면 됨, 버튼에 온클릭 이벤트 추가할 땐 UIManager 오브젝트 사용)

    [Header("Setting")]
    public GameObject setting_Ui;
    public GameObject content_Info_Ui;
    public GameObject keyset_Info_Ui;

    [Header("Resume")]
    public GameObject job_Opening_Ui;
    public GameObject resume_Ui;


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

    public void Quit()
    {
        Application.Quit();
    }

}
