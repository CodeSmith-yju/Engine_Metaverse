using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    Players player;
    public void Init(Players player)
    {
        this.player = player;
    }

    public void InCupIngr(string name)
    {
        if (player.cup)
        {
            switch (name)
            {
                case "Strawberry":
                    GameMgr.Instance.ui.CheckPopup(name, player);
                    break;
                case "Milk":
                    GameMgr.Instance.ui.CheckPopup(name, player);
                    break;
                case "Chocolate":
                    GameMgr.Instance.ui.CheckPopup(name, player);
                    break;
                case "Yogurt":
                    GameMgr.Instance.ui.CheckPopup(name, player);
                    break;
            }
        }
        else
        {
            GameMgr.Instance.ui.OnAlertPopup("컵을 들고 있어야 합니다.");
        }
        
    }
}
