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

			/// Instantiate the pause menu canvas
			PauseMenu.pauseMenuUI = Instantiate(pauseMenuCanvas, new Vector3(0, 0, 0), Quaternion.identity).gameObject;

			/// Keep items persisting across scenes
			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(PauseMenu.pauseMenuUI);
			DontDestroyOnLoad(GameObject.Find("EventSystem"));
		}
		else
		{
			Destroy(this); // or gameObject
		}
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion
}
