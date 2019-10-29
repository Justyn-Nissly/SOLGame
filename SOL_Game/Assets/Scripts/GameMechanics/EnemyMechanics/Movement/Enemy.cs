/******************************************************************************/
/* File Name:      Enemy.cs                                                   */
/* Initial Author: Luke Koser                                                 */
/* File Created:   10/4/19                                                    */
/* Last Updated:   10/6/19                                                    */
/* Purpose:        Comprise the variables common among enemies in the game.   */
/******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Enemy : BaseCharacter
{
	#region Enums and Defined Constants
	#endregion

	#region Public Variables
	public string
		enemyName;
	public float
		attackDmg,   // Base damage from an intentional attack
		contactDmg,  // Base damage from making contact with the player
		aggroRange,  // How far away the enemy can detect the player
		followRange; // How far away the player must get for the enemy to deaggro
	public int
		health;
	public Rigidbody2D
		sprite;      // The enemy's sprite
	public bool
		aggro;
	#endregion

	#region Private Variables
	private Vector2
		playerPos;  // The player's position
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		sprite = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		playerPos = GameObject.Find("tempPlayer").transform.position;
		if (aggro == false && Vector2.Distance(transform.position, playerPos) <= aggroRange)
		{
			aggro = true;
		}
		else if (Vector2.Distance(transform.position, playerPos) >= followRange)
		{
			aggro = false;
		}
	}
	#endregion

	#region Utility Functions
	Enemy() : base()
	{
		health = 5;
	}

	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);

		Debug.Log("enemy health = " + health);

		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}
	#endregion

	#region Coroutines
	#endregion
}