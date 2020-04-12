﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
	#region Enums and Defined Constants
	public enum ItemType
	{
		unlockSword,
		unlockBlaster,
		unlockShield,
		unlockHammer,
		shardGreen,
		shardBlue,
		shardYellow,
		shardRed,
		maxHealth
	}
	protected const float DESPAWN_TIME = 7.0f,
	DISAPPEAR = 4.0f;
	#endregion

	#region Public Variables
	public Player
		player;
	public ItemType
		type;
	public SpriteRenderer
		sprite; // Sprite graphic
	public FloatValue
		playerHealth,
		heartContainers;
	#endregion

	#region Private and Protected Variables
	protected float
		despawnTimer;
	private bool
		willSpin,
		spinPositive;
	private float
		spin;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the Sprite type </summary>
	public virtual void Awake()
	{
		// Prevent the sword module from appearing after the player already has it
		if (type == ItemType.unlockSword && Globals.swordUnlocked)
		{
			Destroy(gameObject);
		}

		spin = 0.0f;
		despawnTimer = 1000000.0f;
		sprite = GetComponent<SpriteRenderer>();
		player = FindObjectOfType<Player>();
		willSpin = (type == ItemType.unlockSword   || type == ItemType.unlockShield ||
		            type == ItemType.unlockBlaster || type == ItemType.unlockHammer);
	}

	protected virtual void FixedUpdate()
	{
		if (willSpin)
		{
			transform.localScale = new Vector3(Mathf.Cos((spin += Time.deltaTime) * 0.5f * Mathf.PI), 1.0f, 1.0f);
		}

		if ((despawnTimer -= Time.deltaTime) <= 0.0f)
		{
			player.playerAnimator.SetBool("AcquiredQuestItem", false);
			player.canTakeDamage = true;
			player.UnFreezePlayer();
			sprite.enabled = false;
			Destroy(gameObject);
		}
		else if (despawnTimer <= DISAPPEAR && despawnTimer >= 3.8f)
		{
			player.playerAnimator.SetBool("AcquiredQuestItem", false);
			player.canTakeDamage = true;
			player.UnFreezePlayer();
			sprite.enabled = false;
		}
		else if (despawnTimer <= DESPAWN_TIME && despawnTimer >= DISAPPEAR)
		{
			player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			player.canTakeDamage = false;
			player.FreezePlayer();
		}
	}

	/// <summary> Apply the Sprite </summary>
	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			GetComponent<AudioSource>().Play();
			switch (type)
			{
				case ItemType.shardGreen:
					;
					break;
				case ItemType.shardBlue:
					player.swordComboUnlocked = true;
					break;
				case ItemType.shardYellow:
					player.hammerComboUnlocked = true;
					break;
				case ItemType.shardRed:
					;
					break;
				case ItemType.unlockSword:
					Globals.swordUnlocked = true;
					break;
				case ItemType.unlockBlaster:
					Globals.blasterUnlocked = true;
					break;
				case ItemType.unlockShield:
					Globals.shieldUnlocked = true;
					player.shieldAnimator.SetBool("ShieldUnlocked", true);
					break;
				case ItemType.unlockHammer:
					Globals.hammerUnlocked = true;
					break;
			}
			player.SetUpInputDetection();
			player.playerAnimator.SetFloat("Horizontal", 0.0f);
			player.playerAnimator.SetFloat("Vertical", -1.0f);
			player.playerAnimator.SetBool("AcquiredQuestItem", true);
			player.FreezePlayer();
			transform.position = GameObject.FindGameObjectWithTag("Arm").transform.position;
			GetComponent<BoxCollider2D>().enabled = false;
			sprite.sortingOrder = LayeredRender.MAX_Y * 2;
			despawnTimer = DESPAWN_TIME;

			DoorLogic[] doors = FindObjectsOfType<DoorLogic>();
			foreach (DoorLogic door in doors)
			{
				if (type >= ItemType.shardGreen && type <= ItemType.shardRed)
				{
					door.playerHasShard = true;
				}
				else if (type >= ItemType.unlockSword && type <= ItemType.unlockHammer)
				{
					door.playerHasChip = true;
				}
			}

			// Shards also heal the player to full
			if (willSpin == false)
			{
				playerHealth.runTimeValue = playerHealth.initialValue = heartContainers.runTimeValue * 2.0f;
				collision.GetComponent<Player>().playerHealthHUD.ChangeNumberOfHearts();
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}
