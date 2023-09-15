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
    stone = 2,
    grass =3,
    dirt =4,
    cobblestone =9,
    brick =10,
    stoneBrick=11,

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

    //31 - last index

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
