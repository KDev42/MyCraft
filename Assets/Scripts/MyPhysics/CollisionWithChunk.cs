using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollisionWithChunk : MonoBehaviour
{
    private Vector3 size = new Vector3(1, 1, 1);
    private Quaternion rotation = new Quaternion(0, 0, 0, 0);

    public Vector3 PossibleMovement(GameWorld gameWorld, Vector3 position, Vector3 direction, Quaternion rotation, Vector3 size)
    {
        float x = direction.x, y= direction.y, z = direction.z;

        Vector3 newPosition = position + direction;
        Vector3Int coordinateInWorld = gameWorld.GetBlockCoordinate(newPosition);

        Vector3 agentCenter = newPosition + new Vector3(0, size.y/2, 0);
        Box agentBox = new Box(agentCenter, size,rotation);

        List<Box> boxes = CreateBoxes( GetPotentialCollisionBlocks(coordinateInWorld, direction));

        foreach (Box box in boxes)
        {
           Vector3 axisCollision = BoxCollision.Collision(agentBox, box);
            if (axisCollision.x != 0)
                x = 0; 
            if (axisCollision.y != 0)
                y = 0; 
            if (axisCollision.z != 0)
                z = 0;
        }

        return new Vector3(x, y, z);
    }

    // ---< Этот метод работает тлько для колайдера мешьше 1х1х2 >---
    private List<Vector3Int> GetPotentialCollisionBlocks(Vector3Int coordinateInWorld, Vector3 direction)
    {
        List<Vector3Int> blocksCoordinate = new List<Vector3Int>() ;
        Vector3Int zeroCoordinate = coordinateInWorld;
        int height =2 + direction.y != 0 ? 1 : 0; 

        if (direction.x != 0)
        {
            zeroCoordinate.x +=Math.Sign(direction.x);
            zeroCoordinate.y -= direction.y<0?1:0;
            zeroCoordinate.z -=1;

            for (int z = 0; z < 3; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    blocksCoordinate.Add(zeroCoordinate + new Vector3Int(0, y, z));
                }
            }
        }

        if (direction.y != 0)
        {
            int yDelta = direction.y < 0 ? -1 : 2;
            blocksCoordinate.Add(coordinateInWorld + new Vector3Int(0, yDelta, 0));
        }

        if (direction.z != 0)
        {
            zeroCoordinate.x -= 1;
            zeroCoordinate.y -= direction.y < 0 ? 1 : 0;
            zeroCoordinate.z += Math.Sign(direction.z);

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    blocksCoordinate.Add(zeroCoordinate + new Vector3Int(x, y, 0));
                }
            }
        }

        return blocksCoordinate;
    }

    private List<Box> CreateBoxes(List<Vector3Int> blocksCoordinate)
    {
        List<Box> boxes = new List<Box>();

        foreach(Vector3Int boxeCoordinate in blocksCoordinate)
        {
            Vector3 center = boxeCoordinate + new Vector3(0.5f, 0.5f, 0.5f);
            boxes.Add(new Box(center, size, rotation));
        }

        return boxes;
    }
}
