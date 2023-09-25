using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public class ResourceDrop : MonoBehaviour
{
    [SerializeField] BlockInfoDataBase blockDataBase;

    private int dropMultiplier =1;
    private ItemFactory itemFactory;

    [Inject]
    private void Construct(ItemFactory itemFactory)
    {
        this.itemFactory = itemFactory;
    }

    private void OnEnable()
    {
        EventsHolder.brokenBlock += DropFromBlock;
        EventsHolder.startDoubleDrop += DoubleDropMultiplier;
        EventsHolder.stopDoubleDrop += RegularDropMultiplier;
    }

    private void OnDisable()
    {
        EventsHolder.brokenBlock -= DropFromBlock;
        EventsHolder.startDoubleDrop -= DoubleDropMultiplier;
        EventsHolder.stopDoubleDrop -= RegularDropMultiplier;
    }

    private void DoubleDropMultiplier()
    {
        dropMultiplier = 2;
    }

    private void RegularDropMultiplier()
    {
        dropMultiplier = 1;
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
            float randomPercent = UnityEngine.Random.Range(0f, 100f);
            int numberStacks = NumberDrop( resource.maxNumber + 1, randomPercent);

            for(int i = 0; i < numberStacks; i++)
            {
                Vector3 spawnPosition = dropCoordinate + new Vector3(0.5f, 0.5f, 0.5f);
                itemFactory.SpawnItem(resource.itemType, spawnPosition, numberStacks, ItemStates.inGround);
            }
        }
    }

    private int NumberDrop( int maxLevel, float value)
    {
        float startValue = 75f;
        float maxValue =98f;

        if (value <= startValue)
            return 1 * dropMultiplier;

        double q = Math.Pow((maxValue / startValue), 1.0d / (maxLevel - 1.0d));

        return ((int)(Math.Log(value / startValue, q) )+1) * dropMultiplier;
    }
}
