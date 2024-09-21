using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class HouseValueRecord : MonoBehaviour
{
    public string houseName;
    public float downPayment, housePassiveIncome;
    public int houseArea, houseType;
    public float earningRise = 0;

    // public PhotonView pv_houseRecord;
    // public PlayerValue playerValue_houseRecord;
    public ActionManager actionManager_houseRecord;
    //public GameObject houseType;

    public TextMeshProUGUI downPayment_text, housePassiveIncome_text, houseName_text, houseType_text, houseArea_text, earningRise_text;

    private void Awake()
    {
        /*PlayerValue[] playerValues = FindObjectsOfType<PlayerValue>();
        foreach (PlayerValue p in playerValues)
        {
            if (p.gameObject.GetComponent<PhotonView>().IsMine)
            {
                playerValue_houseRecord = p;
                break;
            }
        }*/
        actionManager_houseRecord = FindObjectOfType<ActionManager>();
    }

    public void ClickHouseRecord()
    {
        actionManager_houseRecord.houseValueRecord = gameObject.GetComponent<HouseValueRecord>();
        actionManager_houseRecord.saleHousePanel.SetActive(true);
    }
}
