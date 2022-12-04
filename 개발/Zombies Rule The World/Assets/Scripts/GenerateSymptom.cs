using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSymptom : MonoBehaviour
{
    private bool isLoaded = false;
    public void GenerateItem()
    {
        if (!isLoaded)
        {
            var symptomList = CSVReader.Read("CSVs/Symptom");

            foreach (var symptom in symptomList)
            {
                var obj = ObjectPools.Instance.GetPooledObject("Symptom_Item_Background");

                obj.transform.parent = transform;

                var item = obj.GetComponent<SelectGenItem>();
            
                item.title.text = symptom["Title"].ToString();
                item.text.text = symptom["Text"].ToString();

                item.contagious = float.Parse(symptom["전염성"].ToString());
                item.cureDevelopProbability = float.Parse(symptom["치료제 개발 수치"].ToString());
                item.price = int.Parse(symptom["가격"].ToString());
            }

            isLoaded = true;
        }
    }
}
