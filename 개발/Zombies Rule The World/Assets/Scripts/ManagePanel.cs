using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePanel : MonoBehaviour
{
    public GameObject zombieGen;
    public void ShowZombieGen()
    {
        zombieGen.SetActive(true);
    }
}
