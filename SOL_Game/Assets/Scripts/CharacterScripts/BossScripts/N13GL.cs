﻿using System;
using System.Collections;
using System.Collections.Generic;
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
	public GameObject
		guardianArm; // The arm of the guardian
	public Sprite
		finalArmSprite,  // The sprite for the final guardian arm
		gunArmSprite,    // The sprite for the gun guardian arm
		hammerArmSprite, // The sprite for the hammer guardian arm
		nextArmSprite,   // The sprite for the next guardian arm to be spawned in
		shieldArmSprite, // The sprite for the shield guardian arm
		swordArmSprite;  // The sprite for the sword guardian arm
	public Transform
		upperLeftSpawnPointLimit,  // Used to get a random position between these two limits
		lowerRightSpawnPointLimit; // Used to get a random position between these two limits
	public bool
		typeIsChanged; // The current guardian type has been changed
	#endregion

	#region Shield Guardian
	public FloatValue
		damageToGive; // The bosses damage that is dealed to the player
	public Transform
		roomCenterTransform, // A transform at the center of the room used for some of the bosses movement calculation
		shootingPointLeft,   // The left point at which a blaster bullet will be instantiated
		shootingPointRight,  // The right point at which a blaster bullet will be instantiated
		LeftShootingLaneLimit,
		RightShootingLaneLimit;
	public EnemySpawner
		ShieldGuardianEnemySpawner; // A reference to the bosses enemy spawner
	public SpriteRenderer
		bossShieldSprite; // A reference to the bosses shield sprite renderer
	public AttactOrientationControllerEnemy
		armsRotationController; // This is a reference to the script that rotates the arms
	public Animator
		animator; // This is a reference to the animation controller
	#endregion

	#region Gun Guardian
	public GameObject
		gunGuardianArm; // The arm of the gun guardian
	public GameObject
		enemyToSpawn,      // The enemy type that gets spawned in when the ranged guardian gets to half health
		teleportAnimaiton; // ???

	public List<GameObject>
		teleporterPositions;

	public float
		maxTimeBetweenAttacks = 2f,
		minTimeBetweenAttacks = 1f;

	public EncounterManager
		encounterManager;
	#endregion

	#region Hammer Guardian
	public float
		attackTime,      // How long the guardian checks for the player before attacking
		restDelay,       // How long the guardian rests after an attack
		range,           // How far away the guardian will attack from
		attackDelayTime, // How long the guardian takes to attack
		defeatTimer;     // Time between receiving lethal damage and actual defeat
	public GameObject
		explosion, // Guardian explodes upon defeat
		shockWave, // Used to create shockwaves
		spikes;    // Used to create spikes
	public int
		maxSpikes,   // Spike lines spawned by end phase shockwaves
		phase,       // The guardian's battle phase
		phaseHealth; // The guardian's starting health at each phase
	public SpriteRenderer
		sprite; // The guardian's sprite
	public bool
		isAttacking; // Check if the guardian is attacking
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
	private bool
		moving = false;
	#endregion

	#region Shield Guardian
	private bool
		isCharging = false, // Flag for if the enemy is charging at the player
		isStunned = false, // Flag for if the enemy is stunned
		isHittingPlayer = false, // Flag fir is the enemy is colliding with the player right now
		canDoHalfHealthEvent = true,  // Flag so that this health event only happens once
		canDoQuarterHealthEvent = true,  // Flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true,  // Flag so that this health event only happens once
		enemyIsShacking = false, // For making the enemy look "mad"
		canShoot = true;

	private float
		enemyChargeSpeed = 15, // how fast the enemy charges at the player
		shackSpeed = 50.0f, //how fast it shakes
		shackAmount = .01f; //how much it shakes
	#endregion

	#region Gun Guardian
	private bool
		running = false,
		canSpawnEnemies = true;

	private float
		attackCountDownTimer;
	#endregion

	#region Hammer Guardian
	private HammerGuardianMovement
		guardianMove; // Reference the movement script
	private HammerGuardianWeakness
		weakness; // The guardian's weak spot
	private float
		attackDelay,      // Time left before an attack
		attackTimer,      // Time left to check if the guardian will attack
		restTimer,        // Time left resting
		weaknessRotation; // Used to rotate the weak point as the guardian moves
	private Vector2
		facing; // The general direction the guardian is facing
	private bool
		defeated;
	#endregion

	#region Sword Guardian
	#endregion

	#region Final Guardian
	#endregion

	#endregion

	// Unity Named Methods
	#region Main Methods
	// Start is called before the first frame update
	override public void Start()
	{
		allGuardianPatternTypes = Enum.GetValues(typeof(AttackPattern));
		currentGuardianPattern = AttackPattern.finalGuardianPattern;
		typeIsChanged = false;
		guardianPhase = 0;

		isAttacking = false;
		restTimer = 0.0f;
		attackTimer = attackTime;
		phase = 1;
		player = FindObjectOfType<Player>();
		//weakness = FindObjectOfType<HammerGuardianWeakness>();
		guardianMove = FindObjectOfType<HammerGuardianMovement>();
		//weakness.health = phaseHealth;
		canTakeDamage = false;
		defeated = false;
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

	/// <summary> Switch the currently displayed guardian arm </summary>
	public void SwitchArms()
	{
		guardianArm.GetComponent<SpriteRenderer>().sprite = nextArmSprite; // Take this and set this to "nextSprite" rather than just shieldSprite....It is 3:37...go to bed...
	}

	/// <summary> Take damage from the player </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound, bool fireBreathAttack = false)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			encounterManager.EndEncounter();
		}

	}

	/// <summary> starts moving the basilisk to a random point if the basilisk is not moving</summary>
	private void Move()
	{
		// so that you don't call the coroutine again if the boss is already running
		if (moving == false)
		{
			// Move the ranged guardian to the closest teleporter location
			moving = true;
			StartCoroutine(MoveOverSeconds(gameObject, GetRandomPositionBeweenLimits(), 1f));
		}
	}

	/// <summary> gets a random gameobject from the list of pop up positions</summary>
	private Vector2 GetRandomPositionBeweenLimits()
	{
		Vector2 randomPosition = new Vector2();

		// set the random psition to be in the range of the set limits
		randomPosition.x = Random.Range(upperLeftSpawnPointLimit.position.x, lowerRightSpawnPointLimit.position.x);
		randomPosition.y = Random.Range(upperLeftSpawnPointLimit.position.y, lowerRightSpawnPointLimit.position.y);

		return randomPosition;
	}
	#endregion

	#region Shield Guardian
	/// <summary> The attack pattern for the shield guardian </summary>
	public void ShieldGuardianAttackPattern()
	{
		Debug.Log("Shield Attack");
		base.FixedUpdate();

		/*// check if the boss should start charging at the player
		if (canShoot && canAttack && PlayerInShootingLane())
		{
			canAttack = false;
			canShoot = false;
			RandomlySetShootingPoint();
			animator.SetTrigger("shootBlaster");
		}*/
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
		// Make the character lower down overlap the character higher up
		//RenderInOrder();

		//The boss is defeated when its weak point takes sufficient damage
		if (defeated)
		{
			if (defeatTimer > 0.0f)
			{
				defeatTimer -= Time.deltaTime;
				if (defeatTimer > 0.3f && Mathf.Abs(0.3f - defeatTimer) < Time.deltaTime)
				{
					Instantiate(explosion, transform.position, Quaternion.identity);
				}
			}
			else
			{
				Instantiate(powerUp, transform.position, Quaternion.identity);
				Destroy(gameObject);
			}
		}
		else
		{
			// The weak point rotates with the guardian
			//RotateWeakness();

			// When attacking the player the guardian stops
			Targeting();
		}
	}

	/// <summary> Check if the player is in position to be attacked </summary>
	public bool AttackCheck()
	{
		facing = guardianMove.GetDirection();

		return (((guardianMove.targetAngle >= 45.0f && guardianMove.targetAngle <= 135.0f &&
				  facing == Vector2.up) ||
				 (guardianMove.targetAngle >= 225.0f && guardianMove.targetAngle <= 315.0f &&
				  facing == Vector2.down) ||
				 (guardianMove.targetAngle > 135.0f && guardianMove.targetAngle < 225.0f &&
				  facing == Vector2.left) ||
				 (guardianMove.targetAngle > 315.0f || guardianMove.targetAngle < 45.0f &&
				  facing == Vector2.right)) &&
				 (Vector2.Distance(transform.position, player.transform.position) < range));
	}

	/// <summary> Attack the player </summary>
	private void Attack()
	{
		// Stop moving and reset the rest and attack timers
		guardianMove.canMove = false;
		restTimer = restDelay;
		attackTimer = attackTime;

		// Phase 1 emits 1 shockwave
		if (phase == 1)
		{
			Instantiate(shockWave, (Vector2)transform.position + guardianMove.GetDirection() * range * 0.65f,
						Quaternion.identity);
		}
		// Phase 2 emits 2 shockwaves with lines of spikes
		else
		{
			// The guardian attacks on the left and right if it is facing up or down
			if (guardianMove.GetDirection() == Vector2.up || guardianMove.GetDirection() == Vector2.down)
			{
				Instantiate(shockWave, (Vector2)transform.position + Vector2.left * range * 0.65f,
							Quaternion.identity);
				Instantiate(shockWave, (Vector2)transform.position + Vector2.right * range * 0.65f,
							Quaternion.identity);

				// Send out spikes from the shockwaves
				for (int spike = 1; spike <= maxSpikes; spike++)
				{
					Instantiate(spikes, (Vector2)transform.position + Vector2.left * range * 0.65f,
								Quaternion.identity);
					Instantiate(spikes, (Vector2)transform.position + Vector2.right * range * 0.65f,
								Quaternion.identity);
				}
			}
			// The guardian attacks on the top and bottom if it is facing left or right
			else
			{
				Instantiate(shockWave, (Vector2)transform.position + Vector2.up * range * 0.65f,
							Quaternion.identity);
				Instantiate(shockWave, (Vector2)transform.position + Vector2.down * range * 0.65f,
							Quaternion.identity);

				// Send out spikes from the shockwaves
				for (int spike = 1; spike <= maxSpikes; spike++)
				{
					Instantiate(spikes, (Vector2)transform.position + Vector2.up * range * 0.65f,
								Quaternion.identity);
					Instantiate(spikes, (Vector2)transform.position + Vector2.down * range * 0.65f,
								Quaternion.identity);
				}
			}
		}
	}

	/// <summary> Attack if the player is in position and then rest </summary>
	private void Targeting()
	{
		if (isAttacking)
		{
			// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION
			//sprite.color = Color.red;
			// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION

			// The guardian pauses before striking
			guardianMove.canMove = false;
			if (attackDelay > 0.0f)
			{
				attackDelay -= Time.deltaTime;
			}
			else
			{
				Attack();
				restTimer = restDelay;
				isAttacking = false;
			}
		}
		else
		{
			// Check if the guardian is resting
			if (restTimer > 0.0f)
			{
				restTimer -= Time.deltaTime;
				// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION
				//sprite.color = Color.green;
				// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION
			}

			// The guardian pursues the player
			else
			{
				guardianMove.canMove = true;

				// If the player is in position to be attacked long enough the guardian attacks
				if (AttackCheck())
				{
					// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION
					//sprite.color = new Vector4(1.0f - (attackTimer / attackTime), 0.0f, attackTimer / attackTime, 1.0f);
					// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION

					attackTimer -= Time.deltaTime;
					if (attackTimer <= 0.0f)
					{
						isAttacking = true;
						attackDelay = attackDelayTime;
					}
				}
				else
				{
					attackTimer = attackTime;
				}
			}
		}
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
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 endingPosition, float seconds)
	{
		/*basiliskCollider.enabled = false; // disable the collider so it wont hit the player while under ground

		// trigger the under ground animation
		animator.SetTrigger("UnderGround");
		GetComponent<SpriteRenderer>().sortingOrder--; // change that the basilsik is rendered under other sprites like the player while under ground

		yield return new WaitForSeconds(secondsAboveGround / 2); // wait for the animation to fully play

		// spawn some bombs
		SpawnBomb();*/

		float elapsedTime = 0;
		Vector3 startingPosition = objectToMove.transform.position; // save the starting position

		// move the basilisk a little each frame based on how many seconds it should take to get the ending position
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPosition, endingPosition, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// trigger pop out of ground animation
		/*animator.SetTrigger("PopUp");
		GetComponent<SpriteRenderer>().sortingOrder++; // change the render layer
		basiliskCollider.enabled = true; // enable the collider so it will hit the player while not under ground

		// wait for N seconds above the ground
		yield return new WaitForSeconds(secondsAboveGround / 2);*/

		moving = false;
	}
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