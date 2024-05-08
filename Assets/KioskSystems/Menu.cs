using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void OnPurchase()
    {
        switch (gameObject.name)
        {
            case "Coffee":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[0];
                //KioskSystem.single.menuName = "±èÄ¡";
                break;
            case "Smoothie":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[1];
                //KioskSystem.single.menuName = "±èÄ¡";
                break;
            case "Another":
                KioskSystem.single.menuName = GameMgr.Instance.recipe.recipe_Name[2];
                //KioskSystem.single.menuName = "±èÄ¡";
                break;
            default:
                break;
        }
        //KioskSystem.single.menuName = gameObject.name;
        KioskSystem.single.kioskBuyPanel.gameObject.SetActive(true);
    }
}
