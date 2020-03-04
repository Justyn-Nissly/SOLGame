using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitLeviathan : Enemy
{

	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		damageToGive; // Boss's damage to the player
	public Leviathan
		leviathan; // Reference the Leviathan
	public LockOnProjectile
		lockOnProjectile; // Moves the split leviathan
	public GameObject
		poison; // A small poison puddle
	#endregion

	#region Private Variables
	private float
		poisonTimer    = 0.10f, // Time until dropping poison
		poisonMaxTimer = 0.10f; // Interval between poison drops
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Leave a poison trail </summary>
	public override void FixedUpdate()
	{
		PoisonLogic();
	}

	/// <summary> Damage the player on contact and merge the Leviathan </summary>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Damage the player only while charging
		if (collision.gameObject.CompareTag("Player"))
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)damageToGive.initialValue);
		}
		// Merge the Leviathan back into one
		if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<SplitLeviathan>() != null)
		{
			leviathan.Merge(transform.position);
			Destroy(lockOnProjectile.target);
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Leave a poison trail </summary>
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

	/// <summary> Damage the merged Leviathan not the split leviathan </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		if(leviathan != null)
		{
			leviathan.TakeDamage(damage, playSwordImpactSound);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}