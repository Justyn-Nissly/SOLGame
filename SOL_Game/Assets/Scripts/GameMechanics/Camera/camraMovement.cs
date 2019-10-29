using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camraMovement : MonoBehaviour
{
	public GameObject player;

	private Vector3 offset;

	// Use this for initialization
	void Start()
	{
		offset = transform.position - player.transform.position;
	}

	// LateUpdate is called after Update each frame
	void Update()
	{
		transform.position = player.transform.position + offset; // move the camera to the players position plus offset
	}
}
