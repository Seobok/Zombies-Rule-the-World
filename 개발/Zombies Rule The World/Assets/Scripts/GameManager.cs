using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(Instance.gameObject);
        
        InitCountry();
    }
    
    private const float DefaultContagiousProbability = 10f;
    private const float DefaultCureDevelopProbability = 10f;
    
    public const int Contagious = 0;
    public const int CureDevelopAmount = 1;
    public const int PeopleCount = 2;
    public const int InfectionCount = 3;

    private int _days = 0;
    public int days
    {
        get
        {
            return _days;
        }
        set
        {
            _changeDaysCallBack?.Invoke();
            _days = value;
        }
    }
    
    private int _gene = 0;
    public int gene
    {
        get
        {
            return _gene;
        }
        set
        {
            _changeGeneCallBack?.Invoke();
            _gene = value;
        }
    }
    
    [HideInInspector] public string topInfectionCountry = "";
    [HideInInspector] public int totalPeopleCount = 0;
    private int _totalInfectionCount = 0;
    public int totalInfectionCount
    {
        get
        {
            return _totalInfectionCount;
        }
        set
        {
            _changeTotalInfectionAction?.Invoke();
            _totalInfectionCount = value;
        }
    }

    [HideInInspector] public float cureDevelopRate = 0f;
    [HideInInspector] public bool isCountryLoaded = false;
    
    private Action _changeDaysCallBack;

    public void AddChangeDaysCallBack(Action changeDaysAction)
    {
        _changeDaysCallBack += changeDaysAction;
    }

    private Action _changeTotalInfectionAction;

    public void AddChangeTotalInfectionCallBack(Action changeTotalInfectionAction)
    {
        _changeTotalInfectionAction += changeTotalInfectionAction;
    }

    private Action _changeGeneCallBack;

    public void AddChangeGeneCallBack(Action changeGeneCallBack)
    {
        _changeGeneCallBack += changeGeneCallBack;
    }
    
    public Dictionary<string, List<float>> Country;   //("대륙이름", ("전염성", "치료제 개발 수치", "대륙인구", "감염자 수", "치료제 개발률"))

    private void InitCountry()
    {
        if(!isCountryLoaded)
        {
            Country = new Dictionary<string, List<float>>();

            var countryInfoList = CSVReader.Read("CSVs/CountryInfo");

            foreach (var country in countryInfoList)
            {
                var tmp = new List<float>();
                tmp.Add((DefaultContagiousProbability + int.Parse(country["전염성"].ToString())) / 100);             //Contagious
                tmp.Add((DefaultCureDevelopProbability + int.Parse(country["치료제 개발 수치"].ToString())) / 100);    //CureDevelop
                tmp.Add(float.Parse(country["대륙인구"].ToString()));                                                 //PeopleCount
                tmp.Add(0);                                                                                         //InfectionCount

                totalPeopleCount += (int)tmp[2];
                totalInfectionCount += (int)tmp[3];
                
                Country.Add(country["대륙명"].ToString(),tmp);
            }

            isCountryLoaded = true;
        }
    }

    public void Play()
    {
        StartCoroutine(IEUpdate());
    }

    private IEnumerator IEUpdate()
    {
        days++;

        foreach (var value in Country)
        {
            if (value.Value[InfectionCount] / value.Value[PeopleCount] < 1)     //감염률 < 100%
            {
                if (value.Value[InfectionCount] == 0)                           //감염자가 0
                {
                    if (Country[topInfectionCountry][InfectionCount]/Country[topInfectionCountry][PeopleCount] >= 0.1)    //가장 감염자 수가 많은 대륙의 감염률이 10%이상
                    {
                        if (Random.Range(0f, 1f) <= 0.1f)                       //10%
                        {
                            value.Value[InfectionCount]++;                      //감염자 발생
                        }
                    }
                }
                else
                {                                                               //감염자가 존재
                    if (value.Value[Contagious] >= Random.Range(0f, 1f))        //전염성 %
                    {
                        value.Value[InfectionCount] += 1;                       //감염자 발생
                    }
                }
            }
        }
        
        if ((float)totalInfectionCount / totalPeopleCount <= 0.1)
        {       //전체감염률 > 10%
            if (Country[topInfectionCountry][CureDevelopAmount] >= Random.Range(0f, 1f))
            {       //최대 감염률 나라의 치료제개발수치 %
                cureDevelopRate += 0.01f;
            }
            
            if(days%2==0 && )
        }

        if ((float)totalInfectionCount / totalPeopleCount >= 1 || (float)totalInfectionCount / totalPeopleCount <= 0)
        {
            //게임종료
        }
        else
        {
            
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(IEUpdate());
    }
}
