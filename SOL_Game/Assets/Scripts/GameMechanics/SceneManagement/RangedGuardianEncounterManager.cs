using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedGuardianEncounterManager : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject rangedGuardian,
		rangedGuardianSpawnPoint,
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

			GameObject.Find("Main Camera").GetComponent<cameraMovement>().PanCameraToLocation(rangedGuardianSpawnPoint, 2, 1, 1f);

			StartCoroutine(SpawnInRangedGuardian());
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
	private IEnumerator SpawnInRangedGuardian()
	{
		yield return new WaitForSeconds(2f);


		Destroy(Instantiate(teleportAnimaiton, rangedGuardianSpawnPoint.transform.position, new Quaternion(0, 0, 0, 0)), 1);
		rangedGuardian.SetActive(true);
		rangedGuardian.transform.position = rangedGuardianSpawnPoint.transform.position;

		yield return new WaitForSeconds(2f);

		rangedGuardian.GetComponent<RangedGuardian>().canAttack = true;
	}
	#endregion
}
