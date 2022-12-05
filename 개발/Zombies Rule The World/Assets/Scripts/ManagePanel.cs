using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePanel : MonoBehaviour
{
    public GameObject zombieGen;
    public GameObject setting;

    private void Awake()
    {
        GameManager.Instance.AddChangeDaysCallBack(ShowNewsPopUp);
    }

    public void ShowZombieGen()
    {
        zombieGen.SetActive(true);
    }

    public void CloseZombieGen()
    {
        zombieGen.SetActive(false);
    }

    public void ShowSetting()
    {
        setting.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseSetting()
    {
        setting.SetActive(false);
        Time.timeScale = GameManager.Instance.gameSpeed;
    }

    public void ShowNewsPopUp()
    {
        if (GameManager.Instance.days % 60 == 0)
        {
            var obj = ObjectPools.Instance.GetPooledObject("NewsPop-up");

            obj.transform.parent = transform;
            
            obj.transform.localPosition= new Vector3(0, 0, 0);
            obj.GetComponent<ManageNewsPopUp>().SetNewsPopUp();
        }
    }
}
