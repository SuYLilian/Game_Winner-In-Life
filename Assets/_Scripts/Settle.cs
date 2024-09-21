using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class Settle : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI salaryNum_text, buyStockNum_text, saleStokNum_text, buyHouseNum_text, saleHouseNum_text,
                           passiveIncomeNum_text, expenseNum_text, netIncomeNum_text;
    public Image roleIcon;

    public ActionManager actionManager;

    PhotonView pv;

    public float salaryNum, buyStokNum, saleStokNum, buyHouseNum, saleHouseNum, passiveIncomeNum, expenseNum, netIncomeNum;
    public string[] roleSprites;
    void Awake()
    {
        //gameObject.transform.parent = GameObject.FindGameObjectWithTag("SettleGroup").transform;

        //pv = gameObject.GetComponent<PhotonView>();
        // gameObject.transform.parent = actionManager.settleGroup.transform;
        if (photonView.IsMine)
        {

            actionManager = FindObjectOfType<ActionManager>();
            //actionManager.settlePanel.SetActive(true);
            salaryNum = ((float)System.Math.Round(actionManager.salaryNum_settle, 2));
            buyStokNum = ((float)System.Math.Round(actionManager.buyStockNum_settle, 2));
            saleStokNum = ((float)System.Math.Round(actionManager.saleStokNum_settle, 2));
            buyHouseNum = ((float)System.Math.Round(actionManager.buyHouseNum_settle, 2));
            saleHouseNum = ((float)System.Math.Round(actionManager.saleHouseNum_settle, 2));
            passiveIncomeNum = ((float)System.Math.Round(actionManager.passiveIncomeNum_settle, 2));
            expenseNum = ((float)System.Math.Round(actionManager.expenseNum_settle, 2));
            netIncomeNum = ((float)System.Math.Round(((salaryNum + saleStokNum + saleHouseNum + passiveIncomeNum) - (buyStokNum + buyHouseNum + expenseNum)), 2));
            //gameObject.transform.parent = GameObject.FindGameObjectWithTag("SettleGroup").transform;
            //gameObject.transform.parent = actionManager.settleGroup.transform;
            //gameObject.transform.parent = GameObject.FindGameObjectWithTag("SettleGroup").transform;

            HashTable loadSettle = new HashTable();
            loadSettle.Add("SalarySettle", salaryNum);
            loadSettle.Add("BuyStokSettle", buyStokNum);
            loadSettle.Add("SaleStokSettle", saleStokNum);
            loadSettle.Add("BuyHouseSettle", buyHouseNum);
            loadSettle.Add("SaleHouseSettle", saleHouseNum);
            loadSettle.Add("PassiveIncomeSettle", passiveIncomeNum);
            loadSettle.Add("ExpenseSettle", expenseNum);
            loadSettle.Add("NetIncomeSettle", netIncomeNum);
            loadSettle.Add("RoleSprite", roleSprites[actionManager.jobTypeNum]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(loadSettle);


            //gameObject.SetActive(true);
            actionManager.salaryNum_settle = 0;
            actionManager.buyStockNum_settle = 0;
            actionManager.saleStokNum_settle = 0;
            actionManager.buyHouseNum_settle = 0;
            actionManager.saleHouseNum_settle = 0;
            actionManager.passiveIncomeNum_settle = 0;
            actionManager.expenseNum_settle = 0;
            actionManager.salaryNum_settle = 0;
        }
         //int r = 1;
         //HashTable loadSettle = new HashTable();
         //loadSettle.Add("LoadSettle", r);
         //PhotonNetwork.LocalPlayer.SetCustomProperties(loadSettle);*/
        // }




    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {       
        if (targetPlayer == photonView.Owner)
        {
            if (changedProps.ContainsKey("RoleSprite"))
            {
                string roleSpritePath = (string)changedProps["RoleSprite"];
                roleIcon.sprite= Resources.Load<Sprite>(roleSpritePath);
                //roleIcon.sprite = (Sprite)changedProps["RoleSprite"];
                /*salaryNum_text.text = salaryNum + " K";
                roleIcon.sprite = roleSprites[actionManager.jobTypeNum];*/
            }
            if (changedProps.ContainsKey("SalarySettle"))
            {
                salaryNum = (float)changedProps["SalarySettle"];
                salaryNum_text.text = salaryNum + " K";
                //roleIcon.sprite = roleSprites[actionManager.jobTypeNum];
            }
            if (changedProps.ContainsKey("BuyStokSettle"))
            {
                buyStokNum = (float)changedProps["BuyStokSettle"];
                buyStockNum_text.text = buyStokNum + " K";
            }
            if (changedProps.ContainsKey("SaleStokSettle"))
            {
                saleStokNum = (float)changedProps["SaleStokSettle"];
                saleStokNum_text.text = saleStokNum + " K";
            }
            if (changedProps.ContainsKey("BuyHouseSettle"))
            {
                buyHouseNum = (float)changedProps["BuyHouseSettle"];
                buyHouseNum_text.text = buyHouseNum + " K";
            }
            if (changedProps.ContainsKey("SaleHouseSettle"))
            {
                saleHouseNum = (float)changedProps["SaleHouseSettle"];
                saleHouseNum_text.text = saleHouseNum + " K";
            }
            if (changedProps.ContainsKey("PassiveIncomeSettle"))
            {
                passiveIncomeNum = (float)changedProps["PassiveIncomeSettle"];
                passiveIncomeNum_text.text = passiveIncomeNum + " K";
            }
            if (changedProps.ContainsKey("ExpenseSettle"))
            {
                expenseNum = (float)changedProps["ExpenseSettle"];
                expenseNum_text.text = expenseNum + " K";
            }
            if (changedProps.ContainsKey("NetIncomeSettle"))
            {
                netIncomeNum = (float)changedProps["NetIncomeSettle"];
                netIncomeNum_text.text = netIncomeNum + " K";
                gameObject.SetActive(true);
                
            }

        }


    }
}
