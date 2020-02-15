using UnityEngine;

public class HammerGuardianMovement : MonoBehaviour
{
	#region Enums and Defined Constants
	public const float
		ROTATION_SPEED = 1.0f; // How many degrees at a time the enemy turns towards the player
	#endregion

	#region Public Variables
	public float
		moveSpeed,   // How fast the guardian moves
		targetAngle; // Angle the guardian will turn towards
	public Vector2
		direction; // The direction the guardian will go
	public bool
		canMove; // Check if the guardian is able to move
	#endregion

	#region Private Variables
	private Player
		player; // Reference the player
	private float
		currentAngle; // Angle the guardian is currently turned
	private Vector2
		facing; // Used to return the direction of the guardian
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Locate the player and start the guardian facing down </summary>
	void Start()
	{
		player       = FindObjectOfType<Player>();
		currentAngle = 270.0f;
	}

	/// <summary> Turn towards and chase down the player </summary>
	void FixedUpdate()
	{
		if (canMove)
		{
			// Follow the player but not closely enough to trap him
			if (Vector2.Distance(transform.position, player.transform.position) > 4.2f)
			{
				Pursue();
			}

			// Turn towards the player if not right up on him
			if (Vector2.Distance(transform.position, player.transform.position) > 0.2f)
			{
				TurnTowardsTarget();
			}
		}
	}
	#endregion

	/// <summary> The guardian always follows the player </summary>
	#region Utility Methods
	private void Pursue()
	{
		// Move from the current position towards the player
		transform.position =
			Vector2.MoveTowards(transform.position, (Vector2) transform.position +
			                    new Vector2 (Mathf.Cos(currentAngle * Mathf.Deg2Rad),
		                                     Mathf.Sin(currentAngle * Mathf.Deg2Rad)).normalized *
		                                     Vector2.Distance(transform.position, player.transform.position),
		                                     moveSpeed * Time.deltaTime);
	}

	/// <summary> Turn slowly towards the player instead of snapping to facing him </summary>
	private void TurnTowardsTarget()
	{
		// Get the angle between the enemy and player
		direction   = player.transform.position - transform.position;
		targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		// Keep both angles within 0 and 360 degrees
		targetAngle  = (targetAngle + 360.0f) % 360.0f;
		currentAngle = (currentAngle + 360.0f) % 360.0f;

		// Loop the 0 degree angle around to 360 if needed
		currentAngle += ((targetAngle - currentAngle <= 180.0f) ? 0.0f : 360.0f);
		targetAngle  += ((currentAngle - targetAngle <= 180.0f) ? 0.0f : 360.0f);

		// Set the angle to rotate to
		if (Mathf.Abs(currentAngle - targetAngle) > ROTATION_SPEED)
		{
			currentAngle += ROTATION_SPEED * ((targetAngle > currentAngle) ? 1.0f : -1.0f);
		}
		else
		{
			currentAngle = targetAngle;
		}
	}

	/// <summary> Get the general direction the guardian is facing </summary>
	public Vector2 GetDirection()
	{
		if (currentAngle % 360 >= 45.0f && currentAngle % 360 <= 135.0f)
		{
			facing = Vector2.up;
		}
		else if (currentAngle % 360 >= 225.0f && currentAngle % 360 <= 315.0f)
		{
			facing = Vector2.down;
		}
		else if (currentAngle % 360 > 135.0f && currentAngle % 360 < 315.0f)
		{
			facing = Vector2.left;
		}
		else
		{
			facing = Vector2.right;
		}
		return facing;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}