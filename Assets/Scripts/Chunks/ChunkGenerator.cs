using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] BiomeAttributes[] biomes;
    [SerializeField] ChunkRenderer chunkPrefab;

    private SaveLoad saveLoad;
    private GameWorld gameWorld;

    [Inject]
    private void Construct(GameWorld gameWorld, SaveLoad saveLoad)
    {
        this.gameWorld = gameWorld;
        this.saveLoad = saveLoad;
    }

    public ChunkData Generate(Vector2Int chunkCoordinate)
    {
        ChunkData chunkData = new ChunkData();

        int xPos = chunkCoordinate.x * WorldConstants.chunkWidth;
        int zPos = chunkCoordinate.y * WorldConstants.chunkWidth;

        List<Vector3Int> treePositions = new List<Vector3Int>();

        chunkData.bloks = TerrainGenerator.GenerateTerrain(WorldConstants.chunkWidth, WorldConstants.chunkHeight, xPos, zPos, biomes, ref treePositions);

        foreach (Vector3Int treePos in treePositions)
        {
            AddStructure(treePos, chunkData, BlockStructureType.Tree1);
        }

        chunkData.chunkPosition = chunkCoordinate;

        chunkData.parentWorld = gameWorld;

        gameWorld.activeChunkDatas.Add(chunkData.chunkPosition, chunkData);

        return chunkData;
    }

    public void AddStructure(Vector3Int structureLocalCoordinate, ChunkData chunkData, BlockStructureType blockStructureType)
    {
        StructureGenerator.AddStructure(structureLocalCoordinate, chunkData, blockStructureType, gameWorld);
    }

    public void AddSavedBlocks(ChunkData chunkData)
    {
        SaveChunk saveChunk;

        if (saveLoad.HasSavedChunk(chunkData.chunkPosition, out saveChunk))
        {
            foreach (var block in saveChunk.blocks)
            {
                chunkData.bloks[block.coordinate] = block.blockType;
            }
        }
    }

    public void FillStructure(Vector2Int chunkCoordinate, ChunkData chunkData)
    {
        foreach (PositionType coordinateType in gameWorld.needToFillChunk[chunkCoordinate])
        {
            chunkData.bloks[MyMath.GetBlockCoordinate(coordinateType.position)] = coordinateType.blockType;
        }

        gameWorld.needToFillChunk.Remove(chunkCoordinate);
    }

    public void RenderChunk(ChunkData chunkData)
    {
        int xPos = chunkData.chunkPosition.x * WorldConstants.chunkWidth;
        int zPos = chunkData.chunkPosition.y * WorldConstants.chunkWidth;

        ChunkRenderer chunk;

        if (chunkData.isSpawned)
        {
            chunk = gameWorld.activeChunkDatas[chunkData.chunkPosition].chunkRenderer;
        }
        else
        {
            chunk = Instantiate(chunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
            chunkData.isSpawned = true;
        }

        chunk.GenerateChunk(WorldConstants.chunkWidth, WorldConstants.chunkHeight, chunkData);

        chunk.gameObject.SetActive(true);
        chunkData.chunkRenderer = chunk;
    }
}
