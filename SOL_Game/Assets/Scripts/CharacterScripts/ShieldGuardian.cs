using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGuardian : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		damageToGive; // the bosses damage that is dealed to the player
	public EncounterManager
		encounterManager; // this reference is used to send a signal when the basilisk dies
	public Transform
		roomCenterTransform; // a transform at the center of the room used for some of the bosses movement calculation
	public EnemySpawner
		ShieldGuardianEnemySpawner; // a reference to the bosses enemy spawner
	public SpriteRenderer
		bossShieldSprite; // a reference to the bosses shield sprite renderer
	#endregion

	#region Private Variables
	private bool
		Charging = false, // flag for if the enemy is charging at the player
		stunned = false, // flag for if the enemy is stunned
		hittingPlayer = false, // flag fir is the enemy is colliding with the player right now
		canDoHalfHealthEvent = true, // flag so that this health event only happens once
		canDoQuarterHealthEvent = true, // flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true, // flag so that this health event only happens once
		enemyIsShacking = false; // for making the enemy look "mad"
		
	private float
		enemyChargeSpeed = 15, // how fast the enemy charges at the player
		shackSpeed = 50.0f, //how fast it shakes
		shackAmount = .01f; //how much it shakes
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		ShieldGuardianEnemySpawner.AddNewEnemyID(gameObject.GetInstanceID());
		ShieldGuardianEnemySpawner.StartCheckingIfEnemiesDefeated();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		// check if the boss should start charging at the player
		if (Charging == false && stunned == false && canAttack)
		{
			// make the boss charge at the player
			StartCoroutine(MoveInPlayersDirection());
		}

		// if the enemy should be shacking start shacking the enemy
		if (enemyIsShacking)
		{
			transform.position = new Vector2(transform.position.x + (Mathf.Sin(Time.time * shackSpeed) * shackAmount), transform.position.y + (Mathf.Sin(Time.time * shackSpeed) * shackAmount));
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// if the boss collided with the player damage the player
		if (collision.gameObject.CompareTag("Player") && Charging) // only damage the player when charging
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
		}
		// check if the enemy hit a wall
		else if (collision.gameObject.CompareTag("Wall"))
		{
			// check if the enemy is hitting the wall and the player is so move to the center (so that the player doen't get stuck between the wall and the enemy)
			if (hittingPlayer)
			{
				Charging = false;
				canAttack = false;
				StartCoroutine(MoveToCenter(1f));
			}
			// "stun" the enemy because he ran into a wall
			else
			{
				Charging = false;
				StartCoroutine(StunEnemy(3));
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		// set flag for if the player is being collided with
		if (collision.gameObject.CompareTag("Player"))
		{
			hittingPlayer = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		// set flag that the player is not being collided with
		if (collision.gameObject.CompareTag("Player"))
		{
			hittingPlayer = false;
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> shield guardians overridden takeDamage() method, mainly for doing things at curtain health points</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			//encounterManager.EndEncounter();
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
		StartCoroutine(SpawningEnemiesActions(numOfEnemiesToSpawn + 2));

		// increase enemy charge speed
		enemyChargeSpeed += 2.5f;

		// spawn in enemies
		StartCoroutine(ShieldGuardianEnemySpawner.SpawnInEnemies(false, numOfEnemiesToSpawn)); 
	}

	/// <summary> deal damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage((int)damageToGive.initialValue, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}
	#endregion

	#region Coroutines
	/// <summary> Moves a game object in the player direction (at the time this method is called) over N seconds </summary>
	public IEnumerator MoveInPlayersDirection()
	{
		// set flags
		Charging = true;
		canTakeDamage = false;
		bossShieldSprite.enabled = true;

		// get the direction the player is in
		Vector3 targetDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;

		// charge at the player
		while (Charging == true)
		{
			transform.position += targetDirection * enemyChargeSpeed * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// set flags
		canTakeDamage = true;
		bossShieldSprite.enabled = false;
	}

	/// <summary> Moves a game object to a center of the room position over N seconds </summary>
	public IEnumerator MoveToCenter(float delayTime = 0)
	{
		float seconds = 1; // the time in second that it will take to move to the center
		float elapsedTime = 0; // timer variable

		// wait for the optional delay time before moving the boss to the center
		yield return new WaitForSeconds(delayTime);

		Vector3 startingPosition = transform.position; // save the starting position

		// move the enemy a little each frame based on how many seconds it should take to get the ending position
		while (elapsedTime < seconds)
		{
			transform.position = Vector3.Lerp(startingPosition, roomCenterTransform.position, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// add a delay before letting the boss attack again
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

	/// <summary> for enabling the shield, and disabling attacking then after a delay disabling the shield, and enabling attacking again</summary>
	private IEnumerator SpawningEnemiesActions(float delayTime)
	{
		canAttack = false;
		bossShieldSprite.enabled = true;
		enemyIsShacking = true;

		yield return new WaitForSeconds(delayTime);

		canAttack = true;
		bossShieldSprite.enabled = false;
		enemyIsShacking = false;
	}

	/// <summary> this method stuns the enemy and after a N second delay unstuns that enemy</summary>
	private IEnumerator StunEnemy(float delayTime)
	{
		stunned = true;

		yield return new WaitForSeconds(delayTime);

		stunned = false;
	}
	#endregion

}
