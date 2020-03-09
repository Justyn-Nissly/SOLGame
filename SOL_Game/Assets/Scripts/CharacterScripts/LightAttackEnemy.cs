using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackEnemy : Enemy
{
	#region Enums
	#endregion

	#region Public Variables
	public float
		maxTimeBetweenAttacks = 1.2f,
		minTimeBetweenAttacks = 0.7f,
		countDownTimer;
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		// call the logic that is in the base script Enemy first
		base.FixedUpdate();

		// attack with the light melee weapon every couple seconds if the enemy is aggro and the enemy can attack(used so that an enemy doesnt attack if their shield is up)
		if (countDownTimer <= 0 && canAttack && aggro)
		{
			countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks
			MeleeAttack(lightMeleeWeapon, lightMeleeAttackPosition, lightMeleeAttackRange, lightMeleeDamageToGive, true);
		}
		else
		{
			countDownTimer -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}
