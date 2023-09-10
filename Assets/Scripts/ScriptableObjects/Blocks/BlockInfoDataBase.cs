using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Block Info Data Base")]
public class BlockInfoDataBase : ScriptableObject
{
    [SerializeField] List<BlockInfo> blockInfos;

    private Dictionary<BlockType, BlockInfo> blocksCashed = new Dictionary<BlockType, BlockInfo>();
    private void OnEnable()
    {
        blocksCashed.Clear();
        foreach (BlockInfo block in blockInfos)
        {
            blocksCashed.Add(block.blockType, block);
        }
    }

    public BlockInfo GetInfo(BlockType type)
    {
        if (blocksCashed.TryGetValue(type, out BlockInfo blockInfo))
            return blockInfo;

        return null;
    }
}
