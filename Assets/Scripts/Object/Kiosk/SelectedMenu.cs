using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedMenu
{
    // �ֹ����� ���� �����Ͱ� �긦 ���� Slot���� �Ѿ��
    protected string strMenuName;
    protected int intMenuIndex;
    protected Sprite spMenuIcon;

    public int intSlotIndex = 1000;
    public SelectedMenu(string _menuName, int _menuIndex, Sprite _menuSp)
    {
        this.strMenuName = _menuName;
        this.intMenuIndex = _menuIndex;
        this.spMenuIcon = _menuSp;
    }

    public string GetName() { return strMenuName; }
    public int GetIndex() { return intMenuIndex; }
    public Sprite GetIcon() { return spMenuIcon; }
}