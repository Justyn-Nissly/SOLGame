using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// new camera movement script
/// </summary>
public class cameraMovement : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	#endregion

	#region Private Variables
	private Transform target; // the target the camera will follow, the player
	private float smoothing = 0.05f; // how close should the camera follow the player
	#endregion

	// Unity Named Methods
	#region Main Methods

	private void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void LateUpdate()
	{
		if (transform.position != target.position)
		{
			Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
		}
	}

	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion
}
