using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public static bool       isPaused = false;  // The value for if the game is paused
	public GameObject pauseMenuUI;       // The UI object for the pause menu
	public string            sceneName;         // The name of the acctive scene
												//public Canvas pauseMenuCanvas; // The canvas that houses the controls for the pause menu
												//public Signal pauseSignal;
	#endregion

	#region Private Variables

	#endregion
	
	// Unity Named Methods
	#region Main Methods
	void Update()
	{
		sceneName = SceneManager.GetActiveScene().name;
		if (sceneName != "Menu")
		{
			/// Check if the escape key was pressed and if the game was paused resume, otherwise pause the game
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (isPaused)
				{
					Resume();
				}
				else
				{
					Pause();
				}
			}

			/// Load the development menu when 'End' is pressed
			if (Input.GetKeyDown(KeyCode.End))
			{
				SceneManager.LoadScene("DevMenu");
			}
		}
	}
	#endregion

	#region Utility Methods
	/// Instantiates the pause menu when the player presses escape
	public void InstantiatePauseMenu()
	{
		if (GameObject.Find("PauseMenuCanvas(Clone)") == null)
		{
			Instantiate(pauseMenuUI, new Vector3(0, 0, 0), Quaternion.identity);
		}
	}
	/// Resume the game
	public void Resume()
	{
		Destroy(GameObject.Find("PauseMenuCanvas(Clone)"));
		Time.timeScale = 1.0f;
		isPaused = false;	
	}

	/// Pause the game
	void Pause()
	{
		InstantiatePauseMenu();
		Time.timeScale = 0.0f;
		isPaused = true;
	}

	/// Quit to the main menu
	public void QuitGame()
	{
		SceneManager.LoadScene("Menu");
		
		Time.timeScale = 1.0f;
		isPaused = false;
		Destroy(GameObject.Find("PauseMenuCanvas(Clone)"));
	}
	#endregion

	#region Coroutines

	#endregion
}