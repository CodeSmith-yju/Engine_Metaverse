using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDispenser : MonoBehaviour
{
    Players player;

    public void Init(Players player)
    {
        this.player = player;
    }

   public void InCupWater(bool temp)
    {
        string tag = "";
        if (temp) 
        {
            tag = "Water_Hot";
            GameMgr.Instance.ui.CheckPopup(tag, player);
        }
        else
        {
            tag = "Water_Ice";
            GameMgr.Instance.ui.CheckPopup(tag, player);
        }
    }
}
