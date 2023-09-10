using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : ScriptableObject
{
    public Sprite icon;
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
