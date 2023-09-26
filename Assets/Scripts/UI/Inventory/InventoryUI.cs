using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] protected ItemSlot[] handSlots;

    protected ItemDataBase itemDatabase;

    [Inject]
    private void Construct(Database database)
    {
        itemDatabase = database.itemDatabase;
    }

    public virtual void Activation()
    {
        InitializeSlots(handSlots, PlayerData.HandInventory);
    }

    protected void InitializeSlots(ItemSlot[] slots,  Inventory inventory)
    {
        ItemStack[] stacks = inventory.Items;

        for (int i = 0; i < slots.Length; i++)
        {
            ItemStack itemStack = stacks[i];
            slots[i].Initialize(inventory, i);
            if (itemStack != null && stacks[i].itemType !=0)
            {
                slots[i].AddItem(stacks[i]);
            }
            else
            {
                slots[i].DeleteItem();
            }
        }
    }
}
