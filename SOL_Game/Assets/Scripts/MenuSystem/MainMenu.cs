using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private GameObject pauseMenu;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.End))
		{
			SceneManager.LoadScene("DevMenu");
		}
	}
	// Loads the game
	public void PlayGame()
    {
        SceneManager.LoadScene("Hub");
		pauseMenu = GameObject.Find("PauseMenuCanvas(Clone)");
		pauseMenu.GetComponent<Canvas>().enabled = false;
	}


    // Quits the game
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}