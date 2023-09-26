using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 touchDirection;
    private Vector2 PointerOld;
    private int PointerId;
    private bool Pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }

    public Vector2 Direction()
    {
        if (Pressed)
        {
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                touchDirection = Input.touches[PointerId].position - PointerOld;
                PointerOld = Input.touches[PointerId].position;
            }
            else
            {
                touchDirection = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                PointerOld = Input.mousePosition;
            }
        }
        else
        {
            touchDirection = new Vector2();
        }

        return touchDirection;
    }
}