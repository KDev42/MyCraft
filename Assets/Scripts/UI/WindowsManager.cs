using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    [SerializeField] PCGUI pcGui;
    [SerializeField] MobileGUI mobileGui;

    private CommonGUI commonGUI;

    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            commonGUI = mobileGui;
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            commonGUI = pcGui;
        }

#if UNITY_EDITOR
        //commonGUI = mobileGui;
#endif

        EventsHolder.openInventory += OpenInventory;
        EventsHolder.closeInventory += CloseInventory;

        EventsHolder.openCraft += OpenCraft;
        EventsHolder.closeCraft += CloseCraft;

        EventsHolder.openSettings += OpenSettings;
        EventsHolder.closeSettings += CloseSettings;
    }

    private void OnDestroy()
    {
        EventsHolder.openInventory -= OpenInventory;
        EventsHolder.closeInventory -= CloseInventory;

        EventsHolder.openCraft -= OpenCraft;
        EventsHolder.closeCraft -= CloseCraft;

        EventsHolder.openSettings -= OpenSettings;
        EventsHolder.closeSettings -= CloseSettings;
    }

    private void OpenInventory()
    {
        commonGUI.OpenInventory();
    }

    private void CloseInventory()
    {
        commonGUI.CloseInventory();
    }

    private void OpenCraft()
    {
        commonGUI.OpenCraft();
    }

    private void CloseCraft()
    {
        commonGUI.CloseCraft();
    }


    private void OpenSettings()
    {
        commonGUI.OpenSettings();
    }

    private void CloseSettings(bool isGameUI)
    {
        if (isGameUI)
        {
            commonGUI.CloseSettings();
        }
    }
}
