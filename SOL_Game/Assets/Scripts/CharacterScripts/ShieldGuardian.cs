using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGuardian : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		basiliskDamageToGive;
	public EncounterManager
		encounterManager; // this reference is used to send a signal when the basilisk dies
	public Transform
		roomCenterTransform;
	public EnemySpawner
		ShieldGuardianEnemySpawner;
	public SpriteRenderer
		bossShieldSprite;
	#endregion

	#region Private Variables
	private bool
		Charging = false,
		stunned = false,
		hittingPlayer = false,
		canSpawnEnemies = true;
	private float
		enemyChargeSpeed = 15;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (Charging == false && stunned == false && canAttack) // the basilisk is constantly moving
		{
			ChargePlayer();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && Charging) // only damage the player when charging
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
		}
		else if (collision.gameObject.CompareTag("Wall"))
		{
			if (hittingPlayer)
			{
				Charging = false;
				canAttack = false;
				StartCoroutine(MoveToCenter());
			}
			else
			{
				Charging = false;
				stunned = true;
				Invoke("UnstunEnemy", 3f);
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			hittingPlayer = true;
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			hittingPlayer = false;
		}
	}
	#endregion

	#region Utility Methods
	private void UnstunEnemy()
	{
		stunned = false;
	}

	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			encounterManager.EndEncounter();
			canAttack = false;
			Charging = false;
		}
		else if (currentHealth <= maxHealth.initialValue / 2 && canSpawnEnemies)
		{
			// spawn enemies
			canSpawnEnemies = false;
			StartCoroutine(ShieldGuardianEnemySpawner.SpawnInEnemies());
		}
	}

	/// <summary> the method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage((int)basiliskDamageToGive.initialValue, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}

	private void ChargePlayer()
	{
		// Move the ranged guardian to the closest teleporter location
		Charging = true;
		StartCoroutine(MoveInPlayersDirection());
	}
	#endregion

	#region Coroutines
	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveInPlayersDirection()
	{
		Vector3 targetDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;

		canTakeDamage = false;
		bossShieldSprite.enabled = true;

		// charge at the player
		while (Charging == true)
		{
			transform.position += targetDirection * enemyChargeSpeed * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		canTakeDamage = true;
		bossShieldSprite.enabled = false;
	}

	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveToCenter()
	{
		float seconds = 1;
		float elapsedTime = 0;
		Vector3 startingPosition = transform.position; // save the starting position

		// move the basilisk a little each frame based on how many seconds it should take to get the ending position
		while (elapsedTime < seconds)
		{
			transform.position = Vector3.Lerp(startingPosition, roomCenterTransform.position, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// wait for N seconds
		yield return new WaitForSeconds(2f);

		canAttack = true;
	}
	#endregion

}
