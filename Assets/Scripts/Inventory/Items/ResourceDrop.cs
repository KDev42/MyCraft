using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ResourceDrop : MonoBehaviour
{
    [SerializeField] BlockInfoDataBase blockDataBase;
 
    private ItemFactory itemFactory;

    [Inject]
    private void Construct(ItemFactory itemFactory)
    {
        this.itemFactory = itemFactory;
    }

    private void OnEnable()
    {
        EventsHolder.brokenBlock += DropFromBlock;
    }

    private void OnDisable()
    {
        EventsHolder.brokenBlock -= DropFromBlock;
    }

    private void DropFromBlock(BlockType blockType, Vector3Int dropCoordinate)
    {
        List<DropDownResources> resources = blockDataBase.GetInfo(blockType).dropDownResources;

        foreach(DropDownResources resource in resources)
        {
            DropItem(resource, dropCoordinate);
        }
    }

    private void DropItem(DropDownResources resource, Vector3Int dropCoordinate)
    {
        bool isDrop = UnityEngine.Random.Range(0f, 100f)< resource.percent;
        if (isDrop)
        {
            int numberStacks = UnityEngine.Random.Range(resource.minNumber, resource.maxNumber + 1);

            Vector3 spawnPosition =  dropCoordinate + new Vector3(0.5f, 0.2f, 0.5f);
            itemFactory.SpawnItem(resource.itemType, spawnPosition, numberStacks);
        }
    }
}
