using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolbarButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] int index;
    [SerializeField] MobileInput mobileInput;

    public void OnPointerDown(PointerEventData eventData)
    {
        mobileInput.ChangeSlot(index);
    }
}
