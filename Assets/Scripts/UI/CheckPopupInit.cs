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
                break;
            case "Ice":
                if (player.cur_IngrList.Count >= 5)
                {
                    GameMgr.Instance.ui.OnAlertPopup("�ſ� �� �̻� ��Ḧ \n���� �� �����ϴ�.");
                    if (GameMgr.Instance.ui.check_Popup.activeSelf)
                        GameMgr.Instance.ui.check_Popup.SetActive(false);
                    return;
                }
                player.cur_IngrList.Add("����");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Cup":
                player.cup = true;
                if (!GameMgr.Instance.ui.cup_List_BG.activeSelf)
                    GameMgr.Instance.ui.cup_List_BG.SetActive(player.cup);
                break;
            case "Espresso":
                CoffeeMachine coffeemachine = FindObjectOfType<CoffeeMachine>();
                Debug.Log("Ŀ�Ǹ� �����ϴ�. (15��)");
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
                if (player.cur_IngrList.Count >= 5)
                {
                    GameMgr.Instance.ui.OnAlertPopup("�ſ� �� �̻� ��Ḧ \n���� �� �����ϴ�.");
                    if (GameMgr.Instance.ui.check_Popup.activeSelf)
                        GameMgr.Instance.ui.check_Popup.SetActive(false);
                    return;
                }
                player.cur_IngrList.Add("�ü�");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Water_Hot":
                if (player.cur_IngrList.Count >= 5)
                {
                    GameMgr.Instance.ui.OnAlertPopup("�ſ� �� �̻� ��Ḧ \n���� �� �����ϴ�.");
                    if (GameMgr.Instance.ui.check_Popup.activeSelf)
                        GameMgr.Instance.ui.check_Popup.SetActive(false);
                    return;
                }
                player.cur_IngrList.Add("�¼�");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Strawberry":
                if (player.cur_IngrList.Count >= 5)
                {
                    GameMgr.Instance.ui.OnAlertPopup("�ſ� �� �̻� ��Ḧ \n���� �� �����ϴ�.");
                    if (GameMgr.Instance.ui.check_Popup.activeSelf)
                        GameMgr.Instance.ui.check_Popup.SetActive(false);
                    return;
                }
                player.cur_IngrList.Add("����");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Milk":
                if (player.cur_IngrList.Count >= 5)
                {
                    GameMgr.Instance.ui.OnAlertPopup("�ſ� �� �̻� ��Ḧ \n���� �� �����ϴ�.");
                    if (GameMgr.Instance.ui.check_Popup.activeSelf)
                        GameMgr.Instance.ui.check_Popup.SetActive(false);
                    return;
                }
                player.cur_IngrList.Add("����");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Chocolate":
                if (player.cur_IngrList.Count >= 5)
                {
                    GameMgr.Instance.ui.OnAlertPopup("�ſ� �� �̻� ��Ḧ \n���� �� �����ϴ�.");
                    if (GameMgr.Instance.ui.check_Popup.activeSelf)
                        GameMgr.Instance.ui.check_Popup.SetActive(false);
                    return;
                }
                player.cur_IngrList.Add("���ݸ�");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
            case "Yogurt":
                if (player.cur_IngrList.Count >= 5)
                {
                    GameMgr.Instance.ui.OnAlertPopup("�ſ� �� �̻� ��Ḧ \n���� �� �����ϴ�.");
                    if (GameMgr.Instance.ui.check_Popup.activeSelf)
                        GameMgr.Instance.ui.check_Popup.SetActive(false);
                    return;
                }
                player.cur_IngrList.Add("���Ʈ �Ŀ��");
                GameMgr.Instance.ui.CupIcon(tag);
                break;
        }
        if (GameMgr.Instance.ui.check_Popup.activeSelf)
            GameMgr.Instance.ui.check_Popup.SetActive(false);
    } 

}