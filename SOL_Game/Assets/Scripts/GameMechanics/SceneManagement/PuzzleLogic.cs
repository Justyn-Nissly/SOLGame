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
	public bool
		playerCanTriggerPressurePlate = true, // A flag for if the player can trigger the pressure plate
		enemyCanTriggerPressurePlate,
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
				isComplete = true;
				spriteRenderer.sprite = completeSprite;
				if (doorManager.CheckPressurePlatesPressed() == doorManager.pressurePlates.Count)
				{
					OnPuzzleComplete(collision.gameObject);
				}
			}

			if (collision.CompareTag("Enemy") && enemyCanTriggerPressurePlate)
			{
				// Lock the enemy in place and activate the pressure plate
				collision.GetComponent<EnemyMovement>().canMove = false;
				OnPuzzleComplete(collision.gameObject);
			}
		}
	}

	/// <summary> Prevent the player from passing an unsolved puzzle </summary>
	private void OnTriggerExit2D(Collider2D collision)
	{
		isComplete = false;
		if (collision.CompareTag("Player") && playerCanTriggerPressurePlate)
		{
			spriteRenderer.sprite = unlockedSprite;

			// Lock the doors
			doorManager.LockDoors();
		}
	}
	#endregion

	#region Utility Methods
	private void OnPuzzleComplete(GameObject itemOnPressurePlate)
	{
		// Set flags so that the pressure plate cannot be triggered any more
		playerCanTriggerPressurePlate = false;
		//enemyCanTriggerPressurePlate  = false;
		pressurePlateCollider.enabled = false;

		// Set the pressure plate to complete
		isComplete = true;

		// Change sprite to the complete sprite
		spriteRenderer.sprite = completeSprite;
		
		// Prevent the puzzle item from moving
		if (itemOnPressurePlate.CompareTag("Player") == false)
		{
			itemOnPressurePlate.GetComponent<Rigidbody2D>().velocity    = Vector2.zero;
			itemOnPressurePlate.GetComponent<Rigidbody2D>().isKinematic = true;
		}

		// Move the puzzle item to the end location
		if(itemOnPressurePlate.CompareTag("Player") == false)
		{
			StartCoroutine(MoveOverSeconds(itemOnPressurePlate, transform.position, 2.0f));
		}

		// Play the puzzle solved sound
		GetComponent<AudioSource>().Play();

		// Unlock the doors
		if (doorManager.CheckPressurePlatesPressed() == doorManager.pressurePlates.Count)
		{
			doorManager.UnlockDoors();
		}
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