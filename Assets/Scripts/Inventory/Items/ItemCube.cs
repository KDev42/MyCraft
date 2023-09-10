using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemCube : ItemObject
{
    private Mesh mesh;
    private List<Vector3> vertiecies = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int> triangles = new List<int>();

    public void ChangeSkin(BlockInfo blockInfo)
    {
        mesh = new Mesh();
        triangles.Clear();
        vertiecies.Clear();
        uvs.Clear();

        GenerateBlock(blockInfo);

        mesh.triangles = Array.Empty<int>();
        mesh.vertices = vertiecies.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.Optimize();

        //chunkMesh.RecalculateBounds();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void GenerateBlock(BlockInfo blockInfo)
    {
        Vector3Int blockOffset = new Vector3Int(0, 0, 0);

        CubeTexturing.GenerateRightSide(blockOffset, ref vertiecies, ref triangles);
        CubeTexturing.AddUvs(blockInfo, Vector3Int.right, ref uvs);

        CubeTexturing.GenerateLeftSide(blockOffset, ref vertiecies, ref triangles);
        CubeTexturing.AddUvs(blockInfo, Vector3Int.left, ref uvs);

        CubeTexturing.GenerateBackSide(blockOffset, ref vertiecies, ref triangles);
        CubeTexturing.AddUvs(blockInfo, Vector3Int.back, ref uvs);

        CubeTexturing.GenerateFrontSide(blockOffset, ref vertiecies, ref triangles);
        CubeTexturing.AddUvs(blockInfo, Vector3Int.forward, ref uvs);

        CubeTexturing.GenerateBottomSide(blockOffset, ref vertiecies, ref triangles);
        CubeTexturing.AddUvs(blockInfo, Vector3Int.down, ref uvs);

        CubeTexturing.GenerateTopSide(blockOffset, ref vertiecies, ref triangles);
        CubeTexturing.AddUvs(blockInfo, Vector3Int.up, ref uvs);
    }
}
