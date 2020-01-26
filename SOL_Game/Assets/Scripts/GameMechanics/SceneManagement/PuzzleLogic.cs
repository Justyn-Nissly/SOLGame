using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Sprite
		completeSprite,   // The sprite showing the puzzle is solved
		incompleteSprite; // The sprite showing the puzzle is not solved
	public DoorManager
		doorManager; // Unlocks door(s) after the puzzle is solved
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
		// If the puzzle is solved call a method that completes it
		if (collision.CompareTag("PuzzleItem"))
		{
			OnPuzzleComplete(collision.gameObject);
		}
		else if (collision.CompareTag("Player"))
		{
			spriteRenderer.sprite = completeSprite;

			// Unlock the doors
			doorManager.UnlockAllDoors();
		}
	}

	/// <summary> Prevent the player from passing an unsolved puzzle </summary>
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
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
	#endregion

	#region Coroutines
	/// <summary> Move a game object to a specified location over time </summary>
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