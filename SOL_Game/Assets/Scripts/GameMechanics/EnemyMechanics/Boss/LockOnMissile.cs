using UnityEngine;

public class LockOnMissile : MonoBehaviour
{
	#region Enums and Defined Constants
	private const float
		DEFAULT_RANGE = 10.0f; // The default effective range
	#endregion

	#region Public Variables
	public float
		effectiveRange, // How far the missile can effectively track the target from
		moveSpeed,      // Missile speed
		offset;         // How far the missile veers to the side
	public int
		impactDamage; // The missile deals damage when it hits the player
	public bool
		veerLeft; // Determines whether the missile veers left or right
	public Vector2
		direction, // The missile's actual direction of movement (not necessarily directly towards the target)
		targetPos; // The target's position
	#endregion

	#region Private Variables
	private float
		moveAngle; // Angle the missile will move at
	private GameObject
		target; // The missile locks on to this
	private Player
		player; // Reference the player
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Initalize the missile
	void Start()
	{
		// Target defaults to player
		target = GameObject.FindWithTag("Player");

		// Initial lock on
        targetPos = target.transform.position;

		// The effective range cannot be 0; assign it a default range if 0
		if (effectiveRange == 0.0f)
		{
			effectiveRange = DEFAULT_RANGE;
		}

		// The player can take damage from missiles
		player = FindObjectOfType<Player>();
	}

	/// <summary> Lock on to the target and arc towards it </summary>
	void FixedUpdate()
	{
		targetPos = target.transform.position;
		SetAngle();
		Arc();
	}

	/// <summary> Damage the player on contact and destroy the missile </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.enabled)
		{
			if (collision.gameObject.tag == "Player")
			{
				player.TakeDamage(impactDamage, false);
				Destroy(gameObject);
			}
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Set the missile angle to make it arc towards the target </summary>
	void SetAngle()
	{
		// The missile angles towards a distance from and perpendicular to the target
		moveAngle = Mathf.Atan2(targetPos.y - transform.position.y,
		                        targetPos.x - transform.position.x) +
		                      ((veerLeft) ? 90 : -90) * Mathf.Deg2Rad;

		// The missile angles closer to the target as it approaches
		// Offset affects arc width and effective range determines how easily the missile follows the target
		direction = new Vector2(Mathf.Cos(moveAngle), Mathf.Sin(moveAngle)) * offset / effectiveRange *
		                                              Vector2.Distance(targetPos, transform.position);
	}

	/// <summary> Make the missile arc towards the target </summary>
	void Arc()
	{
		transform.position = Vector2.MoveTowards(transform.position, targetPos + direction, moveSpeed * Time.deltaTime);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}