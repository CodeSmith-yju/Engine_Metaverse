using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void OnPurchase()
    {
        KioskSystem.single.menuName = gameObject.name;
        KioskSystem.single.kioskBuyPanel.gameObject.SetActive(true);
    }
}
