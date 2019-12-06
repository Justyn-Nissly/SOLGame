using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
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
		if (collision.gameObject.CompareTag("PlayerHeavyWeapon"))
		{
			Destroy(gameObject);
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
