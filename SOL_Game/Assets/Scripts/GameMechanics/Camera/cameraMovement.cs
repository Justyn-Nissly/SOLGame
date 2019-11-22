using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	private Transform
		target; // Center the camera on the player
	private float
		smoothing = 0.05f; // Make camera movement smooth
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Assign the camera to follow the player </summary>
	private void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	/// <summary> Make the camera follow the player at the end of a frame </summary>
	private void LateUpdate()
	{
		// The camera doesn't move if already centered on the player
		if (transform.position != target.position)
		{
			transform.position = Vector3.Lerp(transform.position,
											  new Vector3(target.position.x, target.position.y, transform.position.z),
											  smoothing);
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}