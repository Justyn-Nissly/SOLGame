using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerGuardianWeakness : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public int
		health;
	public Sprite
		destroyedSprite; // Sprite displayed when object is destroyed
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods

	private void Update()
	{
		;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Check if the right weapon hit this object
		if (collision.gameObject.CompareTag("PlayerLightWeapon"))
		{
			if (health-- < 0)
			{
				DestroyObject();
			}
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> call this method to destroy the destructible object </summary>
	private void DestroyObject()
	{
		// Change to destroyed sprite if available
		if (destroyedSprite != null)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}