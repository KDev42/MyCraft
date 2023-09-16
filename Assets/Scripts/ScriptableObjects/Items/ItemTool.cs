using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Tool")]
public class ItemTool : Item
{
    public ToolType toolType;
    public float accelerationMining;
    public Vector2Int baseOffset;
    public Vector2Int offset;
}

[System.Flags]
public enum ToolType
{
    pick = 1 << 1,
    axe = 1 << 2,
    shovel= 1 << 3,
}
