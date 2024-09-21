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
        downPayment_text.text = "�Y���� : "+(int)downPayment+" K";
        housePassiveIncome_text.text = "�Ыβb���J : "+(int)housePassiveIncome + " K";
        houseName_text.text = houseName;
        switch(houseArea)
        {
            case 0:
                houseArea_text.text = "�a�� : �_�_��";
                break;
            case 1:
                houseArea_text.text = "�a�� : �n����";
                break;
        }
        switch(houseType)
        {
            case 0:
                houseType_text.text = "���� : �װ���";
                break;
            case 1:
                houseType_text.text = "���� : �ӷ~�j��";
                break;
            case 2:
                houseType_text.text = "���� : ��v";
                break;
        }
        earningRise_text.text = "���q���^ : 0 %";
        //rooomNum_text.text = roomNum;
    }

    /*public void ClickHouseType(GameObject _houseType)
    {
        ActionManager.house = _houseType;
    }*/
    
}
