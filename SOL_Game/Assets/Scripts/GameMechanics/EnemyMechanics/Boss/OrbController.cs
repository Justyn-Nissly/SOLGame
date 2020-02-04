using UnityEngine;

public class OrbController : MonoBehaviour
{
	#region Enums and Defined Constants
	private const float
		ATTACK_SPEED_MULTIPLIER = 4.0f; // Match attack speed to revolution speed
	#endregion

	#region Public Variables
	public float
		attackHP; // The orb starts attacking when its health drops to this value
	#endregion

	#region Private Variables
	private LockOnMissile
		missileScript; // Control the lock on missile script
	private RevolveAroundObject
		revolveScript; // Control the revolve around object script
	private DestructibleObject
		destructible; // Control the destructible object script
	private bool
		isAttacking; // Check if the orb is attacking
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Make the orb passive and set up access to its scripts to control its behavior </summary>
	void Start()
	{
		missileScript = GetComponent<LockOnMissile>();
		revolveScript = GetComponent<RevolveAroundObject>();
		destructible  = GetComponent<DestructibleObject>();
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
		return (Mathf.Abs(((revolveScript.angle - ((revolveScript.clockwise) ? 90.0f : -90.0f)) % 360.0f) -
		                   (Mathf.Atan2(missileScript.targetPos.y - transform.position.y,
		                                missileScript.targetPos.x - transform.position.x) * Mathf.Rad2Deg)) <= 3.0f);
	}

	/// <summary> Prevent the orb from trying to revolve and attack simultaneously </summary>
	void ControlMovement()
	{
		// If the orb is passive it does not act like a missile
		if (missileScript.enabled == true && isAttacking == false)
		{
			missileScript.enabled = false;
		}

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