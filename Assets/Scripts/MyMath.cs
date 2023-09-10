using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath 
{
    private static int widthSq => WorldConstants.chunkWidth * WorldConstants.chunkWidth;

    public static Vector2Int MultiplyVectors2Int(Vector2Int v1,Vector2Int v2)
    {
        return new Vector2Int(v1.x * v2.x, v1.y*v2.y);
    }

    public static int GetBlockCoordinate(Vector3Int coordinates)
    {
        return ArrayIndex3D(coordinates);
    }

    public static Vector3Int GetGridCoordinate(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        int z = Mathf.FloorToInt(position.z);

        return new Vector3Int(x, y, z);
    }

    public static float Get2DPerlin(Vector2 position, float offset, float scale)
    {
        return Mathf.PerlinNoise((position.x + 0.1f) / WorldConstants.chunkWidth * scale + offset, (position.y + 0.1f) / WorldConstants.chunkWidth * scale + offset);
    }

    public static bool Get3DPerlin(Vector3 position, float offset, float scale, float threshold)
    {
        // https://www.youtube.com/watch?v=Aga0TBJkchM Carpilot on YouTube

        float x = (position.x + offset + 0.1f) * scale;
        float y = (position.y + offset + 0.1f) * scale;
        float z = (position.z + offset + 0.1f) * scale;

        float AB = Mathf.PerlinNoise(x, y);
        float BC = Mathf.PerlinNoise(y, z);
        float AC = Mathf.PerlinNoise(x, z);
        float BA = Mathf.PerlinNoise(y, x);
        float CB = Mathf.PerlinNoise(z, y);
        float CA = Mathf.PerlinNoise(z, x);

        if ((AB + BC + AC + BA + CB + CA) / 6f > threshold)
            return true;
        else
            return false;
    }

    private static int ArrayIndex3D(Vector3Int coordinates)
    {
        return coordinates.x + coordinates.y * widthSq + coordinates.z * WorldConstants.chunkWidth;
    }
}
