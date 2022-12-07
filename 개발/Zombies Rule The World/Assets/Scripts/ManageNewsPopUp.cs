using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManageNewsPopUp : MonoBehaviour
{
    public GameObject newsTitle;
    private TextMeshProUGUI _newsTitleText;
    public GameObject newsText;
    private TextMeshProUGUI _newsTextText;

    private List<Dictionary<string, object>> _list;

    private void Awake()
    {
        _newsTitleText = newsTitle.GetComponent<TextMeshProUGUI>();
        _newsTextText = newsText.GetComponent<TextMeshProUGUI>();

        _list = CSVReader.Read("CSVs/News");
    }

    public void SetNewsPopUp()
    {
        Dictionary<string, object> newsItem = null;
        
        foreach (var item in _list)
        {
            if (float.Parse(item["발현 확률"].ToString()) >= Random.Range(0f, 1f))
            {
                if (newsItem == null)
                {
                    newsItem = item;
                }
                else
                {
                    if (float.Parse(newsItem["발현 확률"].ToString()) > float.Parse(item["발현 확률"].ToString()))
                    {
                        newsItem = item;
                    }
                }
            }
        }
        
        _newsTitleText.text = newsItem["News Title"].ToString();
        _newsTextText.text = newsItem["News Text"].ToString();

        GameManager.Instance.contagious += float.Parse(newsItem["전염성"].ToString());
        GameManager.Instance.cureDevelopProbability += float.Parse(newsItem["치료제 개발 수치"].ToString());
        
        if(GameManager.Instance.isEffect)
            GetComponent<AudioSource>().Play();
    }

    public void ClosePopUp()
    {
        ObjectPools.Instance.ReleaseObjectToPool(gameObject);
    }
}
