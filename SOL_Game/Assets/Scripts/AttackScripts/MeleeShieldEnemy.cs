using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeShieldEnemy : ShieldBase
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
    public float
        shieldDropTime; // How long the enemy drops it shield
    #endregion

    #region Private Variables
    public float
        shieldDownTimer; // Track when to drop the shield
    private Enemy
        enemy; // Access the enemy's members
    private LightAttackEnemy
        lightAttackEnemy; // This enemy uses light attacks
	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary> Initialize the enemy </summary>
	private void Awake()
    {
        EnableShield();
        enemy = GetComponent<Enemy>();
        lightAttackEnemy = GetComponent<LightAttackEnemy>();
        shieldIsEnabled = true;
        shieldDownTimer = 0.0f;
    }

	///<summary> Toggle the shield on and off based on a timer </summary>
	public void FixedUpdate()
    {
        shieldDownTimer -= Time.deltaTime;
        if (shieldDownTimer <= 0.0f)
        {
            ToggleShield();
            shieldDownTimer = shieldDropTime;
        }
    }
	#endregion

	#region Utility Methods
	///<summary> Turn the shield on </summary>
	public void ReEnableShield()
    {
        EnableShield();
        shieldIsEnabled = true;
    }

	///<summary> Toggle the shield and enable attack if the shield is down </summary>
	public void ToggleShield()
    {
        if (shieldIsEnabled)
        {
            DisableShield();
            shieldIsEnabled = false;
            enemy.canAttack = true;
        }
        else
        {
            ReEnableShield();
            enemy.canAttack = false;
        }
    }
	#endregion

	#region Coroutines (Empty)
	#endregion
}