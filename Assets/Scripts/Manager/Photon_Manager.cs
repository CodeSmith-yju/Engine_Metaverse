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
        //여기서 포톤에서 생성하는 오브젝트를 등록
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


    //포톤 마스터 서버 접속 및 로비 입장
    private void InitPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Photon : ConnectUsingSettings");
        }
    }

    //방 입장
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
            Debug.Log("남자 캐릭터 생성");
            obj_LocalPlayer = PhotonNetwork.Instantiate(list_Photon_Prefabs[0].name, tf_Respawn_Point.position, Quaternion.identity);
            
        }
        else if (m_data.gender == 1)
        {
            obj_LocalPlayer = PhotonNetwork.Instantiate(list_Photon_Prefabs[1].name, tf_Respawn_Point.position, Quaternion.identity);
            Debug.Log("여자 캐릭터 생성");
        }

        


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("서버장이므로 점장 역할을 부여합니다.");
            obj_LocalPlayer.GetComponent<Players>().SetRole(Role.Manager);
        }
        else
        {
            Debug.Log("서버장이 아니므로 고객 역할을 부여합니다.");
            obj_LocalPlayer.GetComponent<Players>().SetRole(Role.Customer);
        }
        photonView.RPC("SetPlayerSetting", RpcTarget.AllBuffered, m_data.name, obj_LocalPlayer.GetComponent<PhotonView>().ViewID);
    }


    [PunRPC]
    public void SetPlayerSetting(string name, int player_Id)
    {
        // 모든 PhotonView를 찾아서
        PhotonView[] views = FindObjectsOfType<PhotonView>();
        foreach (PhotonView view in views)
        {
            // view의 Owner의 ActorNumber가 player_Id와 일치하면
            if (view.Owner != null && view.ViewID == player_Id)
            {
                // 해당 오브젝트의 이름을 설정
                SetPlayer(name, view.gameObject);
                break; // 찾았으니 더 이상 반복할 필요 없음
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


    /*// 이름 설정하는거, 동기화 할때마다 일일히 다른 메서드로 만들어야되는지 모르겠음.
    public void SetPlayerName(string name)
    {
        Hashtable customs = new Hashtable
        {
            ["PlayerName"] = name
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(customs);

    }

    // 동기화 할 데이터 있으면 이 메서드 이용해야할듯, if문 넣으면될듯
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("PlayerName"))
        {
            UpdatePlayerNameUI(targetPlayer);
        }
    }
      
    void UpdatePlayerNameUI(Player targetPlayer) // 빌드 파일에서는 적용되는데 에디터에선 안됨
    {
        object playerName;
        if (targetPlayer.CustomProperties.TryGetValue("PlayerName", out playerName))
        {
            // player.ActorNumber를 이용해 해당 플레이어의 GameObject를 찾아서 UI에 반영
            GameObject playerObject = GetPlayerGameObject(targetPlayer.ActorNumber);
            if (playerObject != null)
            {
                playerObject.GetComponent<Character_Controller>().player_Name.text = (string)playerName;
            }
        }
    }

    public GameObject GetPlayerGameObject(int actorNumber) // 이걸 이용해서 PlayerList의 ActorNum와 View의 OwnerActorNr를 비교해서 맞는 오브젝트를 반환
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


    ///////////////////포톤마스터서버 관련 콜백 시작
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon : OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    ///////////////////포톤마스터서버 관련 콜백 끝

    ///////////////////로비 관련 콜백 시작
    public override void OnJoinedLobby()
    {
        Debug.Log("Photon : OnJoinedLobby");
        JoinRoom();
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Photon : OnLeftLobby");
    }

    ///////////////////로비 관련 콜백 끝

    ///////////////////룸 관련 콜백 시작

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

    ///////////////////룸 관련 콜백 끝

    ///////////////////플레이어 관련 콜백 시작
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
    ///////////////////플레이어 관련 콜백 끝
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

        // 특정 조건을 만족하는 플레이어를 찾아 마스터 클라이언트로 설정
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
            // 모든 플레이어 중에서 가장 먼저 들어온 사람에게 권한을 부여
            newMaster = players.OrderBy(p => p.ActorNumber).FirstOrDefault();
        }


        if (newMaster != null) 
        {
            PhotonNetwork.SetMasterClient(newMaster);
            Debug.Log("마스터 설정");
            if (PhotonNetwork.IsMasterClient)
            {
                ShowClientPopup();
            }
        }
        else
        {
            Debug.Log("플레이어가 없습니다.");
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
                GameMgr.Instance.ui.alert_Popup.GetComponent<AlertInit>().TextInit("기존 점장이 접속을 종료하여 \n점장 권한을 얻었습니다.");
            }
            else if (GameMgr.Instance.ui.alert_Popup.activeSelf)
            {
                GameObject temp_Obj = Instantiate(GameMgr.Instance.ui.alert_Popup, GameMgr.Instance.ui.ui_Main.transform);
                temp_Obj.GetComponent<AlertInit>().TextInit("기존 점장이 접속을 종료하여 \n점장 권한을 얻었습니다.");
            }
                
        }
    }

}




[System.Serializable]
public class UserData
{
    /// <summary>
    /// 0 : 남, 1 : 여
    /// </summary>
    public int gender;
    public string name;

    public UserData()
    {
        gender = 0;
        name = "";
    }
}