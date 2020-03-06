using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	#region Enums and Defined Constants
	public const int
		HEAL = 0, // Heal the player
		POWER = 1, // Boost the player's damage
		SPEED = 2; // Boost the player's speed
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
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Called when the power up spawns </summary>
	void Awake()
	{
		new Random();
		player = GameObject.FindObjectOfType<Player>();

		// Healing power up is the most common
		type   = (int)Random.Range((float)HEAL, (float)SPEED + 1.5f);
		if (type > SPEED)
		{
			type = HEAL;
		}
		this.GetComponent<SpriteRenderer>().sprite = powerUps[type];
	}

	/// <summary> Power ups eventually disappear after dropping </summary>
	void FixedUpdate()
	{
		if (timer > 0.0f)
		{
			timer -= Time.deltaTime;
		}
		else
		{
			Destroy(gameObject);
		}
		StartCoroutine("StartBlinking");
	}

	/// <summary> Apply the power up </summary>
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			// Effects have timers but healing is instantaneous
			player.powerUpTimers[type] = (type == HEAL) ? 0.001f : powerUpTimer;
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
		while (timer < 3.0f)
		{
			// Toggle the sprite's visibility to make it blink
			spriteRenderer.enabled = !spriteRenderer.enabled;

			// Blinking speeds up as the timer runs down
			yield return new WaitForSeconds(timer);
		}
	}
	#endregion
}
