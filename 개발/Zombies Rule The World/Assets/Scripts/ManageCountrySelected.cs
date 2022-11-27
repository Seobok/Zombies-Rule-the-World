using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCountrySelected : MonoBehaviour
{
    public void ShowCountryInfo()
    {
        if (!transform.Find("CountryInfo(Clone)"))
        {
            GameObject obj = ObjectPools.Instance.GetPooledObject("CountryInfo");

            obj.transform.parent = transform;
            obj.transform.position= new Vector3(0, 0, 0);
        }
    }
}
