using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttactOrientationControllerEnemy : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	#endregion

	#region Private Variables
	private GameObject gameObjectToLookAt;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (gameObjectToLookAt != null)
		{
			LookInEightDirectionOfGameObject();
		}
	}

	private void Start()
	{
		gameObjectToLookAt = GameObject.FindGameObjectWithTag("Player").gameObject;
	}
	#endregion

	#region Utility Methods
	public void LookInEightDirectionOfGameObject()
	{
		// This makes the game object that this script is attached to rotate on the z axis to look at the game object to look at
		// and only lets that game object "look" in eight directions
		Vector3 dir = gameObjectToLookAt.transform.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		// Make positive if negative
		if (angle < 0)
		{
			angle += 360;
		}

		// Round to the closest angle in eight directions
		if (angle >= 0 && angle < 22.5f || angle >= 337.5f && angle < 360)
		{
			angle = 0;
		}
		else if (angle >= 22.5f && angle < 67.5f)
		{
			angle = 45;
		}
		else if (angle >= 67.5f && angle < 112.5f)
		{
			angle = 90;
		}
		else if (angle >= 112.5f && angle < 157.5f)
		{
			angle = 135;
		}
		else if (angle >= 157.5f && angle < 202.5f)
		{
			angle = 180;
		}
		else if (angle >= 202.5f && angle < 247.5f)
		{
			angle = 225;
		}
		else if (angle >= 247.5f && angle < 292.5f)
		{
			angle = 270;
		}
		else
		{
			angle = 315;
		}

		// apply rotation
		transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
	}
	#endregion

	#region Coroutines
	#endregion
}
