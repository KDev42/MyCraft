using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CloseArea : MonoBehaviour, IPointerDownHandler
{
    public Action close;

    public void OnPointerDown(PointerEventData eventData)
    {
        close();
    }
}
