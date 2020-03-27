using System.Collections;
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
		shardRed
	}
	protected const float DESPAWN_TIME = 7.0f;
	#endregion

	#region Public Variables
	public Player
		player;
	public ItemType
		type;
	public SpriteRenderer
		sprite; // Sprite graphic
	public Sprite[]
		sprites; // Possible sprite graphics
	#endregion

	#region Private and Protected Variables
	protected float
		despawnTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the Sprite type </summary>
	public virtual void Awake()
	{
		despawnTimer = 1000000.0f;
		sprite = GetComponent<SpriteRenderer>();
		player = FindObjectOfType<Player>();
	}

	protected virtual void FixedUpdate()
	{
		if (despawnTimer <= DESPAWN_TIME)
		{
			player.FreezePlayer();
		}
		if ((despawnTimer -= Time.deltaTime) <= 0.0f)
		{
			player.playerAnimator.SetBool("AcquiredQuestItem", false);
			player.UnFreezePlayer();
			Destroy(gameObject);
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
					GlobalVarablesAndMethods.swordUnlocked = true;
					break;
				case ItemType.unlockBlaster:
					GlobalVarablesAndMethods.blasterUnlocked = true;
					break;
				case ItemType.unlockShield:
					GlobalVarablesAndMethods.shieldUnlocked = true;
					player.shieldAnimator.SetBool("ShieldUnlocked", true);
					break;
				case ItemType.unlockHammer:
					GlobalVarablesAndMethods.hammerUnlocked = true;
					break;
				default:
					break;
			}
			player.SetUpInputDetection();
			player.playerAnimator.SetBool("AcquiredQuestItem", true);
			player.FreezePlayer();
			transform.position = GameObject.FindGameObjectWithTag("Arm").transform.position;
			GetComponent<BoxCollider2D>().enabled = false;
			despawnTimer = DESPAWN_TIME;
			sprite.sortingOrder = LayeredRender.MAX_Y * 2;
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}