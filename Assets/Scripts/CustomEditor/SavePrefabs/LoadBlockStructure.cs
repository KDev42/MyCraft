using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoadBlockStructure 
{
    private static Dictionary<BlockStructureType, SavedStructure> blockStructures = new Dictionary<BlockStructureType, SavedStructure>();

    public static SavedStructure GetStructure(BlockStructureType blockStructureType)
    {
        if (blockStructures.ContainsKey(blockStructureType))
        {
            return blockStructures[blockStructureType];
        }
        else
        {
            string folder = "BlockStructure/";
            string path = folder + blockStructureType.ToString();
            string blockStructureStr = Resources.Load<TextAsset>(path).ToString();

            SavedStructure blockStructure = JsonUtility.FromJson<SavedStructure>(blockStructureStr);

            blockStructures.Add(blockStructureType, blockStructure);

            return blockStructure;
        }
    }
}
