using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HouseValue : MonoBehaviour
{

    public string houseName;
    public float downPayment, housePassiveIncome;
    public int houseArea, houseType;
    public float earningRise=0;
    //public GameObject houseType;

    public TextMeshProUGUI downPayment_text, housePassiveIncome_text, houseName_text, houseType_text, houseArea_text, earningRise_text;

    private void Awake()
    {
        downPayment_text.text = "頭期款 : "+(int)downPayment+" K";
        housePassiveIncome_text.text = "房屋淨收入 : "+(int)housePassiveIncome + " K";
        houseName_text.text = houseName;
        switch(houseArea)
        {
            case 0:
                houseArea_text.text = "地區 : 北北市";
                break;
            case 1:
                houseArea_text.text = "地區 : 南興市";
                break;
        }
        switch(houseType)
        {
            case 0:
                houseType_text.text = "類型 : 度假屋";
                break;
            case 1:
                houseType_text.text = "類型 : 商業大樓";
                break;
            case 2:
                houseType_text.text = "類型 : 住宅";
                break;
        }
        earningRise_text.text = "收益漲跌 : 0 %";
        //rooomNum_text.text = roomNum;
    }

    /*public void ClickHouseType(GameObject _houseType)
    {
        ActionManager.house = _houseType;
    }*/
    
}
