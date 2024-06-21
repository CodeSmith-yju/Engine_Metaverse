using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    // 실제 게임의 에서 판매자가 주문을 받았을때, 받은 주문정보를 쥐고있는 SelectMenu가 넘겨준 실제 주문받은 데이터를 화면에 표시하게될 슬롯에 부착할 스크립트
    // 판매자와 상호작용하게될 각각의 버튼(슬롯), UI로써 사용자에게 보여지게 될 것. 
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textIndex;
    public Image imgIcon;

    public SelectedMenu selectedMenu;
    public void Init(SelectedMenu _selectMenu)
    {
        this.selectedMenu = _selectMenu;

        gameObject.SetActive(true);//생성자를 통해 생성되면 활성화되어 사용자에게 노출됨
        textName.text = selectedMenu.GetName();
        textIndex.text = selectedMenu.GetIndex().ToString();
        imgIcon.sprite = selectedMenu.GetIcon();
    }

    public void OnClick()
    {
        Debug.Log("주문확인의 미니아이콘 클릭");
        KioskSystem.single.PassMenuData(this);

        KioskSystem.single.nowPlayer.nowMakeMenu  = this.selectedMenu.GetName();
    }

}
