using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResumeView : MonoBehaviour
{
    public TMP_Text nickName;
    public TMP_Text gender;
    public TMP_Text desc;
    public Players player;
    public Button yes_Btn;
    public Button no_Btn;
    public Button fire_Btn;
    public Button master_Btn;
    public bool isEmployee;


    public void Init(string name, string gender, string desc)
    {
        nickName.text = name;
        this.gender.text = gender;
        this.desc.text = desc;
        if (!isEmployee) 
        {
            yes_Btn.gameObject.SetActive(true);
            no_Btn.gameObject.SetActive(true);
            fire_Btn.gameObject.SetActive(false);
            master_Btn.gameObject.SetActive(false);
        }
        else
        {
            yes_Btn.gameObject.SetActive(false);
            no_Btn.gameObject.SetActive(false);
            fire_Btn.gameObject.SetActive(true);
            master_Btn.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    public void GetPlayer(int player_id)
    {
        PhotonView playerView = PhotonView.Find(player_id);
        Players player_Obj = playerView.gameObject.GetComponent<Players>();
        this.player = player_Obj;
    }

    public void NameGenderInit(string name, int gender)
    {
        this.nickName.text = name;
        if (gender == 0)
        {
            this.gender.text = "남자";
        }
        else
        {
            this.gender.text = "여자";
        }
    }


}
