using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

			// Keep items persisting across scenes
			DontDestroyOnLoad(gameObject);

			AddGameObjectsToDontDestroy("EventSystem", "AudioManager", "DialogueManager", "Player", "PlayerHealthCanvas", "HUD");
		}
		else
		{
			Destroy(this); // or gameObject
		}

		// load the menu if you are starting in the Start scene
		if (SceneManager.GetActiveScene().name == "Start")
		{
			SceneManager.LoadScene("Menu");
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> this finds and calls DontDestroyOnLoad() on all passed in game object names </summary>
	void AddGameObjectsToDontDestroy(params string[] GOnames)
	{
		foreach (string GOname in GOnames)
		{
			GameObject persistentGO = GameObject.Find(GOname);

			if (persistentGO != null)
			{
				DontDestroyOnLoad(persistentGO);
			}
		}
	}
	#endregion

	#region Coroutines
	#endregion
}