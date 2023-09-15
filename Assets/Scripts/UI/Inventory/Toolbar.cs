using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Toolbar : InventoryUI
{
    [SerializeField] RectTransform highlight;

    private void OnEnable()
    {
        Activation();
        EventsHolder.changeActiveSlot += ChangeActiveSlot;
        EventsHolder.changeToolbar += ChangeSlot;
    }

    private void OnDisable()
    {
        EventsHolder.changeActiveSlot -= ChangeActiveSlot;
        EventsHolder.changeToolbar -= ChangeSlot;
    }

    private void ChangeActiveSlot(int slotIndex)
    {
        highlight.position = handSlots[slotIndex].transform.position;
        //player.selectedBlockIndex = itemSlots[slotIndex].itemID;
    }

    private void ChangeSlot(int slotIndex)
    {
        ItemStack itemStack = PlayerData.HandInventory.Items[slotIndex];
        ItemSlot itemSlot =  handSlots[slotIndex];

        if (itemSlot.IsEmpty)
        {
            itemSlot.AddItem(itemStack);
        }
        else if (itemStack == null)
        {
            itemSlot.DeleteItem();
        }
        else
        {
            itemSlot.ChangeAmount(itemStack.amount);
        }
    }
}