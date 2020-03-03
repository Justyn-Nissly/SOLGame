using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		maxTimeBetweenAttacks, // Longest possible interval between attacks
		minTimeBetweenAttacks; // Shortest possible interval between attacks
	public FloatValue
		shieldEnemyDamageToGive; // Shield bash damage
	#endregion

	#region Private Variables
	private float
		shieldDownTimer; // Controls deactivating shield
	private EvasiveStrike
		strike; // Enable shield bashing the player
	private bool
		canDamagePlayer = false; // Check if shield bash already hit player

	 #endregion

	// Unity Named Methods
	/// <summary> Initialize enemy </summary>
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

	/// <summary> Control the shield and charge at the player </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		// Drop the shield after charging at the player
		if (shieldDownTimer > 0.0f)
		{
			shieldDownTimer -= Time.deltaTime;
			if (shieldDownTimer <= 0.0f)
			{
				DisableShield();
				shieldIsEnabled = false;
				canAttack = true;
				canDamagePlayer = false;
			}
		}
		// Charge at the player
		else if (strike.charging)
		{
			shieldDownTimer = 1.5f;
		}
		else
		{
			StartCoroutine(ReEnableShield(1.5f));
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Deal damage on shield bash </summary>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && canDamagePlayer)
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)shieldEnemyDamageToGive.initialValue);
		}
	}
	#endregion

	#region Coroutines
	/// <summary> Reenable the shield after a delay</summary>
	public IEnumerator ReEnableShield(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);

		EnableShield();
		shieldIsEnabled = true;
		canAttack = false;
		canDamagePlayer = true;
	}
	#endregion
}