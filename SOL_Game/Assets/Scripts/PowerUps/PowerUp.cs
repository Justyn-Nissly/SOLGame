﻿using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	#region Enums and Defined Constants
	public const int
		MAX_MED_KITS = 4, // Player cannot hold moer than this many med kits
		SHIELD = 0, // Grant temporary invulnerability
		POWER = 1, // Boost the player's damage
		SPEED = 2, // Boost the player's speed
		HEAL = 3, // Heal the player
		FULL_HEAL = 4,
		EXTRA_PLAYER_DAMAGE = 2; // Powered up damage multiplier
	public const float
		EXTRA_PLAYER_SPEED = 0.08f, // Powered up speed multiplier
		MAX_POWER_UP_TIME = 30.0f; // How long power up effects last
	#endregion

	#region Public Variables
	public int
		type; // The type of the power up itself
	public float
		timer; // Time for the power up to disappear
	[HideInInspector]
	public float
		timeLeft;     // Time until the power up disappears
	[HideInInspector]
	public BoxCollider2D
		boxCollider;
	[HideInInspector]
	public Sprite
		powerUp; // Power up graphic
	public Sprite[]
		powerUps; // Possible power up graphics
	[HideInInspector]
	public SpriteRenderer
		powerUpSprite; // Power up visual
	public bool
		setType;
	#endregion

	#region Protected Variables
	protected Player
		player; // Apply the power up to the player
	protected float
		spinTimer; // Make the power up appear to spin
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the power up type </summary>
	public virtual void Awake()
	{
		new Random();
		boxCollider  = GetComponent<BoxCollider2D>();
		player       = GameObject.FindObjectOfType<Player>();
		spinTimer    = 0.0f;
		timeLeft     = timer;

		// Med kits are more common than other power ups
		if (setType == false)
		{
			type = (int)Random.Range((float)SHIELD, (float)HEAL + 1.1f);
		}
		if (type > HEAL && setType == false)
		{
			type = HEAL;
		}
		(powerUpSprite = GetComponent<SpriteRenderer>()).sprite = powerUps[type * (HEAL + 1)];
	}

	/// <summary> Power ups eventually disappear after dropping </summary>
	public virtual void FixedUpdate()
	{
		spinTimer += Time.deltaTime * 8.0f;
		powerUpSprite.sprite = powerUps[(int)(spinTimer % 4.0f) + type * (HEAL + 1)];

		if (timeLeft <= 0.0f && setType == false)
		{
			Destroy(gameObject);
		}

		StartCoroutine("StartBlinking");
		timeLeft -= Time.deltaTime;
	}

	/// <summary> Apply the power up </summary>
	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			// Enable the player to use the power up
			if (type == FULL_HEAL)
			{
				player.maxHealth.runTimeValue = player.maxHealth.initialValue;
				player.playerHealthHUD.UpdateHearts();
			}
			else if (type != HEAL)
			{
				player.powerUpTimers[type] += MAX_POWER_UP_TIME * 0.333333f;
				if (player.powerUpTimers[type] >= MAX_POWER_UP_TIME)
				{
					player.powerUpTimers[type] = MAX_POWER_UP_TIME;
				}
				player.powerUpsActive[type] = false;
			}
			else
			{
				if (player.medKits < MAX_MED_KITS)
				{
					player.medKits++;
				}
				if (player.medKitImages != null)
				{
					for (int i = 0; i < MAX_MED_KITS; i++)
					{
						player.medKitImages[i].enabled = (i < player.medKits);
					}
				}
			}

			// Power up disappears atfer being picked up
			Destroy(gameObject);
		}

	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines
	/// <summary> Make a power up blink before disappearing </summary>
	IEnumerator StartBlinking()
	{
		SpriteRenderer
			spriteRenderer = GetComponent<SpriteRenderer>();

		// Make the power up blink
		while (timer < 5.0f)
		{
			// Toggle the sprite's visibility to make it blink
			spriteRenderer.enabled = !spriteRenderer.enabled;

			// Blinking speeds up as the timer runs down
			yield return new WaitForSeconds(timer);
		}
	}
	#endregion
}