using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadSceneOnTrigger : MonoBehaviour
{
	public string sceneToLoad;

	public Image FadeImg;

	/// play fade to black coroutine when there is a collision
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// add an if statement to check if its the player that collided with the teleporter
		StartCoroutine(FadeToBlackCoroutine());
	}

	/// <summary>
	/// fade to clear at the start of this scene
	/// </summary>
	private void Start()
	{
		StartCoroutine(FadeToClearCoroutine());
	}

	/// <summary>
	/// fades an image to black over time, called from a coroutine
	/// </summary>
	private void FadeToBlack()
	{
		float fadeSpeed = 10f;

		FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
	}

	/// <summary>
	/// fades an image to clear over time, called from a coroutine
	/// </summary>
	private void FadeToClear()
	{
		float fadeSpeed = 1f;

		FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
	}

	/// <summary>
	/// fades an image to clear over time
	/// </summary>
	public IEnumerator FadeToClearCoroutine()
	{
		FadeImg.color = Color.black; // make image black

		while (FadeImg.color.a >= 0.05f)
		{
			FadeToClear();

			yield return null; // wait to the next frame to continue
		}

		FadeImg.color = Color.clear; // make image transparent
	}

	
	/// fades an image to black over time, loads "sceneToLoad" after the image is black
	public IEnumerator FadeToBlackCoroutine()
	{
		FadeImg.color = Color.clear; // make image transparent

		while (FadeImg.color.a <= 0.95f)
		{
			FadeToBlack();

			yield return null; // wait to the next frame to continue
		}
		SceneManager.LoadScene(sceneToLoad); // load this scene once the image is black
	}
}
