using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables

	#endregion

	#region Private Variables
	private GameObject pauseMenu;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// Check every frame for the "end" key to open to developer menu
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.End))
		{
			SceneManager.LoadScene("DevMenu");
		}
	}
	#endregion

	#region Utility Methods
	/// Load the game
	public void PlayGame()
	{
		SceneManager.LoadScene("Hub");
	}

	/// Quit the game
	public void QuitGame()
	{
		Debug.Log("QUIT");
		Application.Quit();
	}
	#endregion

	#region Coroutines

	#endregion
}