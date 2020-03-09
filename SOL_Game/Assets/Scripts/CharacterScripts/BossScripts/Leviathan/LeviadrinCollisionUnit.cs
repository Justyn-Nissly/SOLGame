using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeviadrinCollisionUnit : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	public FloatValue
		DamageToGive; // the damage that will be dealed to the player
	public Leviadrin 
		leviadrin;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Unity Main Methods
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		leviadrin.TakeDamage(damage, playSwordImpactSound);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// damage the player
		if (collision.gameObject.CompareTag("Player") && DamageToGive != null)
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
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
	public override IEnumerator Die()
	{
		StartCoroutine(leviadrin.Die());
		yield return null;
	}
	#endregion
}
