using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
	[SerializeField] StickController stickController;
	[SerializeField] PCInput pcMoveInput;
	[SerializeField] protected float minPressingTime = 0.2f;
	[SerializeField] Button closeSettingsButton;

	protected float timePress;
	protected int slotIndex;

	private bool isMining;
	private PlayerInput playerInput;
	private bool inUI = true;
	private bool inventoryIsOpen;
	private bool craftIsOpen;

	protected enum Window
    {
		none,
		inventory,
		craft,
    }

	protected bool windowIsOpen;
	protected Window openWinow;

	private void Awake()
	{
		//#if ANDROID
		//		stickController.gameObject.SetActive(true);
		//		motionInput = stickController;
		//#else
		//		stickController.gameObject.SetActive(false);
		//		playerInput = pcMoveInput;
		//#endif
		closeSettingsButton.onClick.AddListener(CloseSettings);
	}

	public void StartGame()
	{
		LockCursor();
	}

	protected void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	protected void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	protected void Press()
	{
		timePress = Time.time;
	}
	protected void HoldDown()
	{
		if (!isMining && Time.time - timePress >= minPressingTime)
		{
			isMining = true;
			StartMine();
		}
	}
	protected void Unpress()
	{
		isMining = false;
		if (Time.time - timePress < minPressingTime)
		{
			Interection();
		}
		else
		{
			StopMine();
		}
	}

	protected void OpenInventory()
	{
		UnlockCursor();
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
			EventsHolder.OpenInventory();
			windowIsOpen = true;
			openWinow = Window.inventory;
		}
	}

	protected void OpenCraft()
	{
		UnlockCursor();
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
			EventsHolder.OpenCraft();
			windowIsOpen = true;
			openWinow = Window.craft;
		}
	}

	protected void CloseWindow()
    {
        switch (openWinow)
        {
			case Window.inventory:
				EventsHolder.CloseInventory();
				break;

			case Window.craft:
				EventsHolder.CloseCraft();
				break;
		}
	}

	private void CloseAllWindow()
	{
		LockCursor();
		windowIsOpen = false;
		openWinow = Window.none;
	}

	protected void MoveInput(Vector2 direction)
	{
		EventsHolder.MoveInput(direction);
	}

	protected void ViewDirectionInput(Vector2 direction)
	{
		EventsHolder.AttackDirectionInput(direction);
	}

	protected void Interection()
	{
		playerInput.inputLKM = PlayerInput.InputLKM.waitDown;
		EventsHolder.Attack();
	}

	protected void StartMine()
	{
		EventsHolder.StartMine();
	}

	protected void StopMine()
	{
		EventsHolder.StopMine();
	}

	protected void ChangeSlot()
    {
		EventsHolder.ChangeActiveSlot(slotIndex);
	}

	protected void Jump()
	{
		EventsHolder.Jump();
	}

	protected void SaveWorld()
	{
		EventsHolder.SaveWorld();
	}

	protected void OpenSettings()
	{
		UnlockCursor();
		EventsHolder.OpenSettings();
	}

	protected void CloseSettings()
	{
		LockCursor();
	}

	protected void GetReward()
	{
		EventsHolder.GetReward();
	}

	//private void Update()
	//{
 //       if (playerInput.OpenInventory())
	//	{
	//		OpenInventory();
	//	}

	//	if (playerInput.OpenCraft())
	//	{
	//		OpenCraft();
	//	}

	//	if (!inUI)
	//	{
	//		Cursor.lockState = CursorLockMode.Locked;
	//		Cursor.visible = false;

	//		EventsHolder.MoveInput(playerInput.MotionDirection());
	//		EventsHolder.AttackDirectionInput(playerInput.LookDirection());

	//		playerInput.CheckInputLKM();

	//		if (playerInput.SlotIsChanged(ref slotIndex))
	//		{
	//			EventsHolder.ChangeActiveSlot(slotIndex);
	//		}

	//		if (playerInput.inputLKM == PlayerInput.InputLKM.click)
	//		{
	//			playerInput.inputLKM = PlayerInput.InputLKM.waitDown;
	//			EventsHolder.Attack();
	//		}
	//		else if (playerInput.inputLKM == PlayerInput.InputLKM.press)
	//		{
	//			playerInput.inputLKM = PlayerInput.InputLKM.waitUp;
	//			EventsHolder.StartMine();
	//		}
	//		else if (playerInput.inputLKM == PlayerInput.InputLKM.unpress)
	//		{
	//			playerInput.inputLKM = PlayerInput.InputLKM.waitDown;
	//			EventsHolder.StopMine();
	//		}

	//		if (playerInput.Jump())
	//			EventsHolder.Jump();

	//		if (playerInput.SaveWord())
	//			EventsHolder.SaveWorld();

	//		if (playerInput.OpenSettings())
	//		{
	//			inUI = false;
	//			EventsHolder.OpenSettings();
	//		}

	//		if (playerInput.GetReward())
	//			EventsHolder.GetReward();
	//	}
 //       else
	//	{
	//		Cursor.lockState = CursorLockMode.None;
	//		Cursor.visible = true;
	//	}
	//}

}
