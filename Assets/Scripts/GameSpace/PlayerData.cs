using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData 
{
    public static Inventory HandInventory { get; private set; }
    public static Inventory MainInventory { get; private set; }

    public static void Initialize(ItemStack[] handInventory, ItemStack[] mainInventory)
    {
        HandInventory = new Inventory(handInventory);
        MainInventory = new Inventory(mainInventory);
    }

    public static void RemoveItem(ItemType itemType, int amount)
    {
        int index ;
        int removedAmount;
        while (amount > 0)
        {
            if(MainInventory.HasItemType(itemType, out index))
            {
                MainInventory.RemoveItem(index, amount, out removedAmount);
                amount -= removedAmount;
            }
            else if (HandInventory.HasItemType(itemType, out index))
            {
                HandInventory.RemoveItem(index, amount, out removedAmount);
                amount -= removedAmount;
                EventsHolder.ChangeToolbar(index);
            }
            else
            {
                break;
            }
        }
    }

    public static void AddToHandInventory( Item item, out bool canAdd)
    {
        int index = 0;
        canAdd = HandInventory.CanAddItem(ref index, item);
        if(canAdd)
        {
            AddToHand(index, item);
        }
        else if(MainInventory.CanAddItem(ref index, item))
        {
            canAdd = true;
            AddToInventory(index, item);
        }
        else
        {
            canAdd = false;
        }
    }

    public static void AddToMainInventory( Item item, out bool canAdd)
    {
        int index = 0;
        canAdd = MainInventory.CanAddItem(ref index, item);
        if (canAdd)
        {
            AddToInventory(index, item);
        }
        else if (HandInventory.CanAddItem(ref index, item))
        {
            canAdd = true;
            AddToHand(index, item);
        }
        else
        {
            canAdd = false;
        }
    }

    private static void AddToHand(int index, Item item)
    {
        HandInventory.AddItem(index, item, 1);
        EventsHolder.ChangeToolbar(index);
    }

    private static void AddToInventory(int index, Item item)
    {
        MainInventory.AddItem(index, item, 1);
    }
}
