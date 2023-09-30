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
    [SerializeField] List<ItemSlot> toolbarSlots;

    private void Update()
    {
        if (isGameUI)
        {
            if (!settingsIsOpen && !windowIsOpen)
            {
                TouchFieldInput(); 
                StickInput();
                TouckInput();
            }
        }
    }

    public override void StartGame()
    {
        base.StartGame();
        jumpBtn.onClick.AddListener(Jump);
        inventoryBtn.onClick.AddListener(OpenInventory);
        craftBtn.onClick.AddListener(OpenCraft);
        rewardBtn.onClick.AddListener(GetReward);
        settingsBtn.onClick.AddListener(OpenSettings);
        saveBtn.onClick.AddListener(SaveWorld);
    }

    public void ChangeSlot(int index)
    {
        slotIndex = index;
        ChangeSlot();
    }

    private void TouchFieldInput()
    {
        ViewDirectionInput(touchField.Direction());
    }

    private void StickInput()
    {
        MoveInput(stickController.inputDirection) ;
    }

    private void TouckInput()
    {
        if (touchField.touchDown)
        {
            Press();
            touchField.touchDown = false;
        }
        if (touchField.touchUp)
        {
            Unpress();
            touchField.touchUp = false;
        }
        if (touchField.holdDown)
        {
            HoldDown();
        }
    }
}
