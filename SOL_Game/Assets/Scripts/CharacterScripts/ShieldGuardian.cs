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
	public Transform
		roomCenterTransform, // a transform at the center of the room used for some of the bosses movement calculation
		shootingPointLeft, // the left point at which a blaster bullet will be instantiated
		shootingPointRight, // the right point at which a blaster bullet will be instantiated
		LeftShootingLaneLimit,
		RightShootingLaneLimit;
	public EnemySpawner
		ShieldGuardianEnemySpawner; // a reference to the bosses enemy spawner
	public EncounterManager
		manager;
	public SpriteRenderer
		bossShieldSprite; // a reference to the bosses shield sprite renderer
	public AttactOrientationControllerEnemy
		armsRotationController; // this is a reference to the script that rotates the arms
	public Animator
		animator; // this is a reference to the animation controller
	#endregion

	#region Private Variables
	private bool
		Charging                     = false, // Flag for if the enemy is charging at the player
		stunned                      = false, // Flag for if the enemy is stunned
		hittingPlayer                = false, // Flag fir is the enemy is colliding with the player right now
		canDoHalfHealthEvent         = true,  // Flag so that this health event only happens once
		canDoQuarterHealthEvent      = true,  // Flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true,  // Flag so that this health event only happens once
		enemyIsShacking              = false, // For making the enemy look "mad"
		canShoot                     = true;
		
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
		if (canShoot && canAttack && PlayerInShootingLane())
		{
			canAttack = false;
			canShoot = false;
			RandomlySetShootingPoint();
			animator.SetTrigger("shootBlaster");
		}
		else if (Charging == false && stunned == false && canAttack)
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
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)damageToGive.initialValue);
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

		if (maxHealth.runTimeValue <= 0)
		{
			canAttack = false;
			Charging = false;
			manager.EndEncounter();
		}

		// check if the enemy should start a health event
		if (maxHealth.runTimeValue <= maxHealth.initialValue / 1.33333f && canDoThreeQuarterHealthEvent) // check if the enemy is at quarter health
		{
			canDoThreeQuarterHealthEvent = false; // this flag is here so this only can happen once
			StartCoroutine(StartHealthEvent(1)); // start health event
		}
		else if (maxHealth.runTimeValue <= maxHealth.initialValue / 2 && canDoHalfHealthEvent) // check if the enemy is at half health
		{
			canDoHalfHealthEvent = false; // this flag is here so this only can happen once
			StartCoroutine(StartHealthEvent(2)); // start health event
		}
		else if (maxHealth.runTimeValue <= maxHealth.initialValue / 4 && canDoQuarterHealthEvent) // check if the enemy is at quarter health
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

	/// <summary> this method is called with an event in the animator (thats why it exists) and it starts the shooting coroutine </summary>
	public void ShootGuardianBlaster()
	{
		StartCoroutine(ShootGuardianBlasterCoroutine());
	}

	/// <summary> Randomly Sets Shooting Point the guardian will use for the blaster attack</summary>
	private void RandomlySetShootingPoint()
	{
		if (Random.Range(0, 2) == 0)
		{
			animator.SetBool("LeftBlaster", true);
		}
		else
		{
			animator.SetBool("LeftBlaster", false);
		}
	}

	private bool PlayerInShootingLane()
	{
		bool playerInshootingLane = false;

		if(player.transform.position.x >= LeftShootingLaneLimit.position.x && // check if the player is passed the left limit
			player.transform.position.x <= RightShootingLaneLimit.position.x && // check if the player is passed the right limit
			player.transform.position.y < transform.position.y) // don't shoot if the player is above the enemy
		{
			print("player is not in the enemies shooting range");
			playerInshootingLane = true;
		}

		return playerInshootingLane;
	}
	#endregion

	#region Coroutines
	/// <summary> shoots a bullet from ether the left or right side of the shield guardian(its randomly selected)</summary>
	private IEnumerator ShootGuardianBlasterCoroutine()
	{
		Transform bulletSpawnPoint = animator.GetBool("LeftBlaster") ? shootingPointLeft : shootingPointRight;

		GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

		BulletLogic bulletLogic = bulletInstance.GetComponent<BulletLogic>();
		bulletLogic.bulletDamage = (int)rangedAttackDamageToGive.initialValue; // is this right

		yield return new WaitForSeconds(2);
		canShoot = true;
		canAttack = true;
	}

	/// <summary> Moves a game object in the player direction (at the time this method is called) over N seconds </summary>
	public IEnumerator MoveInPlayersDirection()
	{
		// set flags
		Charging = true;
		canTakeDamage = false;
		canAttack = false;
		animator.SetBool("isCharging", true);

		yield return new WaitForSeconds(1);

		armsRotationController.shouldLookAtPlayer = false; // stop the arms from rotating
		bossShieldSprite.enabled = true;

		yield return new WaitForSeconds(1);

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
		animator.SetBool("isCharging", false);
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
		canTakeDamage = false;

		yield return new WaitForSeconds(delayTime);

		canAttack = true;
		bossShieldSprite.enabled = false;
		enemyIsShacking = false;
		canTakeDamage = true;
	}

	/// <summary> this method stuns the enemy and after a N second delay unstuns that enemy</summary>
	private IEnumerator StunEnemy(float delayTime)
	{
		stunned = true;

		yield return new WaitForSeconds(delayTime / 2);

		// let the arms rotate again half way through the stun delay
		armsRotationController.shouldLookAtPlayer = true;

		yield return new WaitForSeconds(delayTime / 2);

		stunned = false;
		canAttack = true;

	}
	#endregion

}