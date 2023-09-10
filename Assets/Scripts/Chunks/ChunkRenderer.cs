using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour
{
    private Mesh chunkMesh;
    private ChunkData chunkData;

    private int chunkWidth;
    private int chunkHeight;
    private List<Vector3> vertiecies = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int> triangles = new List<int>();

    private ChunkData rightChunk;
    private ChunkData leftChunk;
    private ChunkData forwardChunk;
    private ChunkData backChunk;

    //private void Start()
    //{
    //    GenerateMesh();
    //}

    public void GenerateChunk(int chunkWidth,int chunkHeight, ChunkData chunkData)
    {
       chunkData.parentWorld.activeChunkDatas.TryGetValue(chunkData.chunkPosition + Vector2Int.right, out rightChunk );
       chunkData.parentWorld.activeChunkDatas.TryGetValue(chunkData.chunkPosition + Vector2Int.left, out leftChunk );
       chunkData.parentWorld.activeChunkDatas.TryGetValue(chunkData.chunkPosition + Vector2Int.up, out forwardChunk );
       chunkData.parentWorld.activeChunkDatas.TryGetValue(chunkData.chunkPosition + Vector2Int.down, out backChunk );

        this.chunkData = chunkData;
        this.chunkHeight = chunkHeight;
        this.chunkWidth = chunkWidth;

        chunkMesh = new Mesh();

        RegenerateMesh();

        GetComponent<MeshFilter>().mesh = chunkMesh;
        //GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }

    public void SpawnBlock(Vector3Int coordinate, BlockType blockType)
    {
        chunkData.bloks[MyMath.GetBlockCoordinate(coordinate)] = blockType;

        RegenerateMesh();

        CheckAdjacentChunk(coordinate);
    }

    public void RegenerateMesh()
    {
        triangles.Clear();
        vertiecies.Clear();
        uvs.Clear();

        for (int y = 0; y < chunkHeight; y++)
        {
            for (int x = 0; x < chunkWidth; x++)
            {
                for (int z = 0; z < chunkWidth; z++)
                {
                    GenerateBlock(x, y, z);
                }
            }
        }

        chunkMesh.triangles = Array.Empty<int>();
        chunkMesh.vertices = vertiecies.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize();

        //chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();
    }

    private void CheckAdjacentChunk(Vector3Int blockPosition)
    {
        if(blockPosition.x == 0)
        {
            UpdateAdjacentChunk(leftChunk);
        }
        else if(blockPosition.x == WorldConstants.chunkWidth-1)
        {
            UpdateAdjacentChunk(rightChunk);
        }

        if (blockPosition.z == 0)
        {
            UpdateAdjacentChunk(backChunk);
        }
        else if (blockPosition.z == WorldConstants.chunkWidth - 1)
        {
            UpdateAdjacentChunk(forwardChunk);
        }
    }

    private void UpdateAdjacentChunk(ChunkData adjacentChunk)
    {
        if(adjacentChunk!=null)
            adjacentChunk.chunkRenderer.RegenerateMesh();
    }

    private void GenerateBlock(int x, int y, int z)
    {
        Vector3Int blockPosition = new Vector3Int(x, y, z);

        BlockType blockType = GetBlockAtPosition(blockPosition);
        BlockInfo blockInfo = chunkData.parentWorld.blocksDataBase.GetInfo(blockType);

        if (GetBlockAtPosition(blockPosition)==0) return;

        if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0)
        {
            CubeTexturing.GenerateRightSide(blockPosition, ref vertiecies, ref triangles);
            CubeTexturing.AddUvs(blockInfo,  Vector3Int.right, ref uvs);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0) 
        { 
            CubeTexturing.GenerateLeftSide(blockPosition, ref vertiecies, ref triangles);
            CubeTexturing.AddUvs(blockInfo, Vector3Int.left, ref uvs);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0) 
        {
            CubeTexturing.GenerateBackSide(blockPosition, ref vertiecies, ref triangles);
            CubeTexturing.AddUvs(blockInfo, Vector3Int.back, ref uvs);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0) 
        {
            CubeTexturing.GenerateFrontSide(blockPosition, ref vertiecies, ref triangles);
            CubeTexturing.AddUvs(blockInfo, Vector3Int.forward, ref uvs);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.down) == 0) 
        {
            CubeTexturing.GenerateBottomSide(blockPosition, ref vertiecies, ref triangles);
            CubeTexturing.AddUvs(blockInfo, Vector3Int.down, ref uvs);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0) 
        {
            CubeTexturing.GenerateTopSide(blockPosition,ref vertiecies,ref triangles);
            CubeTexturing.AddUvs(blockInfo, Vector3Int.up, ref uvs);
        }
    }

    private BlockType GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < chunkWidth &&
            blockPosition.z >= 0 && blockPosition.z < chunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < chunkHeight)
        {
            return chunkData.bloks[MyMath.GetBlockCoordinate( blockPosition)];
        }
        else
        {
            if (blockPosition.y < 0 || blockPosition.y >= chunkHeight) return BlockType.air;

            if (blockPosition.x < 0)
            {
                if(leftChunk == null)
                {
                    return BlockType.air;
                }

                blockPosition.x += chunkWidth;

                return leftChunk.bloks[MyMath.GetBlockCoordinate(blockPosition)];
            }

            else if (blockPosition.x >= chunkWidth)
            {
                if (rightChunk == null)
                {
                    return BlockType.air;
                }

                blockPosition.x -= chunkWidth;

                return rightChunk.bloks[MyMath.GetBlockCoordinate(blockPosition)];
            }

            if (blockPosition.z < 0)
            {
                if (backChunk == null)
                {
                    return BlockType.air;
                }

                blockPosition.z += chunkWidth;

                return backChunk.bloks[MyMath.GetBlockCoordinate(blockPosition)];
            }

            else if (blockPosition.z >= chunkWidth)
            {
                if (forwardChunk == null)
                {
                    return BlockType.air;
                }

                blockPosition.z -= chunkWidth;

                return forwardChunk.bloks[MyMath.GetBlockCoordinate(blockPosition)];
            }

            return BlockType.air;
        }
    }

    //private void GenerateRightSide(Vector3Int blockPosition)
    //{
    //    vertiecies.Add(new Vector3(1, 0, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 1, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 0, 1) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 1, 1) + blockPosition);

    //    AddLastVerticiesSquare();
    //}

    //private void GenerateLeftSide(Vector3Int blockPosition)
    //{

    //    vertiecies.Add(new Vector3(0, 0, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(0, 0, 1) + blockPosition);
    //    vertiecies.Add(new Vector3(0, 1, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(0, 1, 1) + blockPosition);

    //    AddLastVerticiesSquare();
    //}

    //private void GenerateFrontSide(Vector3Int blockPosition)
    //{
    //    vertiecies.Add(new Vector3(0, 0, 1) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 0, 1) + blockPosition);
    //    vertiecies.Add(new Vector3(0, 1, 1) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 1, 1) + blockPosition);

    //    AddLastVerticiesSquare();
    //}

    //private void GenerateBackSide(Vector3Int blockPosition)
    //{
    //    vertiecies.Add(new Vector3(0, 0, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(0, 1, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 0, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 1, 0) + blockPosition);

    //    AddLastVerticiesSquare();
    //}

    //private void GenerateTopSide(Vector3Int blockPosition)
    //{
    //    vertiecies.Add(new Vector3(0, 1, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(0, 1, 1) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 1, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 1, 1) + blockPosition);

    //    AddLastVerticiesSquare();
    //}

    //private void GenerateBottomSide(Vector3Int blockPosition)
    //{
    //    vertiecies.Add(new Vector3(0, 0, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 0, 0) + blockPosition);
    //    vertiecies.Add(new Vector3(0, 0, 1) + blockPosition);
    //    vertiecies.Add(new Vector3(1, 0, 1) + blockPosition);

    //    AddLastVerticiesSquare();
    //}

    //private void AddLastVerticiesSquare()
    //{
    //    triangles.Add(vertiecies.Count - 4);
    //    triangles.Add(vertiecies.Count - 3);
    //    triangles.Add(vertiecies.Count - 2);

    //    triangles.Add(vertiecies.Count - 3);
    //    triangles.Add(vertiecies.Count - 1);
    //    triangles.Add(vertiecies.Count - 2);
    //}

    //private void AddUvs(BlockType blockType, Vector3Int normal)
    //{
    //    BlockInfo blockInfo = chunkData.parentWorld.blocksDataBase.GetInfo(blockType);
    //    Vector2 uv;

    //    int sizeTile = 16;
    //    int sizeTexure = 256;

    //    if (blockInfo != null)
    //    {
    //        uv = blockInfo.GetPixelsOffset(normal);
    //    }
    //    else
    //    {
    //        uv = new Vector2(160f, 224f);
    //    }

    //    if (normal == Vector3Int.right || normal == Vector3Int.back)
    //    {
    //        uvs.Add(new Vector2(uv.x, uv.y) / sizeTexure);
    //        uvs.Add(new Vector2(uv.x, uv.y + sizeTile) / sizeTexure);
    //        uvs.Add(new Vector2(uv.x + sizeTile, uv.y) / sizeTexure);
    //        uvs.Add(new Vector2(uv.x + sizeTile, uv.y + sizeTile) / sizeTexure);
    //    }
    //    else 
    //    {
    //        uvs.Add(new Vector2(uv.x, uv.y) / sizeTexure);
    //        uvs.Add(new Vector2(uv.x + sizeTile, uv.y) / sizeTexure);
    //        uvs.Add(new Vector2(uv.x, uv.y + sizeTile) / sizeTexure);
    //        uvs.Add(new Vector2(uv.x + sizeTile, uv.y + sizeTile) / sizeTexure);
    //    }
    //}
}
