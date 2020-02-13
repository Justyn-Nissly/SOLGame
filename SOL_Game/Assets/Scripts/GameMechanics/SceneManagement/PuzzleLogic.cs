using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public BoxCollider2D
		pressurePlateCollider; // The collider for the pressure plate that keeps the it locked
	public Sprite completeSprite,   // The sprite that will be changed to when you complete the puzzle
				  incompleteSprite, // The sprite to show the pressure plate is not being pressed
				  lockedSprite,     // The sprite to show the pressure plate is locked
				  openSprite,       // The sprite to show the pressure plate is open
				  unpoweredSprite,  // The sprite to show the pressure plate is unpowered
				  unlockedSprite;   // The sprite to show the pressure plate is unlocked
	public DoorManager
		doorManager; // Used to unlock the door(s) that the puzzle unlocks
	public SpriteRenderer
		LazerSprite; // This sprite is enabled when the puzzle is completed (if this sprite renderer is NULL nothing will happen it will not cause an error)
	public bool
		playerCanTriggerPressurePlate = true, // A flag for if the player can trigger the pressure plate
		isComplete, // A flag checking if the puzzle is complete
		isPowered,  // A flag checking if the pressure plate is powered
		isLocked;   // A flag checking if the preasuer plate is locked
	#endregion

	#region Private Variables
	private SpriteRenderer
		spriteRenderer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set up the sprite renderer </summary>
	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		UpdateDoorState();
	}

	/// <summary> Complete a solved puzzle </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (isLocked == false)
		{
			if (isComplete == false)
			{
				spriteRenderer.sprite = incompleteSprite;
			}
			// If the puzzle is solved call a method that completes it
			if (collision.CompareTag("PuzzleItem"))
			{
				isComplete = true;
				OnPuzzleComplete(collision.gameObject);
			}
			else if (collision.CompareTag("Player") && playerCanTriggerPressurePlate)
			{
				spriteRenderer.sprite = completeSprite;

				// Unlock the doors
				doorManager.UnlockAllDoors();
			}
		}
	}

	/// <summary> Prevent the player from passing an unsolved puzzle </summary>
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(isComplete == false)
		{
			spriteRenderer.sprite = unlockedSprite;
		}
		if (collision.CompareTag("Player") && playerCanTriggerPressurePlate)
		{
			spriteRenderer.sprite = unlockedSprite;

			// Lock the doors
			doorManager.LockDoors();
		}
	}
	#endregion

	#region Utility Methods
	private void OnPuzzleComplete(GameObject puzzleItem)
	{
		// set flag so that the player cant trigger the pressure plate any more
		playerCanTriggerPressurePlate = false;

		// enable the lazer sprite if it is not null
		if(LazerSprite != null)
		{
			Invoke("EnableLazerSprite", .5f); // using invoke to add a timed delay
		}

		// change sprite to the complete sprite
		spriteRenderer.sprite = completeSprite;

		// Prevent the puzzle item from moving
		puzzleItem.GetComponent<Rigidbody2D>().velocity    = Vector2.zero;
		puzzleItem.GetComponent<Rigidbody2D>().isKinematic = true;

		// Move the puzzle item to the end location
		StartCoroutine(MoveOverSeconds(puzzleItem, transform.position, 2.0f));

		// Play the puzzle solved sound
		GetComponent<AudioSource>().Play();


		// Check if all the pressure plates are pressed
		if(doorManager.pressurePlates.Count == doorManager.CheckPressurePlatesPressed())
		{
			// Unlock the doors
			doorManager.UnlockAllDoors();
		}
	}

	/// <summary> method to enable the lazer sprite when the puzzle is completed </summary>
	private void EnableLazerSprite()
	{
		LazerSprite.enabled = true;
	}

	public void UpdateDoorState()
	{
		if (isPowered == false)
		{
			spriteRenderer.sprite = unpoweredSprite;
		}
		else if (isLocked == true && isPowered == true)
		{
			spriteRenderer.sprite = lockedSprite;
		}
		else if (isLocked == false && isPowered)
		{
			spriteRenderer.sprite         = unlockedSprite;
			pressurePlateCollider.enabled = false;
		}
	}
	#endregion

	#region Coroutines
	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
	{
		float
			elapsedTime = 0;
		Vector3
			startingPos = objectToMove.transform.position;

		// Gradually move the object to its destination
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position  = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			elapsedTime                     += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// Make sure the object ends movement at its destination
		objectToMove.transform.position = end;
	}
	#endregion
}
