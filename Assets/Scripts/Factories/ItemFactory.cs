using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] ItemObject defaultItem;
    [SerializeField] ItemDataBase itemDataBase;
    [SerializeField] BlockInfoDataBase blockInfoDataBase;
    [SerializeField] float dropSpeed = 0.5f;

    [Inject] DiContainer diContainer;
    private Dictionary<PoolObject, Pool> itemsPool = new Dictionary<PoolObject, Pool>();

    public ItemObject SpawnItem(ItemType itemType, Vector3 spawnPosition, int numberStacks, ItemStates itemState, Transform parent =null)
    {
        Vector3 spawnRotaion = new Vector3(0,0,0);
        Item item = itemDataBase.GetItem(itemType);
        GameObject itemGO = GetObject(GetItemPrefab(item)) ;
        ItemObject itemObject = itemGO.GetComponent<ItemObject>();
        itemObject.State = itemState;
        itemObject.item = item;

        if (parent != null)
            itemGO.transform.parent = parent;

        if (item.itemForme == ItemForme.block)
        {
            DropCube(itemObject, item, ref spawnPosition, ref spawnRotaion);
        }
        else if(item.itemForme == ItemForme.tool)
        {
            DropTool(itemObject, item, ref spawnRotaion);
        }
        else if(item.itemForme == ItemForme.ore)
        {

        }

        itemGO.transform.position = spawnPosition;
        itemGO.transform.localRotation =Quaternion.Euler(spawnRotaion);

        itemGO.SetActive(true);

        if(itemState == ItemStates.inGround)
        {
            MoveItem(itemGO.transform);
        }

        return itemObject;
    }

    private void MoveItem(Transform item)
    {
        float range = 0.3f;
        float x = UnityEngine.Random.Range(-range, range);
        float z = UnityEngine.Random.Range(-range, range);
        Vector3 targetPosition = item.position +  new Vector3(x,-0.5f,z);
        item.DOMove(targetPosition, dropSpeed).SetEase(Ease.Linear);
    }

    private void DropCube(ItemObject itemObject, Item item, ref Vector3 spawnPosition, ref Vector3 spawnRotaion)
    {
        ItemCube itemCube = itemObject as ItemCube;
        BlockInfo blockInfo = blockInfoDataBase.GetInfo((item as ItemBlock).blockType);
        itemCube.ChangeSkin(blockInfo);
        //float cubeOffset =  WorldConstants.dropedCubeSize / 2;

        spawnRotaion = new Vector3(0, 0, 0);
        //spawnPosition -= new Vector3(cubeOffset, 0, cubeOffset);
    }

    private void DropTool(ItemObject itemObject, Item item, ref Vector3 spawnRotaion)
    {
        ItemShape itemShape = itemObject as ItemShape;
        ItemTool itemTool = item as ItemTool;

        spawnRotaion = new Vector3(135, 0, 0);
        itemShape.ChangeSkin(itemTool);
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
            itemsPool.Add(objectPrefab, new Pool(objectPrefab, 4, container, true, false, diContainer));
        }

        return itemsPool[objectPrefab].GetFreeElement(true, false).gameObject;
    }
}
