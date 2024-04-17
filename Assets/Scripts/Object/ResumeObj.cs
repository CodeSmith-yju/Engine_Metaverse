using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResumeObj : MonoBehaviour
{
    public TMP_Text nameText;
    public Button submit;

    public void SetName(string name)
    {
        this.nameText.text = name;
    }

}
