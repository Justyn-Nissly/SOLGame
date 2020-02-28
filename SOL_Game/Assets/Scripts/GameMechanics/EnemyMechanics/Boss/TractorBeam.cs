using UnityEngine;

public class TractorBeam : MonoBehaviour
{
	#region Enums and Defined Constants (Empty)
	#endregion

	#region Public Variables
	public float
		strength,  // How strongly the beam pulls
		moveSpeed; // Tractor beam speed
	#endregion

	#region Private Variables
	private Vector2
		direction, // The beam's movement direction
		origin,    // Pull the player towards here
		pull;      // Direction the player gets pulled
	private float
		moveAngle, // Angle the beam moves at
		pullAngle, // Angle to pull the player
		time;      // How long the beam lasts
	private Player
		player; // Reference the player
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the tractor beam </summary>
	void Start()
	{
		player = FindObjectOfType<Player>();
		origin = transform.position;
		time   = 1.4f;
	}

	/// <summary> Pull the player </summary>
	void FixedUpdate()
	{
		// The beam dissipates after a time
		if ((time -= Time.deltaTime) <= 0.0f)
		{
			Destroy(gameObject);
		}

		// Fire the beam at the player
		SetAngle();
		Move();
	}

	/// <summary> Pull the character to the origin </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			// Calculate the angle and direction vector for pulling the player in
			pullAngle = Mathf.Atan2(origin.y - player.transform.position.y,
									origin.x - player.transform.position.x);
			pull      = new Vector2(Mathf.Cos(pullAngle), Mathf.Sin(pullAngle));

			// Apply the tractor beam effect
			collision.attachedRigidbody.AddRelativeForce(pull.normalized * strength, ForceMode2D.Force);
			Destroy(gameObject);
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Set the angle to move towards the player </summary>
	void SetAngle()
	{
		moveAngle = Mathf.Atan2(player.transform.position.y - transform.position.y,
								player.transform.position.x - transform.position.x);
		direction = new Vector2(Mathf.Cos(moveAngle), Mathf.Sin(moveAngle));
	}

	/// <summary> Fire the tractor beam at the player </summary>
	void Move()
	{
		transform.position = Vector2.MoveTowards(transform.position, (Vector2) player.transform.position + direction,
		                                         moveSpeed * Time.deltaTime);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}