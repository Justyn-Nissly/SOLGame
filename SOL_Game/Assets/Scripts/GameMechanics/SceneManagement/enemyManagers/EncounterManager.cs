using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject
		enemyToSpawn,
		spawnPoint;
	public bool
		panCamera = true;
	#endregion

	#region Private Variables
	private bool
		firstTimeTriggerEntered = true,
		connectQuestItem = true,
		questItemFound = false,
		ended = false;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (questItemFound == false)
		{
			if (questItemFound = (GameObject.FindWithTag("QuestItem") != null))
			{
				connectQuestItem = false;
			}
		}
		if (ended)
		{
			EndEncounter();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && firstTimeTriggerEntered) // make sure its the player that triggered this and it was there first time
		{
			firstTimeTriggerEntered = false; // set flag
			GetComponent<DoorManager>().LockDoors(); // lock all doors so the player cant leave this boss fight

			// start panning the camera to the bosses spawn point
			if (panCamera)
			{
				GameObject.Find("Main Camera").GetComponent<cameraMovement>().PanCameraToLocation(spawnPoint, 2, 1, 1f);
			}

			// spawn in the boss
			StartCoroutine(SpawnInEnemy());
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> end the boss fight by unlocking the doors</summary>
	public void EndEncounter()
	{
		ended = true;
		if (connectQuestItem == false && (GameObject.FindWithTag("QuestItem") == null))
		{
			GetComponent<DoorManager>().UnlockDoors();
			Destroy(gameObject);
		}
	}
	#endregion

	#region Coroutines
	/// <summary> bass spawn in enemy bass coroutine</summary>
	protected virtual IEnumerator SpawnInEnemy()
	{
		Enemy enemy = enemyToSpawn.GetComponent<Enemy>();
		if(enemy != null)
		{
			enemy.canAttack = false;
		}
		else
		{
			enemy = enemyToSpawn.GetComponentInChildren<Enemy>();
			if (enemy != null)
			{
				enemy.canAttack = false;
			}
		}

		yield return new WaitForSeconds(2f);

		enemyToSpawn.SetActive(true);
		enemyToSpawn.transform.position = spawnPoint.transform.position;

		if (enemy != null)
		{
			enemy.PlayTeleportEffect();
		}

		yield return new WaitForSeconds(2f);

		if (enemy != null)
		{
			enemy.canAttack = true;
			enemy.aggro = true;
			enemy.aggroRange = 100;
		}
	}
	#endregion
}