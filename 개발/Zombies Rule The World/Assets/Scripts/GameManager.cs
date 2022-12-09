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
        
        InitCountry();
    }
    
    public const int PeopleCount = 0;
    public const int InfectionCount = 1;

    public float 감염률 = 0.05f;
    public int 인구비 = 5000;
    public int 대륙내감염 = 1000;
    public float 대륙간감염확률 = 0.2f;
    public float 치료제개발률 = 0.005f;
    public float 전염성 = 0.005f;

    public int geneCnt = 0;

    private int _days = 0;
    public int days
    {
        get
        {
            return _days;
        }
        set
        {
            _days = value;
            _changeDaysCallBack?.Invoke();
        }
    }
    
    private int _gene = 10;
    public int gene
    {
        get
        {
            return _gene;
        }
        set
        {
            _gene = value;
            _changeGeneCallBack?.Invoke();
        }
    }
    
    [HideInInspector] public float topInfectionRate = 0f;
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
            _totalInfectionCount = value;
            _changeTotalInfectionAction?.Invoke();
        }
    }

    private List<float> _addCureDevelopProbability = new List<float>();
    public void AddCureDevelopProbability(float item)
    {
        _addCureDevelopProbability.Add(item);
    }
    
    private float _cureDevelopProbability = 0.1f;

    public float cureDevelopProbability
    {
        get
        {
            var tmp = _cureDevelopProbability;
            foreach (var item in _addCureDevelopProbability)
            {
                tmp += item;
            }
            return tmp;
        }
        set
        {
            _cureDevelopProbability = value;
        }
    }
    
    private List<float> _addContagious = new List<float>();
    public void AddContagious(float item)
    {
        _addContagious.Add(item);
    }
    private float _contagious = 0.1f;

    public float contagious
    {
        get
        {
            var tmp = _contagious;
            foreach (var item in _addContagious)
            {
                tmp += item;
            }
            return tmp;
        }
        set
        {
            _contagious = value;
        }
    }
    
    private float _cureDevelopRate = 0f;

    public float cureDevelopRate
    {
        get
        {
            return _cureDevelopRate;
        }
        set
        {
            _cureDevelopRate = value;
            _changeCureDevelopRateCallBack?.Invoke();
        }
    }
    [HideInInspector] public bool isCountryLoaded = false;
    [HideInInspector] public float gameSpeed = 1f;
    public bool isEffect = true;
    [HideInInspector] public bool isSelectCountry = false;

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

    private Action _changeCureDevelopRateCallBack;

    public void AddChangeCureDevelopRateCallBack(Action changeCureDevelopRateCallBack)
    {
        _changeCureDevelopRateCallBack += changeCureDevelopRateCallBack;
    }
    
    public Dictionary<string, List<float>> Country;   //("대륙이름", ("대륙인구", "감염자 수"))

    private void InitCountry()
    {
        if(!isCountryLoaded)
        {
            Country = new Dictionary<string, List<float>>();

            var countryInfoList = CSVReader.Read("CSVs/CountryInfo");

            foreach (var country in countryInfoList)
            {
                var tmp = new List<float>();
                tmp.Add(float.Parse(country["대륙인구"].ToString())/인구비);                                                 //PeopleCount
                tmp.Add(0);                                                                                         //InfectionCount
                
                totalPeopleCount += (int)tmp[0];
                totalInfectionCount += (int)tmp[1];
                
                Country.Add(country["대륙명"].ToString(),tmp);
            }
            
            Debug.Log("전체인원수 : " + totalPeopleCount);

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
                    if (topInfectionRate >= 감염률)    //가장 감염자 수가 많은 대륙의 감염률이 10%이상
                    {
                        if (Random.Range(0f, 1f) <= 대륙간감염확률)                       //10%
                        {
                            Debug.Log("대륙간전염! +1");
                            value.Value[InfectionCount]++;                      //감염자 발생
                            if (topInfectionRate < value.Value[InfectionCount] / value.Value[PeopleCount])
                            {
                                topInfectionRate = value.Value[InfectionCount] / value.Value[PeopleCount];
                            }
                            totalInfectionCount += 1;
                            gene += 1;
                        }
                    }
                }
                else
                {                                                               //감염자가 존재
                    if (contagious >= Random.Range(0f, 1f))        //전염성 %
                    {
                        Debug.Log("전염! +1000");
                        value.Value[InfectionCount] += 대륙내감염;                       //감염자 발생
                        if (topInfectionRate < value.Value[InfectionCount] / value.Value[PeopleCount])
                        {
                            topInfectionRate = value.Value[InfectionCount] / value.Value[PeopleCount];
                        }
                        totalInfectionCount += 1000;
                    }
                }
            }
        }

        int intInfectionRate = (int)Math.Floor(((float)totalInfectionCount / totalPeopleCount) * 100);
        
        if (intInfectionRate > geneCnt)
        {
            gene += intInfectionRate - geneCnt;
            geneCnt = intInfectionRate;
        }
        
        if ((float)totalInfectionCount / totalPeopleCount >= 0.1)
        {       //전체감염률 > 10%
            if (cureDevelopProbability >= Random.Range(0f, 1f))
            {
                cureDevelopRate += 치료제개발률;
                if (cureDevelopRate > 1f)
                    cureDevelopRate = 1f;
            }

            if (days % 2 == 0 && cureDevelopRate >= Random.Range(0f, 1f))
            {
                contagious -= 전염성;
                if (contagious < 0f)
                    contagious = 0f;
            }
        }

        if (totalInfectionCount >= totalPeopleCount)
        {
            SceneManager.LoadScene("Scenes/EndWin");
        }

        if (cureDevelopRate >= 1f)
        {
            SceneManager.LoadScene("Scenes/EndLose");
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(IEUpdate());
    }
}
