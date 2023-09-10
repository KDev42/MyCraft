using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public BlockInfoDataBase blocksDataBase;
    [SerializeField] ChunkRenderer chunkPrefab;
    [SerializeField] GameObject player;
    [SerializeField] LayerMask unitLayer;

    public Dictionary<Vector2Int, ChunkData> activeChunkDatas = new Dictionary<Vector2Int, ChunkData>();
    public Dictionary<Vector2Int, ChunkData> disableChunkDatas = new Dictionary<Vector2Int, ChunkData>();

    public Dictionary<Vector2Int, List<PositionType>> needToFillChunk = new Dictionary<Vector2Int, List<PositionType>>();

    public bool HasObstacles(Vector3 position, DirectionType directionType, int heinght, int width)
    {
        Vector3Int coordinate = GetBlockCoordinate(position);
        switch (directionType)
        {
            case DirectionType.up:
                return IsSolidBlock(coordinate + Vector3Int.up*heinght);
            case DirectionType.down:
                return IsSolidBlock(coordinate + Vector3Int.down );
            case DirectionType.right:
                return HasSideObstacles(coordinate, Vector3Int.right, heinght);
            case DirectionType.left:
                return HasSideObstacles(coordinate, Vector3Int.left, heinght);
            case DirectionType.back:
                return HasSideObstacles(coordinate, Vector3Int.back, heinght);
            case DirectionType.forward:
                return HasSideObstacles(coordinate, Vector3Int.forward, heinght);
        }

        return false;
    }

    private bool HasSideObstacles(Vector3Int startCoordinate, Vector3Int direction, int heinght )
    {
        for(int y = 0; y < heinght; y++)
        {
            if (IsSolidBlock(startCoordinate + direction  + Vector3Int.up* y))
                return true;
        }

        return false;
    }

    public bool CanBuild(Vector3 position)
    {
        position = GetBlockCoordinate(position);

        Debug.DrawLine(position, position + new Vector3(1, 1, 1), Color.red,10);
        Debug.DrawLine(position, position + new Vector3(1, 1, 0), Color.red, 10);
        Debug.DrawLine(position, position + new Vector3(0, 1, 1), Color.red, 10);

        return !(Physics.Raycast(position, new Vector3(1,1,0), 1,unitLayer)
               || Physics.Raycast(position, new Vector3(0, 1, 1),1, unitLayer)
               || Physics.Raycast(position, new Vector3(1, 1, 1),1.4f, unitLayer));
    }

    public ChunkData GetChunkData(Vector3Int coordinate)
    {
        Vector2Int chunkCoordinate = GetChunckCoordinate(coordinate);

        return activeChunkDatas[chunkCoordinate];
    }

    public Vector3Int GetBlockCoordinate(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        int z = Mathf.FloorToInt(position.z);

        return new Vector3Int(x, y, z);
    }

    public bool IsSolidBlock(Vector3 position)
    {
        BlockType blockType = GetBlock(position);
        if (blockType != BlockType.none)
        {
            BlockInfo blockInfo = blocksDataBase.GetInfo(blockType);
            if (blockInfo != null)
                return blockInfo.isSolid;
        }

        return true;
    }

    public void AddBlock(Vector3 position, BlockType blockType)
    {
        Vector2Int chunkCoordinate = GetChunckCoordinate(position);
        Vector3Int blockCoordinate = GetBlockLocalCoordinate(chunkCoordinate, position);

        activeChunkDatas[chunkCoordinate].chunkRenderer.SpawnBlock(blockCoordinate, blockType);

    }

    public void DestroyBlock(Vector3 position)
    {
        BlockType blockType = GetBlock(position);

        BlockInfo blockInfo = blocksDataBase.GetInfo(blockType);

        if (blockInfo.isDestroyed)
        {
            Vector2Int chunkCoordinate = GetChunckCoordinate(position);
            Vector3Int blockCoordinate = GetBlockLocalCoordinate(chunkCoordinate, position);
            activeChunkDatas[chunkCoordinate].chunkRenderer.SpawnBlock(blockCoordinate, BlockType.air);

            EventsHolder.BrokenBlock(blockType, GetBlockCoordinate(position));
        }
    }

    public bool HasBlock(Vector3 position)
    {
        Vector2Int chunkCoordinate = GetChunckCoordinate(position);

        if (!IsChunkInWorld(chunkCoordinate) || position.y < 0 || position.y > WorldConstants.chunkHeight)
            return false;

        if (activeChunkDatas.ContainsKey(chunkCoordinate))
            return IsSolidBlock(position);

        return false;

    }

    public Vector2Int GetChunckCoordinate(Vector3 position)
    {
        return new Vector2Int((int)(position.x / WorldConstants.chunkWidth), (int)(position.z / WorldConstants.chunkWidth));
    }

    public bool IsChunkInWorld(Vector2Int coord)
    {
        if (coord.x >= 0 && coord.x < WorldConstants.maxSizeWorld && coord.y >= 0 && coord.y < WorldConstants.maxSizeWorld)
            return true;
        else
            return false;
    }

    public Vector3Int GetBlockGlobalCoordinate(Vector2Int chunkCoordinate, Vector3Int localCoordinate)
    {
        return new Vector3Int(chunkCoordinate.x * WorldConstants.chunkWidth, 0, chunkCoordinate.y * WorldConstants.chunkWidth) + localCoordinate;
    }

    public Vector3Int GetBlockLocalCoordinate(Vector2Int chunkCoordinate, Vector3 position)
    {
        Vector3 blockLocalPosition = position - new Vector3(chunkCoordinate.x * WorldConstants.chunkWidth, 0, chunkCoordinate.y * WorldConstants.chunkWidth);

        int x = Mathf.FloorToInt(blockLocalPosition.x);
        int y = Mathf.FloorToInt(blockLocalPosition.y);
        int z = Mathf.FloorToInt(blockLocalPosition.z);

        return new Vector3Int(x,y,z);
    }

    public BlockType GetBlock(Vector3 position)
    {
        Vector2Int chunkCoordinate = GetChunckCoordinate(position);
        Vector3Int blockCoordinate = GetBlockCoordinate( position);
        Vector3Int blockLocalCoordinate = GetBlockLocalCoordinate(chunkCoordinate, position);

        if (BlockInWorld(blockCoordinate) )
            return activeChunkDatas[chunkCoordinate].bloks[MyMath.GetBlockCoordinate(blockLocalCoordinate)];
        else
            return BlockType.none;
    }

    public BlockInfo GetBlockInfo(Vector3 position)
    {
        BlockType blockType = GetBlock(position);
        return blocksDataBase.GetInfo(blockType);
    }

    public DirectionType GetPlaneOrientation(Vector3 impactPosition) 
    {
        Vector3 centerPoint = GetBlockCoordinate(impactPosition) + new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 directionToCenter = impactPosition - centerPoint;

        if (Mathf.Abs( directionToCenter.x) > Mathf.Abs(directionToCenter.y) && Mathf.Abs(directionToCenter.x)> Mathf.Abs(directionToCenter.z))
        {
            if (directionToCenter.x >0 )
            {
                return DirectionType.right;
            }
            else
            {
                return DirectionType.left;
            }
        }
        else if (Mathf.Abs(directionToCenter.y) > Mathf.Abs(directionToCenter.z))
        {
            if (directionToCenter.y > 0)
            {
                return DirectionType.up;
            }
            else
            {
                return DirectionType.down;
            }
        }
        else
        {
            if (directionToCenter.z > 0)
            {
                return DirectionType.forward;
            }
            else
            {
                return DirectionType.back;
            }
        }
    }

    private bool BlockInWorld(Vector3Int coordinate)
    {
        return coordinate.y >= 0 && coordinate.y <= WorldConstants.chunkHeight 
            && coordinate.x >= 0 && coordinate.x < WorldConstants.chunkWidth * WorldConstants.maxSizeWorld 
            && coordinate.z >= 0 && coordinate.z < WorldConstants.chunkWidth* WorldConstants.maxSizeWorld ;
    }
}

public enum DirectionType
{
    up,
    down,
    right,
    left,
    forward,
    back,
    round
}
