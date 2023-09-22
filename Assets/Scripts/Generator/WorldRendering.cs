using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorldRendering : MonoBehaviour
{
    private int renderDistance;
    private bool canUpdate;
    private Vector2Int currentPlayerChunk;
    private Transform player;
    private GameData gameData;
    private GameWorld gameWorld;
    private WorldGenerator worldGenerator;

    [Inject]
    private void Construct(GameWorld gameWorld, GameData gameData)
    {
        this.gameData = gameData;
        this.gameWorld = gameWorld;
    }

    private void FixedUpdate()
    {
        if (canUpdate)
        {
            Vector2Int playerChunk = gameWorld.GetChunckCoordinate(player.position);
            if (currentPlayerChunk != playerChunk)
            {
                List<Vector2Int> activatedList = GetChunkList(playerChunk, playerChunk - currentPlayerChunk, gameWorld.activeChunkDatas,0);

                //Vector2Int deactivateDirection = CheckWorldBorder(playerChunk, playerChunk - currentPlayerChunk);
                //deactivateDirection = MyMath.MultiplyVectors2Int(deactivateDirection, currentPlayerChunk - playerChunk);

                List<Vector2Int> deactivateList = GetChunkList(playerChunk, currentPlayerChunk - playerChunk, gameWorld.disableChunkDatas,1);

                foreach (Vector2Int posChunk in deactivateList)
                {
                    DeactivateChunk(posChunk);
                }

                //foreach (Vector2Int posChunk in activatedList)
                //{
                //    ActivateChunk(posChunk);
                //}
                worldGenerator.DeleteQueue(deactivateList);
                worldGenerator.AddQueue(activatedList);

                currentPlayerChunk = playerChunk;
            }
        }
    }

    public void ActivateRender(WorldGenerator worldGenerator, Transform player)
    {
        renderDistance = gameData.gameSettings.renderDistance;
        this.worldGenerator = worldGenerator;
        this.player = player;
        currentPlayerChunk = gameWorld.GetChunckCoordinate(player.position);
        canUpdate = true;
    }

    private void DeactivateChunk(Vector2Int chunkPosition)
    {
        if (gameWorld.activeChunkDatas.ContainsKey(chunkPosition))
        {
            ChunkData deactivatedChunk = gameWorld.activeChunkDatas[chunkPosition];
            deactivatedChunk.chunkRenderer.gameObject.SetActive(false);

            gameWorld.disableChunkDatas.Add(chunkPosition, deactivatedChunk);

            gameWorld.activeChunkDatas.Remove(chunkPosition);
        }
    }

    private List<Vector2Int> GetChunkList(Vector2Int chunkPosition, Vector2Int direction, Dictionary<Vector2Int, ChunkData> checkDictionary, int extraLength )
    {
        List<Vector2Int> chunkList = new List<Vector2Int>();

        chunkPosition = BorderPosition(chunkPosition);

        int distance = renderDistance + extraLength;

        if (direction.y != 0)
        {
            for (int x = chunkPosition.x - renderDistance; x <= chunkPosition.x + renderDistance; x++)
            {
                Vector2Int checkCnunk = new Vector2Int(x, chunkPosition.y+ distance * direction.y );
                if (gameWorld.IsChunkInWorld(checkCnunk) && !checkDictionary.ContainsKey(checkCnunk))
                {
                    chunkList.Add(checkCnunk);
                }
            }
        }

        if (direction.x != 0)
        {
            for (int z = chunkPosition.y - renderDistance; z <= chunkPosition.y + renderDistance; z++)
            {
                Vector2Int checkCnunk = new Vector2Int(chunkPosition.x + distance * direction.x, z);
                if (gameWorld.IsChunkInWorld(checkCnunk) && !checkDictionary.ContainsKey(checkCnunk))
                {
                    chunkList.Add(checkCnunk);
                }
            }
        }

        if(direction.x!=0 && direction.y != 0)
        {
            Vector2Int checkCnunk = new Vector2Int(chunkPosition.x + distance * direction.x+1, chunkPosition.y + distance * direction.y+1);
            if (gameWorld.IsChunkInWorld(checkCnunk) && !checkDictionary.ContainsKey(checkCnunk))
            {
                chunkList.Add(checkCnunk);
            }
        }

        return chunkList;
    }

    private Vector2Int BorderPosition(Vector2Int chunkPosition)
    {
        Vector2Int border = chunkPosition;

        int borderRight = chunkPosition.x + renderDistance;
        int borderLeft = chunkPosition.x - renderDistance;

        int borderUp= chunkPosition.y  +renderDistance;
        int borderDown = chunkPosition.y -renderDistance;

        if ( borderRight >= WorldConstants.maxSizeWorld)
        {
            border.x = WorldConstants.maxSizeWorld - renderDistance;
        }
        if(borderLeft< 0 )
        {
            border.x =  renderDistance;
        }
        if (borderDown<0)
        {
            border.y = renderDistance;
        }
        if (borderUp >= WorldConstants.maxSizeWorld)
        {
            border.y = WorldConstants.maxSizeWorld - renderDistance;
        }

        return border;
    }

    //private Vector2Int CheckWorldBorder(Vector2Int chunkPosition, Vector2Int direction)
    //{
    //    Vector2Int border = new Vector2Int(1,1);

    //    int xBorder = chunkPosition.x + direction.x * GameSettings.renderDistance;
    //    if (direction.x!=0 &&( xBorder < 0 || xBorder>= WorldConstants.maxSizeWorld))
    //    {
    //        border.x = 0;
    //    }

    //    int zBorder = chunkPosition.y + direction.y * GameSettings.renderDistance;
    //    if (direction.y != 0 && (zBorder < 0 || zBorder >= WorldConstants.maxSizeWorld))
    //    {
    //        border.y = 0;
    //    }

    //    return border;
    //}
}
