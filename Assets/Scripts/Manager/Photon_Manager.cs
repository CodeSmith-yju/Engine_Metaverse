using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using Photon.Pun.Demo.PunBasics;
using System.Data;

public class Photon_Manager : MonoBehaviourPunCallbacks
{
    public List<GameObject> list_Photon_Prefabs;
    public Transform tf_Respawn_Point;
    public UserData m_LocalPlayer_Data;
    public GameObject obj_LocalPlayer;
    public TMP_InputField name_Input;


    private void Start()
    {
/*        //���⼭ ���濡�� �����ϴ� ������Ʈ�� ���
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Clear();
        if (pool != null && list_Photon_Prefabs != null)
        {
            foreach (GameObject prefab in list_Photon_Prefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }
        InitPhotonServer();*/
    }

    public void ResumeSubmit()
    {
        GameObject resume = PhotonNetwork.Instantiate(GameMgr.Instance.ui.resume_List_Prefabs.name, GameMgr.Instance.ui.resume_List_Pos.transform.position, Quaternion.identity);
        ResumeView resume_desc = GameMgr.Instance.ui.resume_Write_UI.GetComponent<ResumeView>();
        PhotonView data = resume.GetComponent<PhotonView>();
        data.RPC("Init", RpcTarget.AllBuffered, resume_desc.nickName.text, resume_desc.gender.text, resume_desc.desc.text, false);
        ResumeViewInit resumeViewInit = resume.GetComponent<ResumeViewInit>();

        Transform parentTransform = GameMgr.Instance.ui.resume_List_Pos.transform;
        data.RPC("SetParentAndTransform", RpcTarget.AllBuffered, parentTransform.GetComponent<PhotonView>().ViewID);


        foreach (GameObject players in GameMgr.Instance.player_List)
        {
            PhotonView player_View = players.GetComponent<PhotonView>();

            if (player_View != null && player_View.IsMine) 
            {
                data.RPC("GetPlayers", RpcTarget.AllBuffered, player_View.ViewID);
                resumeViewInit.player.resume_Done = true;
            }
        }

        GameMgr.Instance.ui.resume_UI.SetActive(false);
        GameMgr.Instance.ui.OnAlertPopup("�̷¼��� ���� �Ǿ����ϴ�.");
    }

    public void AcceptResume()
    {
        GameObject resume = PhotonNetwork.Instantiate(GameMgr.Instance.ui.resume_List_Prefabs.name, GameMgr.Instance.ui.pos_Crew_List_Pos.transform.position, Quaternion.identity);
        ResumeView resume_desc = GameMgr.Instance.ui.resume_Info.GetComponent<ResumeView>();
        PhotonView data = resume.GetComponent<PhotonView>();
        data.RPC("Init", RpcTarget.AllBuffered, resume_desc.nickName.text, resume_desc.gender.text, resume_desc.desc.text, true);
        ResumeViewInit resumeViewInit = resume.GetComponent<ResumeViewInit>();
        resumeViewInit.player = resume_desc.player;



        foreach (GameObject players in GameMgr.Instance.player_List)
        {
            PhotonView player_View = players.GetComponent<PhotonView>();

            if (resumeViewInit.player.gameObject.GetComponent<PhotonView>().ViewID == player_View.ViewID)
            {
                resumeViewInit.player = players.GetComponent<Players>();
                photonView.RPC("ChangePlayerRole", RpcTarget.AllBuffered, player_View.ViewID, "Employee");
                photonView.RPC("AlertPopup", player_View.Owner, "ä�� �Ǿ����ϴ�.");
            }
        }

        foreach (Transform resume_List in GameMgr.Instance.ui.resume_List_Pos)
        {
            if (resume_List.GetComponent<ResumeViewInit>().player == resumeViewInit.player)
            {
                photonView.RPC("DestroyResumeList", RpcTarget.AllBuffered, resume_List.GetComponent<PhotonView>().ViewID);
            }
        }

        GameMgr.Instance.ui.resume_Info.SetActive(false);
        GameMgr.Instance.ui.accept_Popup.SetActive(false); 

        Transform parentTransform = GameMgr.Instance.ui.pos_Crew_List_Pos.transform;
        data.RPC("SetParentAndTransform", RpcTarget.AllBuffered, parentTransform.GetComponent<PhotonView>().ViewID);
    }

    public void NonAcceptResume()
    {
        foreach (Transform resume_List in GameMgr.Instance.ui.resume_List_Pos.transform)
        {
            ResumeViewInit resumeViewInit = resume_List.gameObject.GetComponent<ResumeViewInit>();
            if (resumeViewInit != null)
            {
                foreach (GameObject players in GameMgr.Instance.player_List)
                {
                    Players player = players.GetComponent<Players>();
                    if (player != null && resumeViewInit.player == player)
                    {
                        PhotonView resumePhotonView = resume_List.GetComponent<PhotonView>();
                        PhotonView playerPhotonView = players.GetComponent<PhotonView>();

                        if (resumePhotonView != null && playerPhotonView != null)
                        {
                            // Destroy the resume list item
                            photonView.RPC("DestroyResumeList", RpcTarget.AllBuffered, resumePhotonView.ViewID);

                            // Set player's resume_Done to false
                            player.resume_Done = false;

                            // Alert the player about the rejection
                            photonView.RPC("AlertPopup", playerPhotonView.Owner, "�̷¼��� �ź� ���ϼ̽��ϴ�.");
                        }
                    }
                }
            }
        }
        GameMgr.Instance.ui.nonAccept_Popup.SetActive(false);
        GameMgr.Instance.ui.resume_Info.SetActive(false);
    }

    public void FireEmployee()
    {
        foreach (Transform crew_List in GameMgr.Instance.ui.pos_Crew_List_Pos.transform)
        {
            ResumeViewInit crewViewInit = crew_List.gameObject.GetComponent<ResumeViewInit>();
            if (crewViewInit != null)
            {
                foreach (GameObject players in GameMgr.Instance.player_List)
                {
                    Players player = players.GetComponent<Players>();
                    PhotonView resume_Player_View = crewViewInit.player.GetComponent<PhotonView>();
                    PhotonView player_View = player.GetComponent<PhotonView>();
                    if (player != null && resume_Player_View.ViewID == player_View.ViewID)
                    {
                        PhotonView crewPhotonView = crew_List.GetComponent<PhotonView>();

                        if (crewPhotonView != null)
                        {
                            photonView.RPC("ChangePlayerRole", RpcTarget.AllBuffered, player_View.ViewID, "Customer");
                            // Set player's resume_Done to false
                            player.resume_Done = false;
                            // Destroy the resume list item
                            photonView.RPC("DestroyResumeList", RpcTarget.AllBuffered, crewPhotonView.ViewID);
                            // Alert the player about the rejection
                            photonView.RPC("AlertPopup", player.GetComponent<PhotonView>().Owner, "�ذ� ���ϼ̽��ϴ�.");
                        }
                    }
                }
            }
        }
        GameMgr.Instance.ui.fire_Popup.SetActive(false);
        GameMgr.Instance.ui.resume_Info.SetActive(false);
    }

    public void MasterAssignment()
    {
        foreach (Transform crew_List in GameMgr.Instance.ui.pos_Crew_List_Pos.transform)
        {
            ResumeViewInit crewViewInit = crew_List.gameObject.GetComponent<ResumeViewInit>();
            if (crewViewInit != null)
            {
                foreach (GameObject players in GameMgr.Instance.player_List)
                {
                    Players player = players.GetComponent<Players>();
                    PhotonView resume_Player_View = crewViewInit.player.GetComponent<PhotonView>();
                    PhotonView player_View = player.GetComponent<PhotonView>();
                    if (player != null && resume_Player_View.ViewID == player_View.ViewID)
                    {
                        PhotonView crewPhotonView = crew_List.GetComponent<PhotonView>();

                        if (crewPhotonView != null)
                        {
                            photonView.RPC("ChangePlayerRole", RpcTarget.AllBuffered, player_View.ViewID, "Manager");
                            // Set player's resume_Done to false
                            player.resume_Done = false;
                            // Destroy the resume list item
                            photonView.RPC("DestroyResumeList", RpcTarget.AllBuffered, crewPhotonView.ViewID);
                            // Alert the player about the rejection
                            photonView.RPC("AlertPopup", player.GetComponent<PhotonView>().Owner, "������ �絵 �����̽��ϴ�.");
                        }
                    }
                }
            }
        }
        GameMgr.Instance.ui.master_Popup.SetActive(false);
        GameMgr.Instance.ui.resume_Info.SetActive(false);
    }


    public void StartBnt()
    {
        //���⼭ ���濡�� �����ϴ� ������Ʈ�� ���
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Clear();
        if (pool != null && list_Photon_Prefabs != null)
        {
            foreach (GameObject prefab in list_Photon_Prefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }
        
        foreach (Transform ui in GameMgr.Instance.ui.pos_Menu_UI_Bg.transform)
        {
            if (ui.gameObject.activeSelf)
                ui.gameObject.SetActive(false);

            GameMgr.Instance.ui.pos_Menu_UI_Bg.SetActive(false);
        }
        GameMgr.Instance.ui.resume_Info.SetActive(false);

        GameMgr.Instance.ui.ui_Connect.SetActive(false);

        InitPhotonServer();
    }

    [PunRPC]
    public void ChangePlayerRole(int playerViewID, string role)
    {
        // �̷¼��� ������ �÷��̾��� PhotonView ID�� ����Ͽ� �÷��̾� ã��
        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView != null)
        {
            Players player = playerView.GetComponent<Players>();
            if (player != null)
            {
                switch (role)
                {
                    case "Customer":
                        player.SetRole(Role.Customer);
                        break;
                    case "Employee":
                        player.SetRole(Role.Employee);
                        break;
                    case "Manager":
                        foreach (Player player_List in PhotonNetwork.PlayerList)
                        {
                            if (playerView.OwnerActorNr == player_List.ActorNumber)
                            {
                                PhotonNetwork.SetMasterClient(player_List);
                                break;
                            }
                        }
                        player.SetRole(Role.Manager);
                        break;
                }
                // �÷��̾� ���� ����
            }
        }
    }

    [PunRPC]
    private void AlertPopup(string text)
    {
        Debug.Log("AlertPopup called with message: " + text);
        GameMgr.Instance.ui.OnAlertPopup(text);
    }

    [PunRPC]
    private void DestroyResumeList(int id)
    {
        Debug.Log("DestroyResumeList called with viewID: " + id);
        PhotonView view = PhotonView.Find(id);
        if (view != null)
        {
            Destroy(view.gameObject);
        }
        else
        {
            Debug.LogWarning("DestroyResumeList: ViewID not found, viewID: " + id);
        }
    }

    public void Quit()
    {
        PhotonNetwork.LeaveRoom();
        Application.Quit();
    }


    //���� ������ ���� ���� �� �κ� ����
    private void InitPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Photon : ConnectUsingSettings");
        }
    }

    //�� ����
    private void JoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom("YJ_Cafe", roomOptions, TypedLobby.Default);
    }

    private void CreateCharacter(UserData m_data)
    {
        m_data.name = name_Input.text;
        if (m_data.gender == 0)
        {
            Debug.Log("���� ĳ���� ����");
            obj_LocalPlayer = PhotonNetwork.Instantiate(list_Photon_Prefabs[0].name, tf_Respawn_Point.position, Quaternion.identity);
            obj_LocalPlayer.GetComponent<Players>().gender = m_data.gender;

        }
        else if (m_data.gender == 1)
        {
            obj_LocalPlayer = PhotonNetwork.Instantiate(list_Photon_Prefabs[1].name, tf_Respawn_Point.position, Quaternion.identity);
            obj_LocalPlayer.GetComponent<Players>().gender = m_data.gender;
            Debug.Log("���� ĳ���� ����");
        }

        


        if (PhotonNetwork.IsMasterClient)
        {
            GameMgr.Instance.ui.OnAlertPopup("�������̹Ƿ� ���� ������ \n�ο��մϴ�.");
            obj_LocalPlayer.GetComponent<Players>().SetRole(Role.Manager);
        }
        else
        {
            GameMgr.Instance.ui.OnAlertPopup("�������� �ƴϹǷ� �� ������ \n�ο��մϴ�.");
            obj_LocalPlayer.GetComponent<Players>().SetRole(Role.Customer);
        }
        photonView.RPC("SetPlayerSetting", RpcTarget.AllBuffered, m_data.name, obj_LocalPlayer.GetComponent<PhotonView>().ViewID, m_data.gender);
    }


    [PunRPC]
    public void SetPlayerSetting(string name, int player_Id, int gender)
    {
        // ��� PhotonView�� ã�Ƽ�
        PhotonView[] views = FindObjectsOfType<PhotonView>();
        foreach (PhotonView view in views)
        {
            // view�� Owner�� ActorNumber�� player_Id�� ��ġ�ϸ�
            if (view.Owner != null && view.ViewID == player_Id)
            {
                // �ش� ������Ʈ�� �̸��� ����
                SetPlayer(name, view.gameObject, gender);
                break; // ã������ �� �̻� �ݺ��� �ʿ� ����
            }
        }
    }

    private void SetPlayer(string name, GameObject player, int gender)
    {
        player.GetComponent<Players>().nickName = name;
        player.GetComponent<Players>().gender = gender;
        player.GetComponent<Character_Controller>().player_Name.text = name;
        if (!GameMgr.Instance.player_List.Contains(player))
        {
            GameMgr.Instance.player_List.Add(player);
        }
    }


    /*// �̸� �����ϴ°�, ����ȭ �Ҷ����� ������ �ٸ� �޼���� �����ߵǴ��� �𸣰���.
    public void SetPlayerName(string name)
    {
        Hashtable customs = new Hashtable
        {
            ["PlayerName"] = name
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(customs);

    }

    // ����ȭ �� ������ ������ �� �޼��� �̿��ؾ��ҵ�, if�� ������ɵ�
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("PlayerName"))
        {
            UpdatePlayerNameUI(targetPlayer);
        }
    }
      
    void UpdatePlayerNameUI(Player targetPlayer) // ���� ���Ͽ����� ����Ǵµ� �����Ϳ��� �ȵ�
    {
        object playerName;
        if (targetPlayer.CustomProperties.TryGetValue("PlayerName", out playerName))
        {
            // player.ActorNumber�� �̿��� �ش� �÷��̾��� GameObject�� ã�Ƽ� UI�� �ݿ�
            GameObject playerObject = GetPlayerGameObject(targetPlayer.ActorNumber);
            if (playerObject != null)
            {
                playerObject.GetComponent<Character_Controller>().player_Name.text = (string)playerName;
            }
        }
    }

    public GameObject GetPlayerGameObject(int actorNumber) // �̰� �̿��ؼ� PlayerList�� ActorNum�� View�� OwnerActorNr�� ���ؼ� �´� ������Ʈ�� ��ȯ
    {
        PhotonView[] views = FindObjectsOfType<PhotonView>();
        foreach (PhotonView view in views)
        {
            if (view != null && view.OwnerActorNr == actorNumber)
            {
                return view.gameObject;
            }
        }
        return null;
    }*/

    public void SelectGender(int gender) 
    {
        m_LocalPlayer_Data.gender = gender;
    }


    public void Respawn()
    {
        if (obj_LocalPlayer.GetComponent<PhotonView>().IsMine)
        {
            obj_LocalPlayer.transform.position = tf_Respawn_Point.position;
        }
    }


    ///////////////////���渶���ͼ��� ���� �ݹ� ����
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon : OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    ///////////////////���渶���ͼ��� ���� �ݹ� ��

    ///////////////////�κ� ���� �ݹ� ����
    public override void OnJoinedLobby()
    {
        Debug.Log("Photon : OnJoinedLobby");
        JoinRoom();
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Photon : OnLeftLobby");
    }

    ///////////////////�κ� ���� �ݹ� ��

    ///////////////////�� ���� �ݹ� ����

    public override void OnCreatedRoom()
    {
        Debug.Log("Photon : OnCreatedRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Photon : OnCreateRoomFailed returnCode : " + returnCode + ", message : " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Photon : OnJoinedRoom");
        CreateCharacter(m_LocalPlayer_Data);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Photon : OnJoinRoomFailed returnCode : " + returnCode + ", message : " + message);
    }

    public override void OnLeftRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RPCChangeRole();
        }
    }

    ///////////////////�� ���� �ݹ� ��

    ///////////////////�÷��̾� ���� �ݹ� ����
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Photon : OnPlayerEnteredRoom newPlayer : " + newPlayer.UserId);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left room: " + otherPlayer.NickName);
        PhotonView[] views = FindObjectsOfType<PhotonView>();

        foreach (PhotonView view in views)
        {
            if (view.OwnerActorNr == otherPlayer.ActorNumber)
            {
                Debug.Log("Destroying player object with ViewID: " + view.ViewID);
                photonView.RPC("DestroyPlayer", RpcTarget.AllBuffered, view.ViewID);
            }
        }
    }

    [PunRPC]
    private void DestroyPlayer(int viewID)
    {
        PhotonView view = PhotonView.Find(viewID);
        
        if (view != null)
        {
            GameObject player = view.gameObject;
            if (GameMgr.Instance.player_List.Contains(player))
            {
                GameMgr.Instance.player_List.Remove(player);
                Destroy(player);
            }
        }
    }
    ///////////////////�÷��̾� ���� �ݹ� ��
    ///

    public void RPCChangeRole()
    {
        photonView.RPC("ChangeRole", RpcTarget.AllBuffered);
    }


    [PunRPC]
    public void ChangeRole()
{
    List<Player> players = PhotonNetwork.PlayerList.ToList();
    Player newMaster = null;

    bool hasEmployee = players.Any(player => {
        int id = player.ActorNumber;
        return GameMgr.Instance.player_List.Any(obj => {
            PhotonView photon_Player = obj.GetComponent<PhotonView>();
            Players player_Obj = obj.GetComponent<Players>();
            return photon_Player.OwnerActorNr == id && player_Obj.GetRole() == Role.Employee;
        });
    });

    if (hasEmployee)
    {
        // ������ Employee�� �÷��̾� �߿��� ���� ���� ActorNumber�� ���� �÷��̾ �� ������ Ŭ���̾�Ʈ�� �����մϴ�.
        newMaster = players.OrderBy(p => p.ActorNumber).FirstOrDefault(player => {
            return GameMgr.Instance.player_List.Any(obj => {
                PhotonView photon_Player = obj.GetComponent<PhotonView>();
                Players player_Obj = obj.GetComponent<Players>();
                return photon_Player.OwnerActorNr == player.ActorNumber && player_Obj.GetRole() == Role.Employee;
            });
        });
    }
    else
    {
        // ��� �÷��̾� �߿��� ���� ���� ActorNumber�� ���� �÷��̾ �� ������ Ŭ���̾�Ʈ�� �����մϴ�.
        newMaster = players.OrderBy(p => p.ActorNumber).FirstOrDefault();
    }

    if (newMaster != null)
    {
        PhotonNetwork.SetMasterClient(newMaster);
        Debug.Log("���ο� ������ Ŭ���̾�Ʈ ����: " + newMaster.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            ShowClientPopup();
        }
    }
    else
    {
        Debug.Log("�÷��̾ �����ϴ�.");
    }
}


    public void ShowClientPopup()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!GameMgr.Instance.ui.alert_Popup.activeSelf)
            {
                GameMgr.Instance.ui.OnAlertPopup("���� ������ ������ �����Ͽ� \n���� ������ ������ϴ�.");
            }
            else if (GameMgr.Instance.ui.alert_Popup.activeSelf)
            {
                GameObject temp_Obj = Instantiate(GameMgr.Instance.ui.alert_Popup, GameMgr.Instance.ui.ui_Main.transform);
                temp_Obj.GetComponent<AlertInit>().TextInit("���� ������ ������ �����Ͽ� \n���� ������ ������ϴ�.");
            }
                
        }
    }

}




[System.Serializable]
public class UserData
{
    /// <summary>
    /// 0 : ��, 1 : ��
    /// </summary>
    public int gender;
    public string name;

    public UserData()
    {
        gender = 0;
        name = "";
    }
}