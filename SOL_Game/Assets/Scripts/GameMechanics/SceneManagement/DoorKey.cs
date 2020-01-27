using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public DoorLogic doorKeyUnlocks; // the door that this key unlocks
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			// update that the player has this key
			PlayerHasKey();

			// destroy this key from the facility
			Destroy(gameObject);
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> this method tells the door that the key unlocks that the player
	/// has the key so that it unlocks the next time the player walks next to that door </summary>
	private void PlayerHasKey()
	{
		doorKeyUnlocks.playerHasDoorKey = true;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
