using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Pressed;
    //public bool touchDown;
    //public bool touchUp;
    //public bool holdDown;
    public Vector2 touchDirection;
    private Vector2 PointerOld;
    private int PointerId;
    private float distanceMove = 0.5f;

    public enum TouchState
    {
        none,
        touchDown,
        touchUp,
        holdDown,
    }

    public TouchState touchState;

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;

        if (Application.isMobilePlatform)
        {
            touchState = TouchState.touchDown; 
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;

        if (touchState != TouchState.touchUp)
        {
            touchState = TouchState.touchUp;
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

                if (touchDirection.magnitude> distanceMove && Input.touches[PointerId].phase == TouchPhase.Moved)
                {
                    touchState = TouchState.touchUp;
                }
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

        return touchDirection.normalized;
    }
}