using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonObject : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	public FloatValue
		DamageToGive; // the damage that will be dealed to the player
	public List<Sprite> 
		poisonSprites = new List<Sprite>(); // list of poison sprites
	#endregion

	#region Private Variables
	private float
		timer = 1, // count down timer for damaging the player
		maxTimer = 1; // timer interval time for damaging the player
	private Leviadrin
		boss;
	#endregion

	// Unity Named Methods
	#region Unity Main Methods
	private void Start()
	{
		// set the poison sprite to a random sprite from the list of sprites
		if (poisonSprites.Count > 0)
		{
			GetComponent<SpriteRenderer>().sprite = poisonSprites[Random.Range(0, poisonSprites.Count)];
		}

		boss = FindObjectOfType<Leviadrin>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (boss != null)
		{
			if (boss.maxHealth.runTimeValue <= 0)
			{
				Destroy(gameObject);
			}
		}
		// damage the player
		else if (collision.gameObject.CompareTag("Player") && DamageToGive != null)
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
		}
	}

	/// <summary> constantly damage the player every N seconds if the player is standing on the poison </summary>
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (timer < 0)
		{
			// damage the player
			if (collision.gameObject.CompareTag("Player") && DamageToGive != null)
			{
				DamagePlayer(collision.gameObject.GetComponent<Player>());
			}

			timer = maxTimer;
		}
		else
		{
			timer -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Functions
	/// <summary> the method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null && player.canTakeDamage && player.playerAnimator.GetBool("isShieldUp") == false)
		{
			player.TakeDamage((int)DamageToGive.initialValue, false);
		}
	}

	/// <summary>
	/// called in the animator
	/// </summary>
	public void DestroyGameObject()
	{
		Destroy(gameObject);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}