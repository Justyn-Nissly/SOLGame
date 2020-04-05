using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public bool
		playThisSong;
	public GameObject
		muteLocation,
		songObject;
	#endregion

	#region Private Variables
	private Camera
		sceneCamera;
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Awake()
	{
		sceneCamera = FindObjectOfType<Camera>();
	}

	void FixedUpdate()
	{
		if (playThisSong)
		{
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, sceneCamera.transform.position,
			                                             Time.deltaTime);
		}
		else
		{
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, muteLocation.transform.position,
			                                             Time.deltaTime * 0.16f);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			playThisSong = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			playThisSong = false;
		}
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines (Empty)
	#endregion
}