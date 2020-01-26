using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	public int
		health; // The destructible wall's health
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("PlayerHeavyWeapon"))
		{
			health--;
			if (health <= 0)
			{
				Destroy(gameObject);
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}