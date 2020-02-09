using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGuardian : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		damageToGive;
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
		canDoHalfHealthEvent = true,
		canDoQuarterHealthEvent = true,
		canDoThreeQuarterHealthEvent = true,
		enemyIsShacking = false; // for making the enemy look "mad"
		
	private float
		enemyChargeSpeed = 15;

	private float speed = 50.0f; //how fast it shakes
	private float amount = .01f; //how much it shakes
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

		if (enemyIsShacking)
		{
			transform.position = new Vector2(transform.position.x + (Mathf.Sin(Time.time * speed) * amount), transform.position.y + (Mathf.Sin(Time.time * speed) * amount));
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
				Invoke("StartMovingToCenter", 1f);
				
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
	/// <summary> this method is here so that we can unstun the enemy after a N second delay with Invoke</summary>
	private void UnstunEnemy()
	{
		stunned = false;
	}

	/// <summary> for enabling the shield, and disabling attacking</summary>
	private void StartSpawningEnemiesActions()
	{
		canAttack = false;
		bossShieldSprite.enabled = true;
		enemyIsShacking = true;
	}

	/// <summary> for disabling the shield, and enabling attacking</summary>
	private void StopSpawningEnemiesActions()
	{
		canAttack = true;
		bossShieldSprite.enabled = false;
		enemyIsShacking = false;
	}

	/// <summary> shield guardians overridden takeDamage() method, mainly for doing things at curtain health points</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			encounterManager.EndEncounter();
			canAttack = false;
			Charging = false;
		}

		// check if the enemy should start a health event
		if (currentHealth <= maxHealth.initialValue / 1.33333f && canDoThreeQuarterHealthEvent) // check if the enemy is at quarter health
		{
			canDoThreeQuarterHealthEvent = false; // this flag is here so this only can happen once
			StartCoroutine(StartHealthEvent(1)); // start health event
		}
		else if (currentHealth <= maxHealth.initialValue / 2 && canDoHalfHealthEvent) // check if the enemy is at half health
		{
			canDoHalfHealthEvent = false; // this flag is here so this only can happen once
			StartCoroutine(StartHealthEvent(2)); // start health event
		}
		else if (currentHealth <= maxHealth.initialValue / 4 && canDoQuarterHealthEvent) // check if the enemy is at quarter health
		{
			canDoQuarterHealthEvent = false; // this flag is here so this only can happen once
			StartCoroutine(StartHealthEvent(3)); // start health event
		}
	}

	/// <summary> this is every thing that happens a Health event point </summary>
	private void HealthEvent(int numOfEnemiesToSpawn)
	{
		print(numOfEnemiesToSpawn + " heath event"); // debug print statement

		// start doing boss spawning logic
		StartSpawningEnemiesActions();
		Invoke("StopSpawningEnemiesActions", numOfEnemiesToSpawn + 2);

		// increase enemy charge speed
		enemyChargeSpeed += 2.5f;

		// spawn in enemies
		StartCoroutine(ShieldGuardianEnemySpawner.SpawnInEnemies(numOfEnemiesToSpawn)); 
	}

	/// <summary> the method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage((int)damageToGive.initialValue, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}

	/// <summary> this method is for making the enemy charge in the players direction </summary>
	private void ChargePlayer()
	{
		// Move the ranged guardian to the closest teleporter location
		Charging = true;
		StartCoroutine(MoveInPlayersDirection());
	}

	/// <summary> This method is here because this is the best way to start a coroutine after a N second delay using Invoke</summary>
	private void StartMovingToCenter()
	{
		StartCoroutine(MoveToCenter());
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

	/// <summary> Moves a game object to a center of the room position over N seconds </summary>
	public IEnumerator MoveToCenter()
	{
		float seconds = 1;
		float elapsedTime = 0;
		Vector3 startingPosition = transform.position; // save the starting position

		// move the enemy a little each frame based on how many seconds it should take to get the ending position
		while (elapsedTime < seconds)
		{
			transform.position = Vector3.Lerp(startingPosition, roomCenterTransform.position, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}


		yield return new WaitForSeconds(2f);
		canAttack = true;

	}

	/// <summary> Moves a game object to a center of the room position over N seconds </summary>
	public IEnumerator StartHealthEvent(int numberOfEnemiesToSpawn)
	{
		float seconds = 1;
		float elapsedTime = 0;
		Vector3 startingPosition = transform.position; // save the starting position

		// move the enemy to the center
		while (elapsedTime < seconds)
		{
			transform.position = Vector3.Lerp(startingPosition, roomCenterTransform.position, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// start the health event
		HealthEvent(numberOfEnemiesToSpawn);
	}
	#endregion

}
