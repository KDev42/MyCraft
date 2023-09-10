using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using Zenject;
using System;
using System.IO;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] ChunkRenderer chunkPrefab;
    [SerializeField] GameObject player;
    [SerializeField] WorldRendering worldRendering;
    [SerializeField] BiomeAttributes[] biomes;

    private bool heroIsSpawn;
    private bool chunkIsFill;
    private bool generationCompleted = true;

    private GameWorld gameWorld;
    private List<Vector2Int> renderingChunk = new List<Vector2Int>();
    private List<ChunkData> rendetingChunkDatas = new List<ChunkData>();

    private static ProfilerMarker marker = new ProfilerMarker(ProfilerCategory.Loading, "LoadingWorld");

    [Inject]
    private void Construct(GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;
    }

    private void Update()
    {
        if (!generationCompleted)
        {
            if (renderingChunk.Count > 0)
            {
                int index = renderingChunk.Count - 1;

                ActivateChunk(renderingChunk[index]);
                //GenerateChunk(renderingChunk[index].x, renderingChunk[index].y);

                renderingChunk.RemoveAt(index);
            }
            else if (!chunkIsFill)
            {
                ChunkToFill();
            }
            else if (rendetingChunkDatas.Count > 0)
            {
                int index = rendetingChunkDatas.Count - 1;
                ChunkData chunkData = rendetingChunkDatas[index];
                RenderChunk(chunkData);

                rendetingChunkDatas.RemoveAt(index);
            }
            else
            {
                generationCompleted = true;
                CompleteGeneration();
            }
        }
    }

    public void GenerateMap()
    {
        //marker.Begin();
        GenerateMainStructures();
        List<Vector2Int> chunkPositions = new List<Vector2Int>();

        for (int x = 0; x < GameSettings.renderDistance * 2 + 1; x++)
        {
            for (int z = 0; z < GameSettings.renderDistance * 2 + 1; z++)
            {
                chunkPositions.Add(new Vector2Int(x, z));
                //GenerateChunk(x, z);
            }
        }

        AddQueue(chunkPositions);
        //SpawnQueue(chunkPositions, SpawnPlayer);
    }

    public void DeleteQueue(List<Vector2Int> positions)
    {
        foreach (Vector2Int position in positions)
        {
            if (renderingChunk.Contains(position))
            {
                renderingChunk.Remove(position);
            }
        }
    }

    public void AddQueue(List<Vector2Int> positions)
    {
        foreach (Vector2Int position in positions)
        {
            if (!renderingChunk.Contains(position))
            {
                renderingChunk.Add(position);
            }
        }

        generationCompleted = false;
        chunkIsFill = false;
    }

    //public void SpawnQueue(List<Vector2Int> positions, Action callback)
    //{
    //    StartCoroutine(GenerateChunk(positions, (chunkData) => {
    //        ChunkToFill();
    //        StartCoroutine(RenderChunk(chunkData, callback));
    //    }));
    //}

    private void CompleteGeneration()
    {
        if (!heroIsSpawn)
        {
            heroIsSpawn = true;
            SpawnPlayer();
        }
    }

    private void ActivateChunk(Vector2Int chunkPosition)
    {
        if (gameWorld.disableChunkDatas.ContainsKey(chunkPosition))
        {
            ChunkData activatedChunk = gameWorld.disableChunkDatas[chunkPosition];
            activatedChunk.chunkRenderer.gameObject.SetActive(true);

            gameWorld.activeChunkDatas.Add(chunkPosition, activatedChunk);
            gameWorld.disableChunkDatas.Remove(chunkPosition);
        }
        else
        {
            GenerateChunk(chunkPosition.x, chunkPosition.y);
        }
    }

    private void SpawnPlayer()
    {
        // marker.End();

        float xzCoord = (GameSettings.renderDistance + 0.5f) * WorldConstants.chunkWidth;
        int y = GrassCoordinate((int)xzCoord, (int)xzCoord);

        Vector3Int spawnCoordinate = new Vector3Int((int)xzCoord, y, (int)xzCoord);


        player.transform.position = spawnCoordinate;
        player.SetActive(true);

        worldRendering.ActivateRender(this, player.transform);
    }

    private int GrassCoordinate(int x, int z)
    {
        for(int y = WorldConstants.chunkHeight; y > 0; y--)
        {
            if (gameWorld.IsSolidBlock(new Vector3(x, y, z)))
                return y+1;
        }

        return 0;
    }

    private Vector3Int mineCoordinate;
    private Vector2Int mineChunkCoordinate;
    private Vector3Int houseCoordinate;
    private Vector2Int houseChunkCoordinate;

    private void GenerateMainStructures()
    {
        float xzCoord = (GameSettings.renderDistance + 0.5f) * WorldConstants.chunkWidth;
        mineCoordinate = new Vector3Int((int)xzCoord, 0, (int)xzCoord);
        mineChunkCoordinate = gameWorld.GetChunckCoordinate(mineCoordinate);

        houseCoordinate = mineCoordinate;
        houseCoordinate.x -= 30;
        houseChunkCoordinate = gameWorld.GetChunckCoordinate(houseCoordinate);
    }

    private void GenerateChunk(int posChunkX, int posChunkZ)
    {
        ChunkData chunkData = new ChunkData();

        int xPos = posChunkX * WorldConstants.chunkWidth;
        int zPos = posChunkZ * WorldConstants.chunkWidth;

        List<Vector3Int> treePositions = new List<Vector3Int>();

        chunkData.bloks = TerrainGenerator.GenerateTerrain(WorldConstants.chunkWidth, WorldConstants.chunkHeight, xPos, zPos, biomes, ref treePositions);

        foreach (Vector3Int treePos in treePositions)
        {
            StructureGenerator.AddStructure(treePos, chunkData, BlockStructureType.Tree1, gameWorld);
        }

        chunkData.chunkPosition = new Vector2Int(posChunkX, posChunkZ);

        chunkData.parentWorld = gameWorld;
        gameWorld.activeChunkDatas.Add(new Vector2Int(posChunkX, posChunkZ), chunkData);

        if (posChunkX == mineChunkCoordinate.x && posChunkZ == mineChunkCoordinate.y)
        {
            mineCoordinate.y = 50;
            StructureGenerator.AddStructure(mineCoordinate, chunkData, BlockStructureType.Mine, gameWorld);
        }

        if (posChunkX == houseChunkCoordinate.x && posChunkZ == houseChunkCoordinate.y)
        {
            houseCoordinate.y = GrassCoordinate(houseCoordinate.x, houseCoordinate.z);
            StructureGenerator.AddStructure(houseCoordinate, chunkData, BlockStructureType.House, gameWorld);
        }

        rendetingChunkDatas.Add(chunkData);
    }

    private void RenderChunk( ChunkData chunkData)
    {
        int xPos = chunkData.chunkPosition.x * WorldConstants.chunkWidth;
        int zPos = chunkData.chunkPosition.y * WorldConstants.chunkWidth;
        ChunkRenderer chunk = Instantiate(chunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
        chunk.GenerateChunk(WorldConstants.chunkWidth, WorldConstants.chunkHeight, chunkData);

        chunkData.chunkRenderer = chunk;
    }

    private void ChunkToFill()
    {
        foreach (var chunkData in gameWorld.activeChunkDatas)
        {
            FiilChunk(chunkData.Key, chunkData.Value);
        }
    }

    private void FiilChunk(Vector2Int chunkCoordinate, ChunkData chunkData)
    {
        if (gameWorld.needToFillChunk.ContainsKey(chunkCoordinate))
        {
            foreach(PositionType coordinateType in gameWorld.needToFillChunk[chunkCoordinate])
            {
                chunkData.bloks[MyMath.GetBlockCoordinate(coordinateType.position)] = coordinateType.blockType;
            }

            gameWorld.needToFillChunk.Remove(chunkCoordinate);
        }

        chunkIsFill = true;
    }

    //private IEnumerator GenerateChunk(List<Vector2Int> positions, Action<List<ChunkData>> callback)
    //{
    //    List<ChunkData> chunkDatas = new List<ChunkData>();
    //    for (int i = 0; i < positions.Count; i++)
    //    {
    //        ChunkData chunkData = new ChunkData();
    //        Vector2Int currentPos = positions[i];

    //        int posChunkX = currentPos.x;
    //        int posChunkZ = currentPos.y;
    //        int xPos = posChunkX * WorldConstants.chunkWidth;
    //        int zPos = posChunkZ * WorldConstants.chunkWidth;

    //        List<Vector3Int> treePositions = new List<Vector3Int>();

    //        chunkData.bloks = TerrainGenerator.GenerateTerrain(WorldConstants.chunkWidth, WorldConstants.chunkHeight, xPos, zPos, biomes, ref treePositions);

    //        foreach (Vector3Int treePos in treePositions)
    //        {
    //            StructureGenerator.AddStructure(treePos, chunkData, BlockStructureType.Tree1, gameWorld);
    //        }

    //        chunkData.chunkPosition = new Vector2Int(posChunkX, posChunkZ);


    //        chunkData.parentWorld = gameWorld;
    //        gameWorld.activeChunkDatas.Add(new Vector2Int(posChunkX, posChunkZ), chunkData);

    //        chunkDatas.Add(chunkData);

    //        yield return new WaitForSeconds(0.001f);
    //    }
    //    callback?.Invoke(chunkDatas);
    //}

    //private IEnumerator RenderChunk(List<ChunkData> chunkDatas, Action callback)
    //{
    //    for (int i = 0; i < chunkDatas.Count; i++)
    //    {
    //        ChunkData chunkData = chunkDatas[i];
    //        int xPos = chunkData.chunkPosition.x * WorldConstants.chunkWidth;
    //        int zPos = chunkData.chunkPosition.y * WorldConstants.chunkWidth;

    //        ChunkRenderer chunk = Instantiate(chunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
    //        chunk.GenerateChunk(WorldConstants.chunkWidth, WorldConstants.chunkHeight, chunkData);

    //        chunkData.chunkRenderer = chunk;

    //        yield return new WaitForEndOfFrame();
    //    }

    //    callback?.Invoke();
    //}
}
