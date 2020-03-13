using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		maxTimeBetweenAttacks = 2f,
		minTimeBetweenAttacks = 1f;
	#endregion

	#region Private Variables
	private float
		countDownTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods

	public override void FixedUpdate()
	{
		// call the logic that is in the base script Enemy first
		base.FixedUpdate();

		// attack with the heavy melee weapon every couple seconds if the enemy is aggro
		if (countDownTimer <= 0 && aggro)
		{
			countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks

			MeleeAttack(heavyMeleeWeapon, heavyMeleeAttackPosition, heavyMeleeAttackRange, heavyMeleeDamageToGive, true);
		}
		else
		{
			countDownTimer -= Time.deltaTime;
		}
	}
	#endregion
}
