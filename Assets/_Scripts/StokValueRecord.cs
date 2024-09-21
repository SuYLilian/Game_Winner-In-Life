using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class StokValueRecord : MonoBehaviour
{
    public string stokName;
    public float stockPrice, dividend;
    public int stokType;
    public int boughtStokNum;
    public float earningRise = 0;

    // public PhotonView pv_stokRecord;
    // public PlayerValue playerValue_stokRecord;
    //public GameObject houseType;
    public ActionManager actionManager_stokRecord;


    public TextMeshProUGUI stockPrice_text, dividend_text, stokName_text, stokType_text, boughtStokNum_text, earningRise_text;

    private void Awake()
    {
        /* PlayerValue[] playerValues = FindObjectsOfType<PlayerValue>();
         foreach (PlayerValue p in playerValues)
         {
             if (p.gameObject.GetComponent<PhotonView>().IsMine)
             {
                 playerValue_stokRecord = p;
                 break;
             }
         }*/
        actionManager_stokRecord = FindObjectOfType<ActionManager>();

    }

    public void ClickStokRecord()
    {
        actionManager_stokRecord.stokValueRecord = gameObject.GetComponent<StokValueRecord>();
        actionManager_stokRecord.saleStokNum_max = boughtStokNum;
        actionManager_stokRecord.saleStokPanel.SetActive(true);
    }
}
