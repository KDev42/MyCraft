using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryUI : InventoryUI
{
    [SerializeField] protected ItemSlot[] mainSlots;

    public override void Activation()
    {
        base.Activation();

        //InitializeSlots(mainSlots, PlayerData.mainInventory);
    }
}
