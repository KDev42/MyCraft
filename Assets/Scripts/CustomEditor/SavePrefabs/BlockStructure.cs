using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlockStructure : MonoBehaviour
{
    public SavedStructure savedStructure;
}

[Serializable]
public class SavedStructure
{
    public string directory = "/Resources/BlockStructure/";
    //public Dictionary<Vector3Int, BlockType> blocks;
    public ConfigurationData configurationData;
    public List<PositionType> blocks;
}

[Serializable]
public class PositionType
{
    public Vector3Int position;
    public BlockType blockType;
}

public enum BlockStructureType
{
    //Flora
    Tree1 = 1,
    //Builds
    Mine =100,
    House =101,
}
