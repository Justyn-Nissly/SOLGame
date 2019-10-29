using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : ShieldBase
{
	#region Enums
	#endregion

	#region Public Variables
	public Enemy enemy;
	public bool hasAttack = false;
	public float maxTimeBetweenAttacks = 1.2f;
	public float minTimeBetweenAttacks = 0.7f;
	#endregion

	#region Private Variables
	private float countDownTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void Start()
	{
		EnableShield();
		ShieldIsEnabled = true;
		enemy.canAttack = false;
	}

	public void FixedUpdate()
	{
		if (countDownTimer <= 0 && hasAttack)
		{
			countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks

			// disable shield for half a second
			DisableShield();
			ShieldIsEnabled = false;
			enemy.canAttack = true;

			Invoke("ReEnableShield", 0.5f);
		}
		else
		{
			countDownTimer -= Time.deltaTime;
		}

	}
	#endregion

	#region Utility Methods
	public void ReEnableShield()
	{
		EnableShield();
		ShieldIsEnabled = true;
		enemy.canAttack = false;
	}
	#endregion

	#region Coroutines
	#endregion
}
