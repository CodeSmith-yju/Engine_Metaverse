using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedMenu
{
    // �ֹ����� ���� �����Ͱ� �긦 ���� Slot���� �Ѿ��
    protected string strMenuName;
    protected int intMenuIndex;
    protected Image imgMenuIcon;

    public int intSlotIndex = 1000;
    public SelectedMenu(string _menuName, int _menuIndex)
    {
        this.strMenuName = _menuName;
        this.intMenuIndex = _menuIndex;

    }

    public string GetName() { return strMenuName; }
    public int GetIndex() { return intMenuIndex; }

    public Image GetIcon(string _str)
    {
        switch(_str)
        {
            case "":
                break;
            default:
                break;
        }

        return imgMenuIcon;
    }
}
