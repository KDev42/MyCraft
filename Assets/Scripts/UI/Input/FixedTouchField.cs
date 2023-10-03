using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Pressed;
    public bool touchDown;
    public bool touchUp;
    public bool holdDown;
    public Vector2 touchDirection;
    private Vector2 PointerOld;
    private int PointerId;

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;

        if (Application.isMobilePlatform)
        {
            if (Input.touches[PointerId].phase != TouchPhase.Moved)
            {
                touchDown = true;
                holdDown = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;

        if (holdDown)
        {
            touchUp = true;
        }
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

        if (Application.isMobilePlatform)
        {
            if (Input.touches[PointerId].phase == TouchPhase.Moved)
            {
                holdDown = false;
            }
            }
        }
        else
        {
            touchDirection = new Vector2();
        }

        return touchDirection;
    }
}