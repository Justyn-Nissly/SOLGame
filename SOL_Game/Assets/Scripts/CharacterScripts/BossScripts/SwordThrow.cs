using UnityEngine;

public class SwordThrow : MonoBehaviour
{
	#region Public Variables
	public float
		speed; // How fast the sword moves
	public int
		throwType; // The sword will move straight or arc around when thrown
	public GameObject
		arm,    // Guardian's right arm
		origin; // Point from which the sword is thrown
	public bool
		findTarget; // Control throwing the sword
	#endregion

	#region Private Variables
	private Vector2
		targetPos; // Where the sword will be thrown
	private float
		spinAngle,     // Make the sword spin
		radius,        // Radial distance in the anglular direction
		distance,      // Distance from origin to target
		externalAngle, // Initial angle from origin to target
		moveAngle;     // Current angle of the sword throw
	private Player
		player; // Reference the player
	private bool
		returnOrigin, // Check if the sowrd is returning to its origin
		isThrowing;   // Check if the sword has been thrown
	private MeleeGuardian
		guardianCanMove; // Check if the guardian can move
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the sword </summary>
	void Start()
	{
		guardianCanMove = FindObjectOfType<MeleeGuardian>();
		player          = FindObjectOfType<Player>();
		spinAngle       = 0.0f;
		isThrowing      = false;
	}

	/// <summary> Throw the sword at the player </summary>
	void FixedUpdate()
	{
		ThrowSword(throwType);
	}
	#endregion

	#region Utility Functions
	/// <summary> Throw the sword </summary>
	public void ThrowSword(int type)
	{
		// The sword spins when it is thrown
		if (isThrowing || spinAngle > 15.0f)
		{
			arm.transform.localRotation =
				Quaternion.AngleAxis(spinAngle = ((spinAngle + 15.0f) % 360.0f), Vector3.forward);
		}

		// Ready the sword for throwing
		if (findTarget)
		{
			isThrowing                = true;
			findTarget                = false;
			targetPos                 = player.transform.position;
			origin.transform.position = arm.transform.position;
			moveAngle                 = 0.0f;
			externalAngle             = Mathf.Atan2(targetPos.y - origin.transform.position.y,
			                                        targetPos.x - origin.transform.position.x);
			distance                  = Vector2.Distance(origin.transform.position, targetPos);
		}
		// Throw the sword
		else if (type == 1 && isThrowing)
		{
			// Make the sword return to its origin when it reaches its target position
			if (Vector2.Distance(arm.transform.position, targetPos) <= 0.05f)
			{
				returnOrigin = true;
			}
			// The sword target's the player's position at the time of throwing
			else if (Vector2.Distance(arm.transform.position, origin.transform.position) <= 0.05f)
			{
				targetPos = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
				isThrowing = !returnOrigin;
				if (returnOrigin)
				{
					arm.transform.position = origin.transform.position;
			        guardianCanMove.moving = false;
				    returnOrigin = false;
				}
			}
			// Move the sword along its throw path
			if (isThrowing)
			{
				arm.transform.position =
					(returnOrigin) ? Vector2.Lerp(arm.transform.position, origin.transform.position, speed * Time.deltaTime) :
					                 Vector2.Lerp(arm.transform.position, targetPos, speed * Time.deltaTime);
			}
		}
		// The second throw type is a wide arc
		else if (type == 2 && isThrowing)
		{
			radius                 = Mathf.Cos(2.0f * moveAngle * Mathf.Deg2Rad - Mathf.PI * 0.5f);
			arm.transform.position = (Vector2)origin.transform.position +
				new Vector2(Mathf.Cos(moveAngle * Mathf.Deg2Rad + externalAngle - Mathf.PI * 0.25f),
							Mathf.Sin(moveAngle * Mathf.Deg2Rad + externalAngle - Mathf.PI * 0.25f)).normalized *
							radius * distance;
			isThrowing             = ((moveAngle += speed * 0.4f) < 90.0f);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}