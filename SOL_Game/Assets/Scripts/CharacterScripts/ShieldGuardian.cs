using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGuardian : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		damageToGive; // Damage dealt to the player
	public Transform
		roomCenterTransform,    // Used for some movement calculations
		shootingPointLeft,      // Left side firing position
		shootingPointRight,     // Right side firing position
		LeftShootingLaneLimit,  // How far to the left the guardian can fire
		RightShootingLaneLimit; // How far to the right the guardian can fire
	public EnemySpawner
		ShieldGuardianEnemySpawner; // Reference the guardian spawner
	public SpriteRenderer
		guardianShieldSprite; // Shield sprite renderer
	public AttactOrientationControllerEnemy
		armsRotationController; // Rotate the guardian's arms
	public Animator
		animator; // Reference the animation controller
	#endregion

	#region Private Variables
	private bool
		charging                     = false, // Charging at the player
		stunned                      = false, // Guardian is stunned
		hittingPlayer                = false, // Guardian is colliding with player
		canDoHalfHealthEvent         = true,  // Each health event occurs only once
		canDoQuarterHealthEvent      = true,  // Each health event occurs only once
		canDoThreeQuarterHealthEvent = true,  // Each health event occurs only once
		guardianIsShaking            = false, // Shaking gives an enraged appearance
		canShoot                     = true;  // Guardian can fire at the player
	private float
		guardianChargeSpeed = 15.0f,  // How fast the guardian charges at the player
		shakeSpeed          = 50.0f,  // How quickly the guardian shakes
		shakeAmount         =  0.01f; // Distance the guardian shakes
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize guardian </summary>
	public override void Start()
	{
		base.Start();

		ShieldGuardianEnemySpawner.AddNewEnemyID(gameObject.GetInstanceID());
		ShieldGuardianEnemySpawner.StartCheckingIfEnemiesDefeated();
	}

	/// <summary> Shield bash player as able </summary>
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		// Check if the guardian should charge the player
		if (canShoot && canAttack && PlayerInShootingLane())
		{
			canAttack = false;
			canShoot = false;
			RandomlySetShootingPoint();
			animator.SetTrigger("shootBlaster");
		}
		else if (charging == false && stunned == false && canAttack)
		{
			// Guardian charges at the player
			StartCoroutine(MoveInPlayersDirection());
		}

		// Guardian shakes if enraged
		if (guardianIsShaking)
		{
			transform.position = new Vector2(transform.position.x + (Mathf.Sin(Time.time * shakeSpeed) * shakeAmount), transform.position.y + (Mathf.Sin(Time.time * shakeSpeed) * shakeAmount));
		}
	}

	/// <summary> Shield bash collision detection </summary>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Deal shield bash damage to the player
		if (collision.gameObject.CompareTag("Player") && charging)
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)damageToGive.initialValue);
		}

		// Check if the guardian hit a wall
		else if (collision.gameObject.CompareTag("Wall"))
		{
			// Prevent the player from getting stuck between the guardian and a wall
			charging = false;
			if (hittingPlayer)
			{
				canAttack = false;
				StartCoroutine(MoveToCenter(1.0f));
			}
			// "Stun" the guardian if it hits a wall
			else
			{
				StartCoroutine(StunEnemy(3));
			}
		}
	}

	/// <summary> Check if guardian is colliding with player </summary>
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			hittingPlayer = true;
		}
	}

	/// <summary> Check if guardian is no longer colliding with player </summary>
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			hittingPlayer = false;
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Take damage and execute health events </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		// Guardian deactivates upon defeat
		if (currentHealth <= 0)
		{
			canAttack = false;
			charging  = false;
		}

		// Check if a health event should occur
		if (currentHealth <= maxHealth.initialValue / 1.33333f && canDoThreeQuarterHealthEvent) // At three quarters health
		{
			canDoThreeQuarterHealthEvent = false;
			StartCoroutine(StartHealthEvent(1));
		}
		else if (currentHealth <= maxHealth.initialValue / 2 && canDoHalfHealthEvent)           // At half health
		{
			canDoHalfHealthEvent = false;
			StartCoroutine(StartHealthEvent(2));
		}
		else if (currentHealth <= maxHealth.initialValue / 4 && canDoQuarterHealthEvent)        // At quarter health
		{
			canDoQuarterHealthEvent = false;
			StartCoroutine(StartHealthEvent(3));
		}
	}

	/// <summary> Trigger an event at certain health levels </summary>
	private void HealthEvent(int numOfEnemiesToSpawn)
	{
		// Start enemy spawn logic
		StartCoroutine(SpawningEnemiesActions(numOfEnemiesToSpawn + 2));

		// Increase the guardian's charge speed
		guardianChargeSpeed += 2.5f;

		// Spawn additional enemies
		StartCoroutine(ShieldGuardianEnemySpawner.SpawnInEnemies(false, numOfEnemiesToSpawn)); 
	}

	/// <summary> Start shooting (called in animator) </summary>
	public void ShootGuardianBlaster()
	{
		StartCoroutine(ShootGuardianBlasterCoroutine());
	}

	/// <summary> Choose side to fire from </summary>
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

	/// <summary> Check if player is in position for guardian to fire </summary>
	private bool PlayerInShootingLane()
	{
		return (player.transform.position.x >= LeftShootingLaneLimit.position.x  &&
		        player.transform.position.x <= RightShootingLaneLimit.position.x &&
		        player.transform.position.y <  transform.position.y);
	}
	#endregion

	#region Coroutines
	/// <summary> Fire a bullet </summary>
	private IEnumerator ShootGuardianBlasterCoroutine()
	{
		Transform bulletSpawnPoint = animator.GetBool("LeftBlaster") ? shootingPointLeft : shootingPointRight;
		GameObject bulletInstance  = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
		BulletLogic bulletLogic    = bulletInstance.GetComponent<BulletLogic>();
		bulletLogic.bulletDamage   = (int)rangedAttackDamageToGive.initialValue;

		yield return new WaitForSeconds(2);
		canShoot  = true;
		canAttack = true;
	}

	/// <summary> Move towards player </summary>
	public IEnumerator MoveInPlayersDirection()
	{
		// Prepare to charge the player with invincibility
		charging      = true;
		canTakeDamage = false;
		canAttack     = false;
		animator.SetBool("isCharging", true);

		yield return new WaitForSeconds(1);

		// Temporarily stop rotating the arms
		armsRotationController.shouldLookAtPlayer = false;
		guardianShieldSprite.enabled = true;

		// Get the direction towards the player
		Vector3 targetDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;

		// Charge the player
		while (charging)
		{
			transform.position += targetDirection * guardianChargeSpeed * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// Stop charging
		canTakeDamage                = true;
		guardianShieldSprite.enabled = false;
		animator.SetBool("isCharging", false);
	}

	/// <summary> Move to the room's center over time </summary>
	public IEnumerator MoveToCenter(float delayTime = 0)
	{
		float
			seconds     = 1, // Guardian's time taken moving to the room's center
			elapsedTime = 0; // Time that has passed

		// Delay before moving the guardian to the center
		yield return new WaitForSeconds(delayTime);

		// Save the starting position
		Vector3 startingPosition = transform.position;

		// Move the guardian to the ending position
		while (elapsedTime < seconds)
		{
			transform.position  = Vector3.Lerp(startingPosition, roomCenterTransform.position, (elapsedTime / seconds));
			elapsedTime        += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// Delay before being able to attack again
		yield return new WaitForSeconds(2.0f);
		canAttack = true;
	}

	/// <summary> Moves a game object to a center of the room position over N seconds </summary>
	public IEnumerator StartHealthEvent(int numberOfEnemiesToSpawn)
	{
		float seconds = 1;
		float elapsedTime = 0;
		Vector3 startingPosition = transform.position; // save the starting position

		// move the guardian to the center
		while (elapsedTime < seconds)
		{
			transform.position = Vector3.Lerp(startingPosition, roomCenterTransform.position, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// start the health event
		HealthEvent(numberOfEnemiesToSpawn);
	}

	/// <summary> Temporarily activate an impenetrable shield </summary>
	private IEnumerator SpawningEnemiesActions(float delayTime)
	{
		// Activate the shield and stop moving
		canAttack                    = false;
		guardianShieldSprite.enabled = true;
		guardianIsShaking            = true;
		canTakeDamage                = false;
		yield return new WaitForSeconds(delayTime);

		// Deactivate the shield and resume the attack pattern
		canAttack                    = true;
		guardianShieldSprite.enabled = false;
		guardianIsShaking            = false;
		canTakeDamage                = true;
	}

	/// <summary> Temporarily stun the guardian </summary>
	private IEnumerator StunEnemy(float delayTime)
	{
		// Stun the guardian
		stunned = true;
		yield return new WaitForSeconds(delayTime / 2);

		// Arms rotate halfway through the stun delay
		armsRotationController.shouldLookAtPlayer = true;
		yield return new WaitForSeconds(delayTime / 2);

		// Unstun the guardian
		stunned   = false;
		canAttack = true;

	}
	#endregion
}