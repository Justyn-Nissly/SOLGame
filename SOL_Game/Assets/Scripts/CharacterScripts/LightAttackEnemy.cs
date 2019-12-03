using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackEnemy : Enemy
{
	#region Enums
	#endregion

	#region Public Variables
	public Enemy
        enemy;
    public float
	    maxTimeBetweenAttacks = 1.2f,
	    minTimeBetweenAttacks = 0.7f,
	    countDownTimer;
    public bool
        attacking;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (enemy.aggro)
		{
			if (countDownTimer <= 0 && enemy.canAttack && Vector2.Distance(transform.position, enemy.playerPos) <= lightMeleeAttackRange + 0.6f)
			{
				countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks
				attacking = true;
				MeleeAttack(lightMeleeWeapon, lightMeleeAttackPosition, lightMeleeAttackRange, lightMeleeDamageToGive, false);
			}
			else
			{
				attacking = false;
				countDownTimer -= Time.deltaTime;
			}
		}
		//else
		//{
		//    countDownTimer = 0.0f;
		//}
	}
	#endregion

    #region Utility Methods
    #endregion

	  #region Coroutines
	  #endregion
}
