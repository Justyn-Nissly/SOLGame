using UnityEngine;

public class ShockwaveAttack : MonoBehaviour
{
	#region Enums (Empty)
	public const float
		ROTATION_SPEED = 1.0f; // How many degrees at a time the enemy turns towards the player
	#endregion

	#region Public Variables
	public float
		spreadRate, // How fast the shockwave spreads
		maxSpread;  // How far the shockwave can reach
	#endregion

	#region Private Variables
	private Player
		player;
	private float
		direction; // Angle to the player
	private AudioSource
		source; // Make the sound
	private Vector2
		knockBack;
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Initalize the enemy path
	void Start()
	{
		player = FindObjectOfType<Player>();
		source = GetComponent<AudioSource>();
		source.Play();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		direction = Mathf.Atan2(player.transform.position.y - transform.position.y,
								player.transform.position.x - transform.position.x);
		knockBack = new Vector2(Mathf.Cos(direction), Mathf.Sin(direction));

		if (transform.localScale.x < maxSpread)
		{
			transform.localScale += new Vector3(spreadRate, spreadRate, 0.0f);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.enabled)
		{
			if (collision.gameObject.tag == "Player")
			{
				player.TakeDamage(1, false);
				collision.attachedRigidbody.AddRelativeForce(knockBack * 1000.0f, ForceMode2D.Force);
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}