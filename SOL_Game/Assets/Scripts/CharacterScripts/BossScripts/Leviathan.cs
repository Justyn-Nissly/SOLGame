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
		homingMissile,
		splitLeviathan,
		staticArmLeft,
		staticArmRight,
		spinningArmLeft,
		spinningArmRight,
		poison,
		healthCanvas;

	#endregion

	#region Private Variables
	private bool
		Moving = false; // flag for if the enemy is charging at the player


	private float
		enemyMoveSpeed = 15,
		timer = 5,
		maxTimer = 5,
		poisonTimer = .25f,
		poisonMaxTimer = .25f;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		//LeviathanEnemySpawner.AddNewEnemyID(gameObject.GetInstanceID());
		//LeviathanEnemySpawner.StartCheckingIfEnemiesDefeated();
	}

	private void OnEnable()
	{
		healthCanvas.SetActive(true);
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if(canAttack)
			PoisonLogic();

		if (timer < 0 && canAttack)
		{
			// shoot a missile if the player is a certain distance away
			if (Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position) >= 12)
			{
				HomingMissileAttack();
				timer = maxTimer;
			}
			else if (CheckIfInRange(splitLeviathanSpawnPointOne) && CheckIfInRange(splitLeviathanSpawnPointTwo))
			{
				SplitIntoTwoAttack();

				timer = maxTimer;
			}
		}
		else
		{
			timer -= Time.deltaTime;
		}

		
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// if the boss collided with the player damage the player
		if (collision.gameObject.CompareTag("Player") && Moving) // only damage the player when charging
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
		}
	}
	#endregion

	#region Utility Methods
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


	private void PoisonLogic()
	{
		if (poisonTimer < 0)
		{
			Destroy(Instantiate(poison, transform.position, new Quaternion(0, 0, 0, 0)), 5f);

			poisonTimer = poisonMaxTimer;
		}
		else
		{
			poisonTimer -= Time.deltaTime;
		}
	}

	private GameObject CreateTarget()
	{
		GameObject targetGameObject = new GameObject("target game object");
		targetGameObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;

		return targetGameObject;
	}

	private void HomingMissileAttack()
	{
		GameObject missile = Instantiate(homingMissile, missileSpawnPoint.position, missileSpawnPoint.rotation);

		LockOnProjectile lockOnProjectile = missile.GetComponent<LockOnProjectile>();
		if(lockOnProjectile != null)
		{
			//lockOnProjectile.target = CreateTarget();

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
		// create the first split leviathan
		GameObject splitLeviathanGO = Instantiate(splitLeviathan, splitLeviathanSpawnPointOne.position, new Quaternion(0, 0, 0, 0));

		SplitLeviathan splitLeviathanScript = splitLeviathanGO.GetComponent<SplitLeviathan>();

		if(splitLeviathanScript != null)
		{
			splitLeviathanScript.maxHealth = maxHealth;
			splitLeviathanScript.healthBar = healthBar;
			splitLeviathanScript.currentHealth = currentHealth;
			splitLeviathanScript.leviathan = this;
			splitLeviathanScript.lockOnProjectile.veerLeft = true;
			splitLeviathanScript.lockOnProjectile.target = CreateTarget();
		}

		// create the second split leviathan
		splitLeviathanGO = Instantiate(splitLeviathan, splitLeviathanSpawnPointTwo.position, new Quaternion(0, 0, 0, 0));

		splitLeviathanScript = splitLeviathanGO.GetComponent<SplitLeviathan>();

		if (splitLeviathanScript != null)
		{
			splitLeviathanScript.maxHealth = maxHealth;
			splitLeviathanScript.healthBar = healthBar;
			splitLeviathanScript.currentHealth = currentHealth;
			splitLeviathanScript.leviathan = this;
			splitLeviathanScript.lockOnProjectile.veerLeft = false;
			splitLeviathanScript.lockOnProjectile.target = CreateTarget();
		}


		// move the main leviathan off screen
		//aggro = true; // this disables the enemies movement
		canAttack = false;
		transform.position = offScreenLocation.position;
	}

	public void Merge(Vector3 mergePosition)
	{
		//aggro = false; // this re-enables the enemies movement
		transform.position = mergePosition;

		StartCoroutine(DoSpinAttack());
	}

	/// <summary> shield guardians overridden takeDamage() method, mainly for doing things at curtain health points</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			canAttack = false;
			Moving = false;
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
	#endregion

	#region Coroutines
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

	/// <summary> Moves a game object in the player direction (at the time this method is called) over N seconds </summary>
	public IEnumerator MoveInPlayersDirection()
	{
		// set flags
		Moving = true;
		canTakeDamage = false;

		// get the direction the player is in
		Vector3 targetDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;

		// charge at the player
		while (Moving == true)
		{
			transform.position += targetDirection * enemyMoveSpeed * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// set flags
		canTakeDamage = true;
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
	#endregion
}
