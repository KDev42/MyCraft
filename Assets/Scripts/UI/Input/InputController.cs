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
        if (Input.GetKeyDown(KeyCode.I))
        {
			inUI = !inUI;
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
		//if (Input.mousePresent)
		//{
		//	_moveDelta = Vector2.zero;

		//	if (Input.GetKey(KeyCode.W))
		//		_moveDelta += Vector2.up;

		//	if (Input.GetKey(KeyCode.S))
		//		_moveDelta += Vector2.down;

		//	if (Input.GetKey(KeyCode.A))
		//		_moveDelta += Vector2.left;

		//	if (Input.GetKey(KeyCode.D))
		//		_moveDelta += Vector2.right;

		//	Vector3 mousePos = Input.mousePosition;

		//	RaycastHit hit;
		//	Ray ray = Camera.main.ScreenPointToRay(mousePos);

		//	Vector3 mouseCollisionPoint = Vector3.zero;
		//	// Raycast towards the mouse collider box in the world
		//	if (Physics.Raycast(ray, out hit, Mathf.Infinity, _mouseRayMask))
		//	{
		//		if (hit.collider != null)
		//		{
		//			mouseCollisionPoint = hit.point;
		//		}
		//	}

		//	Vector3 aimDirection = mouseCollisionPoint - _player.turretPosition;
		//	_aimDelta = new Vector2(aimDirection.x, aimDirection.z);
		//}
		//else if (Input.touchSupported)
		//{ 
		//}
	}
}
