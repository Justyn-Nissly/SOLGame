using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGate : DoorLogic
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set up the door sprites </summary>

	/// <summary> Open an unlocked door </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Unlocked doors open automatically when the player approaches
		if (doorIsLocked == false && collision.gameObject.CompareTag("Enemy"))
		{
			OpenDoor();
			doorIsLocked = false;
		}
		else if(doorHasKey && playerHasDoorKey && collision.gameObject.CompareTag("Enemy")) // check if this door has a key and if the player has that key to unlock this door
		{
			// unlock the door and update the doors sprite
			doorIsLocked = false;
			UpdateSprite();

			// open the door after a delay
			Invoke("OpenDoor", .5f);
		}
	}

	/// <summary> Close a door </summary>
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			CloseDoor();
			doorIsLocked = true;
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}