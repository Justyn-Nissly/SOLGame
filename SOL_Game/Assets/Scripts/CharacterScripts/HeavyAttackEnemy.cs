using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public EnemyMovement
		enemyMovement;
	#endregion

	#region Private Variables
	private bool
		usingWeapon = false;
	private float
		attackCountDownTimer = 1,
		attackInterval = .5f;
	#endregion

	// Unity Named Methods
	#region Main Methods

	public override void FixedUpdate()
	{
		// call the logic that is in the base script Enemy first
		base.FixedUpdate();

		// attack with the heavy melee weapon every couple seconds if the enemy is aggro
		if (enemyMovement.canMoveAtPlayer == false &&  // this makes the enemy only attack when next to the player
			 aggro && // is the player in the enemys sight range
			 canAttack && // is the enemy allowed to attack
			 attackCountDownTimer <= 0 && // the cool down between attacks is up?
			 usingWeapon == false) // only start another attack if the enemy's not attacking already
		{
			StartAttackAnimation();
		}
		else
		{
			attackCountDownTimer -= Time.deltaTime;
		}
	}
	#endregion

	/// <summary> starts the attack animations (damage attack method called in an animation event)</summary>
	private void StartAttackAnimation()
	{
		// set flags
		usingWeapon = true;
		enemyMovement.canMove = false;

		// set up the attack animation to play
		enemyAnimator.SetInteger("AttackSeed", Random.Range(0, 3)); // randomly play an attack animations
		enemyAnimator.SetBool("Attacking", true); // set bool flag attacking to true
		enemyAnimator.SetInteger("attackDirection", GetAnimationDirection()); // set the value that plays the right attacking direction animation
		enemyAnimator.SetLayerWeight(2, 2); // increase the attack layer priority
	}

	/// <summary> deal damage called in an animation event</summary>
	public void DealDamage()
	{
		MeleeAttack(heavyMeleeWeapon, heavyMeleeAttackPosition, heavyMeleeAttackRange, heavyMeleeDamageToGive, false);
	}


	/// <summary> ends an attack animation (called with an event in the attack animation)</summary>
	public void EndAttackAnimation()
	{
		// set flag that the enemy is not using the attack anymore
		usingWeapon = false;
		if(enemyMovement.canMoveAtPlayer)
			enemyMovement.canMove = true;

		// end the attack animation
		enemyAnimator.SetLayerWeight(2, 0); // lowers the blaster layer priority
		enemyAnimator.SetBool("Attacking", false); // set flag attacking to false

		attackCountDownTimer = attackInterval;
	}
}
