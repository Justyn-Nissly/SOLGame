using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Enemy : BaseCharacter
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	float
		amountHealed = 0;
	public Image
		healthBar;
    public string
        enemyName; // The enemy's name
	public float
		aggroRange,  // The max range where the enemy can detect the player
		duration,
		followRange, // How far away the player must get for the enemy to deaggro
		attackDmg,   // Base damage from an intentional attack
        contactDmg,  // Base damage from making contact with the player
		healPerLoop,
		healAmount,
		moveSpeed;   // Base movement speed
    public bool
        aggro; // The enemy has detected the player
    public Vector2[]
        patrol; // Enemy patrol points
    public Vector2
        playerPos; // Track the player's position
    public Rigidbody2D
		rb2d; // The enemy's rigidBody
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the enemy </summary>
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		healPerLoop = healAmount / duration;
	}

	/// <summary> Enemy activity depends on whether or not it has detected the player </summary>
	void FixedUpdate()
	{
		// Check if the player is close enough to aggro
		playerPos = GameObject.FindWithTag("Player").transform.position;
		if (aggro == false && Vector2.Distance(transform.position, playerPos) <= aggroRange)
		{
			aggro = true;
		}
		// Enemies that are not aggro heal over time
		else if (Vector2.Distance(transform.position, playerPos) >= followRange)
		{
			aggro = false;
			StartCoroutine(HealOverTimeCoroutine(healAmount, duration));
		}

		// Enemies attack the player only if aggroed
		if (aggro)
		{
			canAttack = true;
		}
		// DEBUG CODE; REMOVE LATER
		Debug.Log("enemy CurrentHealth = " + currentHealth.runTimeValue);
	}
	#endregion

	#region Utility Methods
	// Deal damage to the enemy
	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);
		SetHealth(currentHealth.runTimeValue / maxHealth.initialValue);

		Debug.Log("enemy CurrentHealth = " + currentHealth.runTimeValue);

		if (currentHealth.runTimeValue <= 0)
		{
			Destroy(gameObject);
		}
	}

	void SetHealth(float percentHelth)
	{
		healthBar.fillAmount = percentHelth;
	}
	#endregion

	#region Coroutines
	//Heal over time
	IEnumerator HealOverTimeCoroutine(float healAmount, float duration)
	{

		while (amountHealed < healAmount)
		{
			currentHealth.runTimeValue += healPerLoop;
			amountHealed += healPerLoop;
			SetHealth(currentHealth.runTimeValue);
			yield return new WaitForSeconds(1f);
		}
	}
	#endregion
}	