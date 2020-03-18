using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackEnemy : Enemy
{
	#region Enums
	private const int
		WEST = 0,
		NORTH = 1,
		EAST = 2,
		SOUTH = 3;
	#endregion

	#region Public Variables
	public int
		rangeAttackDamage;
	public float
		maxTimeBetweenAttacks = 2f,
		minTimeBetweenAttacks = 1f;
	#endregion

	#region Private Variables
	private float
		attackCountDownTimer; // Track how long before the next attack
	private Enemy
		enemy;
	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary> Assign the attack's damage </summary>
	private void Awake()
	{
		rangeAttackDamage = (int)rangedAttackDamageToGive.initialValue;
		enemy = GetComponent<Enemy>();
	}

	///<summary> Make the enemy ready to attack </summary>
	public override void Start()
	{
		base.Start();
		attackCountDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
	}

	///<summary> If the enemy is aggro it fires at the player </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (enemy.aggro)
		{
			if (attackCountDownTimer <= 0)
			{
				Shoot(true);
				StartShootAnimation();
				attackCountDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
			}
			else
			{
					attackCountDownTimer -= Time.deltaTime;
			}
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> this method starts playing the shooting animation </summary>
	private void StartShootAnimation()
	{
		enemyAnimator.SetBool("blasting", true); // set bool flag blasting to true
		enemyAnimator.SetInteger("attackDirection", GetAnimationDirection()); // set the value that plays the right blaster direction animation
		enemyAnimator.SetLayerWeight(2, 2); // increase the blaster layer priority
	}

	/// <summary> ends an attack animation (called with an event in the attack animation)</summary>
	public void EndAttackAnimation()
	{
		enemyAnimator.SetLayerWeight(2, 0); // lowers the blaster layer priority

		enemyAnimator.SetBool("blasting", false); // set flag blasting to false
	}

	/// <summary> this gets the direction that an animations should play based on the players idle animation state</summary>
	private int GetAnimationDirection()
	{
		int animationDirection = 0; // return value for the animations direction

		AnimatorClipInfo[] animatorStateInfo = enemyAnimator.GetCurrentAnimatorClipInfo(1);

		switch (animatorStateInfo[0].clip.name)
		{
			case "IdleLeft":
				animationDirection = WEST;
				break;
			case "IdleUp":
				animationDirection = NORTH;
				break;
			case "IdleRight":
				animationDirection = EAST;
				break;
			case "IdleDown":
				animationDirection = SOUTH;
				break;
		}

		return animationDirection;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}