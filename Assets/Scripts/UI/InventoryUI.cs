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
        InitializeSlots(handSlots, PlayerData.handInventory.items);
    }

    protected void InitializeSlots(ItemSlot[] slots, ItemStack[] stacks)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            ItemStack itemStack = stacks[i];
            slots[i].Initialize(PlayerData.handInventory, i);
            if (itemStack != null)
            {
                slots[i].AddItem(stacks[i]);
            }
        }
    }
}
