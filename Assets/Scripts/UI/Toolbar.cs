using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Toolbar : InventoryUI
{
    [SerializeField] RectTransform highlight;

    private void Awake()
    {
        Activation();
    }

    private void OnEnable()
    {
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
        ItemStack itemStack = PlayerData.handInventory.items[slotIndex];
        ItemSlot itemSlot =  handSlots[slotIndex];

        if (itemSlot.IsEmpty)
        {
            itemSlot.AddItem(itemStack);
        }
        else
        {
            itemSlot.ChangeAmount(itemStack.amount);
        }
    }
}