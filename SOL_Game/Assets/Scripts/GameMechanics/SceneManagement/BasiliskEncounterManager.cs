using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasiliskEncounterManager : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject basilisk,
		basiliskSpawnPoint,
		teleportAnimaiton;
	#endregion

	#region Private Variables
	private bool
		firstTimeTriggerEntered = true; // a flag so that the boss is only spawned in the first time the player triggers this script
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && firstTimeTriggerEntered) // make sure its the player that triggered this and it was there first time
		{
			// set flag
			firstTimeTriggerEntered = false;

			// lock all doors so the player cant leave this boss fight
			GetComponent<DoorManager>().LockAllDoors(); 

			// start paning the camera to the bosses spawn point
			GameObject.Find("Main Camera").GetComponent<cameraMovement>().PanCameraToLocation(basiliskSpawnPoint, 2, 1, 1f);

			// spawn in the boss
			StartCoroutine(SpawnInBasilisk());
		}
	}
	#endregion

	#region Utility Methods
	// this ends the encounter by unlocking all the doors that are locking the player in that room
	public void EndEncounter()
	{
		GetComponent<DoorManager>().UnlockAllDoors();
	}
	#endregion

	#region Coroutines
	/// <summary> spawn in the boss </summary>
	private IEnumerator SpawnInBasilisk()
	{
		// wait for N seconds
		yield return new WaitForSeconds(2f);

		// spawn in the boss
		Destroy(Instantiate(teleportAnimaiton, basiliskSpawnPoint.transform.position, new Quaternion(0, 0, 0, 0)), 1);
		basilisk.SetActive(true);
		basilisk.transform.position = basiliskSpawnPoint.transform.position;

		// wait for N seconds
		yield return new WaitForSeconds(2f);

		// enable the boss to attack the player
		basilisk.GetComponent<Basilisk>().canAttack = true;
	}
	#endregion
}
