using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		bulletSpeed = 15f; // How fast the bullet moves
	public int
		bulletDamage; // Damage inflicted by the bullet on impact
	public Rigidbody2D
		bulletRigidbody; // Apply physics to the bullet
	public GameObject
		impactEffect; // Bullet impact visual
	public string
		ignoreTag; // The bullet passes through game objects with this tag
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set the bullet to move </summary>
	public void Start()
	{
		if(bulletRigidbody != null)
		{
			bulletRigidbody.velocity = transform.up * bulletSpeed;
		}
	}

	/// <summary> Handle bullet collisions </summary>
	public void OnCollisionEnter2D(Collision2D collision)
	{
		// Pass through objects with the ignore tag
		if (collision.gameObject.CompareTag(ignoreTag) == false)
		{
			// Enemy bullet hits the player
			if (collision.gameObject.CompareTag("Player"))
			{
				Player player = collision.gameObject.GetComponent<Player>();
				player.TakeDamage(bulletDamage, false);
				// DEBUG CODE, REMOVE LATER
				// DEBUG CODE, REMOVE LATER
				// DEBUG CODE, REMOVE LATER
				Debug.Log("players CurrentHealth = " + player.currentHealth);
				// DEBUG CODE, REMOVE LATER
				// DEBUG CODE, REMOVE LATER
				// DEBUG CODE, REMOVE LATER
			}
			// Player bullet hits an enemy
			else if (collision.gameObject.CompareTag("Enemy"))
			{
				Enemy enemy = collision.gameObject.GetComponent<Enemy>();
				enemy.TakeDamage(bulletDamage, false);
				// DEBUG CODE, REMOVE LATER
				// DEBUG CODE, REMOVE LATER
				// DEBUG CODE, REMOVE LATER
				Debug.Log("enemy's CurrentHealth = " + enemy.currentHealth);
				// DEBUG CODE, REMOVE LATER
				// DEBUG CODE, REMOVE LATER
				// DEBUG CODE, REMOVE LATER
			}
		}
			
		// The bullet explodes on impact
		DestroyBullet();

	}
	#endregion

	/// <summary> Make the bullet explode </summary>
	#region Utility Methods
	private void DestroyBullet()
	{
		// Destroy the bullet after playing its explosion animation
		Destroy(Instantiate(impactEffect, transform.position, transform.rotation), 1.0f);
		Destroy(gameObject);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}