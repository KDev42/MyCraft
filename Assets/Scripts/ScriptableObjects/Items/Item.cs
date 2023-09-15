using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item : ScriptableObject
{
    public Sprite icon;
    public List<Ingredient> ingredientsCraft;
    public ItemForme itemForme;
    public ItemType itemType;
    public int maxNumberStacks = 1;
    public ItemObject prefab;
}

public enum ItemForme
{
    block =1,
    tool =2,
    ore=3,
}

public enum ItemType
{
    stoneBlock =1,
    grassBlock =2,

    // -- Wood --
    // -- Log -- 
    oakLogBlock = 100,
    birchLogBlock = 101,
    spruceLogBlock = 102,
    // -- Planks --
    sprucePlanksBlock = 120,
    oakPlanksBlock = 121,
    birchPlanksBlock = 122,
    junglePlanksBlock = 123,
    acaciaPlanksBlock = 124,
    darkOakPlanksBlock = 125,
    mangrovePlanksBlock = 126,
    bambooPlanksBlock = 127,
    bambooMosaicBlock = 128,
    crimsonPlanksBlock= 129,
    warpedPlanksBlock = 130,
    // -- Stripped Log --
    strippedOakLogBlock = 150,
    strippedSpruceLogBlock = 151,
    strippedBirckLogBlock = 152,
    strippedJungleLogBlock = 153,
    strippedAcaciaLogBlock = 154,
    strippedDarkOakLogBlock = 155,
    strippedMangroveLogBlock = 156,
    strippedBambooBlock = 157,
    strippedCrimsonStemBlock = 158,
    strippedWarpedStemBlock = 159,

    brickBlock = 401,
    stoneBrickBlock = 402,

    emptyHand =999,
    goldPick =1000,
    stonePick =1001,

    coalOre =1100,
    ironOre = 1101,
    copperOre =1102,
    goaldOre =1103,
    redstoneOre =1104,
    lazuliOre =1105,
    diamondOre = 1106,
    emeraldOre =1107,
}

[Serializable]
public class Ingredient
{
    public ItemType itemType;
    public int amount;
}