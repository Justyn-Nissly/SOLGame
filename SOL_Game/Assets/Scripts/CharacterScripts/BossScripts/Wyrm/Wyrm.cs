using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wyrm : Enemy
{

	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		damageToGive; // The bosses damage that is dealt to the player
	public EnemySpawner
		enemySpawner; // A reference to the bosses enemy spawner
	public GameObject
		headGameobject,
		breathAttack,     // The prefab of a breath attack game object that creates a line of fire (more then one is used to create the fire breath attack)
		bigBreathBlast;

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
		attackCountdownTimer = 6,        // The countdown timer for the attacks
		attackIntervalTime = 5,          // The interval time before attacking again
		shackSpeed = 50.0f,              // How fast it shakes
		shackAmount = .01f;              // How much it shakes
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		// Add this boss to the enemy spawner, so that the doors out of the room only unlock when the boss and all spawned in enemies are dead
		enemySpawner.AddNewEnemyID(gameObject.GetInstanceID());
		enemySpawner.StartCheckingIfEnemiesDefeated();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

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
		if(canDoQuarterHealthEvent == false) // if the wyrm is below quarter health chance to do a special attack
		{
			if (Random.Range(1, 3) == 1)
			{
				BreathAttack();
			}
			else
			{
				BigBreathBlast(2);
			}
		}
		else
		{
			BreathAttack();
		}

		attackCountdownTimer = attackIntervalTime;
	}

	/// <summary>  create a line of fire to each fire point in the list of breath target points </summary>
	private void BreathAttack()
	{
		StartCoroutine(SpawningEnemiesActions(2));

		foreach (Transform attactTarget in breathAttackTargets)
		{
			// create the line of fire game object
			GameObject breathAttackGO = Instantiate(breathAttack, headGameobject.transform.position, new Quaternion(0, 0, 0, 0));

			// set that line of fire's target position
			SpikeSurge breathAttackLogic = breathAttackGO.GetComponent<SpikeSurge>();
			if (breathAttackLogic != null)
			{
				breathAttackLogic.target = attactTarget.position;
			}
		}
	}

	/// <summary>  create a line of fire to each fire point in the list of breath target points </summary>
	private void BigBreathBlast(float duration)
	{
		StartCoroutine(SpawningEnemiesActions(duration));

		// create a lazer breath attack (it will destroy itself when finished)
		Instantiate(bigBreathBlast, headGameobject.transform.position, new Quaternion(0, 0, 0, 0));
	}


	/// <summary> overridden takeDamage() method, mainly for doing things at curtain health points</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound, bool fireBreathAttack = false)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			canAttack = false;
			aggro = false;

			print("you won the game!!!");
		}
		else
		{
			// check if the enemy should start a health event
			if (currentHealth <= maxHealth.initialValue / 1.33333f && canDoThreeQuarterHealthEvent) // check if the enemy is at quarter health
			{
				canDoThreeQuarterHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(4)); // start health event
			}
			else if (currentHealth <= maxHealth.initialValue / 2 && canDoHalfHealthEvent) // check if the enemy is at half health
			{
				canDoHalfHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(6)); // start health event
			}
			else if (currentHealth <= maxHealth.initialValue / 4 && canDoQuarterHealthEvent) // check if the enemy is at quarter health
			{
				canDoQuarterHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(4)); // start health event
			}
		}
	}


	/// <summary> this is every thing that happens a Health event point </summary>
	private void HealthEvent(int numOfEnemiesToSpawn)
	{
		// start doing boss spawning logic
		StartCoroutine(SpawningEnemiesActions(numOfEnemiesToSpawn + 2));

		// spawn in enemies
		StartCoroutine(enemySpawner.SpawnInEnemies(false, numOfEnemiesToSpawn));
	}
	#endregion

	#region Coroutines
	/// <summary> disable attacking then after a delay enable attacking again</summary>
	private IEnumerator SpawningEnemiesActions(float delayTime)
	{
		// set flags for disabling attacking, enabling enemy shacking, take no damage, and stop the enemy from moving
		canAttack = false;
		enemyIsShacking = true;
		//canTakeDamage = false;
		aggro = false;

		yield return new WaitForSeconds(delayTime);

		canAttack = true;
		enemyIsShacking = false;
		//canTakeDamage = true;
		aggro = true;
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
	#endregion
}
