using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDispenser : MonoBehaviour
{
    Interactive interactive;
    Players player;

    public void Init(Interactive obj, Players player)
    {
        this.interactive = obj;
        this.player = player;
    }

    public void InCupWater(bool temp)
    {
        if (temp) 
        {
            player.cur_IngrList.Add("�¼�");
            interactive.Cup_Icon("Water_Hot");
        }
        else
        {
            player.cur_IngrList.Add("�ü�");
            interactive.Cup_Icon("Water_Ice");
        }
    }
}
