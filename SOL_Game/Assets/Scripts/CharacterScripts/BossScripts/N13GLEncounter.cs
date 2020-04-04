using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class N13GLEncounter : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public List<GameObject>
		guardianPartsToSpawn = new List<GameObject>(); // Guardian parts to spawn

	public GameObject
		guardian, // A reference to the guardian boss
		N13GL;    // A reference to the N13GL boss

	public DialogueManager
		n13glDialogue; // A reference to the dialogue controller

	public DialogueTrigger
		n13glTrigger; // A reference to the dialogue trigger for N13GL

	public Animator
		guardianAnimator, // The animation controller for the Guardian
		n13glAnimator;    // The animation controller for N13GL

	public Image
		canvasFadeImage; // Fade the whole screen to black

	public bool
		buildGuardian, // Check if the encounter needs to be started or not
		isSpawned;     // Check if all of the Guardian parts are spawned
	#endregion

	#region Private Variables
	private bool
		fadeToWhite; // Check if the screen needs to fade to white
	private float
		fadeTime,               // How long the fading has occurred
		fadeToClearTime = 4.0f; // How long fading to white takes
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Start is called before the first frame update
	void Start()
    {
		n13glDialogue = FindObjectOfType<DialogueManager>();
		fadeTime      = fadeToClearTime;
		fadeToWhite   = true;
		buildGuardian = false;
		isSpawned     = true;
    }

	// Update is called once per frame
	void Update()
	{
		// Check if the guardian parts need to be spawned
		if(buildGuardian == false)
		{
			if (n13glDialogue.dialogueText.text == n13glTrigger.dialogue.sentences[2])
			{
				isSpawned = false;
			}
			if (isSpawned == false)
			{
				foreach (GameObject guardianPart in guardianPartsToSpawn)
				{
					if (guardianPart.GetComponent<_2dxFX_NewTeleportation>()._Fade > 0.009f)
					{
						guardianPart.GetComponent<_2dxFX_NewTeleportation>()._Fade -= 0.007f;
					}
				}
			}

			// Start creating the guardian and fade the screen
			if (n13glDialogue.dialogueText.text == n13glTrigger.dialogue.sentences[3])
			{
				isSpawned = true;
				guardianAnimator.SetTrigger("CreateGuardian");
				buildGuardian = true;
				StartCoroutine(FadeToWhite());
			}

			if (n13glDialogue.animator.GetBool("IsOpen") == false && n13glTrigger.canActivate == false)
			{
				StartCoroutine("WaitToAttack");
				GetComponent<N13GL>().canAttack = true;
				GetComponent<N13GL>().canMove   = true;
				this.enabled                    = false;
			}
		}

    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			if(buildGuardian == false)
			{
				n13glAnimator.SetBool("IsActive", true);
			}
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Set N13GL to have the glitch shader </summary>
	private void SetGlitchEffect()
	{
		N13GL.GetComponent<Renderer>().material.shader = Shader.Find("2DxFX_Extra_Shaders/Glitch");
		N13GL.GetComponent<_2dxFX_BurnFX>().Destroyed = 1.0f;
		N13GL.GetComponent<_2dxFX_BurnFX>().Seed = 0.051f;
	}
	
	/// <summary> Spawn in the final Guardian </summary>
	private void SpawnInGuardian(GameObject guardianParts, int spawnPoint)
	{
		Debug.Log("ENEMY SPAWNED!!!!!!!");
	}

	/// <summary> Slowly fade an image to clear </summary>
	private void FadeScreen()
	{
		canvasFadeImage.color = (fadeToWhite) ? Color.Lerp(canvasFadeImage.color, Color.white, 1.6f * Time.deltaTime) : 
			                                    Color.Lerp(canvasFadeImage.color, Color.clear, 1.6f * Time.deltaTime);
		Debug.Log("work");
		if (fadeTime <= 0.1f && fadeToWhite == true)
		{
			fadeToWhite = false;
			fadeTime = fadeToClearTime;
			StartCoroutine(FadeToClear());
			foreach (GameObject guardianPart in guardianPartsToSpawn)
			{
				Destroy(guardianPart);
				/*guardianPart.SetActive(false)*/;
			}
			guardian.SetActive(true);
		}
	}
	#endregion

	#region Coroutines
	/// <summary> Fade the screen to white </summary>
	public IEnumerator FadeToWhite()
	{
		float timer = fadeToClearTime;

		// Set fade color to clear
		canvasFadeImage.color = Color.clear;

		// Pause before fading
		yield return new WaitForSeconds(.1f);

		// Fade the image frame by frame
		while (timer >= 0)
		{
			FadeScreen();
			timer -= Time.deltaTime;
			fadeTime = timer;
			yield return null;
		}
	}

	///<summary> Fade the screen to clear </summary>
	public IEnumerator FadeToClear()
	{
		float timer = fadeToClearTime;

		// Set fade color to white
		canvasFadeImage.color = Color.white;

		// Pause before fading
		yield return new WaitForSeconds(.1f);

		// Fade the image frame by frame
		while (timer >= 0)
		{
			FadeScreen();
			timer -= Time.deltaTime;
			fadeTime = timer;
			yield return null;
		}
	}

	///<summary> Wait after dialogue before attacking </summary>
	public IEnumerable WaitToAttack()
	{
		yield return new WaitForSeconds(1.0f);
	}
	#endregion
}