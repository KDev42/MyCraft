using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class SaveLoad : MonoBehaviour
{
    private WorldSettings worldSettings;
    private GameSettings gameSettings;
    private Dictionary<Vector2Int, SaveChunk> chunkForSave = new Dictionary<Vector2Int, SaveChunk>();

    private GameData gameData;

    [Inject]
    private void Construct(GameData gameData)
    {
        this.gameData = gameData;
    }

    private void Awake()
    {
        EventsHolder.saveWorld += SaveWorld;
    }

    public void SavePlayerSettings()
    {
        string js = JsonUtility.ToJson(gameSettings);
        PlayerPrefs.SetString("PlayerSettings", js);
    }

    public GameSettings LoadPlayerSettings()
    {
        if (PlayerPrefs.HasKey("PlayerSettings"))
        {
            gameSettings = JsonUtility.FromJson<GameSettings>(PlayerPrefs.GetString("PlayerSettings"));
        }
        else
        {
            gameSettings = new GameSettings();
        }
        
        return gameSettings;
    }

    public bool HasSavedChunk(Vector2Int chunkCoordinate, out SaveChunk savedChunk)
    {
        string path = GetChunkPath(1, chunkCoordinate);

        //PlayerPrefs.DeleteKey(path);

        if (PlayerPrefs.HasKey(path))
        {
            savedChunk = JsonUtility.FromJson<SaveChunk>(PlayerPrefs.GetString(path));
            Debug.Log(JsonUtility.ToJson(savedChunk));
            return true;
        }

        savedChunk = null;
        return false;
    }

    public void AddBlockToSave(Vector2Int chunkCoordinate, int blockLocalCoordinate, BlockType blockType)
    {
        SaveChunk savedChunk;

        if (chunkForSave.ContainsKey(chunkCoordinate))
        {
            savedChunk = chunkForSave[chunkCoordinate];

            if (savedChunk.ContainsBlock(blockLocalCoordinate, out int index))
            {
                SaveBlock saveBlock = new SaveBlock(blockType, blockLocalCoordinate);
                savedChunk.blocks[index] = saveBlock;
            }
            else
            {
                SaveBlock saveBlock = new SaveBlock(blockType, blockLocalCoordinate);
                savedChunk.blocks.Add( saveBlock);
            }
        }
        else
        {
            savedChunk = new SaveChunk();

            SaveBlock saveBlock = new SaveBlock(blockType, blockLocalCoordinate);
            savedChunk.blocks.Add( saveBlock);

            chunkForSave.Add(chunkCoordinate, savedChunk);
        }
    }

    public WorldSettings LoadWorldSettings()
    {
        if (PlayerPrefs.HasKey("WorldSettings"))
        {
            worldSettings = JsonUtility.FromJson<WorldSettings>(PlayerPrefs.GetString("WorldSettings"));
        }
        else
        {
            worldSettings = new WorldSettings();
#if UNITY_EDITOR
            //StartTool();
#endif
        }

        StartTool();

        return worldSettings;
        //ClearWorld();
    }

    public void SaveWorldSettings()
    {
        worldSettings.handInventory = PlayerData.HandInventory.Items;
        worldSettings.mainInventory = PlayerData.MainInventory.Items;
        string js = JsonUtility.ToJson(worldSettings);
        PlayerPrefs.SetString("WorldSettings", js);
    }

    public void ClearWorld()
    {
        foreach(Vector2Int chunkCoordinate in worldSettings.savedChunks)
        {
            string path = GetChunkPath(1, chunkCoordinate);
            PlayerPrefs.DeleteKey(path);
        }

        worldSettings.savedChunks.Clear();

        PlayerPrefs.DeleteKey("WorldSettings");
    }

    private void SaveWorld()
    {
        foreach (var chunk in chunkForSave)
        {
            worldSettings.savedChunks.Add(chunk.Key);
            SaveChunk(chunk.Key);
        }

        worldSettings.playerPosition = gameData.player.transform.position;
        SaveWorldSettings();
        chunkForSave.Clear();
    }

    private string GetChunkPath(int seed, Vector2Int chunkCoordinate)
    {
        return "Chunk_" + seed + "_" + chunkCoordinate;
    }

    private void SaveChunk(Vector2Int chunkCoordinate)
    {
        SaveChunk savedChunk; 
        SaveChunk addedChunck = chunkForSave[chunkCoordinate];

        if (HasSavedChunk(chunkCoordinate,out savedChunk))
        {
            foreach(var block in addedChunck.blocks)
            {
                if(savedChunk.ContainsBlock(block.coordinate, out int index))
                {
                    savedChunk.blocks[index] = block;
                }
                else
                {
                    savedChunk.blocks.Add(block);
                }
            }
        }
        else
        {
            //savedChunk = new SaveChunk();
            savedChunk = addedChunck;
        }

        string path = GetChunkPath(1, chunkCoordinate);


        string js = JsonUtility.ToJson(savedChunk);
        PlayerPrefs.SetString(path, js);
        Debug.Log("dgdgfndfb " + path + "  "  + js);
    }

    private void StartTool()
    {
        worldSettings.handInventory[0] = new ItemStack(ItemType.grassBlock, 1);
        worldSettings.handInventory[1] = new ItemStack(ItemType.dirtBlock, 1);
        worldSettings.handInventory[2] = new ItemStack(ItemType.oakLogBlock, 1);
        worldSettings.handInventory[3] = new ItemStack(ItemType.oakPlanksBlock, 1);
        worldSettings.handInventory[4] = new ItemStack(ItemType.strippedOakLogBlock, 1);
        worldSettings.handInventory[5] = new ItemStack(ItemType.stoneBrickBlock, 1);
        worldSettings.handInventory[6] = new ItemStack(ItemType.brickBlock, 1);
        worldSettings.handInventory[7] = new ItemStack(ItemType.strippedBirckLogBlock, 1);
        worldSettings.handInventory[8] = new ItemStack(ItemType.cobblestoneBlock, 1);

        //worldSettings.mainInventory[0] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[1] = new ItemStack(ItemType.goldBlock, 1);
        //worldSettings.mainInventory[2] = new ItemStack(ItemType.ironBlock, 1);
        //worldSettings.mainInventory[3] = new ItemStack(ItemType.lapisLazuliBlock, 1);
        //worldSettings.mainInventory[4] = new ItemStack(ItemType.redstoneBlock, 1);
        //worldSettings.mainInventory[5] = new ItemStack(ItemType.copperBlock, 1);
        //worldSettings.mainInventory[6] = new ItemStack(ItemType.cutCopperBlock, 1);
        //worldSettings.mainInventory[7] = new ItemStack(ItemType.oxidezedCopperBlock, 1);
        //worldSettings.mainInventory[8] = new ItemStack(ItemType.oxizedCutCopperBlock, 1);
        //worldSettings.mainInventory[9] = new ItemStack(ItemType.emeraldBlock, 1);
        //worldSettings.mainInventory[10] = new ItemStack(ItemType.sandBlock, 64);
        //worldSettings.mainInventory[11] = new ItemStack(ItemType.redSandBlock, 64);
        //worldSettings.mainInventory[12] = new ItemStack(ItemType.oakLogBlock, 64);
        //worldSettings.mainInventory[13] = new ItemStack(ItemType.stoneBlock, 64);
        //worldSettings.mainInventory[14] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[15] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[16] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[17] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[18] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[19] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[20] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[21] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[22] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[23] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[24] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[25] = new ItemStack(ItemType.diamondBlock, 1);
        //worldSettings.mainInventory[26] = new ItemStack(ItemType.diamondBlock, 1);
    }
}

[Serializable]
public class WorldSettings
{
    public int seed;
    public Vector3 playerPosition;
    public List<Vector2Int> savedChunks = new List<Vector2Int>();
    public ItemStack[] handInventory = new ItemStack[9];
    public ItemStack[] mainInventory = new ItemStack[27];
}

[Serializable]
public class GameSettings
{
    public int renderDistance = 3;

    [Range(20, 600)]
    public float sens  = 80;

    [Range(0f, 1f)]
    public float soundValue = 0.6f;
}

[Serializable]
public class SaveChunk
{
    public List< SaveBlock> blocks = new List<SaveBlock>();

    public bool ContainsBlock(int coordinate, out int index)
    {
        for (int i =0;i< blocks.Count;i++)
        {
            if (blocks[i].coordinate == coordinate)
            {
                index = i;
                return true;
            }
        }

        index = -1;
        return false;
    }
}

[Serializable]
public struct SaveBlock
{
    public int coordinate;
    public BlockType blockType;

    public SaveBlock(BlockType blockType, int coordinate)
    {
        this.blockType = blockType;
        this.coordinate = coordinate;
    }
}
