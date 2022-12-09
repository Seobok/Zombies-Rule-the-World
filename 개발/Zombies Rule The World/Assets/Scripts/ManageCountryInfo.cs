using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManageCountryInfo : MonoBehaviour
{
    public GameObject info;

    public void SetInfo(string country)
    {
        var countryInfoList = CSVReader.Read("CSVs/CountryInfo");
        var countryInfoidList = CSVReader.ReadID("CSVs/CountryInfo");

        info.GetComponent<TextMeshProUGUI>().text = countryInfoList[countryInfoidList[country]]["대륙 설명"].ToString();
    }
    
    public void CloseCountryInfo()
    {
        ObjectPools.Instance.ReleaseObjectToPool(gameObject);
    }
}
