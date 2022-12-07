using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageCountrySelected : MonoBehaviour
{
    private string _country = "";
    [SerializeField] private GameObject countryName;
    [SerializeField] private GameObject zombieGraph;

    public GameObject selectCountryBtn;
    
    public void ShowCountryInfo()
    {
        if (!transform.Find("CountryInfo(Clone)"))
        {
            GameObject obj = ObjectPools.Instance.GetPooledObject("CountryInfo");

            obj.transform.parent = transform;
            obj.transform.position= new Vector3(0, 0, 0);
        }
    }

    public void SetCountry(string country)
    {
        _country = country;
        
        countryName.GetComponent<TextMeshProUGUI>().text = _country;
        zombieGraph.GetComponent<Image>().fillAmount =
            GameManager.Instance.Country[_country][GameManager.InfectionCount] / GameManager.Instance.Country[_country][GameManager.PeopleCount];
    }

    public void SelectCountry()
    {
        var list = CSVReader.Read("CSVs/CountryInfo");
        var idList = CSVReader.ReadID("CSVs/CountryInfo");
        
        GameManager.Instance.isSelectCountry = true;

        GameManager.Instance.contagious += float.Parse(list[idList[_country]]["전염성"].ToString()) / 100;
        GameManager.Instance.cureDevelopProbability +=
            float.Parse(list[idList[_country]]["치료제 개발 수치"].ToString()) / 100;

        GameManager.Instance.Country[_country][1] += 1;
        GameManager.Instance.totalInfectionCount += 1;

        if (GameManager.Instance.Country[_country][1] / GameManager.Instance.Country[_country][0] >
            GameManager.Instance.topInfectionRate)
        {
            GameManager.Instance.topInfectionRate =
                GameManager.Instance.Country[_country][1] / GameManager.Instance.Country[_country][0];
        }

        GameManager.Instance.Play();
        selectCountryBtn.SetActive(false);
    }
}
