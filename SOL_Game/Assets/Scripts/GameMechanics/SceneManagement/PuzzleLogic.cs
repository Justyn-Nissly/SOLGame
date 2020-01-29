using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
	// Empty
	#region Enums
	#endregion

	#region Public Variables
	public Sprite completeSprite, // the sprite that will be changed to when you complete the puzzle
					  incompleteSprite;
	public DoorManager doorManager; // used to unlock the door(s) that the puzzle unlocks
	public SpriteRenderer LazerSprite; // this sprite is enabled when the puzzle is completed (if this sprite renderer is NULL nothing will happen it will not cause an error)
	public bool playerCanTriggerPressurePlate = true; // a flag for if the player can trigger the pressure plate
	#endregion

	#region Private Variables
	private SpriteRenderer spriteRenderer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("PuzzleItem"))
		{
			// this puzzle is complete so call a method that completes this puzzle
			OnPuzzleComplete(collision.gameObject);
		}
		else if (collision.CompareTag("Player") && playerCanTriggerPressurePlate)
		{
			spriteRenderer.sprite = completeSprite;
			doorManager.UnlockAllDoors(); // not using the signal system yet
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && playerCanTriggerPressurePlate)
		{
			spriteRenderer.sprite = incompleteSprite;
			doorManager.LockAllDoors(); // not using the signal system yet
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

		// freeze the items movement
		puzzleItem.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // set velocity to zero
		puzzleItem.GetComponent<Rigidbody2D>().isKinematic = true; // stop this item from moving

		// Move the puzzle item to the end location
		StartCoroutine(MoveOverSeconds(puzzleItem, transform.position, 2f));

		// play sound complete puzzle sound
		GetComponent<AudioSource>().Play();

		// send signal to unlock doors
		doorManager.UnlockAllDoors(); // not using the signal system yet
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
		float elapsedTime = 0;
		Vector3 startingPos = objectToMove.transform.position;

		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		objectToMove.transform.position = end;
	}
	#endregion
}
