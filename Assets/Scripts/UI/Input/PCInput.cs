using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PCInput : InputController
{
    private void Update()
    {
        MoveInput(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        ViewDirectionInput(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenInventory();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OpenCraft();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SaveWorld();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            GetReward();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            OpenSettings();
        }

        if (SlotIsChanged())
        {
            ChangeSlot();
        }
        CheckInputLKM();
    }


    private void CheckInputLKM()
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
