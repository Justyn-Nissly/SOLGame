using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeShieldEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	public float
		shieldDropTime         , // Time before the enemy deactivates its shield
		shieldReEnableDelayTime; // Time taken to reactivate the shield
	#endregion

	#region Private Variables
	private float
		shieldDownTimer; // Track when to drop the shield

	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary> Initialize the enemy </summary>
	private void Awake()
	{
		EnableShield();
		shieldIsEnabled = true;
		shieldDownTimer = shieldDropTime;
	}

	///<summary> Toggle the shield on and off </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (shieldDownTimer <= 0.0f && aggro)
		{
			DisableShield();
			shieldDownTimer = shieldDropTime;

			// Light melee attack
			MeleeAttack(lightMeleeWeapon, lightMeleeAttackPosition, lightMeleeAttackRange, lightMeleeDamageToGive);

			Invoke("ReEnableShield", shieldReEnableDelayTime);
		}
		else
		{
			shieldDownTimer -= Time.deltaTime;
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
		if (canAttack = shieldIsEnabled)
		{
			DisableShield();
			shieldIsEnabled = false;
		}
		else
		{
			ReEnableShield();
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}