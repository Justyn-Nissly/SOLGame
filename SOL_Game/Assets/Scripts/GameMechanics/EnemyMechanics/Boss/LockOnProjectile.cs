using UnityEngine;

public class LockOnProjectile : MonoBehaviour
{
	#region Enums and Defined Constants
	private const float
		DEFAULT_RANGE = 10.0f; // The default effective range
	#endregion

	#region Public Variables
	public float
		effectiveRange, // How far the projectile can effectively track the target from
		moveSpeed,      // Projectile speed
		offset;         // How far the projectile veers to the side
	public bool
		veerLeft; // Determines whether the projectile veers left or right
	public GameObject
		target; // The projectile locks on to this
	public Vector2
		direction, // The projectile's actual direction of movement (not necessarily directly towards the target)
		targetPos; // The target's position
	#endregion

	#region Private/Protected Variables
	private float
		moveAngle; // Angle the projectile will move at
    #endregion

    // Unity Named Methods
    #region Main Methods
    // Initalize the projectile
    void Start()
	{
		// Initial lock on
        targetPos = target.transform.position;

		// The effective range cannot be 0; assign it a default range if 0
		if (effectiveRange == 0.0f)
		{
			effectiveRange = DEFAULT_RANGE;
		}
	}

	/// <summary> Lock on to the target and arc towards it </summary>
	void FixedUpdate()
	{
		targetPos = target.transform.position;
		SetAngle();
		Arc();
	}
	#endregion

	#region Utility Methods
	/// <summary> Set the projectile angle to make it arc towards the target </summary>
	void SetAngle()
	{
		// The projectile angles towards a distance from and perpendicular to the target
		moveAngle = Mathf.Atan2(targetPos.y - transform.position.y,
		                        targetPos.x - transform.position.x) +
		                      ((veerLeft) ? 90 : -90) * Mathf.Deg2Rad;

		// The projectile angles closer to the target as it approaches
		// Offset affects arc width and effective range determines how easily the projectile follows the target
		direction = new Vector2(Mathf.Cos(moveAngle), Mathf.Sin(moveAngle)) * offset / effectiveRange *
		                                              Vector2.Distance(targetPos, transform.position);
	}

	/// <summary> Make the projectile arc towards the target </summary>
	void Arc()
	{
		transform.position = Vector2.MoveTowards(transform.position, targetPos + direction, moveSpeed * Time.deltaTime);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}