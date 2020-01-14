using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public Dropdown nameField1,
					nameField2,
					nameField3;
	#endregion

	#region Private Variables
	private GameObject pauseMenu;
	private PasswordGenerator password;
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		nameField1.value = Random.Range(0,6);
		nameField2.value = Random.Range(0,4);
		nameField3.value = Random.Range(0,4);
	}
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