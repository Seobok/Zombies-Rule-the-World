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
    
    private const float ContagiousProbability = 10f;
    private const float CureDevelopProbability = 10f;
    
    public const int Contagious = 0;
    public const int CureDevelop = 1;
    public const int PeopleCount = 2;
    public const int InfectionCount = 3;
    public const int CureDevelopRate = 4;

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
    
    [HideInInspector] public float topInfection = 0;
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
        
            var csvList = CSVReader.Read("CSVs/CountryInfo");

            foreach (var country in csvList)
            {
                var tmp = new List<float>();
                tmp.Add((ContagiousProbability + int.Parse(country["전염성"].ToString())) / 100);             //Contagious
                tmp.Add((CureDevelopProbability + int.Parse(country["치료제 개발 수치"].ToString())) / 100);    //CureDevelop
                tmp.Add(100);                                                                         //PeopleCount
                tmp.Add(0);                                                                           //InfectionCount
                tmp.Add(0);                                                                           //CureDevelopRate

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
            if (value.Value[InfectionCount] / value.Value[PeopleCount] < 1)
            {
                if (value.Value[InfectionCount] == 0)
                {
                    if (topInfection >= 0.1)
                    {
                        if (Random.Range(0f, 1f) <= 0.1f)
                        {
                            value.Value[InfectionCount]++;
                        }
                    }
                }
                else
                {
                    if (value.Value[Contagious] >= Random.Range(0f, 1f))
                    {
                        value.Value[InfectionCount] += 1;   //얼마나 증가하는지 모름
                    }
                }
            }

            if (totalInfectionCount / totalPeopleCount <= 0.1 && value.Value[InfectionCount]/value.Value[PeopleCount] <= 0.1)
            {
                if (value.Value[CureDevelop] >= Random.Range(0f, 1f))
                {
                    value.Value[CureDevelopRate] += 0.01f;
                }

                if (days % 2 == 0 && value.Value[CureDevelopRate] >= Random.Range(0f, 1f))
                {
                    value.Value[Contagious] -= 0.01f;
                }
            }
        }

        if (totalInfectionCount / totalPeopleCount >= 1 || totalInfectionCount / totalPeopleCount <= 0)
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
