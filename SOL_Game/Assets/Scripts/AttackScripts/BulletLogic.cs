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



	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag(ignoreTag) == false)
		{
			// An enemy bullet hits the player
			if (collision.gameObject.CompareTag("Player"))
			{
				Player player = collision.gameObject.GetComponent<Player>();
				player.TakeDamage(bulletDamage, false);
				// DEBUG CODE, REMOVE LATER
				Debug.Log("players CurrentHealth = " + player.currentHealth);
			}
			// An player bullet hits the enemy
			else if (collision.gameObject.CompareTag("Enemy"))
			{
				Enemy enemy = collision.gameObject.GetComponent<Enemy>();
				enemy.TakeDamage(bulletDamage, false);
				// DEBUG CODE, REMOVE LATER
				Debug.Log("enemy's CurrentHealth = " + enemy.currentHealth);
			}

			// The bullet impacts then gets destroyed
			DestroyBullet();
		}
	}
	#endregion

	#region Utility Methods
	private void DestroyBullet()
	{
		Destroy(Instantiate(impactEffect, transform.position, transform.rotation), 1.0f);
		Destroy(gameObject);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}