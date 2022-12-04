using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBtn : MonoBehaviour
{
    public void GetGene()
    {
        GameManager.Instance.gene += 100;
    }
}
