using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoleInfoManager : MonoBehaviour
{
    //public ActionManager actionManager;
    public PlayerValue playerValue;
    public TextMeshProUGUI monthMoney, expenseMoney, passiveIcomeMoney, depositMoney, dreamMoney;

    private void Awake()
    {
        PlayerValue[] playerValues = FindObjectsOfType<PlayerValue>();
        foreach (PlayerValue p in playerValues)
        {
            if (p.gameObject.GetComponent<PhotonView>().IsMine)
            {
                playerValue = p;
                break;
            }
        }
        playerValue = FindObjectOfType<PlayerValue>();
        playerValue.monthMoney_text = monthMoney;
        playerValue.expenseMoney_text = expenseMoney;
        playerValue.passiveIcomeMoney_text = passiveIcomeMoney;
        playerValue.depositMoney_text = depositMoney;
        playerValue.dreamMoney_text = dreamMoney;
    }
}
