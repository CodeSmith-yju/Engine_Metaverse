using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPopupInit : MonoBehaviour
{
    public Button check_Btn;
    public TMP_Text alert_Text;
    Players player;

    public void Init(string text, string tag, Players player)
    {
        check_Btn.onClick.RemoveAllListeners();
        this.alert_Text.text = text;
        this.check_Btn.onClick.AddListener(() => ButtonOnclick(tag));
        this.player = player;
    }


    void ButtonOnclick(string tag)
    {
        switch(tag) 
        {
            case "Mixer":
                Mixer mixer = FindObjectOfType<Mixer>();

                StartCoroutine(mixer.MixerRoutine(mixer));
                /*player.cur_IngrList.Add("믹서기");
                GameMgr.Instance.ui.CupIcon(tag);*/
                break;
            case "Ice":
                player.cur_IngrList.Add("얼음");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Cup":
                player.cup = true;
                if (!GameMgr.Instance.ui.cup_List_BG.activeSelf)
                    GameMgr.Instance.ui.cup_List_BG.SetActive(player.cup);
                break;
            case "Espresso":
                CoffeeMachine coffeemachine = FindObjectOfType<CoffeeMachine>();
                Debug.Log("커피를 내립니다. (15초)");
                coffeemachine.coffee_Check = false;
                coffeemachine.coffee_Icon.gameObject.SetActive(false);

                player.cup = false;

                StartCoroutine(coffeemachine.CoffeeRoutine(coffeemachine));
                break;
            case "Grinder":
                player.coffee = true;
                break;
            case "Dish":
                player.cur_IngrList.Clear();
                GameMgr.Instance.ui.DeleteCupIcon();
                break;
            case "Water_Ice":
                player.cur_IngrList.Add("냉수");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Water_Hot":
                player.cur_IngrList.Add("온수");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Strawberry":
                player.cur_IngrList.Add("딸기");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Milk":
                player.cur_IngrList.Add("우유");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Chocolate":
                player.cur_IngrList.Add("초콜릿");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Yogurt":
                player.cur_IngrList.Add("요거트 파우더");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
        }
        if (GameMgr.Instance.ui.check_Popup.activeSelf)
            GameMgr.Instance.ui.check_Popup.SetActive(false);
    } 

}
