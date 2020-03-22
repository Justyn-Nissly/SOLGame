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
		leviathan; // reference to the main leviathan script
	public LockOnProjectile
		lockOnProjectile; // reference to the script that moves this split leviathan
	public GameObject
		poison; // the prefab of a poison spot that will get Instantiated under the enemy
	#endregion

	#region Private Variables
	private float
		poisonTimer = .10f, // the countdown timer for placing poison
		poisonMaxTimer = .10f; // the interval time before placing a poison spot again

	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		//base.FixedUpdate();

		// creates a poison spot every N seconds
		PoisonLogic();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// if the boss collided with the player damage the player
		if (collision.gameObject.CompareTag("Player")) // only damage the player when charging
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)damageToGive.initialValue);
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
	/// <summary> creates a poison spot every N seconds </summary>
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

	/// <summary>overridden takeDamage() method, mainly for dealing damage to the main leviathan not this split leviathan</summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		// deal damage to the main leviathan not this split leviathan
		if(leviathan != null)
		{
			leviathan.TakeDamage(damage, playSwordImpactSound);
		}
		// destroy this split leviathan if the main leviathan does not exist
		else
		{
			Destroy(gameObject);
		}

	}
	#endregion

	#region Coroutines
	#endregion
}
