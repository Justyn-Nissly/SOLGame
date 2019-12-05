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
	public AudioManager enemyAudioManager;
	public GameObject
		powerUp; // Reference PowerUp prefab.
	#endregion

	#region Private Variables
	private Player
		player; // Check if the player has a damage boost
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the enemy </summary>
	public virtual void Start()
	{
		player        = GameObject.FindObjectOfType<Player>();
		rb2d          = GetComponent<Rigidbody2D>();
		currentHealth = maxHealth.initialValue;
		healPerLoop = healAmount / duration;
		enemyAudioManager = GameObject.FindObjectOfType<AudioManager>();
	}

	/// <summary> Enemy activity depends on whether or not it has detected the player </summary>
	public virtual void FixedUpdate()
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
			//StartCoroutine(HealOverTimeCoroutine(healAmount, duration));
		}

		// Enemies attack the player only if aggroed
		if (aggro)
		{
			canAttack = true;
		}
		// DEBUG CODE; REMOVE LATER
		Debug.Log("enemy CurrentHealth = " + currentHealth);
	}
	#endregion

	#region Utility Methods
	///<summary> Deal damage to the enemy </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage + player.extraDamage, playSwordImpactSound);
		SetHealth(currentHealth / maxHealth.initialValue);

		Debug.Log("enemy CurrentHealth = " + currentHealth);

		// The enemy gets destroyed if it runs out of health
		if (currentHealth <= 0)
		{
			enemyAudioManager.PlaySound();
			// The enemy might drop a power up
			if (true)
			{
				Instantiate(powerUp, transform.position, Quaternion.identity);
			}
			Destroy(gameObject);
		}
	}

	///<summary> Make the health bar show the current health </summary>
	void SetHealth(float percentHelth)
	{
		healthBar.fillAmount = percentHelth;
	}
	#endregion

	#region Coroutines
	///<summary> The enemy heals over time </summary>
	IEnumerator HealOverTimeCoroutine(float healAmount, float duration)
	{
		while (amountHealed < healAmount)
		{
			yield return new WaitForSeconds(20.0f);
			currentHealth += healPerLoop;
			amountHealed               += healPerLoop;
			SetHealth(currentHealth);
		}
	}
	#endregion
}
