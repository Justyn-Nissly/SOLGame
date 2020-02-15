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
	#endregion

	#region Private Variables
	private bool
		firstTimeTriggerEntered = true;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && firstTimeTriggerEntered) // make sure its the player that triggered this and it was there first time
		{
			firstTimeTriggerEntered = false; // set flag
			GetComponent<DoorManager>().LockDoors(); // lock all doors so the player cant leave this boss fight

			// start panning the camera to the bosses spawn point
			GameObject.Find("Main Camera").GetComponent<cameraMovement>().PanCameraToLocation(spawnPoint, 2, 1, 1f);

			// spawn in the boss
			StartCoroutine(SpawnInEnemy());
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> end the boss fight by unlocking the doors</summary>
	public void EndEncounter()
	{
		GetComponent<DoorManager>().UnlockDoors();
	}
	#endregion

	#region Coroutines
	/// <summary> bass spawn in enemy bass coroutine</summary>
	protected virtual IEnumerator SpawnInEnemy()
	{
		enemyToSpawn.GetComponent<Enemy>().canAttack = false;

		yield return new WaitForSeconds(2f);

		enemyToSpawn.SetActive(true);
		enemyToSpawn.transform.position = spawnPoint.transform.position;
		enemyToSpawn.GetComponent<Enemy>().PlayTeleportEffect();

		yield return new WaitForSeconds(2f);

		enemyToSpawn.GetComponent<Enemy>().canAttack = true;
	}
	#endregion
}
