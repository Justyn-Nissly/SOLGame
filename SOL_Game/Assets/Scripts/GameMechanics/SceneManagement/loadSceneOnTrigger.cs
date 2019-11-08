using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadSceneOnTrigger : MonoBehaviour
{
	public string sceneToLoad; // the name of the scene that will be loaded when the player enters this trigger

	public Image canvisFadeImage; // this is a black image that is on the canvas that covers the whole screen

	/// play fade to black coroutine when there is a collision
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// only change scenes if its the player that entered the trigger
		if (collision.CompareTag("Player"))
		{
			StartCoroutine(FadeToBlackCoroutine());
		}
	}

	/// <summary>
	/// fades an image to black over time, called from a coroutine
	/// </summary>
	private void FadeToBlack()
	{
		float fadeSpeed = 10f;

		canvisFadeImage.color = Color.Lerp(canvisFadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
	}

	
	/// fades an image to black over time, loads "sceneToLoad" after the image is black
	public IEnumerator FadeToBlackCoroutine()
	{
		canvisFadeImage.color = Color.clear; // make image transparent

		while (canvisFadeImage.color.a <= 0.95f)
		{
			FadeToBlack();

			yield return null; // wait to the next frame to continue
		}
		SceneManager.LoadScene(sceneToLoad); // load this scene once the image is black
	}
}
