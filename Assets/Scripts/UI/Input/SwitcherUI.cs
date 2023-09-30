using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitcherUI : MonoBehaviour
{
    [SerializeField] GameObject gameScreen;
    [SerializeField] GameObject tipsPanel;
    [SerializeField] GameObject leftButtonsPanel;
    [SerializeField] GameObject rightButtonsPanel;
    [SerializeField] PCInput PCInput;
    [SerializeField] MobileInput mobileInput;

    private InputController inputController;

    public void InitializeGUI()
    {
        if (Application.isMobilePlatform)
        {
            ActivateMobileInput();
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            ActivatePCInput();
        }
#if UNITY_EDITOR
        //ActivateMobileInput();
        //ActivatePCInput();
#endif

    }

    private void ActivateMobileInput()
    {
        inputController = mobileInput;
        PCInput.enabled = false;
        CommonInput();
        leftButtonsPanel.SetActive(true);
        rightButtonsPanel.SetActive(true);
    }

    private void ActivatePCInput()
    {
        inputController = PCInput;
        mobileInput.enabled = false;
        CommonInput();
        tipsPanel.SetActive(true);
    }

    private void CommonInput()
    {
        inputController.StartGame();
        gameScreen.SetActive(true);
    }
}
