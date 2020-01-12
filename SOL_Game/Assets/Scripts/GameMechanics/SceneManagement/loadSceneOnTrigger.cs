using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadSceneOnTrigger : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public string
		sceneToLoad; // The name of the scene to load when triggered
	public bool
		onTeleportStartInBeginingPosition = true; // Rename "teleportToStart"?
	public Image
		canvasFadeImage; // Fades the whole screen to black
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Fade to black on collision </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Change scenes if the player enters a scene-change trigger
		if (collision.CompareTag("Player"))
		{
			StartCoroutine(FadeToBlackCoroutine());
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Fade slowly to black </summary>
	private void FadeToBlack()
	{
		canvasFadeImage.color = Color.Lerp(canvasFadeImage.color, Color.black, 10.0f * Time.deltaTime);
	}
	#endregion

	#region Coroutines
	/// <summary> Load the scene after fading to black </summary>
	public IEnumerator FadeToBlackCoroutine()
	{
		canvasFadeImage.color = Color.clear; // make image transparent

		while (canvasFadeImage.color.a <= 0.95f)
		{
			FadeToBlack();

			yield return null; // wait to the next frame to continue
		}

		// Set the player's starting position
		GlobalVarablesAndMethods.startInBeginingPosition = onTeleportStartInBeginingPosition;

		// Load the new scene after the screen fades to black
		SceneManager.LoadScene(sceneToLoad);
	}
	#endregion
}