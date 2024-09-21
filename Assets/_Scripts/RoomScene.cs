using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class RoomScene : MonoBehaviourPunCallbacks
{
    public Button startGameButton;
    public int[] jobs = new int[4];
    string stringJobs;
    //List<int> jobs = new List<int> { 0, 1, 2, 3 };
    private bool roomClosed = false;


    Player localPlayer = PhotonNetwork.LocalPlayer;

    public GameObject[] readyPlayers;
    public GameObject tutorialPanel, settingPanel;

    public Slider bgmSlider, sfSlider;

    public GameObject rightButton_tutorial, leftButton_tutorial;
    public GameObject[] tutorialImages;
    public int tutorialNum;
    private void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        startGameButton.interactable = PhotonNetwork.IsMasterClient;
        
        if(PhotonNetwork.IsMasterClient)
        {
            RandomJob();

        }
        UpdatePlayerList();
        bgmSlider.value = AudioManager.instance.bgmAudio.volume;
        sfSlider.value = AudioManager.instance.sfAudio.volume;

    }

    public void Slider_BGM()
    {
        AudioManager.instance.bgmAudio.volume = bgmSlider.value;
    }
    public void Slider_SF()
    {
        AudioManager.instance.sfAudio.volume = sfSlider.value;
    }
    public void ClickSettingButton()
    {
        settingPanel.SetActive(true);
    }
    public void ClickCloseButton_Setting()
    {
        settingPanel.SetActive(false);
    }
    #region
    public void ClickTutorialButton()
    {
        tutorialPanel.SetActive(true);
    }
    public void ClickCloseButton_Tutorial()
    {
        tutorialPanel.SetActive(false);
        tutorialImages[tutorialNum].SetActive(false);
        tutorialNum = 0;
        tutorialImages[tutorialNum].SetActive(true);
        leftButton_tutorial.SetActive(false);
        rightButton_tutorial.SetActive(true);
    }
    public void ClickLeftButton_Tutorial()
    {
        if (tutorialNum > 0)
        {
            tutorialNum--;
            tutorialImages[tutorialNum].SetActive(true);
            tutorialImages[tutorialNum + 1].SetActive(false);
            if (tutorialNum == 0)
            {
                leftButton_tutorial.SetActive(false);
            }
            else if (tutorialNum == 2)
            {
                rightButton_tutorial.SetActive(true);
            }
        }
    }
    public void ClickRightButton_Tutorial()
    {
        if (tutorialNum < 3)
        {
            tutorialNum++;
            tutorialImages[tutorialNum].SetActive(true);
            tutorialImages[tutorialNum - 1].SetActive(false);
            if (tutorialNum == 3)
            {
                rightButton_tutorial.SetActive(false);
            }
            else if (tutorialNum == 1)
            {
                leftButton_tutorial.SetActive(true);
            }
        }
    }
    #endregion
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.interactable = PhotonNetwork.IsMasterClient;
        if(PhotonNetwork.IsMasterClient)
        {
            RandomJob();

        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        //int _job = RandomJobs();
        //jobs.Remove(_job);

        /*if(newPlayer==localPlayer)
        {*/
            //DataHolder.instance.customProperties.Add("Job", _job);


            //localPlayer.NickName=_job.ToString();
        //}
       // Debug.Log(newPlayer.NickName);

    }

    public void RandomJob()
    {
        stringJobs = "";

        for(int i=0;i<jobs.Length;i++)
        {
            int randomIndex = Random.Range(0, jobs.Length);
            jobs[i] = randomIndex;
            for(int j=0;j<i;j++)
            {
                while(jobs[i]==jobs[j])
                {
                    j = 0;
                    randomIndex = Random.Range(0, jobs.Length);
                    jobs[i] = randomIndex;
                }
            }
        }

        for(int i=0;i<jobs.Length;i++)
        {
            stringJobs += jobs[i].ToString()+",";
        }

        //string linkString
            

        //json = JsonUtility.ToJson(jobs);
        PlayerPrefs.SetString("Jobs", stringJobs);
        PlayerPrefs.Save();
        //Debug.Log(json);
        Debug.Log(PlayerPrefs.GetString("Jobs"));


    }

    public void OnClickStartGame()
    {
        if (!roomClosed)
        {
            // Close the room to prevent new players from joining
            PhotonNetwork.CurrentRoom.IsOpen = false;
            roomClosed = true;

            // Implement your game start logic here
            Debug.Log("Game started!");
        }
        SceneManager.LoadScene("GameScene");

    }

    public void UpdatePlayerList()
    {
        for(int i=0;i<readyPlayers.Length;i++)
        {
            readyPlayers[i].SetActive(false);
        }
        for(int i=0;i< PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            readyPlayers[i].SetActive(true);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    public void OnClickLeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        //SceneManager.LoadScene("LobbyScene");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("StartScene");
        //Debug.Log("Disconnected from Photon server. Cause: " + cause);
    }
}
