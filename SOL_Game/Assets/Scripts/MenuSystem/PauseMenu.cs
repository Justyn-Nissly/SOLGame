using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public static bool       isPaused = false; // The value for if the game is paused
	public static GameObject pauseMenuUI;      // The UI object for the pause menu
	Scene                    activeScene;      // The scene that is currently active
	public string            sceneName;        // The name of the acctive scene
	#endregion

	#region Private Variables

	#endregion

	// Unity Named Methods
	#region Main Methods
	void Update()
	{
		activeScene = SceneManager.GetActiveScene();
		sceneName = activeScene.name;
		if (sceneName == "Menu")
		{
			pauseMenuUI.GetComponent<Canvas>().enabled = false;
		}
		//pauseSingelton = singleton.
		// Check if the escape key was pressed and if the game was paused resume, otherwise pause the game
		else
		{
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
		}

		// Load the development menu when 'End' is pressed
		if (Input.GetKeyDown(KeyCode.End))
		{
			SceneManager.LoadScene("DevMenu");
		}
	}
	#endregion

	#region Utility Methods
	/// Resume the game
	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		pauseMenuUI.GetComponent<Canvas>().enabled = false;
		Time.timeScale = 1.0f;
		isPaused = false;
	}

	/// Pause the game
	void Pause()
	{
		pauseMenuUI.SetActive(true);
		pauseMenuUI.GetComponent<Canvas>().enabled = true;
		Time.timeScale = 0.0f;
		isPaused = true;
	}

	/// Quit to the main menu
	public void QuitGame()
	{
		SceneManager.LoadScene("Menu");
	}
	#endregion

	#region Coroutines

	#endregion
}