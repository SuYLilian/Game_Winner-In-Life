using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StokValue : MonoBehaviour
{
    public string stokName;
    public float stockPrice, dividend;
    public int stokType;
    public int boughtStokNum;
    public float earningRise = 0;

    //public GameObject houseType;

    public TextMeshProUGUI stockPrice_text, dividend_text, stokName_text, stokType_text, boughtStokNum_text, earningRise_text;

    private void Awake()
    {
        stockPrice_text.text = "一張股票 : " + (int)stockPrice + " K";
        dividend_text.text = "股息 : " + dividend + " %";
        stokName_text.text = stokName;
        switch (stokType)
        {
            case 0:
                stokType_text.text = "類型 : 科技";
                break;
            case 1:
                stokType_text.text = "類型 : 醫療";
                break;
            case 2:
                stokType_text.text = "類型 : 建設";
                break;
            case 3:
                stokType_text.text = "類型 : 再生能源";
                break;
            case 4:
                stokType_text.text = "類型 : 旅遊";
                break;
            case 5:
                stokType_text.text = "類型 : 金融";
                break;
            case 6:
                stokType_text.text = "類型 : 資源";
                break;
            case 7:
                stokType_text.text = "類型 : 新興市場";
                break;
            case 8:
                stokType_text.text = "類型 : 媒體與娛樂";
                break;
        }
        earningRise_text.text = "收益漲跌 : 0 %";

        //rooomNum_text.text = roomNum;
    }
}
