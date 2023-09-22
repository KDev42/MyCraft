using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInput : MonoBehaviour
{
    public enum InputLKM
    {
        click,
        press,
        unpress,
        waitDown,
        waitUp
    }

    public InputLKM inputLKM;

    public abstract Vector2 MotionDirection();
    public abstract Vector2 LookDirection();

    protected Coroutine waitClickCoroutine;

    private void OnDisable()
    {
        if (waitClickCoroutine != null)
           StopCoroutine(waitClickCoroutine);
    }

    public abstract void CheckInputLKM();

    public abstract bool Jump();

    public abstract bool SlotIsChanged(ref int slotIndex);

    public abstract bool OpenInventory();

    public abstract bool OpenCraft();

    public abstract bool SaveWord();

    protected abstract void Press();

    protected abstract void Unpress();
}
