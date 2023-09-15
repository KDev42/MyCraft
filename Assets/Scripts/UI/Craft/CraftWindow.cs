using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CraftWindow : MonoBehaviour
{
    [SerializeField] List<Item> blocks;
    [SerializeField] List<Item> tools;
    [SerializeField] PoolObject craftPanelPrefab;
    [SerializeField] Transform container;

    private UIFactory uIFactory;
    private List<CraftItem> craftItems =new List<CraftItem>();

    [Inject]
    private void Construct(UIFactory uIFactory)
    {
        this.uIFactory = uIFactory;
    }

    private void OnEnable()
    {
        ActivateImes(blocks);
    }

    private void OnDisable()
    {
        foreach(CraftItem craft in craftItems)
        {
            craft.GetComponent<PoolObject>().ReturnToPool();
        }
    }

    private void ActivateImes(List<Item> items)
    {
        craftItems.Clear();
        for (int i =0; i < items.Count; i++)
        {
            CraftItem craftItem = uIFactory.SpawnUIElement(craftPanelPrefab, container).GetComponent<CraftItem>();
            craftItem.Activation(items[i]);
            craftItems.Add(craftItem);
        }
    }
}
