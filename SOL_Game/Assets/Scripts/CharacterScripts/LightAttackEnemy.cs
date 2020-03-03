using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackEnemy : Enemy
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
	/// <summary> Attack player as able </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		// Deal light melee attack if the enemy is aggroed
		if (attackTimer <= 0 && canAttack && aggro)
		{
			// Reset attack interval
			attackTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
			MeleeAttack(lightMeleeWeapon, lightMeleeAttackPosition, lightMeleeAttackRange, lightMeleeDamageToGive);
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