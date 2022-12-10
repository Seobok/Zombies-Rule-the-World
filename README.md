# Zombies Rule the World
___
Zombies Rule the World는 모바일 2D 시뮬레이션 게임입니다.
게임 목표는 전 세계에 좀비바이러스를 퍼트리는 것입니다.

### 다운로드
[구글드라이브](https://drive.google.com/file/d/1Z0_M6dqSRbJ82GXiuO8Co859JoLpQjoB/view?usp=sharing)  
압축을 풀고 Zombies_Rule_the_Word.exe 파일로 실행할 수 있습니다.

### 게임방법
1. 게임시작  
        i. 시작화면
                시작버튼 선택 -> 게임 시작
                종료버튼 선택 -> 게임 종료
        ![image](https://user-images.githubusercontent.com/100834254/206835320-bb14016b-d364-48fa-a9b1-1fdcb257623e.png)
        
2. 인게임
        - 디폴트화면
                대륙을 선택하여 게임을 시작합니다.
                대륙별로 감염률과 치료제개발확률의 시작값이 달라집니다.
                게임이 진행되면서 유전자를 획득할 수 있습니다.
                좌측 하단에는 게임 상에서 지나간 날의 수가 표시됩니다.
                우측 하단에는 획득한 유전자 수가 표시됩니다.
                우측 상단에는 전세계 감염률과 치료제개발률이 표시됩니다.
                하단에는 국가명과 국가별 감염률이 표시됩니다.
                국가명 옆의 +버튼을 누르면 해당 국가의 기후 정보가 표시됩니다.
        ![image](https://user-images.githubusercontent.com/100834254/206835362-60e5f8b6-0a9b-4b03-816e-ff9fc1b88699.png)
        
        - 증상 편집 화면
                창 좌측에서 획득한 유전자 수를 확인할 수 있습니다.
                증상을 선택하여 유전자를 소모해 증상을 구매할 수 있습니다.
                증상을 구매하면 증상 별로 전염성과 치료제개발확률이 변합니다.
        ![image](https://user-images.githubusercontent.com/100834254/206835398-b496f667-8224-4439-8bec-a7b336358867.png)
        
        - 뉴스
                랜덤으로 뉴스가 표시됩니다.
                각 뉴스 별로 전염성과 치료제개발확률이 변합니다.
        ![image](https://user-images.githubusercontent.com/100834254/206835438-3484f19a-7081-4401-8eb4-5c3aa2bba48b.png)
        
3. 게임종료
        - 치료제 개발률이나 전세계 감염률이 100%가 되면 게임이 종료됩니다.
        - 치료제 개발률이 100%로 끝나면 You Lose로 끝나고 전세계 감염률이 100%가 되면 You Win으로 엔딩이 나옵니다.
        - 게임이 종료되면 게임을 다시 시작할지, 게임을 종료할지 선택할 수 있습니다.
        ![image](https://user-images.githubusercontent.com/100834254/206835613-1e74591c-1a60-43f6-bf35-a8436a4e43bf.png)
        
### 코드설명
1. Singletone Pattern  
게임에 관련된 다양한 변수를 관리하는 스크립트인 GameManager와 오브젝트 풀을 관리하는 스크립트인 ObjectPools는 **static**으로 작성하는 것이 올바르기 때문에 Singletone으로 제작 하였습니다.
```
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
```

2. ObjectPools
증상 편집 화면의 아이템들은 동적으로 생성되어야 하기 때문에 객체를 게임내에서 코드로 생성해야 하지만 이 행위가 자주 발생하면 메모리를 많이 잡아먹기 때문에 **여러 아이템을 미리 생성하고 필요할때 꺼내쓰는** 오브젝트 풀 방식을 사용하였습니다.
```
public void CreatePoolObjects(int idx)
    {
        if (!pooledObjects.ContainsKey(poolPrefabs[idx].name))
        {
            List<GameObject> NewList = new();
            pooledObjects.Add(poolPrefabs[idx].name, NewList);
            NameToIndex.Add(poolPrefabs[idx].name, idx);
        }

        GameObject newDoll = Instantiate(poolPrefabs[idx], Instance.transform);
        newDoll.SetActive(false);
        pooledObjects[poolPrefabs[idx].name].Add(newDoll);
    }
```
```
public GameObject GetPooledObject(string _name)
    {

        if (pooledObjects.ContainsKey(_name))
        {
            for (int i = 0; i < pooledObjects[_name].Count; i++)
            {
                if (!pooledObjects[_name][i].activeSelf)
                {
                    pooledObjects[_name][i].SetActive(true);
                    return pooledObjects[_name][i];
                }
            }
            
            int beforeCreateCount = pooledObjects[_name].Count;

            CreatePoolObjects(NameToIndex[_name]);

            pooledObjects[_name][beforeCreateCount].SetActive(true);
            return pooledObjects[_name][beforeCreateCount];
        }
        else
        {
            return null;
        }
    }
```

3.CSV파싱  
개발과 기획이 나누어져있는 프로젝트의 특성상 기획이 주는 데이터를 바로 적용시킬수 있도록 **CSV파일을 파싱**하는 스크립트와 **id를 쉽게 파싱**하여 사용할수 있는 함수를 제작하였습니다.
```
public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load (file) as TextAsset;

        var lines = Regex.Split (data.text, LINE_SPLIT_RE);

        if(lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for(var i=1; i < lines.Length; i++) {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if(values.Length == 0 ||values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for(var j=0; j < header.Length && j < values.Length; j++ ) {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if(int.TryParse(value, out n)) {
                    finalvalue = n;
                } else if (float.TryParse(value, out f)) {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add (entry);
        }
        return list;
    }
```
```
public static Dictionary<string, int> ReadID(string file)
    {
        var list = new Dictionary<string, int>();
        var cnt = 0;
        
        TextAsset data = Resources.Load (file) as TextAsset;
        
        var lines = Regex.Split (data.text, LINE_SPLIT_RE);
        
        if(lines.Length <= 1) return list;
        
        var header = Regex.Split(lines[0], SPLIT_RE);
        for(var i=1; i < lines.Length; i++) {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if(values.Length == 0 ||values[0] == "") continue;

            string value = values[0];
            value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            
            list.Add (value, cnt++);
        }
        return list;
    }
```

4.ChangeText  
간편하게 텍스트를 바꿀수 있도록 StringUI.csv파일을 파싱하여 해당 이름의 텍스트를 자동으로 바꾸어 주는 코드입니다.
```
List<Dictionary<string, object>> list = CSVReader.Read("CSVs/StringUI");
        Dictionary<string, int> idList = CSVReader.ReadID("CSVs/StringUI");
        
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        
        foreach (var txt in texts)
        {
            var key = txt.transform.name.Trim();
            if(idList.ContainsKey(key))
            {
                txt.text = list[idList[key]]["String"].ToString();
            }
        }
```

5. 게임 진행 코드  
게임의 특성상 일정한 시간 주기로 게임이 진행되는데 이를 위하여 GameManager스크립트에서
Coroutine을 사용하여 일정시간마다 재귀적으로 함수를 실행하여 구현
```
private IEnumerator IEUpdate()
    {
        days++;

        foreach (var value in Country)
        {
            if (value.Value[InfectionCount] / value.Value[PeopleCount] < 1)     //감염률 < 100%
            {
                if (value.Value[InfectionCount] == 0)                           //감염자가 0
                {
                    if (topInfectionRate >= 감염률)    //가장 감염자 수가 많은 대륙의 감염률
                    {
                        if (Random.Range(0f, 1f) <= 대륙간감염확률)
                        {
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
```
        
### 데모영상
https://user-images.githubusercontent.com/100834254/206836615-27c5978c-1f62-4456-97d8-b771d3203e32.mp4

## Reference
게임 컨셉 : Plague inc.  
#### 코드 관련
[CSVReader](https://github.com/tikonen/blog/blob/master/csvreader/CSVReader.cs)  
[AlphaBtn](https://sensol2.tistory.com/35)  
[FadeIn](https://m.blog.naver.com/soyokaze75/222033497772)  
#### 이미지
[Sci-fi GUI skin](https://assetstore.unity.com/packages/2d/gui/sci-fi-gui-skin-15606)  
[Simple Button Set 02](https://assetstore.unity.com/packages/2d/gui/icons/simple-button-set-02-184903)  
[Syringe free icon](https://www.flaticon.com/free-icon/syringe_1778916?related_id=1778916&origin=search)  
[Zombie free icon](https://www.flaticon.com/free-icon/zombie_3249360?related_id=3249360&origin=search)  
[Dna free icon](https://www.flaticon.com/free-icon/dna_1694118?term=gene&page=1&position=2&page=1&position=2&related_id=1694118&origin=search)  
[Cardiology free icon](https://www.flaticon.com/free-icon/cardiology_1863303?term=alive&page=1&position=14&page=1&position=14&related_id=1863303&origin=search)  

#### 사운드
[Universe Sounds Free Pack](https://assetstore.unity.com/packages/audio/ambient/sci-fi/universe-sounds-free-pack-118865)  
[SciFi UI Sound FX](https://assetstore.unity.com/packages/audio/sound-fx/scifi-ui-sound-fx-27282)  
[FREE Casual Game SFX Pack](https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116)
#### 폰트
[Rubik Glitch](https://fonts.google.com/specimen/Rubik+Glitch?preview.text=Zombies%20Rule%20the%20World&preview.text_type=custom)  
[Dosis](https://fonts.google.com/specimen/Dosis)  
[한림고딕체](https://www.sandollcloud.com/font/17644.html)

### 개발환경
unity(2021.3.13f1 버전)를 통해 제작되었습니다.
