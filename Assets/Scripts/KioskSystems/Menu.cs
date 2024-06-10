using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    /*public List<Button> Hotmenus = new();
    public List<Button> Icemenus = new();

    public void OnClickPurchase()
    {

    }*/

    public void OnPurchase()
    {
        switch (gameObject.name)
        {
            case "Coffee":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[0];
                KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[0].ToString()];
                //KioskSystem.single.menuName = "김치";
                break;
            case "Smoothie":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[1];
                KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[1].ToString()];
                //KioskSystem.single.menuName = "김치";
                break;
            case "Another":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[2];
                KioskSystem.single.menuSp = ResourceManager.single.menuResource[GameMgr.Instance.recipe.recipe_Name[2].ToString()];
                //KioskSystem.single.menuName = "김치";
                break;
            default:
                break;
        }
        //KioskSystem.single.menuName = gameObject.name;
        KioskSystem.single.kioskBuyPanel.gameObject.SetActive(true);// 얘가지금 구매확정할지 물어보는패널띄우고있는데 얘도 뜯어고쳐야되네 시발
    }
}
