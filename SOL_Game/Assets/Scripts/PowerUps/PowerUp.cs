using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	#region Enums and Defined Constants
	public const int
		HEAL  = 0, // Heal the player
		POWER = 1, // Boost the player's damage
		SPEED = 2; // Boost the player's speed
	#endregion

	#region Public Variables
	public int
		type; // The type of the power up itself
	public float
		timer; // Time until the power up disappears
	#endregion

	#region Private Variables
	private Player
		player; // Apply the power up to the player
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> The player picks up the power up </summary>
	void Awake()
	{
		player = GameObject.FindObjectOfType<Player>();
		/*new Random();*/
		type = (int) Random.Range((float) HEAL, (float) SPEED + 1.0f);type=SPEED;
	}

	/// <summary> Power ups eventually disappear after being dropped </summary>
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
			player.powerUpTimers[type] = (type == HEAL) ? 0.0001f : player.powerUpTimer;
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

			// Put custom timer here?
			// Blinking speeds up as the timer runs down
			yield return new WaitForSeconds(timer);
		}
	}
	#endregion
}