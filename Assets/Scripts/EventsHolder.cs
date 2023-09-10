using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventsHolder
{
    public static Action<BlockType, Vector3Int> brokenBlock;
    public static Action<Vector2> moveInput;
    public static Action<Vector2> attackDirectionInput;
    public static Action attack;
    public static Action startMine;
    public static Action stopMine;
    public static Action jump;
    public static Action<int> changeActiveSlot;
    public static Action<int> changeToolbar;

    public static void BrokenBlock(BlockType blockType, Vector3Int coordinate)
    {
        brokenBlock?.Invoke(blockType, coordinate);
    }

    public static void MoveInput(Vector2 direction)
    {
        moveInput?.Invoke(direction);
    }

    public static void AttackDirectionInput(Vector2 direction)
    {
        attackDirectionInput?.Invoke(direction);
    }

    public static void Attack()
    {
        attack?.Invoke();
    }

    public static void StartMine()
    {
        Debug.Log("Start Mine");
        startMine?.Invoke();
    }

    public static void StopMine()
    {
        Debug.Log("Stop Mine");
        stopMine?.Invoke();
    }

    public static void Jump()
    {
        jump?.Invoke();
    }

    public static void ChangeActiveSlot(int slotIndex)
    {
        changeActiveSlot?.Invoke(slotIndex);
    }

    public static void ChangeToolbar(int index)
    {
        changeToolbar?.Invoke(index);
    }
}
