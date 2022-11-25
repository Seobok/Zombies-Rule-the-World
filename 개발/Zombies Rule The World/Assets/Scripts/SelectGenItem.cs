using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGenItem : MonoBehaviour
{
    public void ShowSelected()
    {
        GameObject obj = ObjectPools.Instance.GetPooledObject("Selected");
        obj.transform.parent = gameObject.transform.parent.parent.parent.parent.parent;

        obj.transform.localPosition= new Vector3(0, 0, 0);
    }
}
