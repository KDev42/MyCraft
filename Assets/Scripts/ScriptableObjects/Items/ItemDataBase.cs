using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Data Base")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] List<Item> items;

    private Dictionary<ItemType, Item> itemsCashed = new Dictionary<ItemType, Item>();

    private void OnEnable()
    {
        itemsCashed.Clear();
        foreach (Item item in items)
        {
            itemsCashed.Add(item.itemType, item);
        }
    }

    public Item GetItem(ItemType type)
    {
        if (itemsCashed.TryGetValue(type, out Item item))
            return item;

        return null;
    }
}
