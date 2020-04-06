using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		shieldEnemyDamageToGive;
	public EnemyMovement
		enemyMovement;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods (Empty)
	#endregion

	#region Utility Methods
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// if the boss collided with the player damage the player
		if (collision.gameObject.CompareTag("Player")) // only damage the player when charging
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)shieldEnemyDamageToGive.initialValue);
		}
	}
	#endregion

	#region Coroutines
	public override IEnumerator Die()
	{
		enemyMovement.enabled = false;

		return base.Die();
	}
	#endregion
}