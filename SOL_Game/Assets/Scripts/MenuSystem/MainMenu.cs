using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

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
        SceneManager.LoadScene("Sif-fiDevRoom");
    }


    // Quits the game
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}