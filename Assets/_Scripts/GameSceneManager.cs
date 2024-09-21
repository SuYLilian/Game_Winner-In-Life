using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;




public class GameSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] progressRate = new GameObject[4];
    HashTable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;

    PhotonView pv;

    public int[] loadedJobs=new int[4];

    public GameObject[] roleInfoMoney = new GameObject[4];
    public GameObject roleInfoBackBoard;

    public ActionManager actionManager;

    void Start()
    {
        actionManager = FindObjectOfType<ActionManager>();
        pv = GetComponent<PhotonView>();
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            if(PhotonNetwork.IsMasterClient)
            {
                LoadJobsPlayerPrefs();
            }
        }
    }

    private void Update()
    {
        
    }

    public void SpawnProgressRate()
    {
        //Debug.Log("PlayerProgressRate_" + PhotonNetwork.LocalPlayer.NickName);
        //Debug.Log("PlayerProgressRate_" + DataHolder.instance.customProperties["Job"]);
        //PhotonNetwork.Instantiate("PlayerProgressRate_"+DataHolder.instance.customProperties["Job"], Vector3.zero, Quaternion.identity);

        Debug.Log("PlayerProgressRate_" + loadedJobs[PhotonNetwork.LocalPlayer.ActorNumber - 1]);
        actionManager.jobTypeNum = loadedJobs[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        PhotonNetwork.Instantiate("PlayerProgressRate_" + loadedJobs[PhotonNetwork.LocalPlayer.ActorNumber-1],
        Vector3.zero, Quaternion.identity);
        Instantiate(roleInfoMoney[loadedJobs[PhotonNetwork.LocalPlayer.ActorNumber - 1]], roleInfoBackBoard.transform);
    
    }

    public void LoadJobsPlayerPrefs()
    {
        Debug.Log(PlayerPrefs.GetString("Jobs", "0123"));
        string savedJson = PlayerPrefs.GetString("Jobs", "0123");
        /*int index = 0;
        foreach(char s in savedJson)
        {
            loadedJobs[index] = s;
            index++;
        }*/
        string[] stringArray = savedJson.Split(',');
        for(int i=0;i<4;i++)
        {
            loadedJobs[i] = int.Parse(stringArray[i]);
        }
        pv.RPC("SyncLoadedJobs", RpcTarget.All, loadedJobs);
        //loadedJobs = stringArray.Select(int.Parse).ToArray();
        //loadedJobs = JsonUtility.FromJson<int[]>(savedJson);
        Debug.Log(savedJson);
    }

    [PunRPC]
    private void SyncLoadedJobs(int[] delta)
    {
        // Update the score on all clients
        loadedJobs = delta;
        SpawnProgressRate();
        Debug.Log(loadedJobs);

    }


    
}
