using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInput : PlayerInput
{

    public override bool Jump()
    {
        return Input.GetButtonDown("Jump");
    }

    public override Vector2 LookDirection()
    {
        return new Vector2( Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")) ;
    }

    public override Vector2 MotionDirection()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public override void CheckInputLKM()
    {
        if(inputLKM == InputLKM.waitDown)
        {
            Press();
        }
        else if(inputLKM == InputLKM.waitUp)
        {
            Unpress();
        }
    }

    public override bool SlotIsChanged(ref int slotIndex)
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

    protected override void Press()
    {
        if (Input.GetMouseButtonDown(0) && waitClickCoroutine==null)
        {
            waitClickCoroutine = StartCoroutine(DelayClick());
        }
    }

    protected override void Unpress()
    {
        if(Input.GetMouseButtonUp(0))
        {
            inputLKM = InputLKM.unpress;
        }
    }

    IEnumerator DelayClick()
    {
        yield return new WaitForSeconds(0.2f);
        if (Input.GetMouseButton(0))
        {
            inputLKM = InputLKM.press;
        }
        else
        {
            inputLKM = InputLKM.click;
        }
        waitClickCoroutine = null;
    }

    public override bool OpenInventory()
    {
        return Input.GetKeyDown(KeyCode.I);
    }

    public override bool OpenCraft()
    {
        return Input.GetKeyDown(KeyCode.C);
    }

    public override bool SaveWord()
    {
        return Input.GetKeyDown(KeyCode.H);
    }
}
