using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class ActionManager : MonoBehaviourPunCallbacks
{
    public HouseValue house;
    public StokValue stok;
    public GameObject housePanel, houseGroup, stokPanel, stokGroup, stokNumBackBoard, stokTypeBackBoard, eventPanel, informationPanel, informationBackBoard;
    public GameObject loanPanel, loanRecoard, loanGroup, ownHouseRecord, ownHouseGroup, insuranceBackBoard, savingInsurancePanel;
    public GameObject gameOverPanel, winText, loseText, ownStokRecord, ownStokGroup, saleHousePanel, saleStokPanel;
    public TextMeshProUGUI stokName_stokNum, stokPrice_stokNum, stokDividend_stokNum, stokNum_text, eventContent_text, eventMoney_text;
    public TextMeshProUGUI loanNum_text, loanInterest_text, savingNum_text, savingInterest_text, saleStokNum_text, month_text;
    //public TextMeshProUGUI monthMoney_text, expenseMoney_text, passiveIcomeMoney_text, depositMoney_text, dreamMoney_text;

    public PlayerValue playerValue;

    public Button confirmButton_house, confirmButton_stok, confirmButton_saving, confirmButton_insurance, confirmButton_loan;
    public Button houseAtionButton, stokActionButton, eventActionButton, savingActionButton, passActionButton;
    public Canvas roleInfoCanvas, ownHoseCanvas, loanCanvas, stokCanvas;

    //public bool haveSelectedHouse=false, haveSelectedStok=false;
    public ActionType actionType;

    public bool isPunish, isClickedMedical = false, isClickedTravel = false, haveMedical, haveTravel, haveSaving;
    public float eventMoney;
    public int stoknum, loanNum, savingNum, savingMoneyValue, savingInterestValue, expenseInsuranceValue, savingMonthNum;
    public string[] eventContent;
    public Vector2[] eventValueRandom;

    public int monthNum;//*
    public Image medicalType, travelType;
    public Image[] month_Image;
    public Sprite[] month_haveActed_sprite;
    public Sprite[] month_noAct_sprite;

    public PhotonView pv_a;

    bool isFirstEnterGame = true;

    public float count, count_settle;
    public int timeCount, timeCount_settle;
    public int roundTime;
    public int watchSettleTime;
    public bool canCountTime = true, canCountTime_settle=false;
    public TextMeshProUGUI countTime_text, newsContent_text;

    public string[] monthNewsContent;
    public GameObject[] newsContent_prefab, storyNewsContent_prefab;
    public GameObject newsContentParent, storyNewsContentParent;
    int channelNum = 0;
    public Button turnChannelButton_right, turnChannelButton_left;
    public int[] monthNewsNum;
    public GameObject[] storyNews;
    public int monthNewsNumRecord;

    public HouseValueRecord houseValueRecord;
    public StokValueRecord stokValueRecord;
    public int saleStokNum, saleStokNum_max;
    public string[] monthWord;

    public int jobTypeNum;
    public GameObject settlePanel;
    public GameObject settleGroup;
    public GameObject settle_prefab;
    public float salaryNum_settle, buyStockNum_settle, saleStokNum_settle, buyHouseNum_settle, saleHouseNum_settle, passiveIncomeNum_settle,
                 expenseNum_settle, netIncomeNum_settle;
    public Sprite[] roleSprites;

    public float[] playersScores;
    public int[] playerNum;

    public GameObject buyScrollView_stok, saleScrollView_stok, buyScrollView_house, saleScrollView_house, confirmBuyHousePanel;
    public Button buyButton_stok, saleButton_stok, buyButton_house, saleButton_house;
    Color disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
    Color normalColor = new Color(1f, 1f, 1f, 1f);

    public int finishActionNum, clickSettleCloseButtonNum;
    public GameObject tutorialPanel, settingPanel;
    public Button savingLable, loanLable, loanRecordLable, plusButton_saving, reduceButton_saving, plusButton_loan, reduceButton_loan;
    public int loanLimit=3;

    public GameObject savingBackBoard, loanBackBoard, loanRecordBackBoard;
    public Animator notEnoughCash_ani;

    public Slider bgmSlider, sfSlider;

    public GameObject rightButton_tutorial, leftButton_tutorial;
    public GameObject[] tutorialImages;
    public int tutorialNum;
    private void Start()
    {
        timeCount = roundTime;
        countTime_text.text = timeCount.ToString();
        pv_a = GetComponent<PhotonView>();
        PlayerValue[] playerValues = FindObjectsOfType<PlayerValue>();
        foreach (PlayerValue p in playerValues)
        {
            if (p.gameObject.GetComponent<PhotonView>().IsMine)
            {
                playerValue = p;
                break;
            }
        }
        /*if(playerValue.deposit<savingNum*1000000)
        {
            confirmButton_saving.interactable = false;
        }*/
        confirmButton_insurance.interactable = false;
        confirmButton_house.interactable = false;
        confirmButton_stok.interactable = false;
        if (PhotonNetwork.IsMasterClient)
        {

            monthNewsNum = new int[12];
            for (int i = 0; i < monthNewsNum.Length; i++)
            {
                monthNewsNum[i] = Random.Range(0, monthNewsNum.Length);
                for (int j = 0; j < i; j++)
                {
                    while (monthNewsNum[i] == monthNewsNum[j])
                    {
                        j = 0;
                        monthNewsNum[i] = Random.Range(0, monthNewsNum.Length);
                    }
                }
            }
            pv_a.RPC("TurnNewsContent", RpcTarget.All, monthNewsNum);
            pv_a.RPC("SyncTimer", RpcTarget.All, timeCount);

        }

        bgmSlider.value = AudioManager.instance.bgmAudio.volume;
        sfSlider.value = AudioManager.instance.sfAudio.volume;

    }

    private void Update()
    {

        if (canCountTime)
        {
            count += Time.deltaTime;
            if (count >= 1)
            {
                timeCount--;
                countTime_text.text = timeCount.ToString();
                /*HashTable countTime_ht = new HashTable();
                countTime_ht.Add("CountTime", timeCount);
                PhotonNetwork.LocalPlayer.SetCustomProperties(countTime_ht);*/
                if (timeCount <= 0)
                {
                    canCountTime = false;
                    canCountTime_settle = true;

                    // ClickCancleButton_Insurance();
                    ClickCancleButton_Loan();
                    ClickCancleButton_ConfirmBuyHouse();
                    ClickCancleButton_SaleHouse();
                    ClickCancleButton_SaleStok();
                    ClickCancleButton_Saving();
                    ClickCancleButton_StokNum();
                    ClickCloseButton_Tutorial();
                    ClickCancle_House();
                    ClickCancle_Stok();
                    ClickCloseInfoButton();
                    eventPanel.SetActive(false);
                    TurnHouseStokValue();
                    timeCount = roundTime;
                    stokActionButton.interactable = false;
                    houseAtionButton.interactable = false;
                    passActionButton.interactable = false;
                    savingActionButton.interactable = false;
                }
                else if (finishActionNum >= PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    canCountTime = false;
                    canCountTime_settle = true;

                    countTime_text.text = "0";
                    ClickCancleButton_Loan();
                    ClickCancleButton_ConfirmBuyHouse();
                    ClickCancleButton_SaleHouse();
                    ClickCancleButton_SaleStok();
                    ClickCancleButton_Saving();
                    ClickCancleButton_StokNum();
                    ClickCloseButton_Tutorial();
                    ClickCancle_House();
                    ClickCancle_Stok();
                    ClickCloseInfoButton();
                    eventPanel.SetActive(false);
                    TurnHouseStokValue();
                    timeCount = roundTime;
                    stokActionButton.interactable = false;
                    houseAtionButton.interactable = false;
                    passActionButton.interactable = false;
                    savingActionButton.interactable = false;
                    count = 0;

                }
                count = 0;
            }
           
        }
        if (canCountTime_settle)
        {
            count_settle += Time.deltaTime;
            if (count_settle >= 1)
            {
                timeCount_settle--;

                if (timeCount_settle <= 0)
                {
                    canCountTime_settle = false;
                    EnterToNextMonth();
                    
                }
                else if (clickSettleCloseButtonNum >= PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    canCountTime_settle = false;
                    EnterToNextMonth();
                }
                count_settle = 0;
                //countTime_text.text = timeCount.ToString();
            }
           
        }
    }

    #region Setting
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
    #endregion

    public void ClickTurnChannelButton(bool isRightButton)
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[1]);
        if (isRightButton && channelNum < 1)
        {
            channelNum++;
            turnChannelButton_right.interactable = false;
            turnChannelButton_left.interactable = true;
            newsContentParent.SetActive(true);
            storyNewsContentParent.SetActive(false);
        }
        else if (!isRightButton && channelNum > 0)
        {
            channelNum--;
            turnChannelButton_left.interactable = false;
            turnChannelButton_right.interactable = true;
            newsContentParent.SetActive(false);
            storyNewsContentParent.SetActive(true);
        }
    }

    #region Settle
    public void ShowSettle()
    {
        //Instantiate(settle_prefab, Vector3.zero, Quaternion.identity);
        settlePanel.SetActive(true);
        PhotonNetwork.Instantiate("Settle", Vector3.zero, Quaternion.identity);

        //_settle.transform.parent = settleGroup.transform;
        /*_settle.SetActive(true);
        _settle.GetComponent<Settle>().salaryNum_text.text = salaryNum_settle+" K";
        _settle.GetComponent<Settle>().buyStockNum_text.text = buyStockNum_settle + " K";
        _settle.GetComponent<Settle>().saleStokNum_text.text = saleStokNum_settle + " K";
        _settle.GetComponent<Settle>().buyHouseNum_text.text = buyHouseNum_settle + " K";
        _settle.GetComponent<Settle>().saleHouseNum_text.text = saleHouseNum_settle + " K";
        _settle.GetComponent<Settle>().passiveIncomeNum_text.text = passiveIncomeNum_settle + " K";
        _settle.GetComponent<Settle>().expenseNum_text.text = expenseNum_settle + " K";
        _settle.GetComponent<Settle>().netIncomeNum_text.text = ((salaryNum_settle + saleStokNum_settle + saleHouseNum_settle + passiveIncomeNum_settle) -
                                                                (buyStockNum_settle + buyHouseNum_settle + expenseNum_settle)) + " K";
        _settle.GetComponent<Settle>().roleIcon.sprite = roleSprites[jobTypeNum];
        settlePanel.SetActive(true);
        salaryNum_settle = 0;
        buyStockNum_settle = 0;
        saleStokNum_settle = 0;
        buyHouseNum_settle = 0;
        saleHouseNum_settle = 0;
        passiveIncomeNum_settle = 0;
        expenseNum_settle = 0;
        salaryNum_settle = 0;*/
    }
    public void SetSettleValue()
    {

    }
    public void ClickCloseButton_Settle()
    {
        pv_a.RPC("ClickSettleCloseButton", RpcTarget.All);
        settlePanel.SetActive(false);
    }
    #endregion

    public enum ActionType
    {
        House,
        Stok,
        Event
    }
    #region Tutorial
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
        if(tutorialNum>0)
        {
            tutorialNum--;
            tutorialImages[tutorialNum].SetActive(true);
            tutorialImages[tutorialNum + 1].SetActive(false);
            if(tutorialNum==0)
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
            else if(tutorialNum == 1)
            {
                leftButton_tutorial.SetActive(true);
            }
        }
    }
    #endregion

    #region House
    public void ClickBuyButton_House()
    {

        buyScrollView_house.SetActive(true);
        saleScrollView_house.SetActive(false);
        buyButton_house.enabled = false;
        saleButton_house.enabled = true;
        saleButton_house.gameObject.GetComponent<Image>().color = disabledColor;
        buyButton_house.gameObject.GetComponent<Image>().color = normalColor;

        //buyButton_house.interactable = false;
        //saleButton_house.interactable = true;
    }
    public void ClickSaleButton_House()
    {
        buyScrollView_house.SetActive(false);
        saleScrollView_house.SetActive(true);
        buyButton_house.enabled = true;
        saleButton_house.enabled = false;
        saleButton_house.gameObject.GetComponent<Image>().color = normalColor;
        buyButton_house.gameObject.GetComponent<Image>().color = disabledColor;
        //buyButton_house.interactable = true;
        //saleButton_house.interactable = false;
    }
    public void ClickHouseAction()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[1]);

        if (playerValue == null)
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
        }
        housePanel.SetActive(true);
    }

    public void ClickCancle_House()
    {
        housePanel.SetActive(false);
        /*for (int i = 0; i < houseGroup.transform.childCount; i++)
        {
            houseGroup.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
        }*/
        //haveSelectedHouse = false;
        //confirmButton_house.interactable = false;

    }

    public void ClickHouseType(HouseValue _house)
    {
        house = _house;
        confirmBuyHousePanel.SetActive(true);
        /*for (int i = 0; i < houseGroup.transform.childCount; i++)
        {
            houseGroup.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
        }*/
        //house.gameObject.GetComponent<Button>().interactable = false;
        //if (confirmButton_house.interactable == false)
        //{
            //haveSelectedHouse = true;
            //confirmButton_house.interactable = true;
       // }


    }
   /* public void ClickBuyHouseButton()
    {

    }*/
   public void ClickCancleButton_ConfirmBuyHouse()
    {
        confirmBuyHousePanel.SetActive(false);
    }
    public void ClickConfirmButton_House()
    {
        //house.GetComponent<HouseValue>()
        confirmBuyHousePanel.SetActive(false);
       // housePanel.SetActive(false);
        if (playerValue.deposit >= house.downPayment)
        {
            AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[0]);

            housePanel.SetActive(false);
            houseAtionButton.interactable = false;
            stokActionButton.interactable = false;
            //passActionButton.interactable = false;
            eventActionButton.interactable = false;

            //month_Image[monthNum].sprite = month_haveActed_sprite[monthNum];
            //monthNum++;
            playerValue.deposit -= house.downPayment;
            playerValue.passiveIncome += house.housePassiveIncome;
            buyHouseNum_settle = house.downPayment;
            //playerValue.deposit += (playerValue.salary + playerValue.passiveIncome - playerValue.expenses);
            for (int i = 0; i < houseGroup.transform.childCount; i++)
            {
                houseGroup.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
            }
            playerValue.ChangePlayerValue();
            /*if (monthNum >= 11)
            {

                if (haveSaving)
                {
                    playerValue.deposit += savingInterestValue + savingMoneyValue;
                }
                if (playerValue.deposit < savingNum * 1000)
                {
                    confirmButton_saving.interactable = false;
                }
                playerValue.ChangePlayerValue();
                savingInsurancePanel.SetActive(true);

            }
            if (playerValue.planSpeed >= 1)
            {
                GameOver_Win();
                //pv_a.RPC("SyncLoadedJobs", RpcTarget.Others);
            }
            monthNum++;*/
           // confirmButton_house.interactable = false;
            GameObject _ownHouse = Instantiate(ownHouseRecord, ownHouseGroup.transform);
            _ownHouse.GetComponent<HouseValueRecord>().houseName = house.houseName;
            _ownHouse.GetComponent<HouseValueRecord>().houseArea = house.houseArea;
            _ownHouse.GetComponent<HouseValueRecord>().houseType = house.houseType;
            _ownHouse.GetComponent<HouseValueRecord>().downPayment = house.downPayment;
            _ownHouse.GetComponent<HouseValueRecord>().housePassiveIncome = house.housePassiveIncome;
            _ownHouse.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(_ownHouse.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
            _ownHouse.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(_ownHouse.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
            _ownHouse.GetComponent<HouseValueRecord>().houseName_text.text = _ownHouse.GetComponent<HouseValueRecord>().houseName;
            _ownHouse.GetComponent<HouseValueRecord>().houseArea_text.text = house.houseArea_text.text;
            _ownHouse.GetComponent<HouseValueRecord>().houseType_text.text = house.houseType_text.text;

            // _ownHouse.GetComponent<HouseValue>().rooomNum_text.text = _ownHouse.GetComponent<HouseValue>().roomNum;

            ownHouseGroup.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 425f + 20);
            //pv_a.RPC("FinishAction", RpcTarget.All);
        }
        else  //向銀行貸款，貸款完要回到房地產業面or重新選擇房地產行動
        {
            /*for (int i = 0; i < houseGroup.transform.childCount; i++)
            {
                houseGroup.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
            }*/
            actionType = ActionType.House;
            //loanPanel.SetActive(true);
            confirmBuyHousePanel.SetActive(false);
            notEnoughCash_ani.SetTrigger("NotEnoughCash");
            //housePanel.SetActive(false);
            Debug.Log("No money");
        }


    }
    #endregion

    #region Stok
    public void ClickBuyButton_Stok()
    {
        buyScrollView_stok.SetActive(true);
        saleScrollView_stok.SetActive(false);
        buyButton_stok.enabled = false;
        saleButton_stok.enabled = true;
        saleButton_stok.gameObject.GetComponent<Image>().color = disabledColor;
        buyButton_stok.gameObject.GetComponent<Image>().color = normalColor;
        //buyButton_stok.interactable = false;
        //saleButton_stok.interactable = true;
    }
    public void ClickSaleButton_Stok()
    {
        buyScrollView_stok.SetActive(false);
        saleScrollView_stok.SetActive(true);
        buyButton_stok.enabled = true;
        saleButton_stok.enabled = false;
        saleButton_stok.gameObject.GetComponent<Image>().color = normalColor;
        buyButton_stok.gameObject.GetComponent<Image>().color = disabledColor;
        //buyButton_stok.interactable = true;
        //saleButton_stok.interactable = false;
    }
    public void ClickStokAction()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[1]);

        if (playerValue == null)
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
        }
        stokPanel.SetActive(true);
    }

    public void ClickCancle_Stok()
    {
        stokPanel.SetActive(false);
        for (int i = 0; i < stokGroup.transform.childCount; i++)
        {
            stokGroup.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
        }
        confirmButton_stok.interactable = false;
    }

    public void ClickStokType(StokValue _stok)
    {
        stok = _stok;
        for (int i = 0; i < stokGroup.transform.childCount; i++)
        {
            stokGroup.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
        }
        stok.gameObject.GetComponent<Button>().interactable = false;
        if (confirmButton_stok.interactable == false)
        {
            confirmButton_stok.interactable = true;
        }

    }

    public void ClickConfirmButton_Stok(StokValue _stok)
    {
        ClickStokType(_stok);
        //stoknum = 1;
        stokName_stokNum.text = stok.stokName;
        stokPrice_stokNum.text = stok.stockPrice_text.text;
        stokDividend_stokNum.text = stok.dividend_text.text;
        stokNum_text.text = stoknum.ToString();
        stokTypeBackBoard.SetActive(false);
        stokNumBackBoard.SetActive(true);

        //house.GetComponent<HouseValue>()
        // stokPanel.SetActive(false);
        /* if (playerValue.deposit >= stok.stockPrice)
         {*/
        /*playerValue.deposit -= stok.stockPrice;
        playerValue.passiveIncome += stok.dividend;*/
        for (int i = 0; i < stokGroup.transform.childCount; i++)
        {
            stokGroup.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
        }
        //playerValue.ChangePlayerValue();
        //}
        confirmButton_stok.interactable = false;
    }

    public void ClickPluse_Stok()
    {
        stoknum++;
        stokNum_text.text = stoknum.ToString();
    }

    public void ClickReduce_Stok()
    {
        if (stoknum > 1)
        {
            stoknum--;
        }
        stokNum_text.text = stoknum.ToString();
    }

    public void ClickConfirmButton_StokNum()
    {
        if (playerValue.deposit >= stok.stockPrice * stoknum)
        {
            AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[0]);

            stokPanel.SetActive(false);

            houseAtionButton.interactable = false;
            stokActionButton.interactable = false;
           // passActionButton.interactable = false;
            eventActionButton.interactable = false;

            //month_Image[monthNum].sprite = month_haveActed_sprite[monthNum];
            //monthNum++;
            playerValue.deposit -= stok.stockPrice * stoknum;
            playerValue.passiveIncome += ((float)System.Math.Round(stok.stockPrice, 2)) * stoknum * (stok.dividend / 100);
            buyStockNum_settle = stok.stockPrice * stoknum;
            //playerValue.deposit += (playerValue.salary + playerValue.passiveIncome - playerValue.expenses);
            playerValue.ChangePlayerValue();
            /* if (monthNum >= 11)
             {

                 if (haveSaving)
                 {
                     playerValue.deposit += savingInterestValue + savingMoneyValue;
                 }
                 if (playerValue.deposit < savingNum * 1000)
                 {
                     confirmButton_saving.interactable = false;
                 }
                 playerValue.ChangePlayerValue();
                 savingInsurancePanel.SetActive(true);
             }
             if (playerValue.planSpeed >= 1)
             {
                 GameOver_Win();
                 pv_a.RPC("SyncLoadedJobs", RpcTarget.Others);
             }
             monthNum++;*/
            stokNumBackBoard.SetActive(false);
            stokTypeBackBoard.SetActive(true);
            GameObject _ownStok = Instantiate(ownStokRecord, ownStokGroup.transform);
            _ownStok.GetComponent<StokValueRecord>().stokName = stok.stokName;
            _ownStok.GetComponent<StokValueRecord>().boughtStokNum = stoknum;
            _ownStok.GetComponent<StokValueRecord>().stokType = stok.stokType;
            //_ownStok.GetComponent<StokValue>().houseArea = house.houseArea;
            //_ownStok.GetComponent<StokValue>().houseType = house.houseType;
            _ownStok.GetComponent<StokValueRecord>().stockPrice = stok.stockPrice;
            _ownStok.GetComponent<StokValueRecord>().dividend = stok.dividend;
            _ownStok.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(_ownStok.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
            _ownStok.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(_ownStok.GetComponent<StokValueRecord>().dividend, 2)) + " %";
            _ownStok.GetComponent<StokValueRecord>().stokName_text.text = _ownStok.GetComponent<StokValueRecord>().stokName;
            _ownStok.GetComponent<StokValueRecord>().boughtStokNum_text.text = "x " + _ownStok.GetComponent<StokValueRecord>().boughtStokNum.ToString();
            _ownStok.GetComponent<StokValueRecord>().stokType_text.text = stok.stokType_text.text;
            ownStokGroup.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 425f + 20);

            //_ownStok.GetComponent<StokValue>().houseArea_text.text = house.houseArea_text.text;
            //_ownStok.GetComponent<StokValue>().houseType_text.text = house.houseType_text.text;
            stoknum = 1;
            //pv_a.RPC("FinishAction", RpcTarget.All);
        }
        else  //向銀行貸款，貸款完要回到房地產業面or重新選擇房地產行動
        {
            stokTypeBackBoard.SetActive(true);
            stokNumBackBoard.SetActive(false);
            actionType = ActionType.Stok;
            notEnoughCash_ani.SetTrigger("NotEnoughCash");
            //loanPanel.SetActive(true);
            //stokPanel.SetActive(false);

            Debug.Log("No money");
        }

    }

    public void ClickCancleButton_StokNum()
    {
        stokNumBackBoard.SetActive(false);
        stokTypeBackBoard.SetActive(true);
        stoknum = 1;
    }
    #endregion

    public void ClickPassButton()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[1]);

        houseAtionButton.interactable = false;
        stokActionButton.interactable = false;
        passActionButton.interactable = false;
        pv_a.RPC("FinishAction", RpcTarget.All);
    }

    #region Event
    public void ClickEventAction()
    {
        if (playerValue == null)
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
        }
        int r = Random.Range(0, eventContent.Length);
        eventMoney = Random.Range((int)eventValueRandom[r].x, (int)eventValueRandom[r].y);
        eventContent_text.text = eventContent[r];

        switch (r)
        {
            case 0://中刮刮樂                
                eventMoney_text.text = "獲得" + eventMoney + " K";
                isPunish = false;
                break;
            case 1://裝修
                eventMoney_text.text = "支付" + eventMoney + " K";
                isPunish = true;
                break;
            case 2://投資成功
                eventMoney_text.text = "獲得" + eventMoney + " K";
                isPunish = false;
                break;
            case 3://金融危機爆發
                eventMoney_text.text = "支付" + eventMoney + " K";
                isPunish = true;
                break;
            case 4://社區最佳人氣獎
                eventMoney_text.text = "獲得" + eventMoney + " K";
                isPunish = false;
                break;
            case 5://行車違規
                eventMoney_text.text = "支付" + eventMoney + " K";
                isPunish = true;
                break;
            case 6://遺失錢包
                eventMoney_text.text = "支付" + eventMoney + " K";
                isPunish = true;
                break;
        }
        eventPanel.SetActive(true);
    }

    public void ClickConfirmButton_Event()
    {
        eventPanel.SetActive(false);
        if (isPunish)
        {
            if (playerValue.deposit >= eventMoney)
            {
                houseAtionButton.interactable = false;
                stokActionButton.interactable = false;
                eventActionButton.interactable = false;

                //month_Image[monthNum].sprite = month_haveActed_sprite[monthNum];
                playerValue.deposit -= eventMoney;
                // playerValue.deposit += (playerValue.salary + playerValue.passiveIncome - playerValue.expenses);
                playerValue.ChangePlayerValue();
                /* if (monthNum >= 11)
                 {

                     if (haveSaving)
                     {
                         playerValue.deposit += savingInterestValue + savingMoneyValue;
                     }
                     if (playerValue.deposit < savingNum * 1000)
                     {
                         confirmButton_saving.interactable = false;
                     }
                     playerValue.ChangePlayerValue();
                     savingInsurancePanel.SetActive(true);
                 }
                 if (playerValue.planSpeed >= 1)
                 {
                     GameOver_Win();
                     pv_a.RPC("SyncLoadedJobs", RpcTarget.Others);
                 }
                 monthNum++;*/
            }
            else
            {
                actionType = ActionType.Event;
                loanPanel.SetActive(true);
                eventPanel.SetActive(false);
                Debug.Log("No money");
            }
        }
        else
        {
            houseAtionButton.interactable = false;
            stokActionButton.interactable = false;
            eventActionButton.interactable = false;

            // month_Image[monthNum].sprite = month_haveActed_sprite[monthNum];
            playerValue.deposit += eventMoney;
            //playerValue.deposit += (playerValue.salary + playerValue.passiveIncome - playerValue.expenses);
            playerValue.ChangePlayerValue();
            /* if (monthNum >= 11)
             {

                 if (haveSaving)
                 {
                     playerValue.deposit += savingInterestValue + savingMoneyValue;
                 }
                 if (playerValue.deposit < savingNum * 1000)
                 {
                     confirmButton_saving.interactable = false;
                 }
                 playerValue.ChangePlayerValue();
                 savingInsurancePanel.SetActive(true);
             }
             if (playerValue.planSpeed >= 1)
             {
                 GameOver_Win();
                 pv_a.RPC("SyncLoadedJobs", RpcTarget.Others);
             }
             monthNum++;*/
        }
    }
    #endregion

    #region Information
    public void ClickInformationButton()
    {
        if (playerValue == null)
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
        }

       /* LoanValue[] loanValue_children = loanGroup.GetComponentsInChildren<LoanValue>();
        foreach (LoanValue l in loanValue_children)
        {
            if (l.loanNumValue <= playerValue.deposit)
            {
                l.repaymentButton.interactable = true;
            }
            else
            {
                l.repaymentButton.interactable = false;
            }
        }*/
        informationPanel.SetActive(true);
        informationBackBoard.SetActive(true);
    }

    public void ClickRoleInfoButton()
    {
        roleInfoCanvas.sortingOrder = 3;
        ownHoseCanvas.sortingOrder = 2;
        loanCanvas.sortingOrder = 2;
        stokCanvas.sortingOrder = 2;
    }
    public void ClickOwnHouseButton()
    {
        roleInfoCanvas.sortingOrder = 2;
        ownHoseCanvas.sortingOrder = 3;
        loanCanvas.sortingOrder = 2;
        stokCanvas.sortingOrder = 2;
    }
    public void ClickLoanButton()
    {
        roleInfoCanvas.sortingOrder = 2;
        ownHoseCanvas.sortingOrder = 2;
        loanCanvas.sortingOrder = 3;
        stokCanvas.sortingOrder = 2;
    }
    public void ClickStokButton()
    {
        roleInfoCanvas.sortingOrder = 2;
        ownHoseCanvas.sortingOrder = 2;
        loanCanvas.sortingOrder = 2;
        stokCanvas.sortingOrder = 3;
    }

    public void ClickCloseInfoButton()
    {

        informationPanel.SetActive(false);
        informationBackBoard.SetActive(false);
        if (isFirstEnterGame)
        {
            if (playerValue == null)
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
            }
            if (playerValue.deposit < savingNum * 100)
            {
                confirmButton_saving.interactable = false;
            }
            //savingInsurancePanel.SetActive(true);
            //insuranceBackBoard.SetActive(true);
            isFirstEnterGame = false;
            playerValue.gameOverPanel_p = gameOverPanel;
            playerValue.winText_p = winText;
            playerValue.loseText_p = loseText;
        }
        salaryNum_settle = playerValue.salary;
    }
    #endregion

    #region ClickBankLable
    public void ClickSavingLable()
    {
        savingBackBoard.SetActive(true);
        loanBackBoard.SetActive(false);
        loanRecordBackBoard.SetActive(false);
        savingLable.gameObject.GetComponent<Image>().color = normalColor;
        loanLable.gameObject.GetComponent<Image>().color = disabledColor;
        loanRecordLable.gameObject.GetComponent<Image>().color = disabledColor;
        savingLable.enabled = false;
        loanLable.enabled = true;
        loanRecordLable.enabled = true;
    }
    public void ClickLoanLable()
    {
        savingBackBoard.SetActive(false);
        loanBackBoard.SetActive(true);
        loanRecordBackBoard.SetActive(false);
        savingLable.gameObject.GetComponent<Image>().color = disabledColor;
        loanLable.gameObject.GetComponent<Image>().color = normalColor;
        loanRecordLable.gameObject.GetComponent<Image>().color = disabledColor;
        savingLable.enabled = true;
        loanLable.enabled = false;
        loanRecordLable.enabled = true;
    }
    public void ClickLoanRecordLable()
    {
        LoanValue[] loanValue_children = loanGroup.GetComponentsInChildren<LoanValue>();
        foreach (LoanValue l in loanValue_children)
        {
            if (l.loanNumValue <= playerValue.deposit)
            {
                l.repaymentButton.interactable = true;
            }
            else
            {
                l.repaymentButton.interactable = false;
            }
        }
        savingBackBoard.SetActive(false);
        loanBackBoard.SetActive(false);
        loanRecordBackBoard.SetActive(true);
        savingLable.gameObject.GetComponent<Image>().color = disabledColor;
        loanLable.gameObject.GetComponent<Image>().color = disabledColor;
        loanRecordLable.gameObject.GetComponent<Image>().color = normalColor;
        savingLable.enabled = true;
        loanLable.enabled = true;
        loanRecordLable.enabled = false;
    }
    #endregion

    #region Loan
    public void ClickPluse_Loan()
    {
        if(loanNum<loanLimit)
        {
            loanNum++;
            loanNum_text.text = loanNum.ToString();
            loanInterest_text.text = "每月利息 : " + loanNum * 1 + " K";
        }
        

    }
    public void ClickReduce_Loan()
    {
        if (loanNum > 1)
        {
            loanNum--;
        }
        loanNum_text.text = loanNum.ToString();
        loanInterest_text.text = "每月利息 : " + loanNum * 1 + " K";
    }

    public void ClickConfirmButton_Loan()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[0]);

        savingInsurancePanel.SetActive(false);
        playerValue.deposit += loanNum * 100;
        playerValue.expenses += loanNum * 1;
        loanLimit -= loanNum;
        if(loanLimit<=0)
        {
            confirmButton_loan.interactable = false;
            plusButton_loan.interactable = false;
            reduceButton_loan.interactable = false;
        }
        /*if (actionType == ActionType.House)
        {
            housePanel.SetActive(true);
        }
        else if (actionType == ActionType.Stok)
        {
            stokPanel.SetActive(true);
        }
        else if (actionType == ActionType.Event)
        {
            eventPanel.SetActive(true);
        }*/
        //loanPanel.SetActive(false);
        playerValue.ChangePlayerValue();
        GameObject _loanRecoard = Instantiate(loanRecoard, loanGroup.transform);
        _loanRecoard.GetComponent<LoanValue>().loanNumValue = loanNum * 100;
        _loanRecoard.GetComponent<LoanValue>().loanNumRecord_text.text = (loanNum * 100).ToString()+" K";
        loanNum = 1;
        loanNum_text.text = loanNum.ToString();
        loanInterest_text.text = "每月利息 : " + loanNum * 1 + " K";
    }
    public void ClickCancleButton_Loan()
    {
        /*if (actionType == ActionType.House)
        {
            housePanel.SetActive(true);
        }
        else if (actionType == ActionType.Stok)
        {
            stokPanel.SetActive(true);
        }
        else if (actionType == ActionType.Event)
        {
            eventPanel.SetActive(true);
        }*/
        // loanPanel.SetActive(false);
        savingInsurancePanel.SetActive(false);
        loanNum = 1;
        loanNum_text.text = loanNum.ToString();
        loanInterest_text.text = "每月利息 : " + loanNum * 10 + " K";
    }
    public void ClickCancleButton_LoanRecord()
    {
        savingInsurancePanel.SetActive(false);
    }
    #endregion

    #region Saving
    public void ClickSavingAction()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[1]);

        savingInsurancePanel.SetActive(true);
    }
    public void ClickPlus_Saving()
    {
        savingNum++;
        if (playerValue.deposit < savingNum * 100)
        {
            confirmButton_saving.interactable = false;
        }
        else
        {
            confirmButton_saving.interactable = true;
        }
        savingNum_text.text = savingNum.ToString();
        savingInterest_text.text = "三個月後可領 " + savingNum * 3 + " K" + " 利息";
    }
    public void ClickReduce_Saving()
    {
        if (savingNum > 1)
        {
            savingNum--;
        }
        if (playerValue.deposit < savingNum * 100)
        {
            confirmButton_saving.interactable = false;
        }
        else
        {
            confirmButton_saving.interactable = true;
        }
        savingNum_text.text = savingNum.ToString();
        savingInterest_text.text = "三個月後可領 " + savingNum * 3 + " K" + " 利息";
    }
    public void ClickCancleButton_Saving()
    {
        //confirmButton_insurance.interactable = false;
        //insuranceBackBoard.SetActive(true);
        savingInsurancePanel.SetActive(false);
        //haveSaving = false;
        savingNum = 1;
        savingNum_text.text = savingNum.ToString();
        savingInterest_text.text = "三個月後可領 " + savingNum * 3 + " K" + " 利息";
    }
    public void ClickConfirmButton_Saving()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[0]);

        //confirmButton_insurance.interactable = false;
        // insuranceBackBoard.SetActive(true);
        savingInsurancePanel.SetActive(false);
        haveSaving = true;
        confirmButton_saving.interactable = false;
        reduceButton_saving.interactable = false;
        plusButton_saving.interactable = false;
        //savingActionButton.interactable = false;
        /*houseAtionButton.interactable = false;
        stokActionButton.interactable = false;
        eventActionButton.interactable = false;*/
        playerValue.deposit -= savingNum * 100;
        savingMoneyValue = savingNum * 100;
        savingInterestValue = savingNum * 3;
        playerValue.ChangePlayerValue();
        savingNum = 1;
        savingNum_text.text = savingNum.ToString();
        savingInterest_text.text = "三個月後可領 " + savingNum * 3 + " K" + " 利息";
    }
    #endregion

    #region Insurance
    /* public void ClickInsuranceType_Medical()
     {
         if (isClickedMedical)
         {
             medicalType.color = new Color(1, 1, 1, 1);
             medicalType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
             isClickedMedical = false;
             expenseInsuranceValue -= 100;
             if (expenseInsuranceValue <= playerValue.deposit && expenseInsuranceValue > 0)
             {
                 confirmButton_insurance.interactable = true;
             }
             else
             {
                 confirmButton_insurance.interactable = false;
             }
         }
         else
         {
             medicalType.color = new Color(1, 1, 1, 0.65f);
             medicalType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.65f);
             isClickedMedical = true;
             expenseInsuranceValue += 100;
             if (expenseInsuranceValue > playerValue.deposit)
             {
                 confirmButton_insurance.interactable = false;
             }
             else
             {
                 confirmButton_insurance.interactable = true;
             }
         }
     }
     public void ClickInsuranceType_Travel()
     {
         if (isClickedTravel)
         {
             travelType.color = new Color(1, 1, 1, 1);
             travelType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
             isClickedTravel = false;
             expenseInsuranceValue -= 100;
             if (expenseInsuranceValue <= playerValue.deposit && expenseInsuranceValue > 0)
             {
                 confirmButton_insurance.interactable = true;
             }
             else
             {
                 confirmButton_insurance.interactable = false;
             }
         }
         else
         {
             travelType.color = new Color(1, 1, 1, 0.65f);
             travelType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.65f);
             isClickedTravel = true;
             expenseInsuranceValue += 100;
             if (expenseInsuranceValue > playerValue.deposit)
             {
                 confirmButton_insurance.interactable = false;
             }
             else
             {
                 confirmButton_insurance.interactable = true;
             }
         }
     }
     public void ClickCancleButton_Insurance()
     {
         savingInsurancePanel.SetActive(false);
         insuranceBackBoard.SetActive(true);
         //savingBackBoard.SetActive(true);
         isClickedMedical = false;
         isClickedTravel = false;
         expenseInsuranceValue = 0;
         medicalType.color = new Color(1, 1, 1, 1);
         travelType.color = new Color(1, 1, 1, 1);
         confirmButton_insurance.interactable = false;
         travelType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
         medicalType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
     }
     public void ClickConfirmButton_Insurance()
     {
         savingInsurancePanel.SetActive(false);
         insuranceBackBoard.SetActive(true);
         for (int i = 0; i < month_Image.Length; i++)
         {
             month_Image[i].sprite = month_noAct_sprite[i];
         }
         monthNum = 0;
         savingBackBoard.SetActive(true);
         if (isClickedMedical)
         {
             haveMedical = true;
         }
         else
         {
             haveMedical = false;
         }
         if (isClickedTravel)
         {
             haveTravel = true;
         }
         else
         {
             haveTravel = false;
         }
         isClickedMedical = false;
         isClickedTravel = false;
         playerValue.deposit -= expenseInsuranceValue;
         playerValue.ChangePlayerValue();
         medicalType.color = new Color(1, 1, 1, 1);
         travelType.color = new Color(1, 1, 1, 1);
         confirmButton_insurance.interactable = false;
         travelType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
         medicalType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
         expenseInsuranceValue = 0;
     }*/
    #endregion

    #region Sale
    public void ClickConfirmButton_SaleHouse()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[0]);

        saleHousePanel.SetActive(false);
        playerValue.deposit += houseValueRecord.downPayment;
        playerValue.passiveIncome -= houseValueRecord.housePassiveIncome;
        saleHouseNum_settle = houseValueRecord.downPayment;
        playerValue.ChangePlayerValue();
        ownHouseGroup.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, 354.1667f + 60);
        Destroy(houseValueRecord.gameObject);
    }
    public void ClickCancleButton_SaleHouse()
    {
        saleHousePanel.SetActive(false);
    }
    public void ClickPlusButton_SaleStok()
    {
        if (saleStokNum < saleStokNum_max)
        {
            saleStokNum++;
            saleStokNum_text.text = saleStokNum.ToString();
        }
    }
    public void ClickReduceButton_SaleStok()
    {
        if (saleStokNum > 1)
        {
            saleStokNum--;
            saleStokNum_text.text = saleStokNum.ToString();
        }
    }
    public void ClickConfirmButton_SaleStok()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[0]);

        saleStokPanel.SetActive(false);
        playerValue.deposit += stokValueRecord.stockPrice * saleStokNum;
        playerValue.passiveIncome -= (int)stokValueRecord.stockPrice * saleStokNum * (stokValueRecord.dividend / 100);
        saleStokNum_settle = stokValueRecord.stockPrice * saleStokNum;
        playerValue.ChangePlayerValue();
        if (saleStokNum < saleStokNum_max)
        {
            stokValueRecord.boughtStokNum -= saleStokNum;
            stokValueRecord.boughtStokNum_text.text = "x " + stokValueRecord.boughtStokNum;
        }
        else
        {
            ownStokGroup.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, 354.1667f + 60);
            Destroy(stokValueRecord.gameObject);
        }
        saleStokNum = 1;
        saleStokNum_text.text = saleStokNum.ToString();
    }
    public void ClickCancleButton_SaleStok()
    {
        saleStokPanel.SetActive(false);
        saleStokNum = 1;
        saleStokNum_text.text = saleStokNum.ToString();
    }
    #endregion
    public void GameOver_Win()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[2]);

        canCountTime = false;
        pv_a.RPC("GameOver_Lose", RpcTarget.Others);
        housePanel.SetActive(false);
        stokPanel.SetActive(false);
        eventPanel.SetActive(false);
        loanPanel.SetActive(false);
        informationPanel.SetActive(false);
        informationBackBoard.SetActive(false);
        savingInsurancePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        winText.SetActive(true);
    }
    [PunRPC]
    public void GameOver_Lose()
    {
        AudioManager.instance.PlayClip_SF(AudioManager.instance.sfClip[3]);


        housePanel.SetActive(false);
        stokPanel.SetActive(false);
        eventPanel.SetActive(false);
        loanPanel.SetActive(false);
        informationPanel.SetActive(false);
        informationBackBoard.SetActive(false);
        savingInsurancePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        loseText.SetActive(true);
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

    [PunRPC]
    public void SyncTimer(int timeNum)
    {
        timeCount = timeNum;
        canCountTime = true;
        finishActionNum = 0;
        clickSettleCloseButtonNum = 0;
    }
    [PunRPC]
    public void FinishAction()
    {
        if(finishActionNum+1>= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            settlePanel.SetActive(true);
        }
        finishActionNum ++;
        timeCount_settle = watchSettleTime;
        //canCountTime = true;
    }
    [PunRPC]
    public void ClickSettleCloseButton()
    {
        clickSettleCloseButtonNum++;
    }
    [PunRPC]
    public void TurnNewsContent(int[] _monthNewsNum)
    {
        monthNewsNum = _monthNewsNum;
        //newsContent_text.text = monthNewsContent[monthNewsNum[monthNum]];
        for (int i = 0; i < newsContentParent.transform.childCount; i++)
        {
            Destroy(newsContentParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < storyNewsContentParent.transform.childCount; i++)
        {
            Destroy(storyNewsContentParent.transform.GetChild(i).gameObject);
        }
        Instantiate(storyNewsContent_prefab[monthNum], storyNewsContentParent.transform);
        Instantiate(newsContent_prefab[monthNewsNum[monthNum]], newsContentParent.transform);
        Debug.Log("news");
        month_text.text = monthWord[monthNum];
        storyNewsContentParent.SetActive(true);
        newsContentParent.SetActive(false);
        turnChannelButton_left.interactable = false;
        turnChannelButton_right.interactable = true;
        channelNum = 0;
        monthNum++;
    }
    public void SumMonthValue()
    {
        informationBackBoard.SetActive(false);
        informationPanel.SetActive(false);

    }
    public void TurnHouseStokValue()//更改房地產和股票的數值，包括玩家已擁有的房地產和股票，要重新計算持續性收入
    {
        ShowSettle();
        //PhotonNetwork.Instantiate("Settle", Vector3.zero, Quaternion.identity);

        switch (monthNewsNum[monthNum - 1])
        {
            case 0:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 3)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 1.005f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend += 0.5f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise += 0.5f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 1 && ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseArea == 0)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.005f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.005f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.5f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";
                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 3)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 1.005f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend += 0.5f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise += 0.5f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 1 && houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseArea == 0)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.005f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.005f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.5f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 1:

                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseArea == 1)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 0.998f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 0.998f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise -= 0.2f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseArea == 1)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 0.998f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 0.998f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise -= 0.2f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 2:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 1)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 1.004f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend += 0.4f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise += 0.4f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 0 && ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseArea == 1)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.004f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.004f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.4f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 1)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 1.004f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend += 0.4f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise += 0.4f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 0 && houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseArea == 1)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.004f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.004f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.4f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 3:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.998f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.2f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.2f;

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 1)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 0.998f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 0.998f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise -= 0.2f;

                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.998f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.2f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.2f;

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 1)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 0.998f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 0.998f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise -= 0.2f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 4:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 2)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 1.003f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend += 0.3f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise += 0.3f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 2)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 1.003f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend += 0.3f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise += 0.3f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }

                break;
            case 5:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.998f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.2f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.2f;

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 2 && ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseArea == 0)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 0.998f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 0.998f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise -= 0.2f;

                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.998f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.2f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.2f;

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 2 && houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseArea == 0)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 0.998f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 0.998f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise -= 0.2f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 6:

                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 0 && ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseArea == 0)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.002f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.002f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.2f;

                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 0 && houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseArea == 0)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.002f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.002f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.2f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 7:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.998f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.2f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.2f;

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 1)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 0.998f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 0.998f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise -= 0.2f;

                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.998f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.2f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.2f;

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 1)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 0.998f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 0.998f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise -= 0.2f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                       
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 8:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 0)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 1.002f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend += 0.2f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise += 0.2f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 1)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.002f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.002f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.2f;

                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 0)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 1.002f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend += 0.2f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise += 0.2f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 1)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.002f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.002f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.2f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 9:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.998f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.2f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.2f;

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.998f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.2f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.2f;

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                }

                break;
            case 10:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.998f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.2f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.2f;

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.002f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.002f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.2f;

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.998f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.2f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.2f;

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.002f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.002f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.2f;

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                }
                break;
            case 11:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType != 1)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.998f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.2f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.2f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType != 1)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.998f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.2f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.2f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }
                break;


        }
        switch (monthNum - 1)
        {
            case 0:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 3 || ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 0)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 1.005f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend += 0.5f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise += 0.5f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 1 && ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 2)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.005f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.005f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.5f;

                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 3 || stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 0)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 1.005f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend += 0.5f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise += 0.5f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 1 && houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 2)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.005f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.005f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.5f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 1:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 0)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.999f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.1f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.1f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 2)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 0.999f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 0.999f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise -= 0.1f;

                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 0)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.999f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.1f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.1f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 2)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 0.999f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 0.999f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise -= 0.1f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 2:

                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 0.998f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 0.998f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise -= 0.2f;

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 0.998f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 0.998f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise -= 0.2f;

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                }
                break;
            case 3:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.999f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.1f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.1f;

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 0.999f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 0.999f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise -= 0.1f;

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.999f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.1f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.1f;

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 0.999f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 0.999f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise -= 0.1f;

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                }
                break;
            case 4:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 5)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.998f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.2f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.2f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }

                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 5)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.998f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.2f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.2f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }
                break;
            case 5:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 9)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 0.998f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend -= 0.2f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise -= 0.2f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }

                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 9)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 0.998f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend -= 0.2f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise -= 0.2f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }

                break;
            case 6:

                break;
            case 7:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 1.002f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend += 0.2f;
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise += 0.2f;

                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                    ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.002f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.002f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.2f;

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 1.002f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend += 0.2f;
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise += 0.2f;

                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                    stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.002f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.002f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.2f;

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                }
                break;
            case 8:

                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.002f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.002f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.2f;

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                }

                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.002f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.002f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.2f;

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                }
                break;
            case 9:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 0)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 1.004f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend += 0.4f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise += 0.4f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }

                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 0)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 1.004f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend += 0.4f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise += 0.4f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }

                break;
            case 10:
                for (int i = 0; i < ownStokGroup.transform.childCount; i++)
                {
                    if (ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 0 || ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stokType == 3)
                    {
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice *= 1.002f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend += 0.2f;
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise += 0.2f;

                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().stockPrice, 2)) + " K";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend_text.text = "股息 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().dividend, 2)) + " %";
                        ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.002f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.002f;
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.2f;

                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                    ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                }
                for (int i = 0; i < stokGroup.transform.childCount; i++)
                {
                    if (stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 0 || stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stokType == 3)
                    {
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice *= 1.002f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend += 0.2f;
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise += 0.2f;

                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice_text.text = "一張股票 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().stockPrice, 2)) + " K";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend_text.text = "股息 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().dividend, 2)) + " %";
                        stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(stokGroup.transform.GetChild(i).gameObject.GetComponent<StokValue>().earningRise, 2)) + " %";

                    }
                }
                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.002f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.002f;
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.2f;

                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                    houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                }
                break;
            case 11:

                for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
                {
                    if (ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().houseType == 1)
                    {
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment *= 1.003f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome *= 1.003f;
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise += 0.3f;

                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().downPayment, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome, 2)) + " K";
                        ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().earningRise, 2)) + " %";

                    }
                }

                for (int i = 0; i < houseGroup.transform.childCount; i++)
                {
                    if (houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().houseType == 1)
                    {
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment *= 1.003f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome *= 1.003f;
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise += 0.3f;

                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment_text.text = "頭期款 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().downPayment, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome_text.text = "房屋淨收入 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().housePassiveIncome, 2)) + " K";
                        houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise_text.text = "收益漲跌 : " + ((float)System.Math.Round(houseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValue>().earningRise, 2)) + " %";

                    }
                }
                break;
        }

        float stokTotal = 0, houseTotal = 0;
        for (int i = 0; i < ownStokGroup.transform.childCount; i++)
        {
            StokValueRecord stokValueRecord_r;
            stokValueRecord_r = ownStokGroup.transform.GetChild(i).gameObject.GetComponent<StokValueRecord>();
            stokTotal += (int)stokValueRecord_r.stockPrice * stokValueRecord_r.boughtStokNum * (stokValueRecord_r.dividend / 100);
        }
        for (int i = 0; i < ownHouseGroup.transform.childCount; i++)
        {
            houseTotal += ownHouseGroup.transform.GetChild(i).gameObject.GetComponent<HouseValueRecord>().housePassiveIncome;
        }
        if (haveSaving && savingMonthNum >= 2)
        {
            playerValue.deposit += savingInterestValue + savingMoneyValue;
            haveSaving = false;
            savingMonthNum = 0;
            if (playerValue.deposit < savingNum * 100)
            {
                confirmButton_saving.interactable = false;
            }
            else
            {
                confirmButton_saving.interactable = true;

            }
        }
        else
        {
            savingMonthNum++;
        }

        Debug.Log("Turn");
        playerValue.passiveIncome = stokTotal + houseTotal;
        playerValue.ChangePlayerValue();
        //StartCoroutine(EnterToNextMonth());
    }

    //IEnumerator EnterToNextMonth()
    public void EnterToNextMonth()
    {
        //yield return new WaitForSeconds(watchSettleTime);
        playerValue.deposit += (playerValue.salary + playerValue.passiveIncome - playerValue.expenses);
        playerValue.ChangePlayerValue();
        salaryNum_settle = playerValue.salary;
        passiveIncomeNum_settle = playerValue.passiveIncome;
        expenseNum_settle = playerValue.expenses;
        //33if (monthNum > 11)
        //33{
        //33 monthNum = 0;
        /*if (haveSaving)
        {
            playerValue.deposit += savingInterestValue + savingMoneyValue;
        }
        if (playerValue.deposit < savingNum * 1000)
        {
            confirmButton_saving.interactable = false;
        }*/
        /*33if (PhotonNetwork.IsMasterClient)
        {
            monthNewsNum = new int[12];
            for (int i = 0; i < monthNewsNum.Length; i++)
            {
                monthNewsNum[i] = Random.Range(0, monthNewsNum.Length);
                for (int j = 0; j < i; j++)
                {
                    while (monthNewsNum[i] == monthNewsNum[j])
                    {
                        j = 0;
                        monthNewsNum[i] = Random.Range(0, monthNewsNum.Length);
                    }
                }
            }
            pv_a.RPC("TurnNewsContent", RpcTarget.All, monthNewsNum);
            pv_a.RPC("SyncTimer", RpcTarget.All, timeCount);
        }
        playerValue.ChangePlayerValue();
        //savingInsurancePanel.SetActive(true);
    }*/
        if (monthNum <= 11)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                pv_a.RPC("TurnNewsContent", RpcTarget.All, monthNewsNum);
                pv_a.RPC("SyncTimer", RpcTarget.All, timeCount);
            }
        }

        else //(playerValue.planSpeed >= 1)
        {
            playersScores = new float[PhotonNetwork.CurrentRoom.PlayerCount];
            playerNum = new int[playersScores.Length];
            /*int r = 0;

            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                playerNum[r] = player.ActorNumber;
                r++;
            }*/
            for(int i=0;i<playerNum.Length;i++)
            {
                playerNum[i] = i + 1;
            }
            for (int i = 0; i < playersScores.Length; i++)
            {
                playersScores[i] = (float)PhotonNetwork.CurrentRoom.Players[playerNum[i]].CustomProperties["PlanSpeed"];//PhotonNetwork.CurrentRoom.Players[2].ActorNumber

                /*for (int j = 0; j < playersScores.Length; j++)
                {
                    if (i + 1 == playerNum[j])
                    {
                        playersScores[i] = (float)PhotonNetwork.CurrentRoom.Players[playerNum[j]].CustomProperties["PlanSpeed"];//PhotonNetwork.CurrentRoom.Players[2].ActorNumber

                        break;
                    }
                }*/
            }
            /*for(int i=0;i<playerNum.Length;i++)
            {
                playerNum[i] = i + 1;
            }*/
            for (int i = 0; i < playersScores.Length - 1; i++)
            {
                for (int j = 0; j < playersScores.Length - 1; j++)
                {
                    if (playersScores[j] < playersScores[j + 1])
                    {
                        float t = playersScores[j];
                        int playerNum_t = playerNum[j];
                        playersScores[j] = playersScores[j + 1];
                        playerNum[j] = playerNum[j + 1];
                        playersScores[j + 1] = t;
                        playerNum[j + 1] = playerNum_t;
                    }
                }
            }
            if (PhotonNetwork.LocalPlayer.ActorNumber == playerNum[0])
            {
                GameOver_Win();
            }
            //pv_a.RPC("SyncLoadedJobs", RpcTarget.Others);
        }
        houseAtionButton.interactable = true;
        stokActionButton.interactable = true;
        eventActionButton.interactable = true;
        savingActionButton.interactable = true;
        passActionButton.interactable = true;

        if (!haveSaving)
        {
            reduceButton_saving.interactable = true;
            plusButton_saving.interactable = true;
            //savingActionButton.interactable = true;
        }

        settlePanel.SetActive(false);
        for (int i = 0; i < settleGroup.transform.childCount; i++)
        {
            Destroy(settleGroup.transform.GetChild(i).gameObject);
        }
        //monthNum++;
    }
    /*public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        if (targetPlayer != photonView.Owner)
        {
            timeCount = (int)changedProps["CountTime"];
            countTime_text.text = timeCount.ToString();
        
    }*/
    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Sending data to other players
            stream.SendNext(currentTime);
        }
        else if (stream.IsReading)
        {
            // Receiving data from the master client
            currentTime = (float)stream.ReceiveNext();
        }
    }*/
}
