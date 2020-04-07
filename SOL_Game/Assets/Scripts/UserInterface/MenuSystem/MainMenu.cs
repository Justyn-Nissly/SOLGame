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

	#endregion

	#region Private Variables
	private GameObject pauseMenu;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void Start()
	{
		GameObject.Find("A").GetComponent<Button>().Select();

		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		if(player != null)
		{
			player.FreezePlayer();
			player.canvasFadeImage.color = Color.clear; // make image transparent
		}
	}

	/// Check every frame if the user has hit the "end" key to open the developer menu
	void Update()
	{
		/*if (Input.GetKeyDown(KeyCode.End))
		{
			SceneManager.LoadScene("DevMenu");
		}*/
	}
	#endregion

	#region Utility Methods
	/// Loads the game
	public void PlayGame()
	{
		//FindObjectOfType<AudioManager>().StartBackground();
		SceneManager.LoadScene("Hub");
		LoadPlayerHub.UseBeginningPostion = true;
	}

	/// continues the game from the last scene the player was in before dying
	public void ContinueGame()
	{
		//FindObjectOfType<AudioManager>().StartBackground();
		SceneManager.LoadScene(Globals.sceneToLoad);
		LoadPlayerHub.UseBeginningPostion = true;
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