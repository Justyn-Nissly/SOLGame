using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// to use this put ONE fade black to clear on any game object in the scene
/// and give it a reference to an image that covers all of a canvas
/// </summary>
public class FadeBlackToClear : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public Image canvisFadeImage; // this is a black image that is on the canvas that covers the whole screen
	#endregion

	#region Private Variables
	private float fadeToClearTime = 2f;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary>
	/// fade to clear at the start of this scene
	/// </summary>
	private void Start()
	{
		StartCoroutine(FadeToClearCoroutine());
	}
	#endregion

	#region Utility Methods
	/// <summary>
	/// fades an image to clear over time, called from a coroutine
	/// </summary>
	private void FadeToClear()
	{
		float fadeSpeed = 1.2f;

		canvisFadeImage.color = Color.Lerp(canvisFadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	#endregion

	#region Coroutines
	/// <summary>
	/// fades an image to clear over time
	/// </summary>
	public IEnumerator FadeToClearCoroutine()
	{
		float timer = fadeToClearTime;

		canvisFadeImage.color = Color.black; // make image black

		yield return new WaitForSeconds(.1f); // add a pause before starting the fade to clear

		while (timer >= 0)
		{
			FadeToClear();
			timer -= Time.deltaTime;

			yield return null; // wait to the next frame to continue
		}
	}
	#endregion
}
