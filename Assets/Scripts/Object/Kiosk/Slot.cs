using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    // ���� ������ ���� �Ǹ��ڰ� �ֹ��� �޾�����, ���� �ֹ������� ����ִ� SelectMenu�� �Ѱ��� ���� �ֹ����� �����͸� ȭ�鿡 ǥ���ϰԵ� ���Կ� ������ ��ũ��Ʈ
    // �Ǹ��ڿ� ��ȣ�ۿ��ϰԵ� ������ ��ư(����), UI�ν� ����ڿ��� �������� �� ��. 
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textIndex;
    public Image imgIcon;

    public SelectedMenu selectedMenu;
    public void Init(SelectedMenu _selectMenu)
    {
        this.selectedMenu = _selectMenu;

        gameObject.SetActive(true);//�����ڸ� ���� �����Ǹ� Ȱ��ȭ�Ǿ� ����ڿ��� �����
        textName.text = selectedMenu.GetName();
        textIndex.text = selectedMenu.GetIndex().ToString();
        imgIcon.sprite = selectedMenu.GetIcon();
    }

    public void OnClick()
    {
        Debug.Log("�ֹ�Ȯ���� �̴Ͼ����� Ŭ��");
        KioskSystem.single.PassMenuData(this);

        KioskSystem.single.nowPlayer.nowMakeMenu  = this.selectedMenu.GetName();
    }

}
