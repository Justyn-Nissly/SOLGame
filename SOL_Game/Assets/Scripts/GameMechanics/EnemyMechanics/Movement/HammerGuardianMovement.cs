using UnityEngine;

public class HammerGuardianMovement : MonoBehaviour
{
	#region Enums and Defined Constants
	public const float
		ROTATION_SPEED = 1.0f; // How many degrees at a time the enemy turns towards the player
	#endregion

	#region Public Variables
	public float
		moveSpeed; // How fast the guardian moves
	#endregion

	#region Private Variables
	private Player
		player; // Reference the player
	private Vector2
		direction,
		directionTest;
	private float
		currentAngle, // Where the guardian is turned
		targetAngle;  // Where the guardian will turn towards
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Initalize the enemy path
	void Start()
	{
		player       = FindObjectOfType<Player>();
		currentAngle = 270.0f;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		direction = GetDirection();

/*		// Remove after debugging
		if (direction == Vector2.up)
		{
				Debug.Log("Direction = UP");
		}
		else if (direction == Vector2.down)
		{
			Debug.Log("Direction = DOWN");
		}
		else if (direction == Vector2.right)
		{
			Debug.Log("Direction = RIGHT");
		}
		else
		{
			Debug.Log("Direction = LEFT");
		}*/

		if (Vector2.Distance(transform.position, player.transform.position) > 4.2f)
		{
			Pursue();
		}
		if (Vector2.Distance(transform.position, player.transform.position) > 0.2f)
		{
			TurnTowardsTarget();
		}
	}
	#endregion

	#region Utility Methods
	public void Pursue()
	{
		transform.position = Vector2.MoveTowards(transform.position, (Vector2) transform.position +
												 new Vector2 (Mathf.Cos(currentAngle * Mathf.Deg2Rad),
		                                                      Mathf.Sin(currentAngle * Mathf.Deg2Rad)).normalized *
		                                                      Vector2.Distance(transform.position, player.transform.position),
		                                         moveSpeed * Time.deltaTime);
	}

	public void TurnTowardsTarget()
	{
		// Get the angle between the enemy and player
		directionTest = player.transform.position - transform.position;
		targetAngle   = Mathf.Atan2(directionTest.y, directionTest.x) * Mathf.Rad2Deg;

		// Keep both angles within 0 and 360 degrees
		targetAngle  = (targetAngle + 360.0f) % 360.0f;
		currentAngle = (currentAngle + 360.0f) % 360.0f;

		// Prevent the 360-degree max on an angle from messing up the calculation
		if (Mathf.Abs(currentAngle - targetAngle) >= 180.0f)
		{
			if (targetAngle > currentAngle)
			{
				currentAngle += 360.0f;
			}
			else
			{
				targetAngle += 360.0f;
			}
		}

		// Set the angle to rotate to
		if (currentAngle < targetAngle - ROTATION_SPEED)
		{
			currentAngle += ROTATION_SPEED;
		}
		else if (currentAngle > targetAngle + ROTATION_SPEED)
		{
			currentAngle -= ROTATION_SPEED;
		}
		else
		{
			currentAngle = targetAngle;
		}

		// Apply the rotation
		transform.rotation = Quaternion.AngleAxis(currentAngle + 90.0f, Vector3.forward);
	}

	private Vector2 GetDirection()
	{
		if ((currentAngle - 45.0f) % 360 >= 45.0f && (currentAngle - 45.0f) % 360 <= 135.0f)
		{
			return Vector2.up;
		}
		else if (((currentAngle - 45.0f) - 45.0f) % 360 >= 225.0f && (currentAngle - 45.0f) % 360 <= 315.0f)
		{
			return Vector2.down;
		}
		else if ((currentAngle - 45.0f) % 360 > 135.0f && (currentAngle - 45.0f) % 360 < 315.0f)
		{
			return Vector2.left;
		}
		else
		{
			return Vector2.right;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}