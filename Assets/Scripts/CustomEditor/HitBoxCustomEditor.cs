using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HitBox))]
public class HitBoxCustomEditor : Editor
{
    float size = 0.05f;

    public Vector3[] points;
    public Vector3[] pointsForward;
    public Vector3[] pointsBack;
    public Vector3[] pointsRight;
    public Vector3[] pointsLeft;
    public Vector3[] pointsUp;
    public Vector3[] pointsDown;

    private void Awake()
    {
        points = new Vector3[17];
        pointsForward = new Vector3[5];
        pointsBack = new Vector3[5];
        pointsRight = new Vector3[5];
        pointsLeft = new Vector3[5];
        pointsUp = new Vector3[5];
        pointsDown = new Vector3[5];
    }

    protected virtual void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            HitBox tr = ((HitBox)target);

            //points[0] = tr.verticesRight.rightUp;
            //points[1] = tr.verticesRight.leftUp;
            //points[2] = tr.verticesRight.leftDown;
            //points[3] = tr.verticesRight.rightDown;
            //points[4] = tr.verticesRight.rightUp;

            //points[5] = tr.verticesLeft.leftUp;
            //points[6] = tr.verticesLeft.rightUp;
            //points[7] = tr.verticesLeft.rightDown;
            //points[8] = tr.verticesLeft.leftDown;
            //points[9] = tr.verticesLeft.leftUp;

            //points[10] = tr.verticesRight.rightUp;
            //points[11] = tr.verticesRight.rightDown;
            //points[12] = tr.verticesLeft.leftDown;
            //points[13] = tr.verticesLeft.rightDown;
            //points[14] = tr.verticesRight.leftDown;
            //points[15] = tr.verticesRight.leftUp;
            //points[16] = tr.verticesLeft.rightUp;


            pointsForward[0] = tr.verticesForward.rightUp;
            pointsForward[1] = tr.verticesForward.leftUp;
            pointsForward[2] = tr.verticesForward.leftDown;
            pointsForward[3] = tr.verticesForward.rightDown;
            pointsForward[4] = tr.verticesForward.rightUp;

            Handles.color = Color.red;
            Handles.DrawPolyLine(pointsForward);

            pointsBack[0] = tr.verticesBack.rightUp;
            pointsBack[1] = tr.verticesBack.leftUp;
            pointsBack[2] = tr.verticesBack.leftDown;
            pointsBack[3] = tr.verticesBack.rightDown;
            pointsBack[4] = tr.verticesBack.rightUp;

            Handles.color = Color.green;
            Handles.DrawPolyLine(pointsBack);

            pointsDown[0] = tr.verticesDown.rightUp;
            pointsDown[1] = tr.verticesDown.leftUp;
            pointsDown[2] = tr.verticesDown.leftDown;
            pointsDown[3] = tr.verticesDown.rightDown;
            pointsDown[4] = tr.verticesDown.rightUp;

            Handles.color = Color.black;
            Handles.DrawPolyLine(pointsDown);

            pointsUp[0] = tr.verticesUp.rightUp;
            pointsUp[1] = tr.verticesUp.leftUp;
            pointsUp[2] = tr.verticesUp.leftDown;
            pointsUp[3] = tr.verticesUp.rightDown;
            pointsUp[4] = tr.verticesUp.rightUp;

            Handles.color = Color.blue;
            Handles.DrawPolyLine(pointsUp);


            pointsRight[0] = tr.verticesRight.rightUp;
            pointsRight[1] = tr.verticesRight.leftUp;
            pointsRight[2] = tr.verticesRight.leftDown;
            pointsRight[3] = tr.verticesRight.rightDown;
            pointsRight[4] = tr.verticesRight.rightUp;

            Handles.color = Color.yellow;
            Handles.DrawPolyLine(pointsRight);

            pointsLeft[0] = tr.verticesLeft.rightUp;
            pointsLeft[1] = tr.verticesLeft.leftUp;
            pointsLeft[2] = tr.verticesLeft.leftDown;
            pointsLeft[3] = tr.verticesLeft.rightDown;
            pointsLeft[4] = tr.verticesLeft.rightUp;

            Handles.color = Color.blue;
            Handles.DrawPolyLine(pointsLeft);

            //Handles.color = Color.red;
            //Handles.DotHandleCap(
            //    0,
            //    tr.drawUpRight,
            //    tr.transform.rotation,
            //    size,
            //    EventType.Repaint
            //);

            //Handles.color = Color.blue;
            //Handles.DotHandleCap(
            //    0,
            //    tr.drawUpLeft,
            //    tr.transform.rotation,
            //    size,
            //    EventType.Repaint
            //);

            //Handles.color = Color.green;
            //Handles.DotHandleCap(
            //    0,
            //    tr.drawDownLeft,
            //    tr.transform.rotation,
            //    size,
            //    EventType.Repaint
            //);

            //Handles.color = Color.black;
            //Handles.DotHandleCap(
            //    0,
            //    tr.drawDownRight,
            //    tr.transform.rotation,
            //    size,
            //    EventType.Repaint
            //);
        }
    }
}
