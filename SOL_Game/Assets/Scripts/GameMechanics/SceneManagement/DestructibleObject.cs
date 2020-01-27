using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	// Empty
	#region Public Variables
	public int
		health;
	public string
		damageTag; // Tag of objects that can damage this object
	#endregion

	// Unity Named Methods
	#region Main Methods
	void FixedUpdate()
	{
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag(damageTag))
		{
			health--;
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}