using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	[SerializeField] StickController stickController;
	[SerializeField] PCInput pcMoveInput;

	private PlayerInput playerInput;
	private int slotIndex;
	private bool inUI;
	private bool inventoryIsOpen;
	private bool craftIsOpen;

	private void Awake()
    {
#if ANDROID
		stickController.gameObject.SetActive(true);
		motionInput = stickController;
#else
		stickController.gameObject.SetActive(false);
		playerInput = pcMoveInput;
#endif

		playerInput.inputLKM = PlayerInput.InputLKM.waitDown;
	}

	private void Update()
	{
        if (playerInput.OpenInventory())
		{
			if (!inventoryIsOpen)
			{
				inventoryIsOpen = true;
				inUI = true;
				CloseCraft();
				EventsHolder.OpenInventory();
			}
			else
			{
				inUI = false;
				CloseInvenoty();
			}
		}

		if (playerInput.OpenCraft())
		{
			if (!craftIsOpen)
			{
				craftIsOpen = true;
				inUI = true;
				CloseInvenoty();
				EventsHolder.OpenCraft();
			}
			else
			{
				inUI = false;
				CloseCraft();
			}
		}

		if (!inUI)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			EventsHolder.MoveInput(playerInput.MotionDirection());
			EventsHolder.AttackDirectionInput(playerInput.LookDirection());

			playerInput.CheckInputLKM();

			if (playerInput.SlotIsChanged(ref slotIndex))
			{
				EventsHolder.ChangeActiveSlot(slotIndex);
			}

			if (playerInput.inputLKM == PlayerInput.InputLKM.click)
			{
				playerInput.inputLKM = PlayerInput.InputLKM.waitDown;
				EventsHolder.Attack();
			}
			else if (playerInput.inputLKM == PlayerInput.InputLKM.press)
			{
				playerInput.inputLKM = PlayerInput.InputLKM.waitUp;
				EventsHolder.StartMine();
			}
			else if (playerInput.inputLKM == PlayerInput.InputLKM.unpress)
			{
				playerInput.inputLKM = PlayerInput.InputLKM.waitDown;
				EventsHolder.StopMine();
			}

			if (playerInput.Jump())
				EventsHolder.Jump();
		}
        else
        {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	private void CloseInvenoty()
	{
		inventoryIsOpen = false;
		EventsHolder.CloseInventory();
	}


	private void CloseCraft()
	{
		craftIsOpen = false;
		EventsHolder.CloseCraft();
	}
}
