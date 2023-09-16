using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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

    [SerializeField] public EventReference mineAudio;

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
    grass =3,
    dirt =4,
    sand =32,
    redSand =33,

    // -- Wood --
    // -- Log -- 
    oakLog = 5,
    birchLog =12,
    spruceLog =13,
    // -- Planks --
    sprucePlanks = 6,
    oakPlanks =14,
    birchPlanks =15,
    junglePlanks =24,
    acaciaPlanks =25,
    darkOakPlanks =26,
    mangrovePlanks =27,
    bambooPlanks =28,
    bambooMosaic =29,
    crimsonPlanks =30,
    warpedPlanks =31,
    // -- Stripped Log --
    strippedOakLog = 7,
    strippedSpruceLog = 8,
    strippedBirckLog=16,
    strippedJungleLog =17,
    strippedAcaciaLog =18,
    strippedDarkOakLog =19,
    strippedMangroveLog =20,
    strippedBamboo =21,
    strippedCrimsonStem =22,
    strippedWarpedStem =23,

    //33 - last index

    // -- Ore --
    goldOre =100,
    coalOre =101,
    copperOre =102,
    diamondOre =103,
    emeraldOre =104,
    ironOre =105,
    lapisLazuliOre =106,
    redstoneOre=107,
    // -- Ore Blocks --
    coal = 108,
    copper = 109,
    oxidezedCopper =110,
    cutCopper =111,
    oxizedCutCopper =112,
    iron =113,
    redstone =114,
    lapisLazuli =115,
    gold =116,
    emerald =117,
    diamond =118,
    quartz = 119,
    chiseledQuartz =120,
    quartzPillar =121,
    quartzBrick =122,
    smoothQuartz = 123,

    // -- Stone -- 
    stone = 200,
    brick = 201,
    stoneBrick = 202,
    smoothStone = 203,
    smoothStoneSlab = 204,
    crackedStoneBrick = 205,
    mossyStoneBrick = 206,
    chiseledStoneBrick = 207,
    cobblestone = 208,
    mossyCobble = 209,
    sandstone = 210,
    cutSandstone = 211,
    chiseledSandstone = 212,
    redSandstone = 213,
    cutRedSandstone = 214,
    chiseledRedSandstone = 215,
    endStoneBrick = 216,

    none = 255,
}
