using UnityEngine;

public class OrbController : MonoBehaviour
{
	#region Enums and Defined Constants
	private const float
		ATTACK_SPEED_MULTIPLIER = 2.0f; // Match attack speed to revolution speed
	#endregion

	#region Public Variables
	public float
		attackHP; // The orb starts attacking when its health drops to this value
	public LockOnMissile
		missileScript; // Control the lock on missile script
	public RevolveAroundObject
		revolveScript; // Control the revolve around object script
	public DestructibleObject
		destructible; // Control the destructible object script
	public GameObject
		target; // Missiles target this
	#endregion

	#region Private Variables
	private bool
		isAttacking; // Check if the orb is attacking
	private float
		targetAngle,
		movementAngle;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Make the orb passive and set up access to its scripts to control its behavior </summary>
	void Start()
	{
		isAttacking   = false;
	}

	/// <summary> Revolve until the orb has taken enough damage </summary>
	void FixedUpdate()
	{
		// Prevent the orb from trying to revolve and attack simultaneously
		ControlMovement();

		// The orb will be ready to attack when its health gets low enough
		if (destructible.health <= attackHP && isAttacking == false)
		{
			// The orb waits to attack the player until its trajectory matches the player's direction
			if (checkAngle())
			{
				isAttacking = true;
				missileScript.moveSpeed = revolveScript.revolutionSpeed * ATTACK_SPEED_MULTIPLIER;
			}
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Check if the orb's trajectory is in the player's direction </summary>
	bool checkAngle()
	{
		// Find the angles to compare
		targetAngle    = (Mathf.Atan2(target.transform.position.y - transform.position.y,
									  target.transform.position.x - transform.position.x) *
		                  Mathf.Rad2Deg + 360.0f + ((revolveScript.clockwise) ? 90.0f :-90.0f)) % 360.0f;
		movementAngle  = ((revolveScript.angle + 360.0f) % 360.0f);

		// Prevent the loop from 360 to 0 from altering the calculation
		movementAngle += ((targetAngle - movementAngle <= 180.0f) ? 0.0f : 360.0f);
		targetAngle   += ((movementAngle - targetAngle <= 180.0f) ? 0.0f : 360.0f);

		return (Mathf.Abs(targetAngle - movementAngle) <= 3.0f);
	}

	/// <summary> Prevent the orb from trying to revolve and attack simultaneously </summary>
	void ControlMovement()
	{
		// If the orb is passive it does not act like a missile
		missileScript.enabled = isAttacking;
		
		// If the orb is ready to attack disable revolving
		if (revolveScript.enabled == true && isAttacking)
		{
			missileScript.enabled       = true;
			revolveScript.enabled       = false;
			revolveScript.contactDamage = 0;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}