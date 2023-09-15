using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CraftItem : MonoBehaviour
{
    [SerializeField] Image targetIcon;
    [SerializeField] IngredientUI[] ingredientsUI;
    [SerializeField] Button button;

    private Database database;
    private Item item;

    [Inject]
    private void Construct(Database database)
    {
        this.database = database;
    }

    private void Awake()
    {
        button.onClick.AddListener(Craft);
    }

    private void OnEnable()
    {
        EventsHolder.updateCraftInfo += UpdateCraftInfo;
    }

    private void OnDisable()
    {
        EventsHolder.updateCraftInfo -= UpdateCraftInfo;
    }

    public void Activation(Item item)
    {
        this.item = item;
        targetIcon.sprite = item.icon;

        for (int i = 0; i < ingredientsUI.Length; i++)
        {
            if (i < item.ingredientsCraft.Count)
            {
                ingredientsUI[i].gameObject.SetActive(true);

                Item ingredient = database.itemDatabase.GetItem(item.ingredientsCraft[i].itemType);

                int ingredientInInventory = AmountIngredient(item.ingredientsCraft[i].itemType);
                
                string amount = ingredientInInventory + "/" + item.ingredientsCraft[i].amount;

                ingredientsUI[i].SetIngredient(ingredient.icon, amount);
            }
            else
            {
                ingredientsUI[i].gameObject.SetActive(false);
            }
        }
    }

    public void Craft()
    {
        if (CanCraft())
        {
            PlayerData.AddToMainInventory(item, out bool canAdd);

            if (canAdd)
            {
                foreach (Ingredient ingredient in item.ingredientsCraft)
                {
                    PlayerData.RemoveItem(ingredient.itemType, ingredient.amount);
                }

                EventsHolder.UpdateCraftInfo();
            }
        }
    }

    private bool CanCraft()
    {
        foreach(Ingredient ingredient in item.ingredientsCraft)
        {
            int ingredientInInventory = AmountIngredient(ingredient.itemType);
            if (ingredient.amount > ingredientInInventory)
                return false;
        }

        return true;
    }

    private int AmountIngredient(ItemType itemType)
    {
        int ingredientInInventory = 0;
        if (PlayerData.MainInventory != null)
            ingredientInInventory += PlayerData.MainInventory.AmountItem(itemType);

        if (PlayerData.HandInventory != null)
            ingredientInInventory += PlayerData.HandInventory.AmountItem(itemType);

        return ingredientInInventory;
    }

    private void UpdateCraftInfo()
    {
        for (int i = 0; i < ingredientsUI.Length; i++)
        {
            if (i < item.ingredientsCraft.Count)
            {
                int ingredientInInventory = AmountIngredient(item.ingredientsCraft[i].itemType);

                string amount = ingredientInInventory + "/" + item.ingredientsCraft[i].amount;

                ingredientsUI[i].UpdateAmount( amount);
            }
        }
    }
}
