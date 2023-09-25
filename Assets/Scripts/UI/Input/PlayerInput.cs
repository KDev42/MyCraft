using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public abstract bool GetReward();

    public abstract bool OpenSettings();

    public abstract void CloseSettings(Action callback);

    protected abstract void Press();

    protected abstract void Unpress();
}
