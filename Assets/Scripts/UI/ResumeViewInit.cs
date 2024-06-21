using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResumeViewInit : MonoBehaviour
{
    public TMP_Text nickName;
    public Button view_Btn;
    public string gender;
    public string desc;
    public Players player;
    public bool isEmployee;

    [PunRPC]
    public void Init(string name, string gender, string desc, bool isEmployee)
    {
        this.nickName.text = name;
        this.gender = gender;
        this.desc = desc;
        this.isEmployee = isEmployee;
        view_Btn.onClick.AddListener(() => ViewResume());
    }

    [PunRPC]
    public void SetParentAndTransform(int parentViewID)
    {
        PhotonView parentView = PhotonView.Find(parentViewID);
        if (parentView != null)
        {
            Transform parentTransform = parentView.transform;
            transform.SetParent(parentTransform, false);

            // 로컬 위치 및 로컬 스케일 재설정
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }
    }

    [PunRPC]
    public void GetPlayers(int id)
    {
        PhotonView playerView = PhotonView.Find(id);
        Players player_Obj = playerView.gameObject.GetComponent<Players>();
        this.player = player_Obj;
    }


    void ViewResume()
    {
        ResumeView resume = GameMgr.Instance.ui.resume_Info.GetComponent<ResumeView>();

        resume.Init(nickName.text, gender, desc);

        int id = player.gameObject.GetComponent<PhotonView>().ViewID;

        resume.GetComponent<PhotonView>().RPC("GetPlayer", RpcTarget.AllBuffered, id);
        resume.isEmployee = isEmployee;
        resume.gameObject.SetActive(true);
    }

}
