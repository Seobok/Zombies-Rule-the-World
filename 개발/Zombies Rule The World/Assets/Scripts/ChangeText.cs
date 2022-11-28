using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    private void Awake()
    {
        List<Dictionary<string, object>> list = CSVReader.Read("CSVs/StringUI");
        Dictionary<string, int> idList = CSVReader.ReadID("CSVs/StringUI");
        
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        
        foreach (var txt in texts)
        {
            var key = txt.transform.name.Trim();
            if(idList.ContainsKey(key))
            {
                txt.text = list[idList[key]]["String"].ToString();
            }
        }
    }
}
