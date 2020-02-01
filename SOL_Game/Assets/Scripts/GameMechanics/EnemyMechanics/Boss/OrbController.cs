using UnityEngine;

public class OrbController : MonoBehaviour
{
	#region Enums and Defined Constants
	private const int
		ATTACK_HP = 2; // The orb attacks when its health drops to this
	#endregion

	#region Public Variables
	public float
		a1,
		a2;
	#endregion

	#region Private Variables
	private LockOnProjectile
		projectileScript; // Control the lock on projectile script
	private RevolveAroundObject
		revolveScript; // Control the revolve around object script
	private DestructibleObject
		destructible; // Control the destructible object script
	private bool
		isAttacking; // Check if the orb is attacking
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Set up access to the object's scripts to control its behavior
	void Start()
	{
		projectileScript = GetComponent<LockOnProjectile>();
		revolveScript    = GetComponent<RevolveAroundObject>();
		destructible     = GetComponent<DestructibleObject>();
		isAttacking      = false;
	}

	/// <summary> Lock on to the target and arc towards it </summary>
	void FixedUpdate()
	{
		// The orb will attack when its health gets low enough
		if (destructible.health <= ATTACK_HP)
		{
			isAttacking = true;
		}
		if (/*isAttacking && */checkAngle())
		{
			Debug.Log("Ha! Goteem!");
		}
	}
	#endregion

	#region Utility Methods
	bool checkAngle()
	{
		return (true);
/*		a1 = Vector2.Angle(projectileScript.targetPos - (Vector2)projectileScript.transform.position, Vector2.up);
		a2 = Vector2.Angle(new Vector2(Mathf.Cos(revolveScript.angle * Mathf.Deg2Rad),
		                               Mathf.Sin(revolveScript.angle * Mathf.Deg2Rad)), Vector2.up);
Debug.Log("a1 = " + a1 + "           a2 = " + a2);
		return (a1 == a2);*/
	}

	#endregion

	#region Coroutines (Empty)
	#endregion
}