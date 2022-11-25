using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPools : MonoBehaviour
{
    public static ObjectPools Instance;

    public GameObject[] poolPrefabs;

    public int poolingCount;

    Dictionary<object, List<GameObject>> pooledObjects = new Dictionary<object, List<GameObject>>();
    Dictionary<string, int> NameToIndex = new Dictionary<string, int>();

    private void Awake()
    {
        Instance = this;

        CreateMultiplePoolObjects();
    }

    public void CreateMultiplePoolObjects()
    {
        for (int i = 0; i < poolPrefabs.Length; i++)
        {
            for (int j = 0; j < poolingCount; j++)
            {
                CreatePoolObjects(i);
            }
        }
    }

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


    // 오브젝트를 풀에서 가져옴
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
            

            // 용량이 꽉차 새로운 오브젝트를 생성할 필요가 생김
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
    
    // 오브젝트를 해제해 풀로 되돌려 놓음
    public void ReleaseObjectToPool(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(Instance.transform);
        go.transform.localPosition = Vector3.zero;
    }
}
