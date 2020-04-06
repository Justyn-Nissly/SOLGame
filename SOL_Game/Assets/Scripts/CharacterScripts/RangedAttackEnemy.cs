using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackEnemy : Enemy
{
	#region Enums
	#endregion

	#region Public Variables
	public EnemyMovement
		enemyMovement;
	public Transform // the point that a bullet will be created at
		firePointNorth,
		firePointEast,
		firePointSouth,
		firePointWest;
	#endregion

	#region Private Variable
	private bool
		usingBlaster = false;
	private float
		attackCountDownTimer = 3,
		attackInterval = 1;
	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary> If the enemy is aggro it fires at the player </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (aggro && canAttack && usingBlaster == false && attackCountDownTimer <= 0 && maxHealth.runTimeValue > 0)
		{
			StartAttackAnimation();
		}
		else
		{
			attackCountDownTimer -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> starts the attack animations (bullet created with an animation event and the shoot method)</summary>
	private void StartAttackAnimation()
	{
		// set flags
		usingBlaster = true;
		enemyMovement.canMove = false;

		// set up the attack animation to play
		characterAnimator.SetBool("blasting", true); // set bool flag blasting to true
		characterAnimator.SetInteger("attackDirection", GetAnimationDirection(1)); // set the value that plays the right blaster direction animation
		characterAnimator.SetLayerWeight(2, 2); // increase the blaster layer priority
	}


	/// <summary> ends an attack animation (called with an event in the attack animation)</summary>
	public void EndAttackAnimation()
	{
		// set flag that the enemy is not using the blaster anymore
		usingBlaster = false;
		enemyMovement.canMove = true;

		// end the attack animation
		characterAnimator.SetLayerWeight(2, 0); // lowers the blaster layer priority
		characterAnimator.SetBool("blasting", false); // set flag blasting to false

		attackCountDownTimer = attackInterval;
	}

	/// <summary> this gets the direction that an animations should play based on the characters idle animation state</summary>
	public override int GetAnimationDirection(int idleLayerIndex)
	{
		int animationDirection = 0; // return value for the animations direction

		AnimatorClipInfo[] animatorStateInfo = characterAnimator.GetCurrentAnimatorClipInfo(idleLayerIndex);

		switch (animatorStateInfo[0].clip.name)
		{
			case "IdleLeft":
				animationDirection = WEST;
				firePoint = firePointWest;
				break;
			case "IdleUp":
				animationDirection = NORTH;
				firePoint = firePointNorth;
				break;
			case "IdleRight":
				animationDirection = EAST;
				firePoint = firePointEast;
				break;
			case "IdleDown":
				animationDirection = SOUTH;
				firePoint = firePointSouth;
				break;
		}

		return animationDirection;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}