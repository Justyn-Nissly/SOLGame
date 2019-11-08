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
	public Image canvisFadeImage; // this is a black image that is on the canvas that covers the whole screen

	/// <summary>
	/// fade to clear at the start of this scene
	/// </summary>
	private void Start()
	{
		StartCoroutine(FadeToClearCoroutine());
	}

	/// <summary>
	/// fades an image to clear over time, called from a coroutine
	/// </summary>
	private void FadeToClear()
	{
		float fadeSpeed = 1f;

		canvisFadeImage.color = Color.Lerp(canvisFadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);
	}


	/// <summary>
	/// fades an image to clear over time
	/// </summary>
	public IEnumerator FadeToClearCoroutine()
	{
		canvisFadeImage.color = Color.black; // make image black

		while (canvisFadeImage.color.a >= 0.05f)
		{
			FadeToClear();

			yield return null; // wait to the next frame to continue
		}

		canvisFadeImage.color = Color.clear; // make image transparent
	}
}
