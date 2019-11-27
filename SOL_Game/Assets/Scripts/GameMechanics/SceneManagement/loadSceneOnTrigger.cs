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
		onTeleportStartInBeginingPosition = true;
	public Image
		canvasFadeImage; // Fades the whole screen to black
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Fade to black on collision </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Change scenes if the player enters the trigger
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
		float fadeSpeed = 10f;

		canvisFadeImage.color = Color.Lerp(canvisFadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
	}
	#endregion

	#region Coroutines
	/// <summary> Load the scene after fading to black </summary>
	public IEnumerator FadeToBlackCoroutine()
	{
		canvisFadeImage.color = Color.clear; // make image transparent

		while (canvisFadeImage.color.a <= 0.95f)
		{
			FadeToBlack();

			yield return null; // wait to the next frame to continue
		}

		GlobalVarablesAndMethods.startInBeginingPosition = onTeleportStartInBeginingPosition;
		SceneManager.LoadScene(sceneToLoad); // load this scene once the image is black
	}
	#endregion
}