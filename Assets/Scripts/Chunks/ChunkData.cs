using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public Vector2Int chunkPosition;
    public ChunkRenderer chunkRenderer;
    public GameWorld parentWorld;
    public BlockType[] bloks;
    public bool isSpawned;
}
