using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables (Empty)
	#endregion

	#region Main Methods
	private void Start()
	{
		PlayerControls inputActions = new PlayerControls(); // this in the reference to the new unity input system
		inputActions.Gameplay.Enable();

		inputActions.Gameplay.PauseMenu.started += _ => LoadMainMenu(); // the pause menu event happens if the player clicks esc key, start controller button, or the select controller button
	}
	#endregion

	#region  Utility Methods
	/// <summary> this ends the credits by changing to the main menu (called at the end of the credits through an animation event and if esc key is pressed)</summary>
	public void LoadMainMenu()
	{
		SceneManager.LoadScene("Menu");
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
