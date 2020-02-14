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
		splitLeviathanSpawnPointTwo;
	public EnemySpawner
		LeviathanEnemySpawner; // a reference to the bosses enemy spawner
	public GameObject
		splitLeviathan;

	#endregion

	#region Private Variables
	private bool
		Moving = false, // flag for if the enemy is charging at the player
		stunned = false, // flag for if the enemy is stunned
		hittingPlayer = false, // flag fir is the enemy is colliding with the player right now
		canDoHalfHealthEvent = true, // flag so that this health event only happens once
		canDoQuarterHealthEvent = true, // flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true, // flag so that this health event only happens once
		enemyIsShacking = false; // for making the enemy look "mad"

	private float
		enemyChargeSpeed = 15, // how fast the enemy charges at the player
		shackSpeed = 50.0f, //how fast it shakes
		shackAmount = .01f; //how much it shakes
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		//LeviathanEnemySpawner.AddNewEnemyID(gameObject.GetInstanceID());
		//LeviathanEnemySpawner.StartCheckingIfEnemiesDefeated();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

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
	private void SplitIntoTwo()
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
		}


		// move the main leviathan off screen
		transform.position = offScreenLocation.position;
	}

	public void Merge(Vector3 mergePosition)
	{
		transform.position = mergePosition;
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

		SplitIntoTwo();
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
			transform.position += targetDirection * enemyChargeSpeed * Time.deltaTime;
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
