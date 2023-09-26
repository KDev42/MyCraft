using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : InputController
{
    [SerializeField] StickController stickController;
    [SerializeField] FixedTouchField touchField;
    [SerializeField] Button jumpBtn;
    [SerializeField] Button inventoryBtn;
    [SerializeField] Button craftBtn;
    [SerializeField] Button rewardBtn;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button saveBtn;

    private void Awake()
    {
        jumpBtn.onClick.AddListener(Jump);
        inventoryBtn.onClick.AddListener(OpenInventory);
        craftBtn.onClick.AddListener(OpenCraft);
        rewardBtn.onClick.AddListener(GetReward);
        settingsBtn.onClick.AddListener(OpenSettings);
        saveBtn.onClick.AddListener(SaveWorld);
    }
}
