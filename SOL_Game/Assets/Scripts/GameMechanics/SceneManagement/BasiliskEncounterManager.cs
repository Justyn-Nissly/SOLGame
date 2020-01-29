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
		firstTimeTriggerEntered = true;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (firstTimeTriggerEntered)
		{
			firstTimeTriggerEntered = false;
			GetComponent<DoorManager>().LockAllDoors();

			GameObject.Find("Main Camera").GetComponent<cameraMovement>().PanCameraToLocation(basiliskSpawnPoint, 2, 1, 1f);

			StartCoroutine(SpawnInBasilisk());
		}
	}
	#endregion

	#region Utility Methods
	public void EndRangedGuardianEncounter()
	{
		GetComponent<DoorManager>().UnlockAllDoors();
	}
	#endregion

	#region Coroutines
	private IEnumerator SpawnInBasilisk()
	{
		yield return new WaitForSeconds(2f);


		Destroy(Instantiate(teleportAnimaiton, basiliskSpawnPoint.transform.position, new Quaternion(0, 0, 0, 0)), 1);
		basilisk.SetActive(true);
		basilisk.transform.position = basiliskSpawnPoint.transform.position;

		yield return new WaitForSeconds(2f);

		basilisk.GetComponent<Basilisk>().canAttack = true;
	}
	#endregion
}
