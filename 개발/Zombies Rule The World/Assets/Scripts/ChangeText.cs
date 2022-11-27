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
            txt.text = list[idList[txt.transform.name.Trim()]]["String"].ToString();
            Debug.Log(txt.transform.name.Trim());
        }
    }
}
