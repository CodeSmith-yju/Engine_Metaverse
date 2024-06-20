using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerViewSlot_Order : MonoBehaviour
{
    public TextMeshProUGUI orderIndex;
    public TextMeshProUGUI orderUserName;
    public TextMeshProUGUI orderMeneName;

    public void Init(SelectedMenu _selectedMenu)
    {
        this.orderIndex.text = _selectedMenu.GetIndex().ToString() + "¹ø";
        this.orderUserName.text = _selectedMenu.GetUserName();
        this.orderMeneName.text = _selectedMenu.GetName();
    }

}
