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
	public Image
		healthBar; // Health bar image
	public string
		enemyName; // The enemy's name
	public float
		aggroRange,        // Max range that an enemy can detect the player
		followRange,       // How far away the player must get for the enemy to deaggro
		attackDmg,         // Base damage from an intentional attack
		contactDmg,        // Base damage from making contact with the player
		healPerLoop,       // Health regained per healing increment
		MAXPOSSIBLEHEALTH, // Health cannot exceed this
		maxHealOverTime,   // Healing speed
		saveSpeed,         // Save speed as enemies spawn not moving
		moveSpeed;         // Base movement speed
	public bool
		aggro; // Check if the enemy has detected the player
	public Vector2[]
		patrol; // Enemy patrol points
	public Vector2
		playerPos,   // Player's position
		enemyVector; // Enemy's movement vector
	public Rigidbody2D
		rb2d; // Apply physics to the enemy
	public AudioManager
		enemyAudioManager; // Control playing audio
	public GameObject
		powerUp; // Enable the enemy to drop a powerup
	public Material
		pixelDissolveMaterial; // Dissolve effect plays upon enemy defeat
	#endregion

	#region Private Variables
	private float
		healTimer;    // Time left before gaining health
	private bool
		canDropPowerUp; // Prevent multiple power ups from spawning upon defeat
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the enemy </summary>
	public virtual void Start()
	{
		enemyAudioManager = GameObject.FindObjectOfType<AudioManager>();
		player            = GameObject.FindObjectOfType<Player>();
		rb2d              = GetComponent<Rigidbody2D>();
		healTimer         = maxHealOverTime;

		if (maxHealth != null)
		{
			currentHealth = maxHealth.initialValue;
		}
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
		// Passive enemies heal over time
		else if (Vector2.Distance(transform.position, playerPos) >= followRange)
		{
			aggro = false;
			if (healTimer <= 0)
			{
				// Reset the heal timer
				healTimer = maxHealOverTime;

				// Heal while below max health
				if (currentHealth < maxHealth.initialValue)
				{
					currentHealth += healPerLoop;
					SetHealth(currentHealth / maxHealth.initialValue);
				}
			}
			else
			{
				healTimer -= Time.deltaTime;
			}
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Damage the player </summary>
	protected virtual void DamagePlayer(Player player, int damageToGive, bool playSwordSoundEffect = false)
	{
		if (player != null)
		{
			player.TakeDamage(damageToGive, playSwordSoundEffect);
		}
	}

	///<summary> Damage the enemy </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage + player.extraDamage, playSwordImpactSound);
		SetHealth(currentHealth / maxHealth.initialValue);

		Debug.Log("enemy CurrentHealth = " + currentHealth);

		// The enemy gets destroyed if it runs out of health
		if (currentHealth <= 0)
		{
			enemyAudioManager.PlaySound();

			// Destroyed enemies might drop a power up
			if (Random.Range(0.0f, 5.0f) > 4.0f && canDropPowerUp)
			{
				Instantiate(powerUp, transform.position, Quaternion.identity);
			}
			canDropPowerUp = false;

			StartCoroutine("Die");
		}
	}

	///<summary> Make the health bar show the current health </summary>
	void SetHealth(float percentHelth)
	{
		healthBar.fillAmount = percentHelth;
	}

	/// <summary> Enemy spawn effect </summary>
	public void PlayTeleportEffect()
	{
		// Add an enemy to a list of enemies
		List<_2dxFX_NewTeleportation2> enemyTeleportScripts = new List<_2dxFX_NewTeleportation2>();
		enemyTeleportScripts.AddRange(GetComponentsInChildren<_2dxFX_NewTeleportation2>());

		// Add an enemy to the scene
		if (enemyTeleportScripts.Count != 0)
		{
			foreach (_2dxFX_NewTeleportation2 enemyTeleportScript in enemyTeleportScripts)
			{
				StartCoroutine(TeleportInEnemy(enemyTeleportScript));
			}
		}
	}
	#endregion

	#region Coroutines
	/// <summary> Defeat enemy </summary>
	private IEnumerator Die()
	{
		float
			percentageComplete = 0; // Percent of finishing the effect

		// Defeated enemy deactivates
		canAttack = false;
		moveSpeed = 0;

		// Make the enemy dissolve
		if (pixelDissolveMaterial != null)
		{
			foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
			{
				renderer.material = pixelDissolveMaterial;
			}
			pixelDissolveMaterial.SetFloat("Disolve_Value", 0);

			// Play dissolve effect
			while (percentageComplete < 1)
			{
				pixelDissolveMaterial.SetFloat("Disolve_Value", Mathf.Lerp(0.0f, 1.0f, percentageComplete));
				percentageComplete += Time.deltaTime * 0.5f;
				yield return null;
			}

			pixelDissolveMaterial.SetFloat("Disolve_Value", 1);
		}

		// Destroy the enemy
		Destroy(gameObject);
	}

	/// <summary> Make enemy appear </summary>
	protected virtual IEnumerator TeleportInEnemy(_2dxFX_NewTeleportation2 teleportScript)
	{
		float
			percentageComplete = 0; // Percent of finishing the effect

		// Enemy spawns invisible
		teleportScript._Fade = 1;

		// Enemy fades into view upon spawning
		while (percentageComplete < 1)
		{
			teleportScript._Fade = Mathf.Lerp(1f, 0f, percentageComplete);
			percentageComplete += Time.deltaTime;
			yield return null;
		}

		teleportScript._Fade = 0;
	}
	#endregion
}