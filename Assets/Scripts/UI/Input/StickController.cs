using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    //[SerializeField] float minDistTM;
    //[SerializeField] float maxDistTM;


    //private int closestTouch;
    //private float distanceToStick;
    //private Vector2 inputMouse;
    //private Vector2 thisPosition;
    //private Transform touchMarker;

    //public override bool Press()
    //{
    //    throw new System.NotImplementedException();
    //}
    //public override bool Jump()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public override Vector2 LookDirection()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public override Vector2 MotionDirection()
    //{
    //    throw new System.NotImplementedException();
    //}

    //private void Start()
    //{
    //    thisPosition = transform.position;
    //    touchMarker = transform.GetChild(0);

    //}

    private void Update()
    {
        //#if UNITY_EDITOR
        //if (Input.GetMouseButton(0))
        //{
        //    inputMouse = Input.mousePosition;
        //    if ((thisPosition - inputMouse).magnitude > minDistTM && (thisPosition - inputMouse).magnitude < maxDistTM)
        //    {
        //        touchMarker.position = inputMouse;
        //        eventManager.MoveStick(inputMouse - thisPosition);
        //    }
        //    else
        //    {
        //        touchMarker.position = thisPosition;
        //        eventManager.StopMoveStick();
        //    }
        //}
        //else
        //{
        //    touchMarker.position = thisPosition;
        //    eventManager.StopMoveStick();
        //}
        //#endif
        //#if ANDROID
        //if (Input.touchCount > 0)
        //{
        //    distanceToStick = 100000;
        //    for (int i = 0; i < Input.touchCount; i++)
        //    {
        //        if ((Input.GetTouch(i).position - thisPosition).magnitude < distanceToStick)
        //        {
        //            distanceToStick = (Input.GetTouch(i).position - thisPosition).magnitude;
        //            closestTouch = i;
        //        }
        //    }

        //    inputMouse = Input.GetTouch(closestTouch).position;
        //    if ((thisPosition - inputMouse).magnitude > minDistTM && (thisPosition - inputMouse).magnitude < maxDistTM)
        //    {
        //        touchMarker.transform.position = inputMouse;
        //        eventManager.MoveStick(inputMouse - thisPosition);
        //    }
        //    else
        //    {
        //        eventManager.StopMoveStick();
        //        touchMarker.transform.position = thisPosition;
        //    }
        //}
        //else
        //{
        //    eventManager.StopMoveStick();
        //    touchMarker.transform.position = thisPosition;
        //}
        //#endif
    }
}
