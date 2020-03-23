using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public List<Transform>
		breathAttackTargets; // Reference to all points that a line of fire should go to

	public Transform
		StartMeleeAttackPoint; // is the players position is higher on the y axis than this transform the Wyrm will use a melee attack
	#endregion

	#region Private Variables
	private bool
		canDoHalfHealthEvent = true,         // Flag so that this health event only happens once
		canDoQuarterHealthEvent = true,      // Flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true, // Flag so that this health event only happens once
		enemyIsShacking = false;             // For making the enemy look "mad"


	private float
		attackCountdownTimer = 2,        // The countdown timer for the attacks
		attackIntervalTime = 2,          // The interval time before attacking again
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
		if (attackCountdownTimer < 0 && canAttack && maxHealth.runTimeValue > 0)
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
		// do melee attack if the player is close enough
		if (player.transform.position.y > StartMeleeAttackPoint.position.y)
		{
			StartAttackAnimation(AttackType.meleeAttack);
		}
		// N% chance to do a lazer attack if below quarter health
		else if (Random.Range(1, 3) >= 1 && canDoQuarterHealthEvent == false)
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

		if (isDead)
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
			// check if the enemy should start a health event
			if (maxHealth.runTimeValue <= maxHealth.initialValue / 1.33333f && canDoThreeQuarterHealthEvent) // check if the enemy is at quarter health
			{
				canDoThreeQuarterHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(4)); // start health event
			}
			else if (maxHealth.runTimeValue <= maxHealth.initialValue / 2 && canDoHalfHealthEvent) // check if the enemy is at half health
			{
				canDoHalfHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(6)); // start health event
			}
			else if (maxHealth.runTimeValue <= maxHealth.initialValue / 4 && canDoQuarterHealthEvent) // check if the enemy is at quarter health
			{
				canDoQuarterHealthEvent = false; // this flag is here so this only can happen once
				StartCoroutine(StartHealthEvent(4)); // start health event
			}
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
		Destroy(gameObject);
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
