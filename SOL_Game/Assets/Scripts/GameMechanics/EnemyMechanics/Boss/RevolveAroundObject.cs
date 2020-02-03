using UnityEngine;

public class RevolveAroundObject : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public bool
		clockwise; // The enemy is patrolling clockwise
	public GameObject
		revolutionObject; // The object to revolve around
	public float
		angle,                 // Current angle of rotation
		maxRevolutionDistance, // Maximum distance this object revolves around its host
		minRevolutionDistance, // Minimum distance this object revolves around its host
		revolutionSpeed,       // How fast this object revolves around its host
		startAngle;            // Where to begin revolving from
	public int
		contactDamage; // Damage dealt to the player on contact
	#endregion

	#region Private Variables
	private Player
		player; // Reference the player
	private float
		oscillation,        // Direction of oscillation
		revolutionDistance; // How far this object revolves around its host
	#endregion

	// Unity Named Methods
	#region Unity Main Methods
	/// <summary> Get the object ready to start revolving </summary>
	void Start()
	{
		angle              = startAngle;
		oscillation        = 1.0f;
		revolutionDistance = minRevolutionDistance + 0.1f;
		player             = FindObjectOfType<Player>();
	}

	/// <summary> Make the object revolve around its host within a distance range </summary>
	void FixedUpdate()
	{
		// Keep this object a certain range away from its host
		if (revolutionDistance >= maxRevolutionDistance || revolutionDistance <= minRevolutionDistance)
		{
			oscillation *= -1.0f;
		}

		// Update the revolution distance
		revolutionDistance += minRevolutionDistance * oscillation * Time.deltaTime;
		Revolve(ref angle);
	}

	/// <summary> Damage the player on contact </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && contactDamage > 0)
		{
			// Replace the line below with actual damage code
			player.TakeDamage(contactDamage, false);
		}
	}
	#endregion

	#region Utility Functions
	/// <summary> Make an object revolve around another object </summary>
	public void Revolve(ref float angle)
	{
		if (revolutionObject != null)
		{
			// Move this object around its host object
			transform.position = ((Vector2)revolutionObject.transform.position +
								  new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
											  Mathf.Sin(angle * Mathf.Deg2Rad)) * revolutionDistance);
		}
		else
		{
			Destroy(gameObject);
		}

		// Update the revolution angle by its direction
		if (clockwise)
		{
			angle -= revolutionSpeed;
		}
		else
		{
			angle += revolutionSpeed;
		}

		// Keep it 0 <= angle < 360
		if (angle >= 360.0f)
		{
			angle -= 360.0f;
		}
		if (angle < 0.0f)
		{
			angle += 360.0f;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}