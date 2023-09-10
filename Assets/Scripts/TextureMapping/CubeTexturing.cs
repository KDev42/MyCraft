using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeTexturing
{
    public static void AddUvs(BlockInfo blockInfo,  Vector3Int normal,ref List<Vector2> uvs)
    {
        Vector2 uv;
        int sizeTile = 16;
        int sizeTexure = 256;

        if (blockInfo != null)
        {
            uv = blockInfo.GetPixelsOffset(normal);
        }
        else
        {
            uv = new Vector2(160f, 224f);
        }

        if (normal == Vector3Int.right || normal == Vector3Int.back)
        {
            uvs.Add(new Vector2(uv.x, uv.y) / sizeTexure);
            uvs.Add(new Vector2(uv.x, uv.y + sizeTile) / sizeTexure);
            uvs.Add(new Vector2(uv.x + sizeTile, uv.y) / sizeTexure);
            uvs.Add(new Vector2(uv.x + sizeTile, uv.y + sizeTile) / sizeTexure);
        }
        else
        {
            uvs.Add(new Vector2(uv.x, uv.y) / sizeTexure);
            uvs.Add(new Vector2(uv.x + sizeTile, uv.y) / sizeTexure);
            uvs.Add(new Vector2(uv.x, uv.y + sizeTile) / sizeTexure);
            uvs.Add(new Vector2(uv.x + sizeTile, uv.y + sizeTile) / sizeTexure);
        }
    }

    public static void GenerateRightSide(Vector3Int blockPosition, ref List<Vector3> vertiecies, ref List<int> triangles)
    {
        vertiecies.Add(new Vector3(1, 0, 0) + blockPosition);
        vertiecies.Add(new Vector3(1, 1, 0) + blockPosition);
        vertiecies.Add(new Vector3(1, 0, 1) + blockPosition);
        vertiecies.Add(new Vector3(1, 1, 1) + blockPosition);

        AddLastVerticiesSquare(ref triangles, ref vertiecies);
    }

    public static void GenerateLeftSide(Vector3Int blockPosition, ref List<Vector3> vertiecies, ref List<int> triangles)
    {

        vertiecies.Add(new Vector3(0, 0, 0) + blockPosition);
        vertiecies.Add(new Vector3(0, 0, 1) + blockPosition);
        vertiecies.Add(new Vector3(0, 1, 0) + blockPosition);
        vertiecies.Add(new Vector3(0, 1, 1) + blockPosition);

        AddLastVerticiesSquare(ref triangles, ref vertiecies);
    }

    public static void GenerateFrontSide(Vector3Int blockPosition, ref List<Vector3> vertiecies, ref List<int> triangles)
    {
        vertiecies.Add(new Vector3(0, 0, 1) + blockPosition);
        vertiecies.Add(new Vector3(1, 0, 1) + blockPosition);
        vertiecies.Add(new Vector3(0, 1, 1) + blockPosition);
        vertiecies.Add(new Vector3(1, 1, 1) + blockPosition);

        AddLastVerticiesSquare(ref triangles, ref vertiecies);
    }

    public static void GenerateBackSide(Vector3Int blockPosition, ref List<Vector3> vertiecies, ref List<int> triangles)
    {
        vertiecies.Add(new Vector3(0, 0, 0) + blockPosition);
        vertiecies.Add(new Vector3(0, 1, 0) + blockPosition);
        vertiecies.Add(new Vector3(1, 0, 0) + blockPosition);
        vertiecies.Add(new Vector3(1, 1, 0) + blockPosition);

        AddLastVerticiesSquare(ref triangles, ref vertiecies);
    }

    public static void GenerateTopSide(Vector3Int blockPosition, ref List<Vector3> vertiecies, ref List<int> triangles)
    {
        vertiecies.Add(new Vector3(0, 1, 0) + blockPosition);
        vertiecies.Add(new Vector3(0, 1, 1) + blockPosition);
        vertiecies.Add(new Vector3(1, 1, 0) + blockPosition);
        vertiecies.Add(new Vector3(1, 1, 1) + blockPosition);

        AddLastVerticiesSquare(ref triangles, ref vertiecies);
    }

    public static void GenerateBottomSide(Vector3Int blockPosition, ref List<Vector3> vertiecies, ref List<int> triangles)
    {
        vertiecies.Add(new Vector3(0, 0, 0) + blockPosition);
        vertiecies.Add(new Vector3(1, 0, 0) + blockPosition);
        vertiecies.Add(new Vector3(0, 0, 1) + blockPosition);
        vertiecies.Add(new Vector3(1, 0, 1) + blockPosition);

        AddLastVerticiesSquare(ref triangles, ref vertiecies);
    }

    private static void AddLastVerticiesSquare(ref List<int> triangles, ref List<Vector3> vertiecies)
    {
        triangles.Add(vertiecies.Count - 4);
        triangles.Add(vertiecies.Count - 3);
        triangles.Add(vertiecies.Count - 2);

        triangles.Add(vertiecies.Count - 3);
        triangles.Add(vertiecies.Count - 1);
        triangles.Add(vertiecies.Count - 2);
    }
}
