using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectGenItem : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI text;

    [HideInInspector] public float contagious;
    [HideInInspector] public float cureDevelopProbability;
    [HideInInspector] public int price;
    
    public void ShowSelected()
    {
        List<Dictionary<string, object>> list = CSVReader.Read("CSVs/StringUI");
        Dictionary<string, int> idList = CSVReader.ReadID("CSVs/StringUI");
        
        GameObject obj = ObjectPools.Instance.GetPooledObject("Selected");
        obj.transform.parent = gameObject.transform.parent.parent.parent.parent.parent;

        obj.transform.localPosition= new Vector3(0, 0, 0);

        var item = obj.GetComponent<ManageSelected>();

        item.itemTitle.text = title.text;
        item.itemText.text = text.text;
        item.confirmText.text = String.Format(list[idList["Confirm_Text"]]["String"].ToString(), price);

        item.contagious = contagious;
        item.cureDevelopProbability = cureDevelopProbability;
        item.price = price;
        item.card = gameObject;
    }
}
