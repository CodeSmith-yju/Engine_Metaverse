using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // ��ü���� UI ������ ���⼭ �ϸ� ������? �̹����� �ؽ�Ʈ �� Ű����ũ UI�� ����� �ű�� ������ (GameMgr���� ����� ���� ��, ��ư�� ��Ŭ�� �̺�Ʈ �߰��� �� UIManager ������Ʈ ���)

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
