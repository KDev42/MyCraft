using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PCInput : InputController
{
    private void Update()
    {
        if (isGameUI)
        {
            if(!settingsIsOpen && !windowIsOpen)
            {
                MouseInput();
            }

            KeyboardInput();
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
        if (!settingsIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenInventory();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                OpenCraft();
            }
        }

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
            if (Input.GetKeyDown(KeyCode.T))
            {
                GetReward();
            }
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
