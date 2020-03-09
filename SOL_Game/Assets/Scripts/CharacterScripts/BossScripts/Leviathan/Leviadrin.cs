using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leviadrin : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public SnakeManager
		snakeManager;
	public FloatValue
		damageToGive; // The bosses damage that is dealt to the player
	public EnemySpawner
		LeviathanEnemySpawner; // A reference to the bosses enemy spawner
	public GameObject
		poison,           // The prefab of a poison spot that will get Instantiated under the enemy
		breathAttack;     // The prefab of a breath attack game object that creates a line of fire (more then one is used to create the fire breath attack)

	public List<Transform>
		breathAttackTargets; // Reference to all points that a line of fire should go to
	#endregion

	#region Private Variables
	private bool
		canDoHalfHealthEvent = true,         // Flag so that this health event only happens once
		canDoQuarterHealthEvent = true,      // Flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true, // Flag so that this health event only happens once
		enemyIsShacking = false;             // For making the enemy look "mad"


	private float
		attackCountdownTimer = 5,        // The countdown timer for the attacks
		attackIntervalTime = 5,          // The interval time before attacking again
		poisonCountdownTimer = .25f,     // The countdown timer for placing poison
		poisonIntervalTimer = .25f,      // The interval time before placing a poison spot again
		shackSpeed = 50.0f,              // How fast it shakes
		shackAmount = .01f;              // How much it shakes
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		// Add this boss to the enemy spawner, so that the doors out of the room only unlock when the boss and all spawned in enemies are dead
		LeviathanEnemySpawner.AddNewEnemyID(gameObject.GetInstanceID());
		LeviathanEnemySpawner.StartCheckingIfEnemiesDefeated();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		snakeManager.canMove = canAttack;

		// Place a poison spot if the enemy can attack
		if (aggro)
			PoisonLogic();

		// Only try and do an attack if the timer countdown is zero and the enemy is allowed to attack
		if (attackCountdownTimer < 0 && canAttack && currentHealth > 0)
		{
			DoAnAttack();
		}
		else
		{
			attackCountdownTimer -= Time.deltaTime;
		}

		// If the enemy should be shacking start shacking the enemy
		if (enemyIsShacking)
		{
			transform.position = new Vector2(transform.position.x + (Mathf.Sin(Time.time * shackSpeed) * shackAmount), transform.position.y + (Mathf.Sin(Time.time * shackSpeed) * shackAmount));
		}

	}
	#endregion

	#region Utility Methods
	/// <summary> this will do an attack based on the situation </summary>
	private void DoAnAttack()
	{
		// check is the enemy has less than half health, if true it will do all attacks that are allowed below half health
		if (currentHealth <= maxHealth.initialValue / 2) // do this attack if bellow half health
		{
			// do a random attack weighted to doing the breath attack more often
			switch (Random.Range(1, 4))
			{
				case 1:
				case 2:
					BreathAttack();
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
		if (snakeManager.SnakeIsSplit)
		{
			return;
		}

		StartCoroutine(SpawningEnemiesActions(2));

		foreach (Transform attactTarget in breathAttackTargets)
		{
			// create the line of fire game object
			GameObject breathAttackGO = Instantiate(breathAttack, snakeManager.FullSnake.bodyParts[0].position, new Quaternion(0, 0, 0, 0));

			// set that line of fire's target position
			SpikeSurge breathAttackLogic = breathAttackGO.GetComponent<SpikeSurge>();
			if (breathAttackLogic != null)
			{
				breathAttackLogic.target = attactTarget.position;
			}
		}
	}

	/// <summary> creates a poison spot every N seconds </summary>
	private void PoisonLogic()
	{
		if (poisonCountdownTimer < 0)
		{
			Destroy(Instantiate(poison, snakeManager.HalfSnakeBody.bodyParts[snakeManager.HalfSnakeBody.bodyParts.Count -1].position, new Quaternion(0, 0, 0, 0)), 5f);
			Destroy(Instantiate(poison, snakeManager.HalfSnakeHead.bodyParts[snakeManager.HalfSnakeHead.bodyParts.Count - 1].position, new Quaternion(0, 0, 0, 0)), 5f);

			poisonCountdownTimer = poisonIntervalTimer;
		}
		else
		{
			poisonCountdownTimer -= Time.deltaTime;
		}
	}

	private void SplitIntoTwoAttack()
	{
		snakeManager.SplitTheSnake = true;
	}

	/// <summary> overridden takeDamage() method, mainly for doing things at curtain health points</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			canAttack = false;
			aggro = false;
			snakeManager.canMove = false;
		}
		else
		{
			// check if the enemy should start a health event
			if (currentHealth <= maxHealth.initialValue / 1.33333f && canDoThreeQuarterHealthEvent) // check if the enemy is at quarter health
			{
				canDoThreeQuarterHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(1)); // start health event
				IncreaseSnakeMovementSpeed(2);
			}
			else if (currentHealth <= maxHealth.initialValue / 2 && canDoHalfHealthEvent) // check if the enemy is at half health
			{
				canDoHalfHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(2)); // start health event
				IncreaseSnakeMovementSpeed(2);
			}
			else if (currentHealth <= maxHealth.initialValue / 4 && canDoQuarterHealthEvent) // check if the enemy is at quarter health
			{
				canDoQuarterHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(1)); // start health event
				IncreaseSnakeMovementSpeed(-2);
			}
		}
	}

	/// <summary> changes the snakes movement speed by N </summary>
	private void IncreaseSnakeMovementSpeed(int increaseAmount)
	{
		snakeManager.FullSnake.movementSpeed += increaseAmount;
		snakeManager.HalfSnakeBody.movementSpeed += increaseAmount;
		snakeManager.HalfSnakeHead.movementSpeed += increaseAmount;
	}

	/// <summary> this is every thing that happens a Health event point </summary>
	private void HealthEvent(int numOfEnemiesToSpawn)
	{
		// start doing boss spawning logic
		StartCoroutine(SpawningEnemiesActions(numOfEnemiesToSpawn + 2));
		snakeManager.SnakeIsSplit = false;

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
		snakeManager.canMove = false;

		foreach(Rigidbody2D rigidbody2D in GetComponentsInChildren<Rigidbody2D>())
		{
			rigidbody2D.isKinematic = true;
		}

		yield return new WaitForSeconds(delayTime);

		canAttack = true;
		enemyIsShacking = false;
		canTakeDamage = true;
		aggro = true;
		snakeManager.canMove = true;
		foreach (Rigidbody2D rigidbody2D in GetComponentsInChildren<Rigidbody2D>())
		{
			rigidbody2D.isKinematic = false;
		}
	}

	/// <summary> Moves the boss to the center then starts spawning in enemies </summary>
	public IEnumerator StartHealthEvent(int numberOfEnemiesToSpawn)
	{
		float seconds = 1;

		canAttack = false;

		// stop the snake
		yield return new WaitForSeconds(seconds);


		// start the health event
		HealthEvent(numberOfEnemiesToSpawn);
	}

	protected override IEnumerator StartBlinking()
	{
		List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
		float
			timer = 0.5f;  // Time to blink after taking damage

		canTakeDamage = false; // Character becomes temporarily invulnerable

		// Toggle sprite's visibility to make it blink
		while (timer >= 0.0f)
		{
			foreach (SpriteRenderer spriteRenderer in spriteRenderers)
			{
				spriteRenderer.enabled = !spriteRenderer.enabled;
			}
			
			timer -= Time.deltaTime + 0.1f;
			yield return new WaitForSeconds(0.1f);
		}

		// Ensure the sprite is visible and the character can take damage after blinking stops
		foreach (SpriteRenderer spriteRenderer in spriteRenderers)
		{
			spriteRenderer.enabled = true;
		}
		canTakeDamage = true;
	}
	#endregion
}
