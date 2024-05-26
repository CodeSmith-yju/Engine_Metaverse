using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedMenu
{
    // 주문받은 실제 데이터가 얘를 통해 Slot으로 넘어갈듯
    protected string strMenuName;
    protected int intMenuIndex;

    public int intSlotIndex = 1000;
    public SelectedMenu(string _menuName, int _menuIndex)
    {
        this.strMenuName = _menuName;
        this.intMenuIndex = _menuIndex;
    }

    public string GetName() { return strMenuName; }
    public int GetIndex() { return intMenuIndex; }
}
