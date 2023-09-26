using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] Image jsContainer;
    [SerializeField] Image joystick;

    public Vector3 inputDirection = Vector3.zero;

    public void OnDrag(PointerEventData ped)
    {
        Vector2 position = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (jsContainer.rectTransform,
                ped.position,
                ped.pressEventCamera,
                out position);

        position.x = (position.x / jsContainer.rectTransform.sizeDelta.x);
        position.y = (position.y / jsContainer.rectTransform.sizeDelta.y);

        //float x = (jsContainer.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
        //float y = (jsContainer.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

        inputDirection = new Vector3(position.x * 2 + 0, position.y * 2);
        inputDirection = (inputDirection.magnitude > 1) ? inputDirection.normalized : inputDirection;

        joystick.rectTransform.anchoredPosition = new Vector3(inputDirection.x * (jsContainer.rectTransform.sizeDelta.x / 3), inputDirection.y * (jsContainer.rectTransform.sizeDelta.y) / 3);
    }

    public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        inputDirection = Vector3.zero;
        joystick.rectTransform.anchoredPosition = Vector3.zero;
    }
    //[SerializeField] float minDistTM;
    //[SerializeField] float maxDistTM;
    //[SerializeField] Transform touchMarker;

    //private int closestTouch;
    //private float distanceToStick;
    //private Vector2 touch;
    //private Vector2 thisPosition;

    //private void Start()
    //{
    //    thisPosition = transform.position;

    //}

    //public Vector2 Direction()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        distanceToStick = 100000;
    //        for (int i = 0; i < Input.touchCount; i++)
    //        {
    //            if ((Input.GetTouch(i).position - thisPosition).magnitude < distanceToStick)
    //            {
    //                distanceToStick = (Input.GetTouch(i).position - thisPosition).magnitude;
    //                closestTouch = i;
    //            }
    //        }

    //        touch = Input.GetTouch(closestTouch).position;
    //        if ((thisPosition - touch).magnitude > minDistTM && (thisPosition - touch).magnitude < maxDistTM)
    //        {
    //            touchMarker.transform.position = touch;
    //            return touch - thisPosition;
    //        }
    //        else
    //        {
    //            touchMarker.transform.position = thisPosition;
    //        }
    //    }

    //    return new Vector2(0, 0);
    //}
}
