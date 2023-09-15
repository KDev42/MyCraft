using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pool
{
    private PoolObject prefab;
    private Transform container;

    public bool autoExpand { get; set; } = true;

    public List<PoolObject> pool;

    private bool isActiveDefault;
    private DiContainer diContainer;

    public Pool(PoolObject prefab, int count, bool isZenjectInit, bool isActiveDefault = true, DiContainer diContainer = null)
    {
        this.diContainer = diContainer;
        this.prefab = prefab;
        container = null;
        this.isActiveDefault = isActiveDefault;

        CreatePool(count, isZenjectInit);
    }

    public Pool(PoolObject prefab, int count, Transform container,  bool isZenjectInit, bool isActiveDefault = true, DiContainer diContainer =null)
    {
        this.diContainer = diContainer;
        this.prefab = prefab;
        this.container = container;
        this.isActiveDefault = isActiveDefault;

        CreatePool(count, isZenjectInit);
    }

    public bool HasFreeElemant(out PoolObject element)
    {
        foreach (var obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                element = obj;

                obj.gameObject.SetActive(isActiveDefault);
                return true;
            }
        }

        element = null;
        return false;
    }

    public PoolObject GetFreeElement(bool isZenjectInit, bool isActiveDefault = true)
    {
        if (HasFreeElemant(out var element))
            return element;

        if (autoExpand)
            return CreateObject(isZenjectInit,isActiveDefault);

        throw new System.Exception($"Pool is overflow");
    }

    private void CreatePool(int count,bool isZenjectInit)
    {
        pool = new List<PoolObject>();

        for (int i = 0; i < count; i++)
        {
            CreateObject( isZenjectInit);
        }
    }

    private PoolObject CreateObject(bool isZenjectInit, bool isActiveDefault = false)
    {
        PoolObject createObject;
        if (isZenjectInit)
            createObject = diContainer.InstantiatePrefab(prefab, new Vector3(0,0,0), new Quaternion(0, 0, 0, 0), container).GetComponent<PoolObject>();
        else
            createObject = Object.Instantiate(prefab, container);
        createObject.gameObject.SetActive(isActiveDefault);
        createObject.Container = container;
        pool.Add(createObject);
        return createObject;
    }
}