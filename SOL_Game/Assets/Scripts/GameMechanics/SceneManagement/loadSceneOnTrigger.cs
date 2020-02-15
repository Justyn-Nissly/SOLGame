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

	#region Private Variables
	private _2dxFX_NewTeleportation2
		teleportScript;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Fade to black on collision </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Change scenes if the player enters a scene-change trigger
		if (collision.CompareTag("Player"))
		{
			teleportScript = collision.gameObject.GetComponent<_2dxFX_NewTeleportation2>();
			StartCoroutine(TeleportOutPlayer());
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

	/// <summary> plays the teleport out shader effect </summary>
	private IEnumerator TeleportOutPlayer()
	{
		float percentageComplete = 0;

		// make the player invisible, this is not set by default in the prefab because
		// then the player would be invisible in Dev rooms because they don't have this script running in them
		teleportScript._Fade = 0;

		GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().FreezePlayer();

		// teleport the player in, it does this by "sliding" a float from 0 to 1 over time
		while (percentageComplete < 1)
		{
			teleportScript._Fade = Mathf.Lerp(0f, 1f, percentageComplete);
			percentageComplete += Time.deltaTime;
			yield return null;
		}

		// set the player to be invisible because the float value might not be exactly 1
		teleportScript._Fade = 1;

		// start fading the screen to black
		StartCoroutine(FadeToBlackCoroutine());
	}
	#endregion
}