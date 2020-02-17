using UnityEngine;

public class SwordThrow : MonoBehaviour
{
	#region Public Variables
	public float
		arcSpeed; // Angle speed
	public GameObject
		origin, // Starting point
		target; // The missile locks on to this
	public bool
		findTarget; // The target is not set
	#endregion

	#region Private Variables
	private Vector2
		targetPos; // The target's position
	private float
		radius,        // Radial distance compared to the angle
		distance,      // Distance from origin to target
		externalAngle, // Initial angle from the origin to the target
		moveAngle;     // The current angle of the throw
	private Player
		player; // Reference the player
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the missile </summary>
	void Start()
	{
		player     = FindObjectOfType<Player>();
		findTarget = true;
	}

	/// <summary> Lock on to the target and arc towards it </summary>
	void FixedUpdate()
	{
		if (findTarget)
		{
			findTarget = false;
			targetPos = player.transform.position;
			origin.transform.position = transform.position;
			moveAngle  = 0.0f;
			externalAngle = Mathf.Atan2(targetPos.y - origin.transform.position.y,
			                            targetPos.x - origin.transform.position.x);
			distance = Vector2.Distance(origin.transform.position, targetPos);
		}

		radius = Mathf.Cos(2.0f * moveAngle * Mathf.Deg2Rad - Mathf.PI * 0.5f);
		transform.position = (Vector2) origin.transform.position +
		             new Vector2 (Mathf.Cos(moveAngle * Mathf.Deg2Rad + externalAngle - Mathf.PI * 0.25f),
		                          Mathf.Sin(moveAngle * Mathf.Deg2Rad + externalAngle - Mathf.PI * 0.25f)).normalized *
		                          radius * distance;
		moveAngle = (moveAngle < 90.0f) ? moveAngle + arcSpeed : 90.0f;
	}
	#endregion
}