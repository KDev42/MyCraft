using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainGenerator
{
    public static BlockType[] GenerateTerrain(int chunkWidth,int chunkHeight, int xOffset, int zOffset, BiomeAttributes[] biomes, ref List<Vector3Int> treePositions)
    {
        var result = new BlockType[MyMath.GetBlockCoordinate(new Vector3Int( chunkWidth, chunkHeight, chunkWidth))];

        for (int x = 0; x < chunkWidth; x++)
        {
            for (int z = 0; z < chunkWidth; z++)
            {
                //float height = Mathf.PerlinNoise((x+xOffset) * 0.2f, (z+zOffset) * 0.2f)  * 5 + 10;
                for (int y = 0; y < chunkHeight; y++)
                {
                    //result[MyMath.GetBlockCoordinate(new Vector3Int(x, y, z))] = BlockType.grass;
                    result[MyMath.GetBlockCoordinate(new Vector3Int(x, y, z))] = GetBlock(new Vector3Int(x + xOffset, y, z +zOffset), biomes, ref treePositions);
                }
            }
        }

        return result;
    }

    private static BlockType GetBlock(Vector3Int pos, BiomeAttributes[] biomes, ref List<Vector3Int> treePositions)
    {
        /* IMMUTABLE PASS */

        // If outside world, return air.
        //if (!IsVoxelInWorld(pos))
        //    return BlockType.air;

        // If bottom block of chunk, return bedrock.
        if (pos.y == 0)
            return BlockType.bedrock;

        /* BIOME SELECTION PASS*/

        float sumOfHeights = 0f;
        int count = 0;
        float strongestWeight = 0f;
        int strongestBiomeIndex = 0;

        for (int i = 0; i < biomes.Length; i++)
        {

            float weight = MyMath.Get2DPerlin(new Vector2(pos.x, pos.z), biomes[i].offset, biomes[i].scale);

            // Keep track of which weight is strongest.
            if (weight > strongestWeight)
            {

                strongestWeight = weight;
                strongestBiomeIndex = i;

            }

            // Get the height of the terrain (for the current biome) and multiply it by its weight.
            float height = biomes[i].terrainHeight * MyMath.Get2DPerlin(new Vector2(pos.x, pos.z), 0, biomes[i].terrainScale) * weight;

            // If the height value is greater 0 add it to the sum of heights.
            if (height > 0)
            {

                sumOfHeights += height;
                count++;

            }

        }

        // Set biome to the one with the strongest weight.
        BiomeAttributes biome = biomes[strongestBiomeIndex];

        // Get the average of the heights.
        sumOfHeights /= count;

        int terrainHeight = Mathf.FloorToInt(sumOfHeights +WorldConstants.solidGroundHeight);


        //BiomeAttributes biome = biomes[index];

        /* BASIC TERRAIN PASS */

        BlockType blockType = 0;

        if (pos.y == terrainHeight)
            blockType = biome.surfaceBlock;
        else if (pos.y < terrainHeight && pos.y > terrainHeight - WorldConstants.subSurfaceBlockHeight)
            blockType = biome.subSurfaceBlock;
        else if (pos.y > terrainHeight)
            return 0;
        else
            blockType = BlockType.stone;

        /* SECOND PASS */

        if (blockType == BlockType.stone)
        {
            foreach (Lode lode in biome.lodes)
            {
                if (pos.y > lode.minHeight && pos.y < lode.maxHeight)
                    if (MyMath.Get3DPerlin(pos, lode.noiseOffset, lode.scale, lode.threshold))
                        blockType = lode.blockType;
            }
        }

        /* TREE PASS */

        if (pos.y == terrainHeight && biome.placeMajorFlora && biome.WithinHeight(pos.y))
        {
            if (MyMath.Get2DPerlin(new Vector2(pos.x, pos.z), 0, biome.majorFloraZoneScale) > biome.majorFloraZoneThreshold)
            {
                if (MyMath.Get2DPerlin(new Vector2(pos.x, pos.z), 0, biome.majorFloraPlacementScale) > biome.majorFloraPlacementThreshold)
                {
                    treePositions.Add(pos);
                    // modifications.Enqueue(Structure.GenerateMajorFlora(biome.majorFloraIndex, pos, biome.minHeight, biome.maxHeight));
                }
            }
        }

        return blockType;
    }
}

