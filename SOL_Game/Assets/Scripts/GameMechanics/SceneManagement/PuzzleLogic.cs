using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Sprite completeSprite, // the sprite that will be changed to when you complete the puzzle
				  incompleteSprite,
				  lockedSprite,
				  unpoweredSprite,
				  unlockedSprite;
	public DoorManager doorManager; // used to unlock the door(s) that the puzzle unlocks
	public SpriteRenderer LazerSprite; // this sprite is enabled when the puzzle is completed (if this sprite renderer is NULL nothing will happen it will not cause an error)
	public bool playerCanTriggerPressurePlate = true, // a flag for if the player can trigger the pressure plate
				isPowered, // A flag checking if the preasure plate is powered
				isLocked;  // A flag checking if the preasuer plate is locked
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
	}

	/// <summary> Complete a solved puzzle </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{

		if (isPowered == false && collision.gameObject.CompareTag("Player"))
		{
			spriteRenderer.sprite = unlockedSprite;
		}
		else if (doorHasKey && playerHasDoorKey && collision.gameObject.CompareTag("Player")) // check if this door has a key and if the player has that key to unlock this door
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
		// If the puzzle is solved call a method that completes it
		if (collision.CompareTag("PuzzleItem"))
		{
			OnPuzzleComplete(collision.gameObject);
		}
		else if (collision.CompareTag("Player") && playerCanTriggerPressurePlate)
		{
			spriteRenderer.sprite = completeSprite;

			// Unlock the doors
			doorManager.UnlockAllDoors();
		}
		/*private void OnTriggerEnter2D(Collider2D collision)
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
	}*/
	}

	/// <summary> Prevent the player from passing an unsolved puzzle </summary>
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && playerCanTriggerPressurePlate)
		{
			spriteRenderer.sprite = incompleteSprite;

			// Lock the doors
			doorManager.LockAllDoors();
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

		// Unlock the doors
		doorManager.UnlockAllDoors();
	}

	/// <summary> method to enable the lazer sprite when the puzzle is completed </summary>
	private void EnableLazerSprite()
	{
		LazerSprite.enabled = true;
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
