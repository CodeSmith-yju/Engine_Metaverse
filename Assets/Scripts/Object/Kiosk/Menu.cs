using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    int index;

    private void MenuSettings()
    {
        switch (gameObject.name)
        {
            case "IceAmericano":
                index = 0;
                break;
            case "Americano":
                index = 1;
                break;
            case "IceCaffeLatte":
                index = 2;
                break;
            case "IceCaffeMocha":
                index = 3;
                break;
            case "StrawberryLatte":
                index = 4;
                break;
            case "Smoothie":
                index = 5;
                break;
            default:
                Debug.Log("Name not Found Exception");
                break;
        }
    }
    public void OnPurchase()
    {
        MenuSettings();
        KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[index];
        KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[index].ToString()];

        KioskSystem.single.textBuyDesc.text = GameMgr.Instance.recipe.recipe_Name[index] + " 1개 \n 가격: 1코인 \n주문 하시겠습니까?";

        Debug.Log(KioskSystem.single.menuName);
        //KioskSystem.single.menuName = gameObject.name;
        KioskSystem.single.kioskBuyPanel.gameObject.SetActive(true);// 얘가지금 구매확정할지 물어보는패널띄우고있는데 얘도 뜯어고쳐야되네 시발
    }
}
/*
 switch (gameObject.name)
        {
            case "IceAmericano":
                index = 0;
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[0];
                KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[0].ToString()];
                break;
            case "Americano":
                index = 1;
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[1];
                KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[1].ToString()];
                break;
            case "IceCaffeLatte":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[2];
                KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[2].ToString()];
                break;
            case "IceCaffeMocha":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[3];
                KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[3].ToString()];
                break;
            case "StrawberryLatte":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[4];
                KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[4].ToString()];
                break;
            case "Smoothie":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[5];
                KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[5].ToString()];
                break;
            default:
                break;
        }
 */