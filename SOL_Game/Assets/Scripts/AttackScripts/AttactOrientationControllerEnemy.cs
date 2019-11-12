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
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 22.5f;


		if (angle < 0f)
		{
			angle += 360f;
		}
		else if (angle >= 360f)
		{
			angle -= 360f;
		}

		// Make positive if negative
		if (angle < 0)
		{
			angle += 360;
		}

		angle = (float)(((int)angle) / 45) * 45.0f;

		// apply rotation
		transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
	}
	#endregion

	#region Coroutines
	#endregion
}
