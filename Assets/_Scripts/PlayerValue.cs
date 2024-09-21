using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using TMPro;
public class PlayerValue : MonoBehaviourPunCallbacks
{
    public float salary, expenses, cash, deposit, dream, passiveIncome;
    public TextMeshProUGUI passiveIncome_text, expense_text, deposit_text;
    public TextMeshProUGUI monthMoney_text, expenseMoney_text, passiveIcomeMoney_text, depositMoney_text, dreamMoney_text;
    public GameObject gameOverPanel_p, winText_p, loseText_p;

    public float planSpeed;

    public Image planSpeed_mask;

    public PhotonView pv_p;


    //PhotonView pv=PhotonNetwork.LocalPlayer
    public float duration, count;
    
    public float timeCount;
    public int roundTime;
    public bool canCountTim = true;
    //public TextMeshProUGUI countTime_text;

    private void Awake()
    {
        pv_p = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            passiveIncome_text = GameObject.FindGameObjectWithTag("PassiveIncome").GetComponent<TextMeshProUGUI>();
            expense_text = GameObject.FindGameObjectWithTag("Expense").GetComponent<TextMeshProUGUI>();
            deposit_text = GameObject.FindGameObjectWithTag("Deposit").GetComponent<TextMeshProUGUI>();
            //countTime_text = GameObject.FindGameObjectWithTag("CountTime").GetComponent<TextMeshProUGUI>();

            passiveIncome_text.text = "0" + " K";
            expense_text.text = ((float)System.Math.Round(expenses, 2)).ToString() + " K";
            deposit_text.text = ((float)System.Math.Round(deposit, 2)).ToString() + " K";
        }
    }

    private void Update()
    {
        /* if (PhotonNetwork.IsMasterClient && canCountTim)
         {
             count += Time.deltaTime;
             if (count >= 1)
             {
                 timeCount++;
                 countTime_text.text = timeCount.ToString();
                 HashTable countTime_ht = new HashTable();
                 countTime_ht.Add("CountTime", timeCount);
                 PhotonNetwork.LocalPlayer.SetCustomProperties(countTime_ht);
                 if (timeCount >= roundTime)
                 {
                     canCountTim = false;
                 }
                 count = 0;
             }
         }*/
    }

    public void ChangePlayerValue()
    {
        /*if (expenses >= passiveIncome)
        {
            planSpeed = passiveIncome / expenses * 0.5f;
        }
        else
        {
            if (deposit < dream)
            {
                planSpeed = 0.5f + deposit / dream;
            }
            else
            {
                planSpeed = 1f;
            }
        }*/
        planSpeed = passiveIncome / expenses;

        passiveIncome_text.text = ((float)System.Math.Round(passiveIncome,2)).ToString() + " K";
        expense_text.text = ((float)System.Math.Round(expenses, 2)).ToString() + " K";
        deposit_text.text = ((float)System.Math.Round(deposit, 2)).ToString() + " K";
        monthMoney_text.text = ((float)System.Math.Round((salary + passiveIncome - expenses), 2)).ToString() + " K";
        expenseMoney_text.text = ((float)System.Math.Round(expenses, 2)).ToString() + " K";
        passiveIcomeMoney_text.text = ((float)System.Math.Round(passiveIncome, 2)).ToString() + " K";
        depositMoney_text.text = ((float)System.Math.Round(deposit, 2)).ToString() + " K";
        dreamMoney_text.text = ((float)System.Math.Round(dream, 2)).ToString() + " K";
        if (photonView.IsMine)
        {
            HashTable planSpeed_ht = new HashTable();
            planSpeed_ht.Add("PlanSpeed", planSpeed);
            PhotonNetwork.LocalPlayer.SetCustomProperties(planSpeed_ht);
        }

        //planSpeed_mask.fillAmount = planSpeed;

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        if (targetPlayer == photonView.Owner)
        {
            if(changedProps.ContainsKey("PlanSpeed"))
            {
                planSpeed = (float)changedProps["PlanSpeed"];
                planSpeed_mask.fillAmount = planSpeed;
            }
           
        }


    }

}
