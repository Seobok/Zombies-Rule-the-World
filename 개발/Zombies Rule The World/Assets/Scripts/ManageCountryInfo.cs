using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCountryInfo : MonoBehaviour
{
    public void CloseCountryInfo()
    {
        ObjectPools.Instance.ReleaseObjectToPool(gameObject);
    }
}
