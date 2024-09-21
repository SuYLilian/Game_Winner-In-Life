using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class SettleSetParent : MonoBehaviourPunCallbacks
{
    public string parentTag;
    public ActionManager actionManager;
    void Awake()
    {
        if (photonView.IsMine)
        {
            actionManager = FindObjectOfType<ActionManager>();
            //actionManager = FindObjectOfType<ActionManager>();
        }
        gameObject.transform.parent = actionManager.settleGroup.transform;
    }

    /*public Transform SettingParent()
    {
        Transform parent;
        parent = GameObject.FindGameObjectWithTag(parentTag).transform;
        return parent;
    }*/
}
