using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeShieldEnemy : ShieldBase
{
    #region Enums
    #endregion

    #region Public Variables
    public bool
        hasAttack = false;
    public float
        shieldDropTime;
    #endregion

    #region Private Variables
    public float
        shieldDownTimer;
    private Enemy
        enemy; // Access the enemy's members
    private LightAttackEnemy
        lightAttackEnemy; // Access the enemy's members
    #endregion

    // Unity Named Methods
    #region Main Methods
    private void Awake()
    {
        EnableShield();
        enemy = GetComponent<Enemy>();
        lightAttackEnemy = GetComponent<LightAttackEnemy>();
        ShieldIsEnabled = true;
        shieldDownTimer = 0.0f;
    }

    public void FixedUpdate()
    {
Debug.Log("ShieldIsEnabled == " + ShieldIsEnabled);
        shieldDownTimer -= Time.deltaTime;
        if (shieldDownTimer <= 0.0f)
        {
            ToggleShield();
            shieldDownTimer = shieldDropTime;
        }
    }
    #endregion

    #region Utility Methods
    public void ReEnableShield()
    {
        EnableShield();
        ShieldIsEnabled = true;
    }

    public void ToggleShield()
    {
        if (ShieldIsEnabled)
        {
            DisableShield();
            ShieldIsEnabled = false;
            enemy.canAttack = true;
        }
        else
        {
            ReEnableShield();
            enemy.canAttack = false;
        }
    }
	#endregion

	#region Coroutines
	#endregion
}