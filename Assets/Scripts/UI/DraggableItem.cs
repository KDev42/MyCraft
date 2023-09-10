using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemSlot ParentSlot { get; set; } 

    private Transform currentParent;
    private Vector3 startPosition;
    private Image imgage => GetComponent<Image>();

    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentSlot = transform.parent.GetComponent<ItemSlot>();
        ParentSlot.OffTxt();
        imgage.raycastTarget = false;
        startPosition = transform.position;
        currentParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BackToParent();
    }

    public void BackToParent()
    {
        ParentSlot.OnTxt();
        Debug.Log("GGGGG " + currentParent);
        transform.SetParent(currentParent);
        transform.SetSiblingIndex(0);
        transform.position = startPosition;
        imgage.raycastTarget = true;
    }
}
