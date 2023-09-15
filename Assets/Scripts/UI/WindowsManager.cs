using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryWindow;
    [SerializeField] GameObject craftWindow;
    [SerializeField] GameObject toolbar;

    private void Start()
    {
        EventsHolder.openInventory += OpenInventory;
        EventsHolder.closeInventory += CloseInventory;

        EventsHolder.openCraft += OpenCraft;
        EventsHolder.closeCraft += CloseCraft;
    }

    private void OnDestroy()
    {
        EventsHolder.openInventory -= OpenInventory;
        EventsHolder.closeInventory -= CloseInventory;

        EventsHolder.openCraft -= OpenCraft;
        EventsHolder.closeCraft -= CloseCraft;
    }

    private void OpenInventory()
    {
        inventoryWindow.SetActive(true);
        toolbar.SetActive(false);
    }

    private void CloseInventory()
    {
        inventoryWindow.SetActive(false);
        toolbar.SetActive(true);
    }

    private void OpenCraft()
    {
        craftWindow.SetActive(true);
        toolbar.SetActive(false);
    }

    private void CloseCraft()
    {
        craftWindow.SetActive(false);
        toolbar.SetActive(true);
    }
}
