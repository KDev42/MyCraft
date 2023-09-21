using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabCraftButton : MonoBehaviour
{
    [SerializeField] public ItemContainer craftItems;
    [SerializeField] CraftWindow craftWindow;

    private Button button => GetComponent<Button>();

    private void Awake()
    {
        button.onClick.AddListener(SelectTab);
    }

    public void SelectButton()
    {
        button.interactable = false;
    }

    public void DeselectButton()
    {
        button.interactable = true;
    }

    private void SelectTab()
    {
        craftWindow.ChangeButton(this);
    }
}
