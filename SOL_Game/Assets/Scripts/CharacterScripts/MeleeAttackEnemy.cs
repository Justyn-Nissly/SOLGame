using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public EnemyMovement
		enemyMovement;// referance to the enemy movement logic
	public bool
		hasExtraAttacks = false; // set this flag to true if the enemy has more than one attack in each direction that you want to randomly chose from
	#endregion

	#region Private Variables
	private bool
		usingWeapon = false; // a flag for knowing if the enemy is using its weapon/attacking
	private float
		attackCountDownTimer = 1, // countdown timer between attacks
		attackInterval = .5f; // pause time interval between attacks
	#endregion

	// Unity Named Methods
	#region Main Methods

	public override void FixedUpdate()
	{
		// call the logic that is in the base script Enemy first
		base.FixedUpdate();

		// attack with the melee weapon every couple seconds if the enemy is aggro
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
		canAttack = false;

		// set up the attack animation to play
		if (hasExtraAttacks)
		{
			characterAnimator.SetInteger("AttackSeed", Random.Range(0, 3)); // randomly play an attack animations, nothing will happen if attack seed is not a animator parameter
		}
		characterAnimator.SetBool("Attacking", true); // set bool flag attacking to true
		characterAnimator.SetInteger("attackDirection", GetAnimationDirection(1)); // set the value that plays the right attacking direction animation
		characterAnimator.SetLayerWeight(2, 2); // increase the attack layer priority
	}

	/// <summary> deal damage called in an animation event</summary>
	public void DealDamage()
	{
		// use the heavy attack logic if its not null
		if(heavyMeleeAttackPosition != null && heavyMeleeDamageToGive != null)
		{
			MeleeAttack(heavyMeleeWeapon, heavyMeleeAttackPosition, heavyMeleeAttackRange, heavyMeleeDamageToGive, false);
		}
		// use the light attack logic if its not null
		else if (lightMeleeAttackPosition != null && lightMeleeDamageToGive != null)
		{
			MeleeAttack(lightMeleeWeapon, lightMeleeAttackPosition, lightMeleeAttackRange, lightMeleeDamageToGive, false);
		}
	}

	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		// Direct hits stun enemies
		if (Mathf.Abs(animationDirection - player.animationDirection) == 2 &&
		    Vector2.Distance(transform.position, player.transform.position) <= 2.0f)
		{
			attackCountDownTimer = 1.4f;
		}
		base.TakeDamage(damage, playSwordImpactSound);
	}


	/// <summary> ends an attack animation (called with an event in the attack animation)</summary>
	public void EndAttackAnimation()
	{
		// set flag that the enemy is not using the attack anymore
		usingWeapon = false;

		// set flag that lets the enemy move again at the player
		if (enemyMovement.canMoveAtPlayer)
			enemyMovement.canMove = true;

		canAttack = true; // set can attack flag

		// end the attack animation
		characterAnimator.SetLayerWeight(2, 0); // lowers the blaster layer priority
		characterAnimator.SetBool("Attacking", false); // set flag attacking to false

		attackCountDownTimer = attackInterval; // reset the countdown timer
	}

	public override IEnumerator Die()
	{
		enemyMovement.enabled = false;

		return base.Die();
	}
}
