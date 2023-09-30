using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonGUI : MonoBehaviour
{
    [SerializeField] GameObject closeField;
    [SerializeField] GameObject inventoryWindow;
    [SerializeField] GameObject craftWindow;
    [SerializeField] GameObject toolbar;

    public virtual void OpenInventory()
    {
        inventoryWindow.SetActive(true);
        DeactivateGui();
    }

    public virtual void CloseInventory()
    {
        inventoryWindow.SetActive(false);
        ActivateGui();
    }

    public virtual void OpenCraft()
    {
        craftWindow.SetActive(true);
        DeactivateGui();
    }

    public virtual void CloseCraft()
    {
        craftWindow.SetActive(false);
        ActivateGui();
    }

    public void OpenSettings()
    {
        DeactivateGui();
    }

    public void CloseSettings()
    {
        ActivateGui();
    }

    protected virtual void ActivateGui()
    {
        closeField.SetActive(false);
        toolbar.SetActive(true);
    }

    protected virtual void DeactivateGui()
    {
        closeField.SetActive(true);
        toolbar.SetActive(false);
    }
}
