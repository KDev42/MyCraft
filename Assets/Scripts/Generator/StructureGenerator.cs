using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructureGenerator
{
    public static void AddStructure(Vector3Int spawnCoordinate,ChunkData chunkData,  BlockStructureType blockStructureType, GameWorld gameWorld)
    {
        SavedStructure blockStructure = LoadBlockStructure.GetStructure(blockStructureType);

        Vector3Int blockCoordinate;

        Vector2Int chunkCoordinate = gameWorld.GetChunckCoordinate(spawnCoordinate);
        //Vector3Int localCoordinate = gameWorld.GetBlockLocalCoordinate(chunkCoordinate, spawnCoordinate);

        if(blockStructure.configurationData.type != ConfigurationType.none)
            ClearTerrain(spawnCoordinate, blockStructure.configurationData, chunkData, gameWorld);

        foreach (PositionType pt in blockStructure.blocks)
        {
            blockCoordinate = spawnCoordinate + pt.position;

            if (WithinChunk(blockCoordinate))
            {
                chunkData.bloks[MyMath.GetBlockCoordinate(blockCoordinate)] = pt.blockType;
            }
            else
            {
                ChunkToFill(gameWorld, spawnCoordinate + pt.position, pt.blockType);
            }
        }
    }

    private static void ClearTerrain(Vector3Int originCoordinate, ConfigurationData configurationData, ChunkData chunkData,GameWorld gameWorld)
    {
        Configuration configuration = new Configuration();
        List<Vector3Int> coordinates = configuration.GetCoordinate(originCoordinate, configurationData);

        foreach (Vector3Int coordinate in coordinates)
        {
            if (WithinChunk(coordinate))
            {
                chunkData.bloks[MyMath.GetBlockCoordinate(coordinate)] = BlockType.air;
            }
            else
            {
                ChunkToFill(gameWorld, coordinate, BlockType.air);
            }
        }
    }

    private static bool WithinChunk(Vector3Int localPosition)
    {
        return localPosition.x >= 0 && localPosition.x < WorldConstants.chunkWidth && localPosition.z >= 0 && localPosition.z < WorldConstants.chunkWidth;
    }

    private static void ChunkToFill(GameWorld gameWorld, Vector3Int blockCoordinate, BlockType blockType)
    {
        Vector2Int chunkCoordinate = gameWorld.GetChunckCoordinate(blockCoordinate);
        Vector3Int localCoordinate = gameWorld.GetBlockLocalCoordinate(chunkCoordinate, blockCoordinate);

        PositionType positionType = new PositionType()
        {
            position = localCoordinate,
            blockType = blockType,
        };

        if (gameWorld.needToFillChunk.ContainsKey(chunkCoordinate))
        { 
            gameWorld.needToFillChunk[chunkCoordinate].Add(positionType);
        }
        else
        {
            List<PositionType> coordinatesType = new List<PositionType>();
            coordinatesType.Add(positionType);

            gameWorld.needToFillChunk.Add(chunkCoordinate, coordinatesType);
        }
    }
}
