using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePanel : MonoBehaviour
{
    public GameObject zombieGen;
    public GameObject setting;
    public GameObject tutorial;

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

    public void CloseTutorial()
    {
        tutorial.SetActive(false);
    }

    public void ShowNewsPopUp()
    {
        if (GameManager.Instance.days % 60 == 0)
        {
            var obj = ObjectPools.Instance.GetPooledObject("NewsPop-up");

            obj.transform.parent = transform.Find("News_popup");
            
            obj.transform.localPosition= new Vector3(0, 0, 0);
            obj.GetComponent<ManageNewsPopUp>().SetNewsPopUp();
        }
    }
}
