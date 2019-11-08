using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camraMovement : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public GameObject player;
	#endregion

	#region Private Variables
	private Vector3 offset;
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Use this for initialization
	void Start()
	{
		if(player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}

		offset.z = -10;
	}

	// LateUpdate is called after Update each frame
	void Update()
	{
		transform.position = player.transform.position + offset; // move the camera to the players position plus offset
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion
}
