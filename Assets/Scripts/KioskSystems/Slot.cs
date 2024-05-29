using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    // ���� ������ ���� �Ǹ��ڰ� �ֹ��� �޾�����, ���� �ֹ������� ����ִ� SelectMenu�� �Ѱ��� ���� �ֹ����� �����͸� ȭ�鿡 ǥ���ϰԵ� ���Կ� ������ ��ũ��Ʈ
    // �Ǹ��ڿ� ��ȣ�ۿ��ϰԵ� ������ ��ư(����), UI�ν� ����ڿ��� �������� �� ��. 
    //[SerializeField] private Image imgIcon;// �ϴ� ���� �ϰ����� �ֹ������޴������ܵ� ǥ��������ϳ�?
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textIndex;
    public Image imgIcon;

    public SelectedMenu selectedMenu;
    public void Init(SelectedMenu _selectMenu)
    {
        this.selectedMenu = _selectMenu;

        gameObject.SetActive(true);//�����ڸ� ���� �����Ǹ� Ȱ��ȭ�Ǿ� ����ڿ��� �����
        textName.text = selectedMenu.GetName();
        textIndex.text = selectedMenu.GetIndex().ToString();
        //imgIcon = _selectMenu.img;
    }

    //        KioskSystem.single.RemoveSlot(selectedMenu);//�̻����� ���ݱ��� ��µ� ��ȣ�� ����� �ٽɱ���� �긦�ٸ���� ��������
    public void OnClick()
    {
        //�Ǹ��ڰ� �޴��ϼ��ϰ� ���� Ŭ���ϸ� ��µ� �޼��� ����� ȣ���Ͻðڽ��ϱ�? || Destroyer
        //SellerMenus.single.
        KioskSystem.single.RemoveSlot(selectedMenu);//�׷��� ���� ����? 
        Debug.Log("OnClick Slot Index: "+selectedMenu.GetIndex() );

        gameObject.SetActive(false);

        // Pass MenuData(tihs);
    }

    public void Delete()
    {

    }
}
