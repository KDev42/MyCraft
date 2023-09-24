using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveLoad : MonoBehaviour
{
    private GameSettings gameSettings;
    private Dictionary<Vector2Int, SaveChunk> chunkForSave = new Dictionary<Vector2Int, SaveChunk>();

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

    private void SaveWorld()
    {
        Debug.Log("dgdgfndfb ");
        foreach (var chunk in chunkForSave)
        {
            SaveChunk(chunk.Key);
        }

        chunkForSave.Clear();
    }

    private string GetChunkPath(int indexWorld, Vector2Int chunkCoordinate)
    {
        return "Chunk_" + indexWorld + "_" + chunkCoordinate;
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
            savedChunk = new SaveChunk();
            savedChunk = addedChunck;
        }

        string path = GetChunkPath(1, chunkCoordinate);


        string js = JsonUtility.ToJson(savedChunk);
        PlayerPrefs.SetString(path, js);
        Debug.Log("dgdgfndfb " + path + "  "  + js);
    }

}


[Serializable]
public class GameSettings
{
    public int renderDistance = 3;

    [Range(200, 2000)]
    public float sens  = 600;

    [Range(0f, 1f)]
    public float soundValue = 0.7f;
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
