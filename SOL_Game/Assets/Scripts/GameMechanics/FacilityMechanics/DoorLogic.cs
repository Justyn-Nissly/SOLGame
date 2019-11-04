using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public SpriteRenderer doorOpen;
	public SpriteRenderer doorClosed;

	public BoxCollider2D doorBoxCollider; // the box collider so the player cant walk through the door
	#endregion

	#region Private Variables
	private bool doorIsOpen = false;
	private bool canOpenDoor = false;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (canOpenDoor && Input.GetKeyUp(KeyCode.E))
		{
			ToggleDoor();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("You can open/close the door now by clicking E");
		canOpenDoor = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Debug.Log("You can't open/close the door now");
		canOpenDoor = false;
	}
	#endregion

	#region Utility Methods
	/// <summary>
	/// Toggles the door to be open or closed
	/// </summary>
	private void ToggleDoor()
	{
		if (doorIsOpen)
		{
			// close the door
			doorClosed.enabled = true; // enable the door closed sprite
			doorOpen.enabled = false; // disable the door open sprite
			doorBoxCollider.enabled = true; // enable the doors box collider so the player cant walk through the door
			doorIsOpen = false; // set bool flag
		}
		else
		{
			// open the door
			doorClosed.enabled = false;  // disable the door closed sprite
			doorOpen.enabled = true; // enable the door open sprite
			doorBoxCollider.enabled = false;  // disable the doors box collider so the player can walk through the door
			doorIsOpen = true; // set bool flag
		}
	}
	#endregion

	#region Coroutines
	#endregion
}
