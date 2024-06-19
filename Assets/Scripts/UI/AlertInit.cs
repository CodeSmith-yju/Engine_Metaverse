using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertInit : MonoBehaviour
{
    public TMP_Text text; 

    public void TextInit(string text)
    {
        this.text.text = text;
    }
}
