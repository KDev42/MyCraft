using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    public ItemStack[] items;

    public Inventory(int lengh)
    {
        items = new ItemStack[lengh];
    }

    public bool HasFreeIndex(ref int freeIndex)
    {
        for(int i=0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                freeIndex = i;
                return true;
            }
        }

        return false;
    }

    public bool CanAddItem(ref int index, Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemType == item.itemType && items[i].amount < item.maxNumberStacks)
            {
                index = i;
                return true;
            }
        }

        return HasFreeIndex(ref index);
    }

    public void AddItem(int index, Item item, int amount)
    {
        if (items[index] == null)
        {
            ItemStack itemStack = new ItemStack(item.itemType, amount);
            items[index] = itemStack;
        }
        else
        {
            items[index].amount++;
        }
    }

    public void SwapItems(int index, ItemStack itemStack)
    {
        //ItemStack cloneItemStack = new ItemStack(itemStack.itemType, itemStack.amount);

        items[index] = itemStack;
    }
}
