using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    public ItemStack[] Items { get; private set; }

    public Inventory(ItemStack[] items)
    {
        Items = items;
    }

    public bool HasFreeIndex(ref int freeIndex)
    {
        for(int i=0; i < Items.Length; i++)
        {
            if(Items[i] == null || Items[i].itemType ==0)
            {
                freeIndex = i;
                return true;
            }
        }

        return false;
    }

    public bool CanAddItem(ref int index, Item item)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != null  && Items[i].itemType == item.itemType && Items[i].amount < item.maxNumberStacks)
            {
                index = i;
                return true;
            }
        }

        return HasFreeIndex(ref index);
    }

    public void AddItem(int index, Item item, int amount)
    {
        if (Items[index] == null || Items[index].itemType == 0)
        {
            ItemStack itemStack = new ItemStack(item.itemType, amount);
            Items[index] = itemStack;
        }
        else
        {
            Items[index].amount++;
        }
    }

    public void RemoveItem(int index, int amount, out int removedAmount)
    {
        ItemStack item = Items[index];
        if (item.amount - amount <= 0)
        {
            removedAmount = item.amount;
            RemoveAllItem(index);
        }
        else
        {
            removedAmount = amount;
            item.amount -= amount;
        }
    }

    public void RemoveAllItem(int index)
    {
        Items[index] = null;
    }

    public void SwapItems(int index, ItemStack itemStack)
    {
        //ItemStack cloneItemStack = new ItemStack(itemStack.itemType, itemStack.amount);

        Items[index] = itemStack;
    }

    public int AmountItem(ItemType itemType)
    {
        int amount = 0;

        foreach (ItemStack item in Items)
        {
            if (item != null && item.itemType == itemType)
            {
                amount += item.amount;
            }
        }

        return amount;
    }

    public bool HasItemType(ItemType itemType, out int index)
    {
        index = 0;
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != null && Items[i].itemType == itemType)
            {
                index = i;
                return true;
            }
        }

        return false;
    }
}
