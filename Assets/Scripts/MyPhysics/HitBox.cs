using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class HitBox : MonoBehaviour
{
    [SerializeField] float characterWidth = 0.25f;
    [SerializeField] float characterHeight = 2;
    [SerializeField] float boundsTolerance = 0.1f;
    [SerializeField] int pointsOnEdge;
    [SerializeField] GameWorld world;
    [SerializeField] float distanceBetweenPoints;

    public RectangleVertex verticesForward;
    public RectangleVertex verticesBack;
    public RectangleVertex verticesRight;
    public RectangleVertex verticesLeft;
    public RectangleVertex verticesUp;
    public RectangleVertex verticesDown;

    private int pointsOnEdgeX;
    private int pointsOnEdgeY;
    private int pointsOnEdgeZ;

    private void Awake()
    {
        CacheData();
    }

    private void CacheData()
    {
        pointsOnEdgeX = (int)(characterWidth / distanceBetweenPoints) +1;
        pointsOnEdgeZ = (int)(characterWidth / distanceBetweenPoints)+1;
        pointsOnEdgeY = (int)(characterHeight / distanceBetweenPoints)+1;
    }

    public Vector3 GetHitSides(Vector3 direction, ref bool isGroundet)
    {
        float x = direction.x;
        float y = direction.y;
        float z = direction.z;

        verticesUp = CalculateUpBottomRectangle(characterWidth, characterWidth, characterHeight);
        verticesDown = CalculateUpBottomRectangle(characterWidth, characterWidth, -boundsTolerance*2);

        verticesRight = CalculateSideRectangle(characterWidth , characterWidth + boundsTolerance * 2, characterHeight - boundsTolerance, transform.right);
        verticesLeft = CalculateSideRectangle(characterWidth , characterWidth + boundsTolerance * 2, characterHeight - boundsTolerance, -transform.right);

        verticesForward = CalculateSideRectangle(characterWidth , characterWidth + boundsTolerance * 2, characterHeight - boundsTolerance, transform.forward);
        verticesBack = CalculateSideRectangle(characterWidth, characterWidth + boundsTolerance * 2, characterHeight - boundsTolerance, -transform.forward);

        if (y != 0)
        {
            if (y > 0)
            {
                //verticesUp = CalculateUpBottomRectangle(characterWidth, characterWidth, characterHeight);

                if (RectangleHit(verticesUp, pointsOnEdgeZ, pointsOnEdgeX, new Vector3(0, direction.x, 0))) //нижн€€ грань
                {
                    y = 0;
                }
                else
                {
                    y = 1;
                }
            }
            else
            {
                //verticesDown = CalculateUpBottomRectangle(characterWidth, characterWidth, 0);

                if (RectangleHit(verticesDown, pointsOnEdgeZ, pointsOnEdgeX, new Vector3(0, direction.x, 0))) //нижн€€ грань
                {
                    isGroundet = true;
                    y = 0;
                }
                else
                {
                    isGroundet = false;
                    y = 1;
                }
            }
        }

        if (x != 0)
        {
            if (x * transform.right.x > 0)
            {
                //verticesRight = CalculateSideRectangle(characterWidth + boundsTolerance * 2, characterWidth, characterHeight - boundsTolerance, transform.right);
                if (RectangleHit(verticesRight, pointsOnEdgeZ, pointsOnEdgeY, new Vector3(direction.x, 0, 0))) //права€ грань
                {
                    x = 0;
                }
                else
                {
                    x = 1;
                }
            }
            else
            {
                //verticesLeft = CalculateSideRectangle(characterWidth + boundsTolerance * 2, characterWidth, characterHeight - boundsTolerance, -transform.right);
                if (RectangleHit(verticesLeft, pointsOnEdgeZ, pointsOnEdgeY, new Vector3(direction.x, 0, 0)))  //лева€ грань
                {
                    x = 0;
                }
                else
                {
                    x = 1;
                }
            }
        }

        if (z != 0)
        {
            if (z * transform.forward.z > 0)
            {
                //verticesForward = CalculateSideRectangle(characterWidth, characterWidth + boundsTolerance*2, characterHeight - boundsTolerance, transform.forward);
                if (RectangleHit(verticesForward, pointsOnEdgeX, pointsOnEdgeY, new Vector3(0, 0, direction.z))) //передн€€ грань
                {
                    z = 0;
                }
                else
                {
                    z = 1;
                }
            }
            else
            {
                //verticesBack = CalculateSideRectangle(characterWidth, characterWidth + boundsTolerance * 2, characterHeight - boundsTolerance, -transform.forward);
                if (RectangleHit(verticesBack, pointsOnEdgeX, pointsOnEdgeY, new Vector3(0,0,direction.z)))  //задн€€ грань
                {
                    z = 0;
                }
                else
                {
                    z = 1;
                }
            }
        }

        return new Vector3(x, y, z);
    }

    private bool RectangleHit(RectangleVertex rectangle, int numberPointsI, int numberPointsJ, Vector3 direction)
    {
        Vector3 origin = rectangle.leftDown + direction;
        Vector3 directionI = rectangle.rightDown - rectangle.leftDown; 
        Vector3 directionJ = rectangle.leftUp - rectangle.leftDown;


        float distanceBetweenPointsI = directionI.magnitude/(numberPointsI -1);
        float distanceBetweenPointsJ = directionJ.magnitude/(numberPointsJ -1);

        for (int i = 0; i < numberPointsI; i++)
        {
            for (int j = 0; j < numberPointsJ; j++)
            {

                Vector3 checkPoint = origin + directionI.normalized * i * distanceBetweenPointsI + directionJ.normalized * j * distanceBetweenPointsJ;
                Debug.DrawLine(origin, checkPoint, Color.blue, 0.05f);
                //Debug.DrawLine(transform.position, checkPoint2, Color.red, 1);
                if (world.IsSolidBlock(checkPoint))
                    return true;
            }
        }

        return false;
    }

    private RectangleVertex CalculateUpBottomRectangle(float width, float lenght, float height)
    {
        Vector3 rectangleOrigin = transform.position;

        Vector2 vertexTriangle = CalculationTriangleVertex(transform.forward, width / 2, lenght / 2, out float angle);

        Vector2 mirrorVertexTriangle = RotateVector2(vertexTriangle, -90 );
        Vector2 mirrorVertexTriangle2 = RotateVector2(vertexTriangle, 180 );
        Vector2 mirrorVertexTriangle3 = RotateVector2(vertexTriangle, 90 );

        RectangleVertex vertices;

        vertices.leftUp = rectangleOrigin + new Vector3(vertexTriangle.x, height, vertexTriangle.y);
        vertices.leftDown = rectangleOrigin + new Vector3(mirrorVertexTriangle.x, height, mirrorVertexTriangle.y);

        vertices.rightDown = rectangleOrigin + new Vector3(mirrorVertexTriangle2.x, height, mirrorVertexTriangle2.y);
        vertices.rightUp = rectangleOrigin + new Vector3(mirrorVertexTriangle3.x, height, mirrorVertexTriangle3.y);

        return vertices;
    }

    private RectangleVertex CalculateSideRectangle(float width, float lenght,float height , Vector3 direction)
    {
        Vector3 rectangleOrigin = transform.position;

        Vector2 vertexTriangle = CalculationTriangleVertex(direction, width/2, lenght/2, out float angle);

        Vector2 mirrorVertexTriangle = RotateVector2(vertexTriangle, -angle *2);

        RectangleVertex vertices;

        vertices.leftUp = rectangleOrigin + new Vector3(vertexTriangle.x, height, vertexTriangle.y);
        vertices.leftDown = rectangleOrigin + new Vector3(vertexTriangle.x, boundsTolerance, vertexTriangle.y);
        vertices.rightDown = rectangleOrigin+ new Vector3(mirrorVertexTriangle.x, boundsTolerance, mirrorVertexTriangle.y);
        vertices.rightUp = rectangleOrigin + new Vector3(mirrorVertexTriangle.x, height, mirrorVertexTriangle.y);

        return vertices;
    }

    private Vector2 CalculationTriangleVertex(Vector3 direction, float width, float lenght, out float angle)
    {
        #region «акэшировать в старте
        double sum = Math.Pow(width, 2) + Math.Pow(lenght, 2);
        float diagonal =(float) Math.Sqrt(sum);                   //Ќаходим длинну от начала координат до искомой точки
        angle = (float)(Math.Asin(width / diagonal));      //угол между направлением движени€ и вектором к искомой точке в радианах
        #endregion

        float x = direction.x * (float)Math.Cos(angle) - direction.z * (float)Math.Sin(angle);
        float z = direction.z * (float)Math.Cos(angle) + direction.x * (float)Math.Sin(angle);

        angle *= 57.3f;
        return new Vector2(x,z) * diagonal;
        //return new Vector2(diagonal * (float)Math.Cos(angle), diagonal * (float)Math.Sin(angle));
    }

    private Vector2 RotateVector2(Vector2 originalVector, double angle)
    {
        angle *= Math.PI / 180;

        float x = originalVector.x * (float)Math.Cos(angle) - originalVector.y * (float)Math.Sin(angle);
        float z = originalVector.y * (float)Math.Cos(angle) + originalVector.x * (float)Math.Sin(angle);

        return new Vector2(x,z);
    }
}

[Serializable]
public struct RectangleVertex
{
    public Vector3 rightUp;
    public Vector3 rightDown;
    public Vector3 leftUp;
    public Vector3 leftDown;
}
