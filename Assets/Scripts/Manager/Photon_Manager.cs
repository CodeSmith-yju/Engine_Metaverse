using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class Photon_Manager : MonoBehaviourPunCallbacks
{
    public List<GameObject> list_Photon_Prefabs;
    public Transform tf_Respawn_Point;
    public UserData m_LocalPlayer_Data;
    public GameObject obj_LocalPlayer;


    private void Start()
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
        InitPhotonServer();
    }

    public void StartBnt()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Clear();
        if (pool != null && list_Photon_Prefabs != null)
        {
            foreach (GameObject prefab in list_Photon_Prefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }
        InitPhotonServer();
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
        if (m_data.gender == 0)
        {
            Debug.Log("���� ĳ���� ����");
            obj_LocalPlayer = PhotonNetwork.Instantiate(list_Photon_Prefabs[0].name, tf_Respawn_Point.position, Quaternion.identity);
            
        }
        else if (m_data.gender == 1)
        {
            obj_LocalPlayer = PhotonNetwork.Instantiate(list_Photon_Prefabs[1].name, tf_Respawn_Point.position, Quaternion.identity);
            Debug.Log("���� ĳ���� ����");
        }

        


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("�������̹Ƿ� ���� ������ �ο��մϴ�.");
            obj_LocalPlayer.GetComponent<Players>().SetRole(Role.Manager);
        }
        else
        {
            Debug.Log("�������� �ƴϹǷ� �� ������ �ο��մϴ�.");
            obj_LocalPlayer.GetComponent<Players>().SetRole(Role.Customer);
        }
        photonView.RPC("SetPlayerSetting", RpcTarget.AllBuffered, m_data.name, obj_LocalPlayer.GetComponent<PhotonView>().ViewID);
    }


    [PunRPC]
    public void SetPlayerSetting(string name, int player_Id)
    {
        // ��� PhotonView�� ã�Ƽ�
        PhotonView[] views = FindObjectsOfType<PhotonView>();
        foreach (PhotonView view in views)
        {
            // view�� Owner�� ActorNumber�� player_Id�� ��ġ�ϸ�
            if (view.Owner != null && view.ViewID == player_Id)
            {
                // �ش� ������Ʈ�� �̸��� ����
                SetPlayer(name, view.gameObject);
                break; // ã������ �� �̻� �ݺ��� �ʿ� ����
            }
        }
    }

    private void SetPlayer(string name, GameObject player)
    {
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
        //if (!GameMgr.Instance.ui.coin_UI.activeSelf)
            GameMgr.Instance.ui.coin_UI.SetActive(true);
        CreateCharacter(m_LocalPlayer_Data);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Photon : OnJoinRoomFailed returnCode : " + returnCode + ", message : " + message);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Photon : OnLeftRoom");
    }

    ///////////////////�� ���� �ݹ� ��

    ///////////////////�÷��̾� ���� �ݹ� ����
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Photon : OnPlayerEnteredRoom newPlayer : " + newPlayer.UserId);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonView[] views = FindObjectsOfType<PhotonView>();

        foreach (PhotonView view in views)
        {
            if (view.OwnerActorNr == otherPlayer.ActorNumber)
            {
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
            }
        }
    }
    ///////////////////�÷��̾� ���� �ݹ� ��
    ///
/*
    public void ChangeRole()
    {
        List<Player> players = PhotonNetwork.PlayerList.ToList();
        bool hasEmployee = false;

        foreach (Player player in players) 
        {
            int id = player.ActorNumber;

            foreach (GameObject obj in  GameMgr.Instance.player_List)
            {
                Players player_Obj = obj.GetComponent<Players>();
                PhotonView photon_Player = obj.GetComponent<PhotonView>();

                if (player != null && photon_Player.OwnerActorNr == id && player_Obj.GetRole() == Role.Employee)
                {
                    hasEmployee = true;
                    break;
                }
            }
        }

        // Ư�� ������ �����ϴ� �÷��̾ ã�� ������ Ŭ���̾�Ʈ�� ����
        Player newMaster = null;

        if (hasEmployee)
        {
            int num = int.MaxValue;
            foreach (Player player in players)
            {
                foreach (GameObject obj in GameMgr.Instance.player_List)
                {
                    Players player_Obj = obj.GetComponent<Players>();
                    PhotonView photon_Player = obj.GetComponent<PhotonView>();

                    if (player_Obj.GetRole() == Role.Employee && photon_Player.OwnerActorNr < num)
                    {
                        newMaster = player;
                        num = photon_Player.OwnerActorNr;
                    }
                }
            }
        }
        else
        {
            // ��� �÷��̾� �߿��� ���� ���� ���� ������� ������ �ο�
            newMaster = players.OrderBy(p => p.ActorNumber).FirstOrDefault();
        }


        if (newMaster != null) 
        {
            PhotonNetwork.SetMasterClient(newMaster);
            Debug.Log("������ ����");
            if (PhotonNetwork.IsMasterClient)
            {
                ShowClientPopup();
            }
        }
        else
        {
            Debug.Log("�÷��̾ �����ϴ�.");
            return;
        }

    }*/


    public void ShowClientPopup()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!GameMgr.Instance.ui.alert_Popup.activeSelf)
            {
                GameMgr.Instance.ui.OnPopup(GameMgr.Instance.ui.alert_Popup);
                GameMgr.Instance.ui.alert_Popup.GetComponent<AlertInit>().TextInit("���� ������ ������ �����Ͽ� \n���� ������ ������ϴ�.");
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