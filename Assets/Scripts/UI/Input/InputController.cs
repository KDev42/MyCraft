using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
	[SerializeField] CloseArea closeArea;
	[SerializeField] protected float minPressingTime = 0.2f;
	[SerializeField] Button closeSettingsButton;
	[SerializeField] Button applySettingsButton;

	protected enum Window
	{
		none,
		inventory,
		craft,
		settings,
	}

	protected bool isBlock;
	protected bool isGameUI;
	protected bool windowIsOpen;
	protected bool settingsIsOpen;
	protected int slotIndex;
	protected float timePress;
	protected Window openWinow;

	private bool isMining;

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
		applySettingsButton.onClick.AddListener(ApplySettings);
	}

	public virtual void StartGame()
	{
		closeArea.close += () => { 
			CloseWindow(); 
			CloseAllWindow(); 
		}; 
		isGameUI = true;
	}

	public void InputReset()
    {
        if (isMining)
        {
			StopMine();
        }

		MoveInput(new Vector2(0,0));
		ViewDirectionInput(new Vector2(0,0));
	}

	public void LockInput()
	{
		InputReset();
		isBlock = true;
	}

	public void UnlockInput()
    {
		isBlock = false;
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

	protected virtual void OpenInventory()
	{
		EventsHolder.OpenInventory();
		windowIsOpen = true;
		openWinow = Window.inventory;
		InputReset();
	}

	protected virtual void OpenCraft()
	{
		EventsHolder.OpenCraft();
		windowIsOpen = true;
		openWinow = Window.craft;
		InputReset();
	}

	protected virtual void CloseWindow()
    {
        switch (openWinow)
        {
			case Window.inventory:
				EventsHolder.CloseInventory();
				break;

			case Window.craft:
				EventsHolder.CloseCraft();
				break;

			case Window.settings:
				CloseSettings();
				break;
		}
	}

	protected virtual void CloseAllWindow()
	{
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
		EventsHolder.Attack();
	}

	protected void StartMine()
	{
		isMining = true;
		EventsHolder.StartMine();
	}

	protected void StopMine()
	{
		isMining = false;
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

	protected virtual void OpenSettings()
	{
		openWinow = Window.settings;
		windowIsOpen = true;
		settingsIsOpen = true;
		EventsHolder.OpenSettings();
		InputReset();
	}

	protected virtual void CloseSettings()
	{
		settingsIsOpen = false;
		openWinow = Window.none;
		windowIsOpen = false;
		EventsHolder.CloseSettings(isGameUI);
	}

	protected virtual void ApplySettings()
	{
		EventsHolder.ApplySettings();
		CloseSettings();
	}

	protected void GetReward()
	{
		EventsHolder.GetReward();
	}
}
