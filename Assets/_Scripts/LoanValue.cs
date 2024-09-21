using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class LoanValue : MonoBehaviour
{
    public TextMeshProUGUI loanNumRecord_text;
    public int loanNumValue;
    public PlayerValue playerValue;
    public GameObject loanGroup;
    public Button repaymentButton;
    public ActionManager actionManager;

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
        actionManager = FindObjectOfType<ActionManager>();
        loanGroup = gameObject.transform.parent.gameObject;
        loanGroup.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 120);
    }

    public void ClickRepaymentButton()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[0]);

        LoanValue[] loanValue_children = loanGroup.GetComponentsInChildren<LoanValue>();
        playerValue.deposit -= loanNumValue;
        playerValue.expenses -= loanNumValue / 100;
        foreach (LoanValue l in loanValue_children)
        {
            if (l.loanNumValue > playerValue.deposit)
            {
                l.repaymentButton.interactable = false;
            }
           
        }       
        playerValue.ChangePlayerValue();
        /*if (playerValue.planSpeed >= 1)
        {
            actionManager.GameOver_Win();
            actionManager.pv_a.RPC("SyncLoadedJobs", RpcTarget.Others);
        }*/
        loanGroup.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, 120);
        actionManager.confirmButton_loan.interactable = true;
        actionManager.plusButton_loan.interactable = true;
        actionManager.reduceButton_loan.interactable = true;
        actionManager.loanLimit += loanNumValue/100;
        Destroy(gameObject);
    }
}
