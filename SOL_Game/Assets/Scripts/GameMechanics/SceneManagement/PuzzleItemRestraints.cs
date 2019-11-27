using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItemRestraints : MonoBehaviour
{
	// Empty
	#region Enums
	#endregion

	// Empty
	#region Public Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary>
	/// this is for letting the player and enemies go through this collider, because this is meant to stop only puzzle items
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
			Physics2D.IgnoreCollision(collision.collider, this.GetComponent<BoxCollider2D>());
	}
	#endregion

	// Empty
	#region Utility Methods
	#endregion

	// Empty
	#region Coroutines
	#endregion


}

