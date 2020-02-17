using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poisonObject : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	public FloatValue
		DamageToGive;
	#endregion

	#region Private Variables
	private float
		timer = 1,
		maxTimer = 1;
	#endregion

	// Unity Named Methods
	#region Unity Main Methods
	/// <summary> constantly damage the player every N seconds if the player is standing on the poison </summary>
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (timer < 0)
		{
			// damage the player
			if (collision.gameObject.CompareTag("Player") && DamageToGive != null)
			{
				DamagePlayer(collision.gameObject.GetComponent<Player>());
			}

			timer = maxTimer;
		}
		else
		{
			timer -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Functions
	/// <summary> the method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage((int)DamageToGive.initialValue, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
