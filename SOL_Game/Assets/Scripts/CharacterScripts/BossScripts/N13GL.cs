using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class N13GL : Enemy
{
	#region Enums
	public enum AttackPattern
	{
		finalGuardianPattern,
		shieldGuardianPattern,
		gunGuardianPattern,
		hammerGuardianPattern,
		swordGuardianPattern
	}
	#endregion

	#region Public Variables
	#region Custom Editor
	public bool
		isShield,
		isGun,
		isHammer,
		isSword,
		isFinal;
	#endregion

	#region Shared Variables
	public AttackPattern
		currentGuardianPattern; // The current guardian attack pattern type
	public Sprite
		finalArmSprite,  // The sprite for the final guardian arm
		gunArmSprite,    // The sprite for the gun guardian arm
		hammerArmSprite, // The sprite for the hammer guardian arm
		nextArmSprite,   // The sprite for the next guardian arm to be spawned in
		shieldArmSprite, // The sprite for the shield guardian arm
		swordArmSprite;  // The sprite for the sword guardian arm
	public bool
		typeIsChanged; // The current guardian type has been changed
	#endregion

	#region Shield Guardian
	public bool flag;
	public Animator
		animator; // This is a reference to the animation controller
	public EnemySpawner
		ShieldGuardianEnemySpawner; // A reference to the bosses enemy spawner
	public FloatValue
		damageToGive; // The bosses damage that is dealt to the player
	public GameObject
		shieldGuardianArm; // The arm of the shield guardian
	public SpriteRenderer
		bossShieldSprite; // A reference to the bosses shield sprite renderer
	public Transform
		roomCenterTransform, // a transform at the center of the room used for some of the bosses movement calculation
		shootingPointLeft,   // the left point at which a blaster bullet will be instantiated
		shootingPointRight;  // the right point at which a blaster bullet will be instantiated
	public bool
		isCharging = false, // Flag for if the enemy is charging at the player
		isStunned = false, // Flag for if the enemy is stunned
		isHittingPlayer = false, // Flag for is the enemy is colliding with the player right now
		canDoHalfHealthEvent = true,  // Flag so that this health event only happens once
		canDoQuarterHealthEvent = true,  // Flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true,  // Flag so that this health event only happens once
		enemyIsShacking = false, // For making the enemy look "mad"
		canShoot = true;  // Can the guardian shoot
	public float
		chargeSpeed,              // The speed at which the enemy will charge
		enemyChargeSpeed = 15,    // how fast the enemy charges at the player
		shackSpeed = 50.0f, // how fast it shakes
		shackAmount = .01f;  // how much it shakes
	#endregion

	#region Gun Guardian
	public GameObject
		gunGuardianArm; // The arm of the gun guardian
	public float
		maxTimeBetweenAttacks = 2f,
		minTimeBetweenAttacks = 1f;
	#endregion

	#region Hammer Guardian
	#endregion

	#region Sword Guardian
	#endregion

	#region Final Guardian
	public Animator
		armAnimator,   // The animator for the guardian's arm
		n13glAnimator; // The animator for N13GL
	#endregion

	#endregion

	#region Private Variables
	#region Shared Variables
	private Array
		allGuardianPatternTypes; // All possible guardian attack pattern types
	private System.Random
		randomGuardianPattern; // The number of the random guardian attack pattern to choose
	private Color32 guardianColour = new Color32(0x3C, 0x71, 0x6F, 0xFF);
	private int
		guardianPhase; // The current phase the guardian is in
	#endregion

	#region Shield Guardian

	#endregion

	#region Gun Guardian
	private float
		attackCountDownTimer;
	#endregion
	#region Hammer Guardian
	#endregion

	#region Sword Guardian
	#endregion

	#region Final Guardian
	#endregion

	#endregion

	// Unity Named Methods
	#region Main Methods
	// Start is called before the first frame update
	void Awake()
	{
		allGuardianPatternTypes = Enum.GetValues(typeof(AttackPattern));
		currentGuardianPattern = AttackPattern.finalGuardianPattern;
		typeIsChanged = false;
		guardianPhase = 0;
	}

	// Update is called once per frame
	void Update()
	{
		// Change to the next guardian type
		if (typeIsChanged == true)
		{
			ChangeAttackPattern();
			if(guardianPhase < allGuardianPatternTypes.Length)
			{
				guardianPhase += 1;
			}
		}

		// Check which attack pattern should be used
		switch (currentGuardianPattern)
		{
			case AttackPattern.finalGuardianPattern:
			{
				// Execute the final guardian attack pattern
				FinalGuardianAttackPattern();
				break;
			}
			case AttackPattern.shieldGuardianPattern:
			{
				ShieldGuardianAttackPattern();
				break;
			}
			case AttackPattern.gunGuardianPattern:
			{
				GunGuardianAttackPattern();
				break;
			}
			case AttackPattern.hammerGuardianPattern:
			{
				// Execute the hammer guardian attack pattern
				HammerGuardianAttackPattern();
				break;
			}
			case AttackPattern.swordGuardianPattern:
			{
				// Execute the sword guardian attack pattern
				SwordGuardianAttackPattern();
				break;
			}
		};
	}

	#region Shield Guardian
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Check if the boss collided with the player then deal damage the player
		if (collision.gameObject.CompareTag("Player") && isCharging) // only damage the player when charging
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)damageToGive.initialValue);
		}
		// Check if the enemy hit a wall
		else if (collision.gameObject.CompareTag("Wall"))
		{
			Debug.Log("WWWWWAAAAAALLLLLLLLLLLL");
			/// <summary>
			/// Check if the enemy is hitting the wall and the player is so move to the center
			/// (so that the player doesn't get stuck between the wall and the enemy) 
			/// <summary>
			if (isHittingPlayer)
			{
				isCharging = false;
				canAttack  = false;
				StartCoroutine(MoveToCenter(1f));
			}
			// "Stun" the enemy because he ran into a wall
			else
			{
				isCharging = false;
				StartCoroutine(StunEnemy(3));
			}
		}
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		// set flag for if the player is being collided with
		if (collision.gameObject.CompareTag("Player"))
		{
			isHittingPlayer = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		// set flag that the player is not being collided with
		if (collision.gameObject.CompareTag("Player"))
		{
			isHittingPlayer = false;
		}
	}
	#endregion

	#endregion

	#region Utility Methods

	#region Shared Methods
	public void ChangeAttackPattern()
	{
		currentGuardianPattern = (AttackPattern)allGuardianPatternTypes.GetValue(guardianPhase);
		switch (currentGuardianPattern)
		{
			case AttackPattern.finalGuardianPattern:
			{
				typeIsChanged = false;
				nextArmSprite = finalArmSprite;
				break;
			}
			case AttackPattern.shieldGuardianPattern:
			{
				// Set the attack type to the shield guardian
				typeIsChanged = false;
				nextArmSprite = shieldArmSprite;
				break;
			}
			case AttackPattern.gunGuardianPattern:
			{
				// Spawn in the gun guardian arm
				typeIsChanged = false;
				//gunGuardianArm.SetActive(true);
				nextArmSprite = gunArmSprite;
				/*shieldGuardianArm.GetComponent<_2dxFX_NewTeleportation2>().TeleportationColor = guardianColour;*/
				/*StartCoroutine(SpawnNewArm(gunGuardianArm, shieldGuardianArm));*/
				break;
			}
			case AttackPattern.hammerGuardianPattern:
			{
				nextArmSprite = hammerArmSprite;
				break;
			}
			case AttackPattern.swordGuardianPattern:
			{
				nextArmSprite = swordArmSprite;
				break;
			}
		};
		GetComponent<Animator>().SetTrigger("SwitchArm");
	}
	public void SwitchArms()
	{
		shieldGuardianArm.GetComponent<SpriteRenderer>().sprite = nextArmSprite; // Take this and set this to "nextSprite" rather than just shieldSprite....It is 3:37...go to bed...
	}
	#endregion

	#region Shield Guardian
	/// <summary> The attack pattern for the shield guardian </summary>
	public void ShieldGuardianAttackPattern()
	{
		Debug.Log("Shield Attack");
		base.FixedUpdate();

		// check if the boss should start charging at the player
		/*if (canShoot && canAttack && PlayerInShootingLane())
		{
			canAttack = false;
			canShoot = false;
			RandomlySetShootingPoint();
			animator.SetTrigger("shootBlaster");
		}
		*/
		if (isCharging == false && isStunned == false && canAttack)
		{
			// make the boss charge at the player
			StartCoroutine(MoveInPlayersDirection());
		}



		// if the enemy should be shacking start shacking the enemy
		if (enemyIsShacking)
		{
			transform.position = new Vector2(transform.position.x + (Mathf.Sin(Time.time * shackSpeed) * shackAmount),
											 transform.position.y + (Mathf.Sin(Time.time * shackSpeed) * shackAmount));
		}
	}
	/// <summary> shield guardians overridden takeDamage() method, mainly for doing things at curtain health points</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			canAttack = false;
			isCharging = false;
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

	#endregion

	#region Gun Guardian
	/// <summary> The attack pattern for the gun guardian </summary>
	public void GunGuardianAttackPattern()
	{
		Debug.Log("Gun Attack");
		// Execute the gun guardian attack pattern
		base.FixedUpdate();
		if (canAttack)
		{
			if (attackCountDownTimer <= 0)
			{
				Shoot(true);
				attackCountDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
			}
			else
			{
				attackCountDownTimer -= Time.deltaTime;
			}
		}
	}
	#endregion

	#region Hammer Guardian
	/// <summary> The attack pattern for the hammer guardian </summary>
	public void HammerGuardianAttackPattern()
	{
		Debug.Log("Hammer Attack");
	}
	#endregion

	#region Sword Guardian
	/// <summary> The attack pattern for the shield guardian </summary>
	public void SwordGuardianAttackPattern()
	{
		Debug.Log("Sword Attack");
	}
	#endregion

	#region Final Guardian
	/// <summary> The attack pattern for the shield guardian </summary>
	public void FinalGuardianAttackPattern()
	{
		Debug.Log("Final Attack");
	}
	#endregion

	#endregion

	#region Coroutines

	#region Shared Coroutines
	#endregion

	#region Shield Guardian
	/// <summary> Moves a game object in the player direction (at the time this method is called) over N seconds </summary>
	public IEnumerator MoveInPlayersDirection()
	{
		// set flags
		isCharging = true;
		canTakeDamage = false;
		canAttack = false;
		//animator.SetBool("isCharging", true);

		yield return new WaitForSeconds(1);

		bossShieldSprite.enabled = true;

		// get the direction the player is in
		Vector3 targetDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;

		// charge at the player
		while (isCharging == true)
		{
			transform.position += targetDirection * enemyChargeSpeed * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// set flags
		canTakeDamage = true;
		bossShieldSprite.enabled = false;
		//animator.SetBool("isCharging", false);
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
		isStunned = true;

		yield return new WaitForSeconds(delayTime / 2);


		yield return new WaitForSeconds(delayTime / 2);

		isStunned = false;
		canAttack = true;

	}
	#endregion

	#region Gun Guardian
	#endregion

	#region Hammer Guardian
	#endregion

	#region Sword Guardian
	#endregion

	#region Final Guardian
	/// <summary> Switch the primary attack arm to the arm of another guardian</summary>
	private IEnumerator SpawnNewArm(GameObject armToSpawn, GameObject armToDespawn)
	{
		float percentageComplete = 0; // The percentage of completion the teleport animation is at

		// Set the arm that is to be teleported in to invisible
		armToSpawn.GetComponent<_2dxFX_NewTeleportation2>()._Fade = 1;

		// Teleport the current arm away
		while (percentageComplete < 1)
		{
			armToDespawn.GetComponent<_2dxFX_NewTeleportation2>()._Fade = Mathf.Lerp(0f, 1f, percentageComplete);
			percentageComplete += Time.deltaTime;
			yield return null;
		}

		// Teleport the new arm in
		percentageComplete = 0;
		while (percentageComplete < 1)
		{
			armToSpawn.GetComponent<_2dxFX_NewTeleportation2>()._Fade = Mathf.Lerp(1f, 0f, percentageComplete);
			percentageComplete += Time.deltaTime;
			yield return null;
		}
		armToDespawn.SetActive(false);
		armToSpawn.GetComponent<_2dxFX_NewTeleportation2>()._Fade = 0;
	}
	#endregion

	#endregion
}