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
	public Player
		player;
	public ItemType
		type;
	public SpriteRenderer
		sprite; // Sprite graphic
	public Sprite[]
		sprites; // Possible sprite graphics
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the Sprite type </summary>
	public virtual void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	/// <summary> Apply the Sprite </summary>
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