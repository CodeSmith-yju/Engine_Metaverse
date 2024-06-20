using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResumeViewInit : MonoBehaviour
{
    public TMP_Text nickName;
    public Button view_Btn;
    public int gender;
    public string desc;
    Players player;

    public void Init(string name, int gender, string desc, Players player)
    {
        this.nickName.text = name;
        this.gender = gender;
        this.desc = desc;
        this.player = player;
        view_Btn.onClick.AddListener(() => ViewResume());
    }


    void ViewResume()
    {
        bool check_emp;

        if (player.GetRole() == Role.Employee)
        {
            check_emp = true;
        }
        else
        {
            check_emp = false;
        }

        ResumeView resume = GameMgr.Instance.ui.resume_Info.GetComponent<ResumeView>();
        resume.Init(nickName.text, gender, desc, check_emp);
        resume.gameObject.SetActive(true);
    }

}
