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
        stockPrice_text.text = "�@�i�Ѳ� : " + (int)stockPrice + " K";
        dividend_text.text = "�Ѯ� : " + dividend + " %";
        stokName_text.text = stokName;
        switch (stokType)
        {
            case 0:
                stokType_text.text = "���� : ���";
                break;
            case 1:
                stokType_text.text = "���� : ����";
                break;
            case 2:
                stokType_text.text = "���� : �س]";
                break;
            case 3:
                stokType_text.text = "���� : �A�ͯ෽";
                break;
            case 4:
                stokType_text.text = "���� : �ȹC";
                break;
            case 5:
                stokType_text.text = "���� : ����";
                break;
            case 6:
                stokType_text.text = "���� : �귽";
                break;
            case 7:
                stokType_text.text = "���� : �s������";
                break;
            case 8:
                stokType_text.text = "���� : �C��P�T��";
                break;
        }
        earningRise_text.text = "���q���^ : 0 %";

        //rooomNum_text.text = roomNum;
    }
}
