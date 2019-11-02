using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackEneny : MeleeAttackBase
{
	#region Enums
	#endregion

	#region Public Variables
	public float maxTimeBetweenAttacks = 2f;
	public float minTimeBetweenAttacks = 1f;
	#endregion

	#region Private Variables
	private float countDownTimer;
	private Enemy enemy;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (countDownTimer <= 0 && enemy.aggro)
		{
			countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks

			Attack();
		}
		else
		{
			countDownTimer -= Time.deltaTime;
		}
	}

	private void Start()
	{
		enemy = GetComponent<Enemy>();
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion
}
