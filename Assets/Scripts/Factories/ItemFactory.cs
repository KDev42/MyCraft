using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] ItemObject defaultItem;
    [SerializeField] ItemDataBase itemDataBase;
    [SerializeField] BlockInfoDataBase blockInfoDataBase;

    private Dictionary<PoolObject, Pool> itemsPool = new Dictionary<PoolObject, Pool>();

    public ItemObject SpawnItem(ItemType itemType, Vector3 spawnPosition, int numberStacks, Transform parent =null)
    {
        Item item = itemDataBase.GetItem(itemType);
        GameObject itemGO = GetObject(GetItemPrefab(item)) ;
        ItemObject itemObject = itemGO.GetComponent<ItemObject>();
        itemObject.item = item;

        if (parent != null)
            itemGO.transform.parent = parent;

        if (item.itemForme == ItemForme.block)
        {
            DropCube(itemObject, item, ref spawnPosition);
        }
        else if(item.itemForme == ItemForme.tool)
        {
            DropTool(itemObject, item, itemGO);
        }
        else if(item.itemForme == ItemForme.ore)
        {

        }

        itemGO.transform.position = spawnPosition;

        itemGO.SetActive(true);

        return itemObject;
    }

    private void DropCube(ItemObject itemObject, Item item, ref Vector3 spawnPosition)
    {
        ItemCube itemCube = itemObject as ItemCube;
        BlockInfo blockInfo = blockInfoDataBase.GetInfo((item as ItemBlock).blockType);
        itemCube.ChangeSkin(blockInfo);
        float cubeOffset =  WorldConstants.dropedCubeSize / 2;
        spawnPosition -= new Vector3(cubeOffset, 0, cubeOffset);
    }

    private void DropTool(ItemObject itemObject, Item item, GameObject itemGO)
    {
        ItemShape itemShape = itemObject as ItemShape;
        ItemTool itemTool = item as ItemTool;

        itemShape.ChangeSkin(itemTool);

        itemGO.transform.localRotation = Quaternion.Euler(-90, 0, 0);
    }

    private PoolObject GetItemPrefab(Item item)
    {
        if (item != null)
        {
            return item.prefab.GetComponent<PoolObject>();
        }
        else
        {
            return defaultItem.GetComponent<PoolObject>();
        }
    }

    private GameObject GetObject(PoolObject objectPrefab)
    {
        if (!itemsPool.ContainsKey(objectPrefab))
        {
            itemsPool.Add(objectPrefab, new Pool(objectPrefab, 4, container, false));
        }

        return itemsPool[objectPrefab].GetFreeElement(false).gameObject;
    }
}
