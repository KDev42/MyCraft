using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text amountTxt;

    public void SetIngredient(Sprite iconImg, string amont)
    {
        icon.sprite = iconImg;
        amountTxt.text = amont;
    }

    public void UpdateAmount(string amont)
    {
        amountTxt.text = amont;
    }
}
