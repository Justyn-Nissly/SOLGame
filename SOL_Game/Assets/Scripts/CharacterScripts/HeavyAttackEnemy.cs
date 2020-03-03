using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		maxTimeBetweenAttacks, // Longest possible interval between attacks
		minTimeBetweenAttacks; // Shortest possible interval between attacks
	#endregion

	#region Private Variables
	private float
		attackTimer; // Time until the next attack
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Attack the player if possible </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		// Deal heavy melee attack if the enemy is aggroed
		if (attackTimer <= 0 && aggro)
		{
			attackTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
			MeleeAttack(heavyMeleeWeapon, heavyMeleeAttackPosition, heavyMeleeAttackRange, heavyMeleeDamageToGive);
		}
		else
		{
			attackTimer -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}