using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leviathan : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		damageToGive; // Boss's damage dealt to the player
	public Transform
		roomCenterTransform,         // Center of the room used for calculating movement
		offScreenLocation,           // Moves the Leviathan off screen
		splitLeviathanSpawnPointOne, // Where one part of the Leviathan starts upon splitting
		splitLeviathanSpawnPointTwo, // Where the other part of the Leviathan starts upon splitting
		missileSpawnPoint,           // Where a missile will fire from
		upperLeftSpawnPointLimit,    // Used to get a random position between two coordinates
		lowerRightSpawnPointLimit;   // Used to get a random position between two coordinates
	public EnemySpawner
		LeviathanEnemySpawner; // A reference to the bosses enemy spawner
	public GameObject
		homingMissile,    // Homing missile prefab
		splitLeviathan,   // Instantiate the split leviathan
		staticArmLeft,    // Reference static left arm
		staticArmRight,   // Reference static right arm
		spinningArmLeft,  // Reference spinning left arm
		spinningArmRight, // Reference spinning right arm
		poison,           // Parts along the Leviathan's poison trail
		fireBreath;     // Leviathan's flame breath

	public List<Transform>
		fireBreathTargets; // List of all points a line of fire should go to
	#endregion

	#region Private Variables
	private bool
		canDoHalfHealthEvent         = true,  // Each health event happens only once
		canDoQuarterHealthEvent      = true,  // Each health event happens only once
		canDoThreeQuarterHealthEvent = true,  // Each health event happens only once
		enemyIsShaking               = false; // Shaking gives the Leviathan an enraged appearance
	private float
		attackCountdownTimer =  5.0f,  // Timer for the attacks
		attackInterval   =  5.0f,  // Interval between attacks
		poisonCountdownTimer =  0.25f, // Timer for leaving poison
		poisonTimer  =  0.25f, // Interval between leaving the poison trail
		shakeAmount          =  0.01f, // How far it shakes
		shakeSpeed           = 50.0f,  // How fast it shakes
		missileMinRange      = 10.0f;  // Missiles won't fire if the player is within this range
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the Leviathan </summary>
	public override void Start()
	{
		base.Start();

		// Doors stay locked until all spawned enemies are defeated
		LeviathanEnemySpawner.AddNewEnemyID(gameObject.GetInstanceID());
		LeviathanEnemySpawner.StartCheckingIfEnemiesDefeated();
	}

	/// <summary> Attack the player and trail poison </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		// Leave a poison trail while aggroed
		if (aggro)
		{
			PoisonLogic();
		}

		// Leviathan attacks periodically
		if (attackCountdownTimer < 0 && canAttack && currentHealth > 0)
		{
			Attack();
		}
		else
		{
			attackCountdownTimer -= Time.deltaTime;
		}

		// Leviathan shakes if enraged
		if (enemyIsShaking)
		{
			transform.position = new Vector2(transform.position.x + (Mathf.Sin(Time.time * shakeSpeed) * shakeAmount),
			                                 transform.position.y + (Mathf.Sin(Time.time * shakeSpeed) * shakeAmount));
		}
	}

	/// <summary> Damage the player on contact </summary>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Check if the Leviathan has charged into the player
		if (collision.gameObject.CompareTag("Player"))
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)damageToGive.initialValue);
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Perform one of various attacks </summary>
	private void Attack()
	{
		// Fire a missile if the player is far enough away
		if (Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position,
		                     gameObject.transform.position) >= missileMinRange)
		{
			HomingMissileAttack();
			attackCountdownTimer = attackInterval;
		}
		// The Leviathan varies attacks when its health is below half
		else if (currentHealth <= maxHealth.initialValue * 0.5f)
		{
			// Perform a random attack (fire breath is more likely to be used)
			switch (Random.Range(1, 5))
			{
				case 1:
				case 2:
					FireBreath();
					break;
				case 3:
					StartCoroutine(DoSpinAttack());
					break;
				default:
					SplitIntoTwoAttack();
					break;
			}
			attackCountdownTimer = attackInterval;
		}
		// Split in two
		else
		{
			SplitIntoTwoAttack();
			attackCountdownTimer = attackInterval;
		}
	}

	/// <summary> Breathe fanned out fire streams </summary>
	private void FireBreath()
	{
		StartCoroutine(SpawningEnemiesActions(2.0f));

		foreach (Transform attackTarget in fireBreathTargets)
		{
			GameObject
				fireBreathGO = Instantiate(fireBreath, transform.position, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
				// Line of fire game object

			SpikeSurge
				fireBreathLogic = fireBreathGO.GetComponent<SpikeSurge>(); // The target position

			// Breathe streams of fire
			if (fireBreathLogic != null)
			{
				fireBreathLogic.target = attackTarget.position;
			}
		}
	}

	/// <summary> Check if an object is in the room </summary>
	private bool CheckIfInRange(Transform objectPosition)
	{
		return (objectPosition.position.x >= upperLeftSpawnPointLimit.position.x  &&
		        objectPosition.position.x <= lowerRightSpawnPointLimit.position.x &&
		        objectPosition.position.y >= lowerRightSpawnPointLimit.position.y &&
		        objectPosition.position.y <= upperLeftSpawnPointLimit.position.y);
	}

	/// <summary> Periodically trail poison </summary>
	private void PoisonLogic()
	{
		if (poisonCountdownTimer <= 0.0f)
		{
			Destroy(Instantiate(poison, transform.position, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)), 5.0f);
			poisonCountdownTimer = poisonTimer;
		}
		else
		{
			poisonCountdownTimer -= Time.deltaTime;
		}
	}

	/// <summary> Set the target location for the Leviathan's attack </summary>
	private GameObject SetTarget()
	{
		GameObject
			target = new GameObject("target game object"); // Where to aim the attack

		target.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;

		return target;
	}

	/// <summary> Fire a homing missile </summary>
	private void HomingMissileAttack()
	{
		GameObject
			missile = Instantiate(homingMissile, missileSpawnPoint.position, missileSpawnPoint.rotation);
			// Leviathan's missile object

		LockOnProjectile
			lockOnProjectile = missile.GetComponent<LockOnProjectile>();
			// Make the missile curve towards a set point

		// Set the target point and veer left or right
		if (lockOnProjectile != null)
		{
			lockOnProjectile.target   = GameObject.FindGameObjectWithTag("Player");
			lockOnProjectile.veerLeft = (Random.Range(0, 1) == 0);
		}
	}

	/// <summary> Split in two and arc towards the player </summary>
	private void SplitIntoTwoAttack()
	{
		// Prevent the Leviathan from splitting into the wall
		if (CheckIfInRange(splitLeviathanSpawnPointOne) && CheckIfInRange(splitLeviathanSpawnPointTwo))
		{
			// Create the split Leviathans
			CreateSplitLeviathan(splitLeviathanSpawnPointOne.transform.position, true);
			CreateSplitLeviathan(splitLeviathanSpawnPointTwo.transform.position, false);

			// Move the actual Leviathan off screen
			canAttack = false;
			transform.position = offScreenLocation.position;
		}
	}

	/// <summary> Splits the Leviathan </summary>
	private void CreateSplitLeviathan(Vector3 spawnPoint, bool veerLeft)
	{
		GameObject
			splitLeviathanGO = Instantiate(splitLeviathan, spawnPoint, new Quaternion(0, 0, 0, 0));
			// Split Leviathan game object

		SplitLeviathan
			splitLeviathanScript = splitLeviathanGO.GetComponent<SplitLeviathan>();
			// Control the split Leviathan

		// Split and arc towards the player
		if (splitLeviathanScript != null)
		{
			splitLeviathanScript.leviathan = this;
			splitLeviathanScript.lockOnProjectile.veerLeft = veerLeft;
			splitLeviathanScript.lockOnProjectile.target   = SetTarget();
		}
	}

	/// <summary> Rejoin the Leviathan and spin attack </summary>
	public void Merge(Vector3 mergePosition)
	{
		transform.position = mergePosition;
		StartCoroutine(DoSpinAttack());
	}

	/// <summary> Take damage and execute health-based events </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		// The Leviathan deactivates upon defeat
		if (currentHealth <= 0)
		{
			canAttack = false;
			aggro     = false;
		}
		else
		{
			// Start a health event if applicable (each happens only once)
			if (currentHealth <= maxHealth.initialValue * 0.75f && canDoThreeQuarterHealthEvent) // At three quarters health
			{
				canDoThreeQuarterHealthEvent = false;
				StartCoroutine(StartHealthEvent(1));
			}
			else if (currentHealth <= maxHealth.initialValue * 0.5f && canDoHalfHealthEvent)     // At half health
			{
				canDoHalfHealthEvent = false;
				StartCoroutine(StartHealthEvent(2));
			}
			else if (currentHealth <= maxHealth.initialValue * 0.25f && canDoQuarterHealthEvent) // At quarter health
			{
				canDoQuarterHealthEvent = false;
				StartCoroutine(StartHealthEvent(3));
			}
		}
	}

	/// <summary> Execute a health event </summary>
	private void HealthEvent(int numOfEnemiesToSpawn)
	{
		// Start spawning enemies
		StartCoroutine(SpawningEnemiesActions(numOfEnemiesToSpawn + 2));

		attackInterval--; // Attacks become more frequent

		// Spawn enemies
		StartCoroutine(LeviathanEnemySpawner.SpawnInEnemies(false, numOfEnemiesToSpawn));
	}
	#endregion

	#region Coroutines
	/// <summary> Temporarily disable attacking </summary>
	private IEnumerator SpawningEnemiesActions(float delayTime)
	{
		// Stop and become enraged
		canAttack      = false;
		enemyIsShaking = true;
		canTakeDamage  = false;
		aggro          = false;

		yield return new WaitForSeconds(delayTime);

		// Rage ends and movement resumes
		canAttack      = true;
		enemyIsShaking = false;
		canTakeDamage  = true;
		aggro          = true;
	}

	/// <summary> Moves the boss to the center then starts spawning in enemies </summary>
	public IEnumerator StartHealthEvent(int numberOfEnemiesToSpawn)
	{
		float
			seconds     = 1.0f, // Control enemy movement speed
			elapsedTime = 0.0f; // Time that has passed
		Vector3
			startingPosition = transform.position; // Save the starting position

		canAttack = false;

		// Move Leviathan to the center
		while (elapsedTime < seconds)
		{
			transform.position  = Vector3.Lerp(startingPosition, roomCenterTransform.position, (elapsedTime / seconds));
			elapsedTime        += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// Start the health event
		HealthEvent(numberOfEnemiesToSpawn);
	}

	/// <summary> Do an arm spin attack</summary>
	private IEnumerator DoSpinAttack()
	{
		// Start spinning the arms
		spinningArmLeft.SetActive(true);
		spinningArmRight.SetActive(true);
		staticArmLeft.SetActive(false);
		staticArmRight.SetActive(false);

		// After a delay stop spinning the arms
		yield return new WaitForSeconds(2.0f);
		spinningArmLeft.SetActive(false);
		spinningArmRight.SetActive(false);
		staticArmLeft.SetActive(true);
		staticArmRight.SetActive(true);

		// After a delay resume the normal attack pattern
		yield return new WaitForSeconds(1.0f);
		canAttack = true;
	}
	#endregion
}