using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class Hand : MonoBehaviour
{
    [SerializeField] Mine mine;
    [SerializeField] ItemObject defaultItem;
    public ItemObject ItemObj { get; set; }

    private ItemFactory itemFactory;

    [Inject]
    private void Construct(ItemFactory itemFactory)
    {
        this.itemFactory = itemFactory;
    }

    private void OnEnable()
    {
        EventsHolder.changeActiveSlot += ChangeItemObject;
    }

    private void OnDisable()
    {
        EventsHolder.changeActiveSlot -= ChangeItemObject;
    }

    public void StartMine(Action callback,  MiningBlock miningBlock)
    {
        if (ItemObj == null)
            ItemObj = defaultItem;
        //if(mine.CanMine(blockInfo, ItemObj.item))

        mine.StartMine( callback, ItemObj.item, miningBlock);
    }

    public void StopMine()
    {
        mine.StopMine();
    }

    public void Attack()
    {

    }

    public void InteractionWithEnvironment()
    {

    }

    public void DropItem()
    {

    }

    private void ChangeItemObject(int slotIndex)
    {
        ItemStack itemStack = PlayerData.handInventory.items[slotIndex];
        if(itemStack != null)
        {
            ItemType itemType = itemStack.itemType;
            if (ItemObj != null)
            {
                ItemObj.GetComponent<PoolObject>().ReturnToPool();
            }

            ItemObj = itemFactory.SpawnItem(itemType, transform.position, 1, transform);
        }
        else
        {
            if(ItemObj != null && ItemObj.GetComponent<PoolObject>())
                ItemObj.GetComponent<PoolObject>().ReturnToPool();

            defaultItem.gameObject.SetActive(true);
            ItemObj = defaultItem;
        }
    }
}
