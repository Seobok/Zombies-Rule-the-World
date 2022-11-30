using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageDefault : MonoBehaviour
{
    public GameObject daysNum;
    private TextMeshProUGUI _daysNumText;
    public GameObject zombieGraphBar;
    private Image _zombieGraphBarImage;
    public GameObject cureGraphBar;
    private Image _cureGraphBarImage;
    public GameObject zombiePercentage;
    private TextMeshProUGUI _zombiePercentageText;
    public GameObject curePercentage;
    private TextMeshProUGUI _curePercentageText;
    public GameObject geneNum;
    private TextMeshProUGUI _geneNumText;
    
    private List<Dictionary<string, object>> _list;
    private Dictionary<string, int> _idList;
    
    private void Awake()
    {
        _daysNumText = daysNum.GetComponent<TextMeshProUGUI>();
        _zombieGraphBarImage = zombieGraphBar.GetComponent<Image>();
        _cureGraphBarImage = cureGraphBar.GetComponent<Image>();
        _zombiePercentageText = zombiePercentage.GetComponent<TextMeshProUGUI>();
        _curePercentageText = curePercentage.GetComponent<TextMeshProUGUI>();
        _geneNumText = geneNum.GetComponent<TextMeshProUGUI>();
        
        _list = CSVReader.Read("CSVs/StringUI");
        _idList = CSVReader.ReadID("CSVs/StringUI");
        
        ChangeDaysText();
        GameManager.Instance.AddChangeDaysCallBack(ChangeDaysText);
        
        ChangeZombieGraph();
        GameManager.Instance.AddChangeTotalInfectionCallBack(ChangeZombieGraph);
        
        ChangeGeneText();
        GameManager.Instance.AddChangeGeneCallBack(ChangeGeneText);
    }

    private void ChangeDaysText()
    {
        _daysNumText.text =
            String.Format(_list[_idList["Days_Num"]]["String"].ToString(), GameManager.Instance.days);
    }

    private void ChangeZombieGraph()
    {
        var zombiePercentageValue = GameManager.Instance.totalInfectionCount / GameManager.Instance.totalPeopleCount;

        _zombiePercentageText.text =
            String.Format(_list[_idList["Zombie_Percentage_World"]]["String"].ToString(), zombiePercentageValue * 100);
        _zombieGraphBarImage.fillAmount = zombiePercentageValue;
    }

    private void ChangeCureGraph()
    {
        /*var curePercentage

        _curePercentageText.text =
            String.Format(list[idList["Cure_Percentage"]]["String"].ToString(), ???);
        _cureGraphBarImage.fillAmount = ???;*/
    }

    private void ChangeGeneText()
    {
        _geneNumText.text =
            String.Format(_list[_idList["Gene_Num"]]["String"].ToString(), GameManager.Instance.gene);
    }
    
    public void ShowCountrySelect(string country)
    {
        if (!transform.Find("CountrySelected(Clone)"))
        {
            GameObject obj = ObjectPools.Instance.GetPooledObject("CountrySelected");

            obj.GetComponent<ManageCountrySelected>().SetCountry(country);
            
            obj.transform.parent = transform;
            obj.transform.localPosition= new Vector3(0, 0, 0);
        }
        else
        {
            transform.Find("CountrySelected(Clone)").GetComponent<ManageCountrySelected>().SetCountry(country);
        }
    }
}
