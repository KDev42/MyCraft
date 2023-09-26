using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Zenject;
using System;

public class ItemSlot:MonoBehaviour,IDropHandler
{
    [SerializeField] Image itemIcon;
    [SerializeField] TMP_Text amountTxt;
    [SerializeField] public bool isToolbarSlot;

    private ItemDataBase itemDatabase;

    [Inject]
    private void Construct(Database database)
    {
        itemDatabase = database.itemDatabase;
    }

    public bool IsEmpty { get; set; } = true;

    public ItemStack ItemStack { get; set; }
    public int IndexSlot { get; set; }
    public Inventory Inventory { get; set; }

    public void Initialize(Inventory inventory, int indexSlot)
    {
        IndexSlot = indexSlot;
        Inventory = inventory;
    }

    public void AddItem(ItemStack itemStack)
    {
        ItemStack = itemStack;
        Item item = itemDatabase.GetItem(ItemStack.itemType);

        itemIcon.gameObject.SetActive(true);
        amountTxt.gameObject.SetActive(true);
        itemIcon.sprite = item.icon;
        amountTxt.text = "" + ItemStack.amount;
        IsEmpty = false;
    }

    public void DeleteItem()
    {
        Inventory.RemoveAllItem(IndexSlot);
        itemIcon.gameObject.SetActive(false);
        amountTxt.gameObject.SetActive(false);
        itemIcon.sprite = null;
        amountTxt.text = "";
        IsEmpty = true;
    }

    public void ChangeAmount(int amount)
    {
        amountTxt.text = "" + amount;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("GGGGGG 2");
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        ItemSlot dropItemSlot = draggableItem.ParentSlot;
        ItemStack dropItemStack = dropItemSlot.ItemStack;

        ItemStack cloneDropItemStack = new ItemStack(dropItemStack.itemType, dropItemStack.amount);
        if (IsEmpty)
        {
            ChangeItem(cloneDropItemStack);
            dropItemSlot.DeleteItem();
        }
        else
        {
            ItemStack cloneItemStack = new ItemStack(ItemStack.itemType, ItemStack.amount);
            ChangeItem(cloneDropItemStack);
            dropItemSlot.ChangeItem(cloneItemStack);
        }

        draggableItem.BackToParent();

        if (isToolbarSlot)
        {
            EventsHolder.ChangeToolbar(IndexSlot);
        }
        if (dropItemSlot.isToolbarSlot)
        {
            EventsHolder.ChangeToolbar(dropItemSlot.IndexSlot);
        }
    }

    public void ChangeItem(ItemStack itemStack)
    {
        Inventory.SwapItems(IndexSlot, itemStack);
        AddItem(itemStack);
    }

    public void OffTxt()
    {
        amountTxt.gameObject.SetActive(false);
    }

    public void OnTxt()
    {
        amountTxt.gameObject.SetActive(true);
    }
}

[Serializable]
public class ItemStack
{
    public ItemType itemType;
    public int amount;

    public ItemStack(ItemType itemType, int amount)
    {
        this.itemType = itemType;
        this.amount = amount;
    }
}
