using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitLeviathan : Enemy
{

	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		damageToGive; // the bosses damage that is dealed to the player
	public Leviathan
		leviathan;
	public LockOnProjectile
		lockOnProjectile;
	public GameObject
		poison;
	#endregion

	#region Private Variables
	private bool
		Moving = false;

	private float
		enemyChargeSpeed = 5, // how fast the enemy charges at the player
		poisonTimer = .10f,
		poisonMaxTimer = .10f;

	#endregion

	// Unity Named Methods
	#region Main Methods
	private void Awake()
	{
		lockOnProjectile.target = CreateTarget();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		// creates a poison spot every N seconds
		PoisonLogic();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// if the boss collided with the player damage the player
		if (collision.gameObject.CompareTag("Player")) // only damage the player when charging
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
		}
		if(collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<SplitLeviathan>() != null)
		{
			leviathan.Merge(transform.position);

			Destroy(lockOnProjectile.target);
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}
	#endregion

	#region Utility Methods
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

	/// <summary> shield guardians overridden takeDamage() method, mainly for doing things at curtain health points</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		//base.TakeDamage(damage, playSwordImpactSound);
		leviathan.TakeDamage(damage, playSwordImpactSound);
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
	#endregion
}
