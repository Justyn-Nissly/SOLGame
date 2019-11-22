using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Enemy : BaseCharacter
{
	#region Enums
	#endregion

	#region Public Variables
	float amountHealed = 0;
	public Image healthBar;
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
        patrol; // The enemy's patrol points
    public Vector2
        playerPos;  // The player's position
    public Rigidbody2D
		sprite; // The enemy's sprite
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		sprite = GetComponent<Rigidbody2D>();
		healPerLoop = healAmount / duration;
	}

	void FixedUpdate()
	{
		playerPos = GameObject.FindWithTag("Player").transform.position;
		if (aggro == false && Vector2.Distance(transform.position, playerPos) <= aggroRange)
		{
			aggro = true;
		}
		else if (Vector2.Distance(transform.position, playerPos) >= followRange)
		{
			aggro = false;
			StartCoroutine(HealOverTimeCoroutine(healAmount, duration));
		}
		if (aggro)
		{
			canAttack = true;
		}
		Debug.Log("enemy CurrentHealth = " + currentHealth.runTimeValue);
	}
	#endregion

	#region Utility Methods
	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);

		float percentHealth = currentHealth.runTimeValue / maxHealth.initialValue;
		SetHealth(percentHealth);

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