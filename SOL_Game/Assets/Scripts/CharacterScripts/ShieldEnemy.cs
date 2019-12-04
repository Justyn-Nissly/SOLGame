using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		maxTimeBetweenAttacks,
		minTimeBetweenAttacks;
	#endregion

	#region Private Variables
	private float
		shieldDownTimer; // When this timer runs out the enemy drops its shield
	private EvasiveStrike
		strike;
    #endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		EnableShield();
		strike          = GetComponent<EvasiveStrike>();
		shieldIsEnabled = true;
		canAttack = false;
		shieldDownTimer = 0.0f;
    }

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (shieldDownTimer > 0.0f)
		{
			shieldDownTimer -= Time.deltaTime;
			if (shieldDownTimer <= 0.0f)
			{
					DisableShield();
					shieldIsEnabled = false;
					canAttack = true;
			}
		}
		else if (strike.charging)
		{
			shieldDownTimer = 1.5f;
		}
		else
		{
			Invoke("ReEnableShield", 1.5f);
		}
	}
	#endregion

	#region Utility Methods
	public void ReEnableShield()
	{
		EnableShield();
		shieldIsEnabled = true;
		canAttack = false;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}