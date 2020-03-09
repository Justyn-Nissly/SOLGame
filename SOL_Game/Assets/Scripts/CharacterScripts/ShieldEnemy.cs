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
	public FloatValue
		shieldEnemyDamageToGive;
	#endregion

	#region Private Variables
	private float
		shieldDownTimer; // When this timer runs out the enemy drops its shield
	private EvasiveStrike
		strike;
	private bool
		canDamagePlayer = false;

	 #endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		EnableShield(true);
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
				canDamagePlayer = false;
			}
		}
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
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// if the boss collided with the player damage the player
		if (collision.gameObject.CompareTag("Player") && canDamagePlayer) // only damage the player when charging
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)shieldEnemyDamageToGive.initialValue);
		}
	}
	#endregion

	#region Coroutines
	/// <summary> re enables the enemy's shield after a delay</summary>
	public IEnumerator ReEnableShield(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);

		EnableShield(true);
		shieldIsEnabled = true;
		canAttack = false;
		canDamagePlayer = true;
	}
	#endregion
}