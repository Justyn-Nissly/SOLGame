using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeShieldEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	#endregion

	#region Private Variables
	private float
		shieldDropTime = 3, // How often the enemy disables his shield to attack
		shieldReEnableDelayTime = 1, // how long it takes for the enemy to re enable his shield after attacking
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

	///<summary> Toggle the shield on and off based on a timer </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (shieldDownTimer <= 0.0f && aggro)
		{
			shieldDownTimer = shieldDropTime; // reset timer

			DisableShield(); // make the enemy disable there shield

			// attack with a light attack
			MeleeAttack(lightMeleeWeapon, lightMeleeAttackPosition, lightMeleeAttackRange, lightMeleeDamageToGive, true);

			Invoke("ReEnableShield", shieldReEnableDelayTime); // re enable the enemies shield after N seconds
			
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
		if (shieldIsEnabled)
		{
			DisableShield();
			shieldIsEnabled = false;
			canAttack = true;
		}
		else
		{
			ReEnableShield();
			canAttack = false;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}