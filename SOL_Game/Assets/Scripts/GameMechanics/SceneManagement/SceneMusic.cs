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

	#region Protected Variables
	protected Player
		player;
	#endregion

	// Unity Named Methods
	#region Main Methods
	protected virtual void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	protected virtual void FixedUpdate()
	{
		if (playThisSong)
		{
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, player.transform.position,
			                                             Time.deltaTime * 0.75f);
		}
		else
		{
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, muteLocation.transform.position,
			                                             Time.deltaTime * 0.25f);
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			playThisSong = true;
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D collider)
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