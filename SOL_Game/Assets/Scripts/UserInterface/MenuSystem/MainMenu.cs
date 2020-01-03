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
	/// Check every frame if the user has hit the "end" key to open the developer menu
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.End))
		{
			SceneManager.LoadScene("DevMenu");
		}
	}
	#endregion

	#region Utility Methods
	/// Create a new Game
	public void NewGame()
	{
		//FindObjectOfType<AudioManager>().StartBackground();
		SceneManager.LoadScene("Hub");
	}

	/// Load a game save
	public void LoadGame()
	{

	}

	/// Quits the game
	public void QuitGame()
	{
		Debug.Log("QUIT");
		Application.Quit();
	}
	#endregion
 
	#region Coroutines

	#endregion
}