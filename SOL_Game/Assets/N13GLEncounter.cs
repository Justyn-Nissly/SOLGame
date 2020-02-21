using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N13GLEncounter : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public List<GameObject>
		guardianPartsToSpawn = new List<GameObject>(), // Guardian parts to spawn
		partsSpawnpoint      = new List<GameObject>(); // Spawn points for the Guardian parts
	public GameObject
		N13GL;         // A reference to the N13GL boss
	public DialogueManager
		n13glDialogue; // A reference to the dialogue controler
	public DialogueTrigger
		n13glTrigger; // A reference to the dialogue trigger for N13GL
	public bool
		buildGuardian, // Check if the encounter needs to be started or not
		isSpawned;     // Check if all of the Guardian parts are spawned
	public Animator
		guardianAnimator, // The animation controler for the Guardian
		n13glAnimator;    // The animation controler for N13GL
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
    // Start is called before the first frame update
    void Start()
    {
		buildGuardian = false;
		isSpawned     = false;
    }

	// Update is called once per frame
	void Update()
	{
		if (n13glDialogue.dialogueText.text == n13glTrigger.dialogue.sentences[2] && isSpawned == false)
		{
			foreach (GameObject guardianPart in partsSpawnpoint)
			{
				if (guardianPart.GetComponent<_2dxFX_NewTeleportation>()._Fade > 0.009f)
				{
					guardianPart.GetComponent<_2dxFX_NewTeleportation>()._Fade -= 0.007f;
				}
			}
		}

		if (n13glDialogue.animator.GetBool("IsOpen") == false && n13glTrigger.canActivate == false)
		{
			isSpawned = true;
			guardianAnimator.SetTrigger("DoThing");
			buildGuardian = false;
		}


    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(buildGuardian == false)
		{
			n13glAnimator.SetBool("IsActive", true);
		}
	}
	#endregion

	#region Utility Methods
	public void StartGuardianCreation()
	{
		N13GL.GetComponent<Renderer>().material.shader = Shader.Find("2DxFX_Extra_Shaders/Glitch");
		N13GL.GetComponent<_2dxFX_BurnFX>().Destroyed = 1.0f;
		N13GL.GetComponent<_2dxFX_BurnFX>().Seed = 0.051f;
	}
	private void SpawnInGuardian(GameObject guardianParts, int spawnPoint)
	{
		// instantiate the current enemy in the loop at a spawn point and remove that spawn point from the list of spawn points
		GameObject tempEnemy = Instantiate(guardianParts, partsSpawnpoint[spawnPoint].transform.position, new Quaternion(0, 0, 0, 0));
		Debug.Log("ENEMY SPAWNED!!!!!!!");
	}
	#endregion

	#region Coroutines
	/// <summary> method to spawn in this spawner's enemies </summary>
	/// <param name="numberOfEnemiesToSpawn">only fill in if you want to limit the number of spawned in enemies
	/// (the reason the default value is 100 is because that is a limit that we will never reach)</param>
	public IEnumerator SpawnGuardianParts(int numberOfEnemiesToSpawn = 7)
	{
		// spawn in each enemy in the enemies to spawn list
		foreach (GameObject guardianPart in partsSpawnpoint)
		{
			while(guardianPart.GetComponent<_2dxFX_NewTeleportation>()._Fade > 0.009f)
			{
				guardianPart.GetComponent<_2dxFX_NewTeleportation>()._Fade -= 0.07f;
			}
			yield return new WaitForSeconds(.5f); // spawn in an enemy every N seconds
		}
	}
	#endregion
}