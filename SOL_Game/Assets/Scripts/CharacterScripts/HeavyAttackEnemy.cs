using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackEnemy : Enemy
{
	#region Enums
	#endregion

	#region Public Variables
	public float maxTimeBetweenAttacks = 2f;
	public float minTimeBetweenAttacks = 1f;
	#endregion

	#region Private Variables
	private float countDownTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (countDownTimer <= 0 && aggro)
		{
			countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks

			MeleeAttack(heavyMeleeWeapon, heavyMeleeAttackPosition, heavyMeleeAttackRange, heavyMeleeDamageToGive, false);
		}
		else
		{
			countDownTimer -= Time.deltaTime;
		}
	}
	#endregion

	

}
