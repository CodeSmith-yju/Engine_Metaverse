using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedMenu
{
    // 주문받은 실제 데이터가 얘를 통해 Slot으로 넘어갈듯
    protected string strMenuName;
    protected int intMenuIndex;
    protected Sprite spMenuIcon;

    public int intSlotIndex = 1000;

    //06-20 Add ConsumerText
    public bool oneTimeCall = false;

    public string buyUserName;
    public SelectedMenu(string _menuName, int _menuIndex, Sprite _menuSp)
    {
        this.strMenuName = _menuName;
        this.intMenuIndex = _menuIndex;
        this.spMenuIcon = _menuSp;
        this.oneTimeCall = true;
    }

    public string GetName() { return strMenuName; }
    public int GetIndex() { return intMenuIndex; }
    public Sprite GetIcon() { return spMenuIcon; }

    public string GetUserName()
    {
        return buyUserName;
    }
    public void SetUserName(string _name)
    {
        this.buyUserName = _name;
    }
}
