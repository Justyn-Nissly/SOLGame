using UnityEngine;

public class BlastObject : MonoBehaviour
{
	#region Enums and Defined Constants (Empty)
	#endregion

	#region Public Variables
	public float
		damageTime, // How long the blast can damage for
		delayTime;  // Starting time until the blast appears
	#endregion

	#region Private Variables
	private float
		damageTimer, // How much longer the blast can deal damage
		delayTimer; // Time until the blast appears
	private bool
		isBlasting; // Check if the blasts are appearing
	private CircleCollider2D
		blastCollider; // Makes the player take damage if caught in a blast
	private Player
		player; // Reference the player
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Start the blast delay </summary>
	void Start()
	{
		delayTimer    = delayTime;
		isBlasting    = true;
		blastCollider = GetComponent<CircleCollider2D>();
		player        = FindObjectOfType<Player>();
	}

	/// <summary> Count down the blast timer then activate the blast </summary>
	void FixedUpdate()
	{
		// Count down and don't damage the player before the actual blast
		CountDown();

		// Activate the blast
		Blast();

		// After the blast the object despawns
		if (delayTimer <= -1.6f)
		{
			Destroy(gameObject);
		}
	}

	/// <summary> Damage the player if caught in the blast </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			player.TakeDamage(1, false);
		}
	}
	#endregion

	/// <summary> Activate the blast </summary>
	#region Utility Methods
	void Blast()
	{
				// Make the blast appear
		if (delayTimer <= 0.0f && isBlasting == true)
		{
			// Replace the line below with the animation
			transform.localScale  *= (new Vector2(4.0f, 4.0f));
			isBlasting             = false;
			blastCollider.enabled  = true;
			damageTimer = damageTime;
		}
	}

	/// <summary> Count down and don't damage the player before the actual blast </summary>
	void CountDown()
	{
		if (blastCollider.enabled == true)
		{
			damageTimer -= Time.deltaTime;
			if (damageTimer <= 0.0f)
			{
				blastCollider.enabled = false;
			}
		}
		delayTimer -= Time.deltaTime;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}