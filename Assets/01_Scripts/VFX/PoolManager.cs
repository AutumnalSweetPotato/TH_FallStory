using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Single<PoolManager>
{
    private Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();
    [SerializeField] private Pool[] pool;
    [SerializeField] private Transform objectPoolTransform;
    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
    }

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            CreatePool(pool[i].prefab, pool[i].poolSize);
        }
    }

    private void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();
        string prefabName = prefab.name;
        
        GameObject parentGameObject = new GameObject(prefabName + "Anchor");
        parentGameObject.transform.SetParent(objectPoolTransform);

        if(!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey,new Queue<GameObject>());
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab, parentGameObject.transform);
                obj.SetActive(false);
                poolDictionary[poolKey].Enqueue(obj);
            }
        }
    }

    public GameObject ReuseObject(GameObject prefab,Vector3 position,Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();
        if(poolDictionary.ContainsKey(poolKey))
        {
            GameObject objectToReuse = GetObjectFromPool(poolKey);
            ResetObject(objectToReuse, position, rotation,prefab);

            return objectToReuse;
        }
        else
        {
            return null;
        }
    }
    private GameObject GetObjectFromPool(int poolKey)
    {
        GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(objectToReuse);
        if(objectToReuse.activeSelf == true)
        {
            objectToReuse.SetActive(false);
        }
        return objectToReuse;
    }

    private void ResetObject(GameObject objectToReuse, Vector3 position, Quaternion rotation, GameObject prefab)
    {
        objectToReuse.transform.position = position;
        objectToReuse.transform.rotation = rotation;
        objectToReuse.transform.localScale = prefab.transform.localScale;
    }

}
