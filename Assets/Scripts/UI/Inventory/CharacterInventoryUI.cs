using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryUI : InventoryUI
{
    [SerializeField] protected ItemSlot[] mainSlots;

    private void OnEnable()
    {
        Activation();
    }

    public override void Activation()
    {
        base.Activation();

        InitializeSlots(mainSlots, PlayerData.MainInventory);
    }
}
