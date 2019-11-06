using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackPlayer : MeleeAttackBase
{
	#region Enums
	#endregion

	#region Public Variables
	public Player player;
	public float startTimeBetweenAttacks = .3f;
	#endregion

	#region Private Variables
	private float timeBetweenAttacks;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (timeBetweenAttacks <= 0 && Input.GetButtonDown("A") && player.CanAttack)
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
