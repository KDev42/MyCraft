using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollisionWithChunk : MonoBehaviour
{
    [SerializeField] BoxCollision boxCollision;
    //[SerializeField] float accuracyY;
    ////[SerializeField] float accuracyXZ;
    //private Vector3 size = new Vector3(1.0f, 1.0f, 1.0f);
    //private Quaternion rotation = Quaternion.Euler(0, 0, 0);

    public Vector3 PossibleMovement(GameWorld gameWorld, Vector3 position, Vector3 direction, Quaternion rotation, Vector3 size, ref bool isGrounded)
    {
        float x = direction.x, y= direction.y, z = direction.z;
        float maxIntersectionX = 0, maxIntersectionY = 0, maxIntersectionZ = 0;
        //Vector3 newPosition = position + direction;

        if (direction.x != 0)
        {
            Vector3 agentCenter = position + new Vector3(direction.x, size.y / 2, 0);
            Box agentBox = new Box(agentCenter, size, rotation);
            List<Box> boxesX = CreateBoxes(GetPotentialCollisionBlocksX(position, direction, gameWorld));

            foreach (Box box in boxesX)
            {
                Vector3 axisCollision = boxCollision.Collision(agentBox, box);
                if (Math.Abs(axisCollision.x) > Math.Abs(maxIntersectionX))
                    maxIntersectionX = (axisCollision.x);
            }
        }

        if (direction.z != 0)
        {
            Vector3 agentCenter = position + new Vector3(0, size.y / 2, direction.z);
            Box agentBox = new Box(agentCenter, size, rotation);
            List<Box> boxesZ = CreateBoxes(GetPotentialCollisionBlocksZ(position, direction, gameWorld));

            foreach (Box box in boxesZ)
            {
                Vector3 axisCollision = boxCollision.Collision(agentBox, box);
                if (Math.Abs(axisCollision.z) > Math.Abs(maxIntersectionZ))
                    maxIntersectionZ = (axisCollision.z);
            }
        }

        if (direction.y != 0)
        {
            Vector3 agentCenter = position + new Vector3(0, direction.y+ size.y / 2, 0);
        
            Box agentBox = new Box(agentCenter, size, rotation);
            List<Box> boxesY = CreateBoxes(GetPotentialCollisionBlocksY(position, direction, gameWorld));

            foreach (Box box in boxesY)
            {
                Vector3 axisCollision = boxCollision.Collision(agentBox, box);
                if (Math.Abs(axisCollision.y) > Math.Abs(maxIntersectionY))
                    maxIntersectionY = (axisCollision.y);
                if (axisCollision.y != 0)
                {
                    isGrounded = true;
                }
            }
        }
        //Vector3 agentCenter = newPosition + new Vector3(0, size.y/ 2, 0); 
        //Box agentBox = new Box(agentCenter, size,rotation);

        //bool borderX =false, borderZ = false;

        //foreach (Box box in boxes)
        //{
        //    Vector3 axisCollision = BoxCollision.Collision(agentBox, box);
        //    //if (axisCollision.magnitude != 0)
        //    //    gameObject.transform.position -= axisCollision;

        //    if (Math.Abs(axisCollision.x) > Math.Abs(maxIntersectionX))
        //        maxIntersectionX = (axisCollision.x);

        //    if (Math.Abs(axisCollision.y) > Math.Abs(maxIntersectionY))
        //        maxIntersectionY = (axisCollision.y);

        //    if (Math.Abs(axisCollision.z) > Math.Abs(maxIntersectionZ))
        //        maxIntersectionZ =  (axisCollision.z);

        //    if (axisCollision.y != 0)
        //    {
        //        isGrounded = true;
        //    }
        //    //borderX = axisCollision.x != 0;
        //    //borderZ = axisCollision.z != 0;
        //}

        x -= maxIntersectionX;
        y -= maxIntersectionY;
        z -= maxIntersectionZ;

            //if (borderX)
            //    x = 0;

            //if (borderZ)
            //    z = 0;
        //transform.position += direction;
        return new Vector3(x, y, z);
    }
    private List<Vector3Int> GetPotentialCollisionBlocksX(Vector3 position, Vector3 direction, GameWorld gameWorld)
    {
        Vector3Int coordinateInWorld = gameWorld.GetBlockCoordinate(position + new Vector3(direction.x, 0, 0));
        List<Vector3Int> blocksCoordinate = new List<Vector3Int>();
        int height = 2 + (direction.y != 0 ? 1 : 0);

        if (direction.x != 0)
        {
            Vector3Int zeroCoordinate = coordinateInWorld;
            zeroCoordinate.x += Math.Sign(direction.x);
            zeroCoordinate.z -= 1;

            for (int z = 0; z < 3; z++)
            {
                for (int y = 0; y < height + 1; y++)
                {
                    Vector3Int coordinate = zeroCoordinate + new Vector3Int(0, y, z);
                    if (gameWorld.IsSolidBlock(coordinate))
                        blocksCoordinate.Add(coordinate);
                }
            }
        }

        return blocksCoordinate;
    }
    private List<Vector3Int> GetPotentialCollisionBlocksZ(Vector3 position, Vector3 direction, GameWorld gameWorld)
    {
        Vector3Int coordinateInWorld = gameWorld.GetBlockCoordinate(position + new Vector3(0, 0, direction.z));
        List<Vector3Int> blocksCoordinate = new List<Vector3Int>();
        int height = 2 + (direction.y != 0 ? 1 : 0);

        if (direction.z != 0)
        {
            Vector3Int zeroCoordinate = coordinateInWorld;
            zeroCoordinate.x -= 1;
            //zeroCoordinate.y -= direction.y < 0 ? 1 : 0;
            zeroCoordinate.z += Math.Sign(direction.z);

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < height + 1; y++)
                {
                    Vector3Int coordinate = zeroCoordinate + new Vector3Int(x, y, 0);
                    if (gameWorld.IsSolidBlock(coordinate))
                        blocksCoordinate.Add(coordinate);
                }
            }
        }

        return blocksCoordinate;
    }
    private List<Vector3Int> GetPotentialCollisionBlocksY(Vector3 position, Vector3 direction, GameWorld gameWorld)
    {
        Vector3Int coordinateInWorld = gameWorld.GetBlockCoordinate(position + new Vector3(0, direction.y, 0));
        List<Vector3Int> blocksCoordinate = new List<Vector3Int>();

        if (gameWorld.IsSolidBlock(coordinateInWorld))
            blocksCoordinate.Add(coordinateInWorld);

        if (gameWorld.IsSolidBlock(coordinateInWorld + new Vector3Int(0, 1, 0)))
            blocksCoordinate.Add(coordinateInWorld + new Vector3Int(0, 1, 0));

        if (direction.y != 0)
        {
            int yDelta = direction.y < 0 ? -1 : 2;
            Vector3Int coordinate = coordinateInWorld + new Vector3Int(0, yDelta, 0);
            if (gameWorld.IsSolidBlock(coordinate))
                blocksCoordinate.Add(coordinate);
        }

        return blocksCoordinate;
    }
    // ---< Этот метод работает тлько для колайдера мешьше 1х1х2 >---
    //private List<Vector3Int> GetPotentialCollisionBlocks(Vector3Int coordinateInWorld, Vector3 direction, GameWorld gameWorld)
    //{
    //    List<Vector3Int> blocksCoordinate = new List<Vector3Int>() ;
    //    int height =2 + (direction.y != 0 ? 1 : 0);

    //    if (direction.x != 0)
    //    {
    //        Vector3Int zeroCoordinate = coordinateInWorld;
    //        zeroCoordinate.x +=Math.Sign(direction.x);
    //        //zeroCoordinate.y -= direction.y<0?1:0;
    //        zeroCoordinate.z -=1;

    //        for (int z = 0; z < 3; z++)
    //        {
    //            for (int y = 0; y < height+1; y++)
    //            {
    //                Vector3Int coordinate = zeroCoordinate + new Vector3Int(0, y, z);
    //                if (gameWorld.IsSolidBlock(coordinate))
    //                    blocksCoordinate.Add(coordinate);
    //            }
    //        }
    //    }

    //    if (direction.z != 0)
    //    {
    //        Vector3Int zeroCoordinate = coordinateInWorld;
    //        zeroCoordinate.x -= 1;
    //        //zeroCoordinate.y -= direction.y < 0 ? 1 : 0;
    //        zeroCoordinate.z += Math.Sign(direction.z);

    //        for (int x = 0; x < 3; x++)
    //        {
    //            for (int y = 0; y < height+1; y++)
    //            {
    //                Vector3Int coordinate = zeroCoordinate + new Vector3Int(x, y, 0);
    //                if (gameWorld.IsSolidBlock(coordinate))
    //                    blocksCoordinate.Add(coordinate);
    //            }
    //        }
    //    }

    //    if (gameWorld.IsSolidBlock(coordinateInWorld))
    //        blocksCoordinate.Add(coordinateInWorld);

    //    if (gameWorld.IsSolidBlock(coordinateInWorld + new Vector3Int(0, 1, 0)))
    //        blocksCoordinate.Add(coordinateInWorld + new Vector3Int(0, 1, 0));

    //    if (direction.y != 0)
    //    {
    //        int yDelta = direction.y < 0 ? -1 : 2;
    //        Vector3Int coordinate = coordinateInWorld + new Vector3Int(0, yDelta, 0);
    //        if (gameWorld.IsSolidBlock(coordinate))
    //            blocksCoordinate.Add(coordinate);
    //    }

    //    return blocksCoordinate;
    //}

    private List<Box> CreateBoxes(List<Vector3Int> blocksCoordinate)
    {
        Vector3 size = new Vector3(1.0f, 1.0f, 1.0f);
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        List<Box> boxes = new List<Box>();

        foreach (Vector3Int boxeCoordinate in blocksCoordinate)
        {
            //Debug.DrawRay(boxeCoordinate, Vector3.up, Color.red, 1);
            //Debug.DrawRay(boxeCoordinate, Vector3.forward, Color.red, 1);
            //Debug.DrawRay(boxeCoordinate, Vector3.right, Color.red, 1);
            //Debug.DrawRay(boxeCoordinate + new Vector3Int(1,1,1), Vector3.down, Color.red, 1);
            //Debug.DrawRay(boxeCoordinate + new Vector3Int(1, 1, 1), Vector3.back, Color.red, 1);
            //Debug.DrawRay(boxeCoordinate + new Vector3Int(1, 1, 1), Vector3.left, Color.red, 1);

            //Debug.DrawRay(boxeCoordinate + new Vector3Int(1, 0, 0), Vector3.forward, Color.red, 1);
            //Debug.DrawRay(boxeCoordinate + new Vector3Int(1, 0, 0), Vector3.up, Color.red, 1);

            //Debug.DrawRay(boxeCoordinate + new Vector3Int(1, 1, 0), Vector3.left, Color.red, 1);

            //Debug.DrawRay(boxeCoordinate + new Vector3Int(0, 1, 0), Vector3.forward, Color.red, 1);

            //Debug.DrawRay(boxeCoordinate + new Vector3Int(0, 0, 1), Vector3.right, Color.red, 1);
            //Debug.DrawRay(boxeCoordinate + new Vector3Int(0, 0, 1), Vector3.up, Color.red, 1);

            Vector3 center = boxeCoordinate + new Vector3(0.5f, 0.5f, 0.5f);
            boxes.Add(new Box(center, size, rotation));
        }

        return boxes;
    }
}
