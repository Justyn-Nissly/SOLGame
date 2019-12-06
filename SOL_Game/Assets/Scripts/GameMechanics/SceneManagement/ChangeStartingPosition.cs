using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStartingPosition : MonoBehaviour
{
	// Empty
	#region Enums
	#endregion

	// Empty
	#region Public Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			// change the players starting position
			LoadPlayerHub.UseBeginningPostion = false;
		}
	}
	#endregion

	// Empty
	#region Utility Methods
	#endregion

	// Empty
	#region Coroutines
	#endregion
}
