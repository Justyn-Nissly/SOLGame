using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public Sprite doorOpenSprite;
	public Sprite doorLockedClosedSprite;
	public Sprite doorUnlockedClosedSprite;

	public BoxCollider2D doorBoxCollider; // the box collider so the player cant walk through the door

	public bool doorIsLocked = true,
					doorHasKey = false,
					playerHasDoorKey = false;

	#endregion

	#region Private Variables
	private bool doorIsOpen = false;
	private SpriteRenderer doorSpriteRenderer;
	#endregion

	// Unity Named Methods
	#region Main Methods

	private void Start()
	{
		doorSpriteRenderer = GetComponent<SpriteRenderer>();

		doorSpriteRenderer.sprite = GetDoorClosedSprite();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(doorIsLocked == false && collision.gameObject.CompareTag("Player"))
		{
			OpenDoor();
		}
		else if(doorHasKey && playerHasDoorKey) // check if this door has a key and if the player has that key to unlock this door
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

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			CloseDoor();
		}
	}
	#endregion

	#region Utility Methods

	// get the sprite of the closed door locked or unlocked
	private Sprite GetDoorClosedSprite()
	{
		return (doorIsLocked) ? doorLockedClosedSprite : doorUnlockedClosedSprite;
	}

	public void UpdateSprite()
	{
		if(doorIsOpen == false)
		{
			doorSpriteRenderer.sprite = GetDoorClosedSprite();
		}
	}

	/// <summary>
	/// Toggles the door to be open or closed
	/// </summary>
	private void ToggleDoor()
	{
		if (doorIsOpen)
		{
			// close the door
			CloseDoor();
		}
		else
		{
			// open the door
			OpenDoor();
		}
	}


	public void CloseDoor()
	{
		// close the door
		doorSpriteRenderer.sprite = GetDoorClosedSprite();
		doorBoxCollider.enabled = true; // enable the doors box collider so the player cant walk through the door
		doorIsOpen = false; // set bool flag
	}

	public void OpenDoor()
	{
		// open the door
		doorSpriteRenderer.sprite = doorOpenSprite;
		doorBoxCollider.enabled = false;  // disable the doors box collider so the player can walk through the door
		doorIsOpen = true; // set bool flag
	}
	#endregion

	#region Coroutines
	#endregion
}
