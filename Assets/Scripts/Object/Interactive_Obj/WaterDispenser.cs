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

   /*public void InCupWater(bool temp)
    {
        if (temp) 
        {
            player.cur_IngrList.Add("�¼�");
        }
        else
        {
            player.cur_IngrList.Add("�ü�");
        }
    }*/
}
