﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : ShieldBase
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
    private Enemy
        enemy; // Access the enemy's members
    private EvasiveStrike
        strike;
    #endregion

    // Unity Named Methods
    #region Main Methods
    private void Start()
	{
		EnableShield();
        enemy           = GetComponent<Enemy>();
        strike          = GetComponent<EvasiveStrike>();
		shieldIsEnabled = true;
        enemy.canAttack = false;
        shieldDownTimer = 0.0f;
    }

	public void FixedUpdate()
	{
        if (shieldDownTimer > 0.0f)
        {
            shieldDownTimer -= Time.deltaTime;
            if (shieldDownTimer <= 0.0f)
            {
                DisableShield();
                shieldIsEnabled = false;
                enemy.canAttack = true;
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
		enemy.canAttack = false;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}