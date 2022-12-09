using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageCountrySelected : MonoBehaviour
{
    private string _country = "";
    [SerializeField] private GameObject countryName;
    private TextMeshProUGUI countryNameText;
    [SerializeField] private GameObject zombieGraph;
    private Image zombieGraphImage;

    public GameObject selectCountryBtn;

    private void Awake()
    {
        countryNameText = countryName.GetComponent<TextMeshProUGUI>();
        zombieGraphImage = zombieGraph.GetComponent<Image>();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            countryNameText.text = _country;
            zombieGraphImage.fillAmount =
                GameManager.Instance.Country[_country][GameManager.InfectionCount] /
                GameManager.Instance.Country[_country][GameManager.PeopleCount];
        }
    }

    public void ShowCountryInfo()
    {
        var info = transform.Find("CountryInfo(Clone)");
        if (!info)
        {
            GameObject obj = ObjectPools.Instance.GetPooledObject("CountryInfo");
            
            obj.transform.parent = transform;
            obj.transform.position= new Vector3(0, 0, 0);
            
            obj.GetComponent<ManageCountryInfo>().SetInfo(_country);
        }
        else
        {
            info.GetComponent<ManageCountryInfo>().SetInfo(_country);
        }
    }

    public void SetCountry(string country)
    {
        _country = country;
        
        countryNameText.text = _country;
        zombieGraphImage.fillAmount =
            GameManager.Instance.Country[_country][GameManager.InfectionCount] /
            GameManager.Instance.Country[_country][GameManager.PeopleCount];
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
