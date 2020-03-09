using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackEnemy : Enemy
{
	#region Enums (Empty)
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
	}

	///<summary> Make the enemy ready to attack </summary>
	public override void Start()
	{
		base.Start();

		enemy          = GetComponent<Enemy>();
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
					attackCountDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
			}
			else
			{
					attackCountDownTimer -= Time.deltaTime;
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}