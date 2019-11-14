using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackPlayer : MeleeAttackBase
{
	#region Enums
	#endregion

	#region Public Variables
	public Player player;
	public float startTimeBetweenAttacks = .6f;
	#endregion

	#region Private Variables
	private float timeBetweenAttacks;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (timeBetweenAttacks <= 0 && Input.GetButtonDown("X") && player.canAttack)
		{
			timeBetweenAttacks = startTimeBetweenAttacks; // reset the time between attacks

			Attack();
		}
		else
		{
			timeBetweenAttacks -= Time.deltaTime;
		}
	}
    #endregion

    #region Utility Methods
    #endregion

    #region Coroutines
    #endregion


   

}
