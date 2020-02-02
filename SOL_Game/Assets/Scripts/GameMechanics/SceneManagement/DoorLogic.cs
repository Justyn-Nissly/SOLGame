using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Sprite
		doorOpenSprite,
		doorLockedClosedSprite,
		doorUnlockedClosedSprite;

	public BoxCollider2D
		doorBoxCollider; // Prevent the player from walking through the door

	public bool doorIsLocked = true,
					doorHasKey = false,
					playerHasDoorKey = false;

	#endregion

	#region Private Variables
	private bool
		doorIsOpen = false;
	private SpriteRenderer
		doorSpriteRenderer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set up the door sprites </summary>
	private void Start()
	{
		doorSpriteRenderer        = GetComponent<SpriteRenderer>();
		doorSpriteRenderer.sprite = GetDoorClosedSprite();
	}

	/// <summary> Open an unlocked door </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Unlocked doors open automatically when the player approaches
		if (doorIsLocked == false && collision.gameObject.CompareTag("Player"))
		{
			OpenDoor();
		}
		else if(doorHasKey && playerHasDoorKey && collision.gameObject.CompareTag("Player")) // check if this door has a key and if the player has that key to unlock this door
		{
			// unlock the door and update the doors sprite
			doorIsLocked = false;
			UpdateSprite();

			// open the door after a delay
			Invoke("OpenDoor", .5f);
		}
		else
		{
			Debug.Log("This door is locked solve a puzzle to unlock it");
		}
	}

	/// <summary> Close a door </summary>
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			CloseDoor();
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Get the locked or unlocked door sprite </summary>
	private Sprite GetDoorClosedSprite()
	{
		return (doorIsLocked) ? doorLockedClosedSprite : doorUnlockedClosedSprite;
	}

	/// <summary> Update the closed door's sprite </summary>
	public void UpdateSprite()
	{
		// If the door is closed show that
		if(doorIsOpen == false)
		{
			doorSpriteRenderer.sprite = GetDoorClosedSprite();
		}
	}

	/// <summary> Toggle the door open or closed </summary>
	private void ToggleDoor()
	{
		if (doorIsOpen)
		{
			CloseDoor();
		}
		else
		{
			OpenDoor();
		}
	}

	/// <summary> Close the door </summary>
	public void CloseDoor()
	{
		doorSpriteRenderer.sprite = GetDoorClosedSprite();

		// Prevent the player from walking through the door
		doorBoxCollider.enabled = true;
		doorIsOpen = false;
	}

	/// <summary> Open the door </summary>
	public void OpenDoor()
	{
		doorSpriteRenderer.sprite = doorOpenSprite;

		// Allow the player to walk through the door
		doorBoxCollider.enabled = false;
		doorIsOpen = true;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
