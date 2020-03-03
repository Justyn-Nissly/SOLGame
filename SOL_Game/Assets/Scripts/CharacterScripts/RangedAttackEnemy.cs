using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public int
		rangeAttackDamage; // Projectile damage
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
	///<summary> Assign attack damage </summary>
	private void Awake()
	{
		rangeAttackDamage = (int)rangedAttackDamageToGive.initialValue;
	}

	///<summary> Get the enemy ready to attack </summary>
	public override void Start()
	{
		base.Start();
		attackTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
	}

	///<summary> Fire at the player if aggro </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (aggro)
		{
			if (attackTimer <= 0)
			{
				Shoot();
				attackTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
			}
			else
			{
				attackTimer -= Time.deltaTime;
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}