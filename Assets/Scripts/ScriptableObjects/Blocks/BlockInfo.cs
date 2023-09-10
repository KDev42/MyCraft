using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Block Info")]
public class BlockInfo : ScriptableObject
{
    public BlockType blockType;
    public bool isSolid;
    public bool isTransparent;
    public bool isReplaceable;
    public bool isDestroyed;
    public float miningTime;
    public ToolType acceleratingTool;
    public List<DropDownResources> dropDownResources;

    public Vector2 textureIndex;

    protected int textureSize = 16;

    public virtual Vector2 GetPixelsOffset(Vector3Int normal)
    {
        return textureIndex * textureSize;
    }
}

public enum BlockType : byte
{
    air =0,
    bedrock = 1,
    stone = 2,
    grass =3,
    dirt =4,
    oakLog =5,
    sprucePlanks =6,
    strippedOakLog =7,
    strippedSpruceLog=8,
    cobblestone =9,
    brick =10,
    stoneBrick=11,

    // Minerals
    goldOre =100,
    coalOre =101,
    copperOre =102,
    diamondOre =103,
    emeraldOre =104,
    ironOre =105,
    lapisLazuliOre =106,
    redstoneOre=107,

    none = 255,
}
