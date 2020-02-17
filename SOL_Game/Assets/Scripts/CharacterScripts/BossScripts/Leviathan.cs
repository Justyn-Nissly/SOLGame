using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leviathan : Enemy
{

	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		damageToGive; // the bosses damage that is dealed to the player
	public Transform
		roomCenterTransform, // a transform at the center of the room used for some of the bosses movement calculation
		offScreenLocation,
		splitLeviathanSpawnPointOne,
		splitLeviathanSpawnPointTwo,
		missileSpawnPoint,
		upperLeftSpawnPointLimit, // used to get a random position between these two limits
		lowerRightSpawnPointLimit;
	public EnemySpawner
		LeviathanEnemySpawner; // a reference to the bosses enemy spawner
	public GameObject
		homingMissile, // the prefab of the homing missile that will get Instantiated
		splitLeviathan, // the prefab of the split Leviathan that will get Instantiated twice when the leviathan splits
		staticArmLeft, // reference to the static left arm
		staticArmRight, // reference to the static right arm
		spinningArmLeft, // reference to the spinning left arm
		spinningArmRight, // reference to the spinning right arm
		poison, // the prefab of a poison spot that will get Instantiated under the enemy
		breathAttack; // the prefab of a breath attack game object that creates a line of fire (more then one is used to create the fire breath attack)

	public List<Transform>
		breathAttackTargets; // referance to all points that a line of fire should go to

	#endregion

	#region Private Variables
	private bool
		canDoHalfHealthEvent = true, // flag so that this health event only happens once
		canDoQuarterHealthEvent = true, // flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true, // flag so that this health event only happens once
		enemyIsShacking = false; // for making the enemy look "mad"


	private float
		attackCountdownTimer = 5, // the countdown timer for the attacks
		attackIntervalTime = 5, // the interval time before attacking again
		poisonCountdownTimer = .25f, // the countdown timer for placing poison
		poisonIntervalTimer = .25f, // the interval time before placing a poison spot again
		requiredPlayerDistanceAway = 10, // the missile will not shoot if the player is to close
		shackSpeed = 50.0f, //how fast it shakes
		shackAmount = .01f; //how much it shakes
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		// add this boss to the enemy spawner, so that the doors out of the room only unlock when the boss and all spawned in enemies are dead
		LeviathanEnemySpawner.AddNewEnemyID(gameObject.GetInstanceID());
		LeviathanEnemySpawner.StartCheckingIfEnemiesDefeated();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		// place a poison spot if the enemy can attack
		if(aggro)
			PoisonLogic();

		// only try and do an attack if the timer countdown is zero and the enemy is allowed to attack
		if (attackCountdownTimer < 0 && canAttack && currentHealth > 0)
		{
			DoAnAttack();
		}
		else
		{
			attackCountdownTimer -= Time.deltaTime;
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
		if (collision.gameObject.CompareTag("Player")) // only damage the player when charging
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> this will do an attack based on the situation</summary>
	private void DoAnAttack()
	{
			// shoot a missile if the player is a certain distance away
			if (Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) >= requiredPlayerDistanceAway)
			{
				HomingMissileAttack();
				attackCountdownTimer = attackIntervalTime;
			}
			// check is the enemy has less than half health, if true it will do all attacks that are allowed below half health
			else if (currentHealth <= maxHealth.initialValue / 2) // do this attack if bellow half health
			{
				// do a random attack weighted to doing the breath attack more often
				switch (Random.Range(1, 5))
				{
					case 1:
					case 2:
						BreathAttack();
						break;
					case 3:
						StartCoroutine(DoSpinAttack());
						break;
					default:
						SplitIntoTwoAttack();
						break;

				}
				attackCountdownTimer = attackIntervalTime;
			}
			// do the default attack of splitting into two enemies
			else
			{
				SplitIntoTwoAttack();
				attackCountdownTimer = attackIntervalTime;
			}
	}



	/// <summary>  create a line of fire to each fire point in the list of breath target points </summary>
	private void BreathAttack()
	{
		foreach (Transform attactTarget in breathAttackTargets)
		{
			// create the line of fire game object
			GameObject breathAttackGO = Instantiate(breathAttack, transform.position, new Quaternion(0, 0, 0, 0));

			// set that line of fire's target position
			SpikeSurge breathAttackLogic = breathAttackGO.GetComponent<SpikeSurge>();
			if (breathAttackLogic != null)
			{
				breathAttackLogic.target = attactTarget.position;
			}
		}
	}

	/// <summary> this makes sure that the passed in objects position is in room </summary>
	private bool CheckIfInRange(Transform objectPosition)
	{
		bool IsInRange = false;

		if (objectPosition.position.x >= upperLeftSpawnPointLimit.position.x && objectPosition.position.x < lowerRightSpawnPointLimit.position.x &&
					objectPosition.position.y >= lowerRightSpawnPointLimit.position.y && objectPosition.position.y < upperLeftSpawnPointLimit.position.y)
		{
			IsInRange = true;
		}

		return IsInRange;
	}

	/// <summary> creates a poison spot every N seconds </summary>
	private void PoisonLogic()
	{
		if (poisonCountdownTimer < 0)
		{
			Destroy(Instantiate(poison, transform.position, new Quaternion(0, 0, 0, 0)), 5f);

			poisonCountdownTimer = poisonIntervalTimer;
		}
		else
		{
			poisonCountdownTimer -= Time.deltaTime;
		}
	}

	/// <summary> creates and returns an empty game object at the players current position</summary>
	private GameObject CreateTarget()
	{
		GameObject targetGameObject = new GameObject("target game object");
		targetGameObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;

		return targetGameObject;
	}

	/// <summary> do a homing missile attack (it will not do any thing if the player is too close)</summary>
	private void HomingMissileAttack()
	{
		// create the missile
		GameObject missile = Instantiate(homingMissile, missileSpawnPoint.position, missileSpawnPoint.rotation);

		// set target and random veer direction
		LockOnProjectile lockOnProjectile = missile.GetComponent<LockOnProjectile>();
		if(lockOnProjectile != null)
		{
			// make the projectile veer in a random direction
			if(Random.Range(0, 1) == 0)
			{
				lockOnProjectile.veerLeft = true;
			}
			else
			{
				lockOnProjectile.veerLeft = false;
			}

			// get the projectiles target
			lockOnProjectile.target = GameObject.FindGameObjectWithTag("Player");
		}
	}

	private void SplitIntoTwoAttack()
	{
		// if one of the split leviathans would be created outside the walls don't do anything
		if (CheckIfInRange(splitLeviathanSpawnPointOne) == false || CheckIfInRange(splitLeviathanSpawnPointTwo) == false)
		{
			return;
		}

		// create the first split leviathan
		CreateSplitLeviathan(splitLeviathanSpawnPointOne.transform.position, true);

		// create the second split leviathan
		CreateSplitLeviathan(splitLeviathanSpawnPointTwo.transform.position, false);

		// move the main leviathan off screen
		canAttack = false;
		transform.position = offScreenLocation.position;
	}

	/// <summary> Creates a Split Leviathan </summary>
	private void CreateSplitLeviathan(Vector3 spawnPoint, bool veerLeft)
	{
		// create the splitLeviathan game object
		GameObject splitLeviathanGO = Instantiate(splitLeviathan, spawnPoint, new Quaternion(0, 0, 0, 0));

		// get a reference to the split Leviathan's Script
		SplitLeviathan splitLeviathanScript = splitLeviathanGO.GetComponent<SplitLeviathan>();

		if (splitLeviathanScript != null) // null check
		{
			splitLeviathanScript.leviathan = this; // this is used for linking health and for merging the two split leviathan back together
			splitLeviathanScript.lockOnProjectile.veerLeft = veerLeft;
			splitLeviathanScript.lockOnProjectile.target = CreateTarget(); // set the target to the players current position
		}
	}

	/// <summary> merges the leviathan back together and does a spin attack</summary>
	public void Merge(Vector3 mergePosition)
	{
		transform.position = mergePosition;

		StartCoroutine(DoSpinAttack());
	}

	/// <summary> overridden takeDamage() method, mainly for doing things at curtain health points</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			canAttack = false;
			aggro = false;
		}
		else
		{
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

	/// <summary> this is every thing that happens a Health event point </summary>
	private void HealthEvent(int numOfEnemiesToSpawn)
	{
		// start doing boss spawning logic
		StartCoroutine(SpawningEnemiesActions(numOfEnemiesToSpawn + 2));

		attackIntervalTime--; // decrease the time between each attack

		// spawn in enemies
		StartCoroutine(LeviathanEnemySpawner.SpawnInEnemies(false, numOfEnemiesToSpawn));
	}
	#endregion

	#region Coroutines
	/// <summary> disable attacking then after a delay enable attacking again</summary>
	private IEnumerator SpawningEnemiesActions(float delayTime)
	{
		// set flags for disabling attacking, enabling enemy shacking, take no damage, and stop the enemy from moving
		canAttack = false;
		enemyIsShacking = true;
		canTakeDamage = false;
		aggro = false;

		yield return new WaitForSeconds(delayTime);

		canAttack = true;
		enemyIsShacking = false;
		canTakeDamage = true;
		aggro = true;
	}

	/// <summary> Moves the boss to the center then starts spawning in enemies </summary>
	public IEnumerator StartHealthEvent(int numberOfEnemiesToSpawn)
	{
		float seconds = 1;
		float elapsedTime = 0;
		Vector3 startingPosition = transform.position; // save the starting position
		
		canAttack = false;

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

	/// <summary> this does a spinning the arms type attack</summary>
	private IEnumerator DoSpinAttack()
	{
		spinningArmLeft.SetActive(true);
		spinningArmRight.SetActive(true);

		staticArmLeft.SetActive(false);
		staticArmRight.SetActive(false);

		yield return new WaitForSeconds(2);

		spinningArmLeft.SetActive(false);
		spinningArmRight.SetActive(false);

		staticArmLeft.SetActive(true);
		staticArmRight.SetActive(true);

		yield return new WaitForSeconds(1);

		canAttack = true;
	}
	#endregion
}
