using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private PoolObject prefab;
    private Transform container;

    public bool autoExpand { get; set; } = true;

    public List<PoolObject> pool;

    private bool isActiveDefault;

    public Pool(PoolObject prefab, int count, bool isActiveDefault = true)
    {
        this.prefab = prefab;
        container = null;
        this.isActiveDefault = isActiveDefault;

        CreatePool(count);
    }

    public Pool(PoolObject prefab, int count, Transform container, bool isActiveDefault = true)
    {
        this.prefab = prefab;
        this.container = container;
        this.isActiveDefault = isActiveDefault;

        CreatePool(count);
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

    public PoolObject GetFreeElement(bool isActiveDefault = true)
    {
        if (HasFreeElemant(out var element))
            return element;

        if (autoExpand)
            return CreateObject(isActiveDefault);

        throw new System.Exception($"Pool is overflow");
    }

    private void CreatePool(int count)
    {
        pool = new List<PoolObject>();

        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private PoolObject CreateObject(bool isActiveDefault = false)
    {
        PoolObject createObject = Object.Instantiate(prefab, container);
        createObject.gameObject.SetActive(isActiveDefault);
        createObject.Container = container;
        pool.Add(createObject);
        return createObject;
    }
}