using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashBin : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        ItemSlot dropItemSlot = draggableItem.ParentSlot;

        dropItemSlot.DeleteItem();
        draggableItem.BackToParent();
    }
}
