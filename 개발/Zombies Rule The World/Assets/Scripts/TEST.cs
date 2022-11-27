using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public void TESTCSV()
    {
        List<Dictionary<string, object>> list = CSVReader.Read("CSVs/CountryInfo");
        Dictionary<string, int> idList = CSVReader.ReadID("CSVs/CountryInfo");

        Debug.Log(list[idList["동아시아"]]["대륙 설명"]);
    }
}
