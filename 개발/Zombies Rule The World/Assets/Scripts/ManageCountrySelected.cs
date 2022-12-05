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
    [SerializeField] private GameObject countryState;
    
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
}
