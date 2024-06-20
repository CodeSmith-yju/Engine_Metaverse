using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResumeView : MonoBehaviour
{
    public TMP_Text nickName;
    public TMP_Text gender;
    public TMP_Text desc;
    public Button yes_Btn;
    public Button no_Btn;
    public Button fire_Btn;

    public void Init(string name, int gender, string desc, bool isEmployee)
    {
        nickName.text = name;
        if (gender == 0) 
        {
            this.gender.text = "남자";
        }
        else
        {
            this.gender.text = "여자";
        }
        this.desc.text = desc;

        if (!isEmployee) 
        {
            yes_Btn.gameObject.SetActive(true);
            no_Btn.gameObject.SetActive(true);
            fire_Btn.gameObject.SetActive(false);
        }
        else
        {
            yes_Btn.gameObject.SetActive(false);
            no_Btn.gameObject.SetActive(false);
            fire_Btn.gameObject.SetActive(true);
        }
           
    }

}
