using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CraftWindow : MonoBehaviour
{
    [SerializeField] PoolObject craftPanelPrefab;
    [SerializeField] Transform container;
    [SerializeField] TabCraftButton selectedButton;

    //private TabCraftButton currentButton;
    private UIFactory uIFactory;
    private List<CraftItem> craftItems =new List<CraftItem>();

    [Inject]
    private void Construct(UIFactory uIFactory)
    {
        this.uIFactory = uIFactory;
    }

    private void OnEnable()
    {
        ChangeButton(selectedButton);
    }

    public void ChangeButton(TabCraftButton newButton)
    {
        if(craftItems!=null)
            DeactivationItems();
        selectedButton.DeselectButton();
        selectedButton = newButton;
        selectedButton.SelectButton();
        ActivateImes(selectedButton.craftItems.items);
    }

    private void OnDisable()
    {
        DeactivationItems();
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

    private void DeactivationItems()
    {
        foreach (CraftItem craft in craftItems)
        {
            craft.GetComponent<PoolObject>().ReturnToPool();
        }
    }
}
