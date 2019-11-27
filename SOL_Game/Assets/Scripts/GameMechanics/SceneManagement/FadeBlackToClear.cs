using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlackToClear : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Image
		canvasFadeImage; // Fade the whole screen to black
	#endregion

	#region Private Variables
	private float
		fadeToClearTime = 2f; // How long fading to black takes
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Fade into the scene </summary>
	private void Start()
	{
		StartCoroutine(FadeToClearCoroutine());
	}
	#endregion

	#region Utility Methods
	/// <summary> Slowly fade an image to clear </summary>
	private void FadeToClear()
	{
		canvasFadeImage.color = Color.Lerp(canvasFadeImage.color, Color.clear, 1.2f * Time.deltaTime);
	}
	#endregion

	#region Coroutines
	/// <summary> Slowly fade an image to clear </summary>
	public IEnumerator FadeToClearCoroutine()
	{
		float timer = fadeToClearTime;

		// Set fade color to black
		canvasFadeImage.color = Color.black;

		// Pause before fading
		yield return new WaitForSeconds(.1f);

		// Fade the image frame by frame
		while (timer >= 0)
		{
			FadeToClear();
			timer -= Time.deltaTime;
			yield return null;
		}
	}
	#endregion
}