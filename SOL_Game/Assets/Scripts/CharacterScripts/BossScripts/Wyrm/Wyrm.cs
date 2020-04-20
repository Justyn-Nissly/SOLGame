using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wyrm : Enemy
{

	#region Enums (Empty)
	private enum AttackType
	{
		breathAttack,
		meleeAttack,
		lazerBreathAttack,
	}
	#endregion

	#region Public Variables
	public FloatValue
		damageToGive; // The bosses damage that is dealt to the player
	public EnemySpawner
		enemySpawner; // A reference to the bosses enemy spawner
	public GameObject
		headGameobject,
		breathAttack,     // The prefab of a breath attack game object that creates a line of fire (more then one is used to create the fire breath attack)
		lazerBreathBlast;
	public AudioSource
		damaged,
		defeated,
		fireBreath,
		laser,
		roar,
		startingRoar;
	public ShockwaveAttack
		wyrmWave;

	public List<Transform>
		breathAttackTargets; // Reference to all points that a line of fire should go to

	public Transform
		StartMeleeAttackPoint; // is the players position is higher on the y axis than this transform the Wyrm will use a melee attack

	public EncounterManager
		encounterManager; // for loading the credits scene when this enemy is destroyed
	#endregion

	#region Private Variables
	private bool
		canDoHalfHealthEvent = true,         // Flag so that this health event only happens once
		canDoQuarterHealthEvent = true,      // Flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true, // Flag so that this health event only happens once
		enemyIsShacking = false,             // For making the enemy look "mad"
		enemiesCleared = false;

	private float
		attackCountdownTimer = 2,        // The countdown timer for the attacks
		attackIntervalTime = 2,          // The interval time before attacking again
		shackSpeed = 50.0f,              // How fast it shakes
		shackAmount = .01f,              // How much it shakes
		maxRoarInterval = 12.0f,
		minRoarInterval = 6.0f,
		roarTimer,
		wyrmWaveTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		roarTimer = Random.Range(maxRoarInterval, maxRoarInterval);
		wyrmWaveTimer = 100000.0f;

		// Add this boss to the enemy spawner, so that the doors out of the room only unlock when the boss and all spawned in enemies are dead
		enemySpawner.AddNewEnemyID(gameObject.GetInstanceID());
		enemySpawner.StartCheckingIfEnemiesDefeated();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if ((damaged.isPlaying || fireBreath.isPlaying || laser.isPlaying || roar.isPlaying || startingRoar.isPlaying || isDead) == false)
		{
			if ((roarTimer -= Time.deltaTime) <= 0.0f)
			{
				roarTimer = Random.Range(maxRoarInterval, maxRoarInterval);
				startingRoar.Play();
			}
		}

		if ((wyrmWaveTimer -= Time.deltaTime) <= 0.0f)
		{
			Instantiate(wyrmWave, transform.position + new Vector3(-5.05f, -5.6f, 0.0f), Quaternion.identity);
			Instantiate(wyrmWave, transform.position + new Vector3(5.05f, -5.6f, 0.0f), Quaternion.identity);
			wyrmWaveTimer = 100000.0f;
		}

		// Only try and do an attack if the timer countdown is zero and the enemy is allowed to attack
		if (attackCountdownTimer < 0 && canAttack && maxHealth.runTimeValue > 0)
		{
			DoAnAttack();
		}
		else if (isDead && canAttack) // trigger the death animation if the enemy is not attacking
		{
			// trigger death animations
			characterAnimator.SetBool("Dead", true);
		}
		else if ((attackCountdownTimer -= Time.deltaTime) <= 0.0f && (characterAnimator.GetBool("BreathAttack") ||
		          characterAnimator.GetBool("MeleeAttack")        ||  characterAnimator.GetBool("LazerBreathAttack")) == false)
		{
			canAttack = true;
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
		// do melee attack if the player is close enough
		if (player.transform.position.y > StartMeleeAttackPoint.position.y)
		{
			StartAttackAnimation(AttackType.meleeAttack);
		}
		// N% chance to do a lazer attack if below quarter health
		else if (canDoQuarterHealthEvent == false && Random.Range(0.0f, 3.0f) >= 1.0f)
		{
			StartAttackAnimation(AttackType.lazerBreathAttack);
		}
		// do default breath attack
		else
		{
			StartAttackAnimation(AttackType.breathAttack);
		}
	}

	/// <summary>  create a line of fire to each fire point in the list of breath target points </summary>
	public void BreathAttack()
	{
		fireBreath.Play();

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


	/// <summary> triggers the Wyrm's attack animations</summary>
	private void StartAttackAnimation(AttackType attackType)
	{
		// set flags for disabling attacking, enabling enemy shacking, take no damage, and stop the enemy from moving
		canAttack = false;
		enemyIsShacking = true;
		aggro = false;

		// trigger the right attack animation
		if (attackType == AttackType.meleeAttack)
		{
			// trigger the melee attack animation
			characterAnimator.SetBool("MeleeAttack", true);
			wyrmWaveTimer = 1.0f;
		}
		else if(attackType == AttackType.breathAttack)
		{
			// trigger the breath attack animation
			characterAnimator.SetBool("BreathAttack", true);
		}
		else
		{
			// trigger the laser breath attack animation
			characterAnimator.SetBool("LazerBreathAttack", true);
		}
	}

	/// <summary>stops any playing attack animations and returns to the idle animation (called with an animation event)</summary>
	private void EndAttackAnimation()
	{
		// stop any attack animations
		characterAnimator.SetBool("BreathAttack", false);
		characterAnimator.SetBool("MeleeAttack", false);
		characterAnimator.SetBool("LazerBreathAttack", false);

		if (isDead) // trigger the enemies death after the attack
		{
			// trigger death animations
			characterAnimator.SetBool("Dead", true);
		}
		else
		{
			// set flags for disabling attacking, enabling enemy shacking, take no damage, and stop the enemy from moving
			canAttack = true;
			enemyIsShacking = false;
			aggro = true;

			// reset the countdown timer
			attackCountdownTimer = attackIntervalTime;
		}
	}

	/// <summary>  create a line of fire to each fire point in the list of breath target points </summary>
	public void LazerBreathBlast()
	{
		laser.Play();

		// create a lazer breath attack (it will destroy itself when finished)
		Instantiate(lazerBreathBlast, headGameobject.transform.position, new Quaternion(0, 0, 0, 0));
	}

	/// <summary> deal melee damage (called in an animation event)</summary>
	public void DealMeleeDamage()
	{
		// use the heavy attack logic if its not null
		if (heavyMeleeAttackPosition != null && heavyMeleeDamageToGive != null)
		{
			MeleeAttack(heavyMeleeWeapon, heavyMeleeAttackPosition, heavyMeleeAttackRange, heavyMeleeDamageToGive, false);
		}
	}


	/// <summary> overridden takeDamage() method, mainly for doing things at curtain health points</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (maxHealth.runTimeValue > 0)
		{
			// play the damaged sound
			damaged.Play();

			// check if the enemy should start a health event
			if (maxHealth.runTimeValue <= maxHealth.initialValue * 0.75f && canDoThreeQuarterHealthEvent) // check if the enemy is at quarter health
			{
				canDoThreeQuarterHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(4)); // start health event
			}
			else if (maxHealth.runTimeValue <= maxHealth.initialValue * 0.5f && canDoHalfHealthEvent) // check if the enemy is at half health
			{
				canDoHalfHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(6)); // start health event
			}
			else if (maxHealth.runTimeValue <= maxHealth.initialValue * 0.25f && canDoQuarterHealthEvent) // check if the enemy is at quarter health
			{
				canDoQuarterHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(4)); // start health event
			}
		}
		else if (enemiesCleared == false)
		{
			if (FindObjectOfType<Enemy>() != null)
			{
				Enemy[] enemies = FindObjectsOfType<Enemy>();
				for (int i = 0; i < enemies.Length; i++)
				{
					if (enemies[i] != this)
					{
						enemies[i].GetComponent<Enemy>().canDropPowerUp = false;
						enemies[i].GetComponent<Enemy>().DisableColliders();
						enemies[i].GetComponent<Enemy>().TakeDamage(100, false);
					}
				}
			}
			isDead = enemiesCleared = true;
			defeated.Play();
		}
	}

	/// <summary> override the die method to do nothing (don't play dissolve effect)</summary>
	public override IEnumerator Die()
	{
		print("you won the game!!!");
		yield return null;
	}

	/// <summary> called after the death animations play</summary>
	public void DestroyGameObject()
	{
		Globals.wyrmDefeated = true;
		Destroy(gameObject);
		encounterManager.LoadCredits();
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