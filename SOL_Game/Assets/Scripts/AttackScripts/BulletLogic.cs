using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		bulletSpeed = 15f;
	public int
		bulletDamage;
	public Rigidbody2D
		bulletRigidbody;
	public GameObject
		impactEffect; // Bullet impact visual
	public string
		ignoreTag; // The bullet ignores game objects with this tag
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set the bullet to move </summary>
	public void Start()
	{
		bulletRigidbody.velocity = transform.up * bulletSpeed;
	}

	/// <summary> The bullet makes contact with an object </summary>
	public void OnTriggerEnter2D(Collider2D collision)
	{
		// The bullet cannot cause friendly fire damage
		if (collision.CompareTag(ignoreTag) == false)
		{
			// An enemy bullet hits the player
			if (collision.CompareTag("Player"))
			{
				Player player = collision.GetComponent<Player>();
				player.TakeDamage(bulletDamage, false);
				// DEBUG CODE, REMOVE LATER
				Debug.Log("players CurrentHealth = " + player.currentHealth);
			}
			else if (collision.CompareTag("Enemy"))
			{
				Enemy enemy = collision.GetComponent<Enemy>();
				enemy.TakeDamage(bulletDamage, false);
				// DEBUG CODE, REMOVE LATER
				Debug.Log("enemy's CurrentHealth = " + enemy.currentHealth);
			}

			// The bullet impacts then gets destroyed
			Destroy(Instantiate(impactEffect, transform.position, transform.rotation), 1.0f);
			Destroy(gameObject);
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}