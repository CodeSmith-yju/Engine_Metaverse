using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    Interactive interactive;
    Players player;
    public void Init(Interactive obj, Players player)
    {
        this.interactive = obj;
        this.player = player;
    }

    /*public void InCupIngr(string name)
    {
        switch (name)
        {
            case "Strawberry":
                player.cur_IngrList.Add("딸기");
                interactive.Cup_Icon(tag);
                break;
            case "Milk":
                player.cur_IngrList.Add("우유");
                interactive.Cup_Icon(tag);
                break;
            case "Chocolate":
                player.cur_IngrList.Add("초콜릿");
                interactive.Cup_Icon(tag);
                break;
            case "Yogurt":
                player.cur_IngrList.Add("요거트 파우더");
                interactive.Cup_Icon(tag);
                break;
        }
    }*/
}
