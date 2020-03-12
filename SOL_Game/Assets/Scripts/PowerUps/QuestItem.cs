using System.Collections;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
	#region Enums and Defined Constants
	public const int
		MAX_MED_KITS = 4, // Player cannot hold moer than this many med kits
		SHIELD = 0, // Grant temporary invulnerability
		POWER = 1, // Boost the player's damage
		SPEED = 2, // Boost the player's speed
		HEAL = 3; // Heal the player
	#endregion

	#region Public Variables
	public int
		type; // The type of the power up itself
	public float
		powerUpTimer, // How long power ups last
		timer;        // Time until the power up disappears
	public Sprite
		powerUp; // Power up graphic
	public Sprite[]
		powerUps; // Possible power up graphics
	#endregion

	#region Private Variables
	private Player
		player; // Apply the power up to the player
	private float
		spinTimer; // Make the power up appear to spin
	private SpriteRenderer
		powerUpSprite; // Power up visual
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the power up type </summary>
	void Awake()
	{
		new Random();
		player = GameObject.FindObjectOfType<Player>();
		spinTimer = 0.0f;

		// Med kits are more common than other power ups
		type = (int)Random.Range((float)SHIELD, (float)HEAL + 1.1f);
		if (type > HEAL)
		{
			type = HEAL;
		}
	}

	/// <summary> Power ups eventually disappear after dropping </summary>
	void FixedUpdate()
	{

	}

	/// <summary> Apply the power up </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Debug.Log("Got quest item!");
			Destroy(gameObject);
		}

	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}