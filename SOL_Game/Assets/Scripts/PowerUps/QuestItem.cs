using System.Collections;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
	#region Enums
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
	#endregion

	#region Public Variables
	public ItemType
		type;
	public Sprite
		powerUp; // Power up graphic
	public Sprite[]
		powerUps; // Possible power up graphics
	#endregion

	#region Private Variables
	private Player
		player;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the power up type </summary>
	void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	/// <summary> Power ups eventually disappear after dropping </summary>
	void FixedUpdate()
	{
		;
	}

	/// <summary> Apply the power up </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
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
					break;
				case ItemType.unlockHammer:
					GlobalVarablesAndMethods.hammerUnlocked = true;
					break;
			}
			Destroy(gameObject);
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}