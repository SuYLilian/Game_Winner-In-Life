using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if(PhotonNetwork.IsConnected==false)
        {
            SceneManager.LoadScene("StartScene");
        }
        else 
        {
            //if (PhotonNetwork.IsConnected == false)
            //{
                PhotonNetwork.JoinLobby();
            //}
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        OnClickJoinButton();
        Debug.Log("Lobby joined");
    }

    public int maxPlayers = 4;

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    public void OnClickJoinButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("JoinFail!");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room!");
        SceneManager.LoadScene("RoomScene");
    }


}
