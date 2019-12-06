using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public static GameManager gameManager;     // The game manager object keeps track of the items that need to persist across scenes
	public        Canvas      pauseMenuCanvas; // The canvas that houses the controls for the pause menu
	#endregion

	#region Private Variables
  
	#endregion
	// Unity Named Methods
	#region Main Methods
	void Awake()
	{
		if (gameManager == null)
		{
			gameManager = this;
			/*
			-Player health
			-Bosses defeated
			-Doors unlocked
			-Upgrades
			-Weapons unlocked

			(Puzzles)
			-Player position*/
			/// Keep items persisting across scenes
			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(GameObject.Find("EventSystem"));
			DontDestroyOnLoad(GameObject.Find("AudioManager"));
			DontDestroyOnLoad(GameObject.Find("DialogueManager"));
		}
		else
		{
			Destroy(this); // or gameObject
		}
	}
	#endregion

	#region Utility Methods
	/// Save the game
	/// SaveGame()
	/// {
	/// 
	/// }
	
	/// Save the player's current state to start a puzzle
	/// SaveState()
	/// {
	/// 
	/// }
	#endregion

	#region Coroutines
	#endregion
}