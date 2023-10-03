using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using Zenject;
using System;
using System.Linq;
using System.Threading.Tasks;

[RequireComponent(typeof(ChunkGenerator))]
public class WorldGenerator : MonoBehaviour
{
    [SerializeField] ChunkGenerator chunkGenerator;
    [SerializeField] GameObject player;
    [SerializeField] WorldRendering worldRendering;
    //[SerializeField] BiomeAttributes[] biome

    private bool heroIsSpawn;
    private Action endGenetation;
    //private bool chunkIsFill;
    //private bool generationCompleted = true;

    private Vector3 playerPosition;
    private int renderDistance;
    private SaveLoad saveLoad;
    private GameData gameData;
    private GameWorld gameWorld;
    private List<Vector2Int> toCreate = new List<Vector2Int>();
    private Queue< ChunkData> toRender = new Queue< ChunkData>();
    private Dictionary<Vector2Int, ChunkData> toGenerate = new Dictionary<Vector2Int, ChunkData>();

    private static ProfilerMarker marker = new ProfilerMarker(ProfilerCategory.Loading, "LoadingWorld");

    [Inject]
    private void Construct(GameWorld gameWorld, GameData gameData, SaveLoad saveLoad)
    {
        this.gameData = gameData;
        this.gameWorld = gameWorld;
        this.saveLoad = saveLoad;
    }

    private void OnValidate()
    {
        chunkGenerator ??= GetComponent<ChunkGenerator>();
    }

    private void Awake()
    {
        LoadBlockStructure.LoadStructure(BlockStructureType.Tree1);
        LoadBlockStructure.LoadStructure(BlockStructureType.House);
        LoadBlockStructure.LoadStructure(BlockStructureType.Mine);
    }

    public void GenerateMap(Action callback, Vector3 playerPosition)
    {
        //marker.Begin();
        this.playerPosition = playerPosition;
        Debug.Log("pos " + this.playerPosition);

        Vector2Int startCoordinate = gameWorld.GetChunckCoordinate(playerPosition) ;
        startCoordinate.x -= gameData.gameSettings.renderDistance;
        startCoordinate.y -= gameData.gameSettings.renderDistance;

        renderDistance = gameData.gameSettings.renderDistance;
        GenerateMainStructures();
        List<Vector2Int> chunkPositions = new List<Vector2Int>();

        for (int x = startCoordinate.x; x < startCoordinate.x + renderDistance * 2 + 1; x++)
        {
            for (int z = startCoordinate.y; z < startCoordinate.y + renderDistance * 2 + 1; z++)
            {
                chunkPositions.Add(new Vector2Int(x, z));
                //GenerateChunk(x, z);
            }
        }

        AddQueue(chunkPositions, callback);
        //SpawnQueue(chunkPositions, SpawnPlayer);
    }

    public void Update()
    {
        if (toRender.Count > 0)
        {
            chunkGenerator.RenderChunk(toRender.Dequeue());
        }
    }

    public void DeleteQueue(List<Vector2Int> positions)
    {
        foreach (Vector2Int position in positions)
        {
            if (toCreate.Contains(position))
            {
                toCreate.Remove(position);
            }
        }
    }

    public void AddQueue(List<Vector2Int> positions, Action callback = null)
    {
        foreach (Vector2Int position in positions)
        {
            if (!toCreate.Contains(position))
            {
                toCreate.Add(position);
            }
        }

        endGenetation = callback;
        StartCoroutine( GenerationChunks());
    }


    //private async Task GenerationChunks()
    //{
    //    await Task.Run(() =>
    //    {
    //        while (toCreate.Count > 0)
    //        {
    //            int index = toCreate.Count - 1;

    //            toGenerate.Add(toCreate[index] , ActivateChunk(toCreate[index]));
    //            //toRender.Add(toCreate[index]);

    //            toCreate.RemoveAt(index);
    //        }
    //    });

    //    GenerationStructures();
    //}

    IEnumerator GenerationChunks()
    {
        while (toCreate.Count > 0)
        {
            int index = toCreate.Count - 1;

            toGenerate.Add(toCreate[index], ActivateChunk(toCreate[index]));
            //toRender.Add(toCreate[index]);

            toCreate.RemoveAt(index);

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("created");
        GenerationStructures();
    }

    private void GenerationStructures()
    {
        foreach(var cnunk in toGenerate)
        {
            if (cnunk.Key.x == mineChunkCoordinate.x && cnunk.Key.y == mineChunkCoordinate.y)
            {
                mineCoordinate.y = 50;
                chunkGenerator.AddStructure(mineCoordinate, cnunk.Value, BlockStructureType.Mine);
            }

            if (cnunk.Key.x == houseChunkCoordinate.x && cnunk.Key.y == houseChunkCoordinate.y)
            {
                houseCoordinate.y =  GrassCoordinate(houseCoordinate.x, houseCoordinate.z);
                chunkGenerator.AddStructure(houseCoordinate, cnunk.Value, BlockStructureType.House);
            }
        }

        FillChunks();
    }

    private void FillChunks()
    {
        foreach (var chunk in gameWorld.needToFillChunk)
        {
            if (gameWorld.activeChunkDatas.ContainsKey(chunk.Key) && !toGenerate.ContainsKey(chunk.Key))
            {
                toGenerate.Add(chunk.Key, gameWorld.activeChunkDatas[chunk.Key]);
            }
        }

        foreach (var chunk in toGenerate)
        {
            if (gameWorld.needToFillChunk.ContainsKey(chunk.Key))
            {
                chunkGenerator.FillStructure(chunk.Key,chunk.Value);
            }
        }

        AddSaveBlocks();
    }

    private void AddSaveBlocks()
    {
        foreach (var chunk in toGenerate)
        {
            chunkGenerator.AddSavedBlocks(chunk.Value);
            toRender.Enqueue( chunk.Value);
        }

        toGenerate.Clear();

        endGenetation?.Invoke();
        CompleteGeneration();
    }

    //private void RenderChunks()
    //{
    //    foreach (var chunk in toGenerate)
    //    {
    //        chunkGenerator.RenderChunk(chunk.Value);
    //    }

    //    toGenerate.Clear();
    //}

    private void CompleteGeneration()
    {
        if (!heroIsSpawn)
        {
            Debug.Log("Spawn");
            heroIsSpawn = true;
            SpawnPlayer();
        }
    }

    private ChunkData ActivateChunk(Vector2Int chunkPosition)
    {
        ChunkData activatedChunk;
        if (gameWorld.disableChunkDatas.ContainsKey(chunkPosition))
        {
            activatedChunk = gameWorld.disableChunkDatas[chunkPosition];
            //activatedChunk.chunkRenderer.gameObject.SetActive(true);

            gameWorld.activeChunkDatas.Add(chunkPosition, activatedChunk);
            gameWorld.disableChunkDatas.Remove(chunkPosition);
        }
        else
        {
            activatedChunk = chunkGenerator.Generate(new Vector2Int(chunkPosition.x, chunkPosition.y));
        }

        return activatedChunk;
    }

    private void SpawnPlayer()
    {
        // marker.End();
        playerPosition.y =  GrassCoordinate((int)playerPosition.x, (int)playerPosition.z);

        player.transform.position = playerPosition;
        player.SetActive(true);
        gameData.player = player;

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
        float coord = (WorldConstants.maxSizeWorld * WorldConstants.chunkWidth) / 2 + 0.5f;

        Vector3 pos = new Vector3(coord,10, coord);

        houseCoordinate = gameWorld.GetBlockCoordinate(pos); 
        houseChunkCoordinate = gameWorld.GetChunckCoordinate(houseCoordinate);

        mineCoordinate = houseCoordinate;
        mineCoordinate.x -= 30;
        mineChunkCoordinate = gameWorld.GetChunckCoordinate(mineCoordinate);
    }


    //public void SpawnQueue(List<Vector2Int> positions, Action callback)
    //{
    //    StartCoroutine(GenerateChunk(positions, (chunkData) => {
    //        ChunkToFill();
    //        StartCoroutine(RenderChunk(chunkData, callback));
    //    }));
    //}

    //private void RenderChunk( ChunkData chunkData)
    //{
    //    int xPos = chunkData.chunkPosition.x * WorldConstants.chunkWidth;
    //    int zPos = chunkData.chunkPosition.y * WorldConstants.chunkWidth;
    //    ChunkRenderer chunk = Instantiate(chunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
    //    chunk.GenerateChunk(WorldConstants.chunkWidth, WorldConstants.chunkHeight, chunkData);

    //    chunkData.chunkRenderer = chunk;
    //}


    //private void GenerateChunk(int posChunkX, int posChunkZ)
    //{
    //    ChunkData chunkData = new ChunkData();

    //    int xPos = posChunkX * WorldConstants.chunkWidth;
    //    int zPos = posChunkZ * WorldConstants.chunkWidth;

    //    List<Vector3Int> treePositions = new List<Vector3Int>();

    //    chunkData.bloks = TerrainGenerator.GenerateTerrain(WorldConstants.chunkWidth, WorldConstants.chunkHeight, xPos, zPos, biomes, ref treePositions);

    //    foreach (Vector3Int treePos in treePositions)
    //    {
    //        StructureGenerator.AddStructure(treePos, chunkData, BlockStructureType.Tree1, gameWorld);
    //    }

    //    chunkData.chunkPosition = new Vector2Int(posChunkX, posChunkZ);

    //    chunkData.parentWorld = gameWorld;
    //    gameWorld.activeChunkDatas.Add(new Vector2Int(posChunkX, posChunkZ), chunkData);

    //    if (posChunkX == mineChunkCoordinate.x && posChunkZ == mineChunkCoordinate.y)
    //    {
    //        mineCoordinate.y = 50;
    //        StructureGenerator.AddStructure(mineCoordinate, chunkData, BlockStructureType.Mine, gameWorld);
    //    }

    //    if (posChunkX == houseChunkCoordinate.x && posChunkZ == houseChunkCoordinate.y)
    //    {
    //        houseCoordinate.y = GrassCoordinate(houseCoordinate.x, houseCoordinate.z);
    //        StructureGenerator.AddStructure(houseCoordinate, chunkData, BlockStructureType.House, gameWorld);
    //    }

    //    rendetingChunkDatas.Add(chunkData);
    //}

    //private void AddSavedBlocks( ChunkData chunkData)
    //{
    //    SaveChunk saveChunk;

    //    if(saveLoad.HasSavedChunk(chunkData.chunkPosition, out saveChunk))
    //    {
    //        foreach (var block in saveChunk.blocks)
    //        {
    //            chunkData.bloks[block.coordinate] = block.blockType;
    //        }
    //    }
    //}

    //private void FiilChunk(Vector2Int chunkCoordinate, ChunkData chunkData)
    //{
    //    if (gameWorld.needToFillChunk.ContainsKey(chunkCoordinate))
    //    {
    //        foreach(PositionType coordinateType in gameWorld.needToFillChunk[chunkCoordinate])
    //        {
    //            chunkData.bloks[MyMath.GetBlockCoordinate(coordinateType.position)] = coordinateType.blockType;
    //        }

    //        gameWorld.needToFillChunk.Remove(chunkCoordinate);
    //    }

    //    chunkIsFill = true;
    //}

    //private bool ContaineCoordinate(List<Vector2Int> coordinates, Vector2Int coordinate)
    //{
    //    return coordinates.Any(s => s == coordinate);
    //}
}
