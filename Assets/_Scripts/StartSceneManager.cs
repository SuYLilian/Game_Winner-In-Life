using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;


public class StartSceneManager : MonoBehaviourPunCallbacks
{
    public int maxPlayers = 4;
    public void OnClickStart()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("ClickStart");
        /*if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
            //SceneManager.LoadScene("StartScene");
        }
        else
        {
            if (PhotonNetwork.IsConnected == false)
            {
                PhotonNetwork.JoinLobby();
            }
        }*/
    }
    public void ClickLeave()
    {
        Application.Quit();
    }
    public override void OnConnectedToMaster()
    {
        //PhotonNetwork.JoinLobby();
        Debug.Log("Connected!");
        SceneManager.LoadScene("LobbyScene");
    }
    /*public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Lobby joined");
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
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }*/
}
