using System.Collections;
using System.Collections.Generic;
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
		revolutionDistance, // How far this object revolves around its host
		revolutionSpeed,    // How fast this object revolves around its host
		startAngle;         // Where to begin revolving from
	#endregion

	#region Private Variables
	private float
		angle; // Current angle of rotation
	private Rigidbody2D
		body;
	#endregion

	// Unity Named Methods
	#region Unity Main Methods
	void Start()
	{
		angle = startAngle;
	}

	void FixedUpdate()
	{
		Revolve(ref angle);
	}
	#endregion

	#region Utility Functions
	// Patrol  in a circular pattern
	public void Revolve(ref float angle)
	{
		body.MovePosition((Vector2)revolutionObject.transform.position +
						  new Vector2(Mathf.Cos(revolutionDistance * angle * Mathf.Deg2Rad),
									  Mathf.Sin(revolutionDistance * angle * Mathf.Deg2Rad)) * Time.deltaTime);

		// Update the angle
		if (clockwise)
		{
			angle -= revolutionSpeed;
		}
		else
		{
			angle += revolutionSpeed;
		}
		if (angle > 360.0f)
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