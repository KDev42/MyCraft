using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIFactory : MonoBehaviour
{
    [SerializeField] RectTransform container;

    [Inject] DiContainer diContainer;
    private Dictionary<PoolObject, Pool> itemsPool = new Dictionary<PoolObject, Pool>();

    public GameObject SpawnUIElement(PoolObject elementPrefab, Transform parent)
    {
        GameObject uiElement = GetObject(elementPrefab);
        uiElement.transform.SetParent(parent);
        uiElement.SetActive(true);

        return uiElement;
    }

    private GameObject GetObject(PoolObject objectPrefab)
    {
        if (!itemsPool.ContainsKey(objectPrefab))
        {
            itemsPool.Add(objectPrefab, new Pool(objectPrefab, 4, container, true, false, diContainer));
        }

        return itemsPool[objectPrefab].GetFreeElement(true, false).gameObject;
    }
}
