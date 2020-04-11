using UnityEngine;

public class ShockwaveAttack : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		spreadRate, // How fast the shockwave spreads
		maxSpread;  // How far the shockwave can reach
	#endregion

	#region Private Variables
	private Player
		player; // Reference the player
	private AudioSource
		source; // Make the sound
	private float
		spread = 0.0f,
		waveDirection, // Shockwave knocks the player back
		spinAngle;     // Make the shockwave spin
	private Vector2
		knockBack; // Force applied to the player on knockback
	private bool
		canDamage = true; // Prevent the shockwave from dealing damage more than once
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the shockwave and play its sound </summary>
	void Awake()
	{
		spinAngle = 0.0f;
		player    = FindObjectOfType<Player>();
		source    = GetComponent<AudioSource>();
		source.Play();
	}

	/// <summary> Get the angle to the player and spread the shockwave </summary>
	void FixedUpdate()
	{
		// The shockwave expands until it reaches its max size
		if (transform.localScale.x < maxSpread)
		{
			spread += spreadRate * 0.1f;

			transform.localScale +=
				new Vector3((0.2f - Mathf.Log(spread / maxSpread + 0.2f) / Mathf.Log(8.0f)) * 0.1f,
				            (0.2f - Mathf.Log(spread / maxSpread + 0.2f) / Mathf.Log(8.0f)) * 0.1f, 0.0f);
			SpinShockwave();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	/// <summary> Knock back and damage the player </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.enabled)
		{
			waveDirection = Mathf.Atan2(player.transform.position.y - transform.position.y,
			                            player.transform.position.x - transform.position.x);
			knockBack = new Vector2(Mathf.Cos(waveDirection), Mathf.Sin(waveDirection)).normalized;

			// The shockwave damages through shields
			if (canDamage && collision.gameObject.tag == "Player")
			{
				player.canTakeDamage = true;
				collision.attachedRigidbody.AddRelativeForce(knockBack * 1000.0f, ForceMode2D.Force);
				player.TakeDamage(1, false);
				player.canTakeDamage = false;
				canDamage            = false;
			}
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Make the shockwave spin (aesthetic effect) </summary>
	void SpinShockwave()
	{
		transform.rotation = Quaternion.AngleAxis(spinAngle -= 7.5f, Vector3.forward);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}