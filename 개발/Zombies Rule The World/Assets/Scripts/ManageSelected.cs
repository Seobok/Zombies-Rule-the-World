using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSelected : MonoBehaviour
{
    public void Cancel()
    {
        ObjectPools.Instance.ReleaseObjectToPool(gameObject);
    }

    public void Purchase()
    {
        ObjectPools.Instance.ReleaseObjectToPool(gameObject);
    }
}
