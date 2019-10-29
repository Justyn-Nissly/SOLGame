using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public float bulletSpeed = 15f;
	public int bulletDamage = 2;
	public Rigidbody2D bulletRigidbody;
	public GameObject impactEffect;
	public string ignoreTag; // lets the bullet ignore any game objects it hits with this tag
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	public void Start()
	{
		bulletRigidbody.velocity = transform.up * bulletSpeed;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(ignoreTag) == false)
		{
			if (collision.CompareTag("Player"))
			{
				Player player = collision.GetComponent<Player>();
				player.TakeDamage(bulletDamage);
				Debug.Log("players health = " + player.health);

				if (player.health <= 0)
				{
					Debug.Log("the player died");
				}
			}
			else if (collision.CompareTag("Enemy"))
			{
				Enemy enemy = collision.GetComponent<Enemy>();
				enemy.TakeDamage(bulletDamage);
				Debug.Log("enemy's health = " + enemy.health);

				if (enemy.health <= 0)
				{
					Destroy(collision.gameObject);
				}
			}

			// instantiate a impact effect then destroy the bullet and impact effect
			GameObject instance = Instantiate(impactEffect, transform.position, transform.rotation);
			Destroy(instance, 1f); // destroy the impact effect after N seconds
			Destroy(gameObject);
		}
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion

}
