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
		healthBar;
    public string
        enemyName; // The enemy's name
	public float
		aggroRange,  // The max range where the enemy can detect the player
						 //duration,
		followRange, // How far away the player must get for the enemy to deaggro
		attackDmg,   // Base damage from an intentional attack
		  contactDmg,  // Base damage from making contact with the player
		healPerLoop,
		MAXPOSSIBLEHEALTH,
		  maxHealOverTime,
		  moveSpeed,   // Base movement speed
			maxMoveSpeed;
    public bool
        aggro; // The enemy has detected the player
    public Vector2[]
        patrol; // Enemy patrol points
	public Vector2
		playerPos,   // Track the player's position
		enemyVector; // Track the enemy's vector
    public Rigidbody2D
		rb2d; // The enemy's rigidBody
	public AudioManager
		enemyAudioManager;
	public GameObject
		powerUp; // Reference PowerUp prefab.

	public Material pixelDesolveMaterial;
	#endregion

	#region Private Variables
	private float
    amountHealed = 0,
    countDownTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the enemy </summary>
	public virtual void Start()
	{
		player        = GameObject.FindObjectOfType<Player>();
		rb2d          = GetComponent<Rigidbody2D>();

		if(maxHealth != null)
		{
			currentHealth = maxHealth.initialValue;
		}

		enemyAudioManager = GameObject.FindObjectOfType<AudioManager>();
    countDownTimer = maxHealOverTime;
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
            if (countDownTimer <= 0)
            {
                countDownTimer = maxHealOverTime; // reset the time after going to 0

                if (currentHealth < maxHealth.initialValue) // only heal if health less than full
                {
                    currentHealth += healPerLoop;
                    SetHealth(currentHealth / maxHealth.initialValue);
                    //Debug.Log("enemy CurrentHealth = " + currentHealth);
                }
            }
            else
            {
                countDownTimer -= Time.deltaTime;
            }
        }

		//// Enemies attack the player only if aggroed
		//if (aggro)
		//{
		//	canAttack = true;
		//}
	}
	#endregion

	#region Utility Methods
	/// <summary> deal damage to the passed in player</summary>
	protected virtual void DamagePlayer(Player player, int damageToGive, bool playSwordSoundEffect = false)
	{
		if (player != null)
		{
			player.TakeDamage(damageToGive, playSwordSoundEffect);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}

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
			if (Random.Range(0.0f, 5.0f) > 4.0f)
			{
				Instantiate(powerUp, transform.position, Quaternion.identity);
			}

			StartCoroutine("Die");
			
		}
	}

	///<summary> Make the health bar show the current health </summary>
	void SetHealth(float percentHelth)
	{
		healthBar.fillAmount = percentHelth;
	}

	/// <summary>
	/// play the teleport shader effect if there is one on the enemy
	/// (it will find all effects on the enemy because some enemies have more than one like the shield enemy)
	/// </summary>
	public void PlayTeleportEffect()
	{
		List<_2dxFX_NewTeleportation2> enemyTeleportScripts = new List<_2dxFX_NewTeleportation2>();
		enemyTeleportScripts.AddRange(GetComponentsInChildren<_2dxFX_NewTeleportation2>());

		if (enemyTeleportScripts.Count != 0) // check for empty list
		{
			foreach (_2dxFX_NewTeleportation2 enemyTeleportScript in enemyTeleportScripts)
			{
				StartCoroutine(TeleportInEnemy(enemyTeleportScript));
			}
		}
	}
	#endregion

	#region Coroutines
	private IEnumerator Die()
	{
		float percentageComplete = 0;

		// freeze the enemy because they are dead...
		canAttack = false;
		moveSpeed = 0;

		if(pixelDesolveMaterial != null)
		{
			foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()	)
			{
				renderer.material = pixelDesolveMaterial;
			}

			pixelDesolveMaterial.SetFloat("Disolve_Value", 0); // set starting value

			// play the pixel die effect from start(0) to finish(1)
			while (percentageComplete < 1)
			{
				pixelDesolveMaterial.SetFloat("Disolve_Value", Mathf.Lerp(0f, 1f, percentageComplete));
				percentageComplete += Time.deltaTime /2;
				yield return null;
			}

			pixelDesolveMaterial.SetFloat("Disolve_Value", 1); // set ending value
		}

		// destroy the enemy
		Destroy(gameObject);
	}

	protected virtual IEnumerator TeleportInEnemy(_2dxFX_NewTeleportation2 teleportScript)
	{
		float percentageComplete = 0;

		// make the enemy invisible, this is not set by default in the prefab because
		// then the enemy would be invisible in Dev rooms because they don't have this script running in them
		teleportScript._Fade = 1;

		// teleport the enemy in, it does this by "sliding" a float from 0 to 1 over time
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