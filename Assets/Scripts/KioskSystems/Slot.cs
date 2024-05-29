using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    // 실제 게임의 에서 판매자가 주문을 받았을때, 받은 주문정보를 쥐고있는 SelectMenu가 넘겨준 실제 주문받은 데이터를 화면에 표시하게될 슬롯에 부착할 스크립트
    // 판매자와 상호작용하게될 각각의 버튼(슬롯), UI로써 사용자에게 보여지게 될 것. 
    //[SerializeField] private Image imgIcon;// 일단 구상만 하고있음 주문받은메뉴아이콘도 표시해줘야하나?
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textIndex;
    public Image imgIcon;

    public SelectedMenu selectedMenu;
    public void Init(SelectedMenu _selectMenu)
    {
        this.selectedMenu = _selectMenu;

        gameObject.SetActive(true);//생성자를 통해 생성되면 활성화되어 사용자에게 노출됨
        textName.text = selectedMenu.GetName();
        textIndex.text = selectedMenu.GetIndex().ToString();
        //imgIcon = _selectMenu.img;
    }

    //        KioskSystem.single.RemoveSlot(selectedMenu);//이새끼가 지금까지 출력된 번호를 지우는 핵심기능임 얘를다른대로 빼내야함
    public void OnClick()
    {
        //판매자가 메뉴완성하고 슬롯 클릭하면 출력될 메서드 사용자 호출하시겠습니까? || Destroyer
        //SellerMenus.single.
        KioskSystem.single.RemoveSlot(selectedMenu);//그래서 어디로 빼냄? 
        Debug.Log("OnClick Slot Index: "+selectedMenu.GetIndex() );

        gameObject.SetActive(false);

        // Pass MenuData(tihs);
    }

    public void Delete()
    {

    }
}
