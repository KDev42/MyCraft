using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PCInput : InputController
{
    private void Update()
    {
        if (isGameUI && !isBlock)
        {
            if(!settingsIsOpen && !windowIsOpen)
            {
                MouseInput();
            }

            KeyboardInput();
        }
    }

    public override void StartGame()
    {
        base.StartGame();
        LockCursor();
    }

    protected override void OpenInventory()
    {
        if (windowIsOpen && openWinow != Window.inventory)
        {
            CloseWindow();
        }

        if (windowIsOpen && openWinow == Window.inventory)
        {
            CloseWindow();
            CloseAllWindow();
        }
        else
        {
            base.OpenInventory();
            UnlockCursor();
        }
    }

    protected override void OpenCraft()
    {
        if (windowIsOpen && openWinow != Window.craft)
        {
            CloseWindow();
        }

        if (windowIsOpen && openWinow == Window.craft)
        {
            CloseWindow();
            CloseAllWindow();
        }
        else
        {
            base.OpenCraft();
            UnlockCursor();
        }
    }

    protected override void OpenSettings()
    {
        base.OpenSettings();
        UnlockCursor();
    }

    protected override void CloseAllWindow()
    {
        base.CloseAllWindow();
        LockCursor();
    }

    protected override void CloseSettings()
    {
        base.CloseSettings();
        LockCursor();
    }

    protected override void CloseWindow()
    {
        base.CloseWindow();
        LockCursor();
    }

    protected override void ApplySettings()
    {
        base.ApplySettings();
        LockCursor();
    }


    private void LockCursor()
    {
        //Debug.Log("lock");
        if (isGameUI)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void UnlockCursor()
    {
        //Debug.Log("unlock");
        if (isGameUI)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void MouseInput()
    {
        MoveInput(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        ViewDirectionInput(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));

        if (SlotIsChanged())
        {
            ChangeSlot();
        }

        LMBInput();
    }

    private void LMBInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Press();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Unpress();
        }
        if (Input.GetMouseButton(0))
        {
            HoldDown();
        }
    }

    private void KeyboardInput()
    {
        //if (!settingsIsOpen)
        //{
        //    if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        OpenInventory();
        //    }
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        OpenCraft();
        //    }
        //}

        if (!windowIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                OpenSettings();
            }
        }

        if (!settingsIsOpen && !windowIsOpen)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                SaveWorld();
            }
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    GetReward();
            //}
        }
    }

    private bool SlotIsChanged()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            if (scroll > 0)
                slotIndex--;
            else
                slotIndex++;

            if (slotIndex > WorldConstants.handSlotsAmount- 1)
                slotIndex = 0;
            if (slotIndex < 0)
                slotIndex = WorldConstants.handSlotsAmount - 1;
        }

        return scroll != 0;
    }
}
