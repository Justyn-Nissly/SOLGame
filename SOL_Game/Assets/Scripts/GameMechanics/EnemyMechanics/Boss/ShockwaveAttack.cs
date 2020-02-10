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
		direction; // The shockwave knocks the player back
	private Vector2
		knockBack; // Amount of force applied to the player on knockback
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the shockwave </summary>
	void Start()
	{
		player = FindObjectOfType<Player>();
		source = GetComponent<AudioSource>();
		source.Play();
	}

	/// <summary> Get the angle to the player and spread the shockwave </summary>
	void FixedUpdate()
	{
		direction = Mathf.Atan2(player.transform.position.y - transform.position.y,
								player.transform.position.x - transform.position.x);
		knockBack = new Vector2(Mathf.Cos(direction), Mathf.Sin(direction)).normalized;

		// The shockwave disappears after reaching its max size
		if (transform.localScale.x < maxSpread)
		{
			transform.localScale += new Vector3(spreadRate, spreadRate, 0.0f);
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
			if (collision.gameObject.tag == "Player")
			{
				collision.attachedRigidbody.AddRelativeForce(knockBack * 1000.0f, ForceMode2D.Force);
				player.TakeDamage(1, false);
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}