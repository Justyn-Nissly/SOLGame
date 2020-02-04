using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public List<GameObject>
		enemiesToSpawn = new List<GameObject>(), // a list of all enemies that will be spawned in
		enemySpawnPoints = new List<GameObject>(); // a list of all points that the enemies will be spawned at 
																 //(these lists should be the same length but the code will still work if they are not)
	#endregion

	#region Private Variables (Empty)
	private bool 
		enemiesHaveSpawned = false;
	private float
		enemySpawnRate = .5f; // the amount of time before the next enemy (from the list if enemies to spawn) will spawn in
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// if the player collides with this trigger spawn in enemies
		if (collision.gameObject.CompareTag("Player") && enemiesHaveSpawned == false)
		{
			StartCoroutine(SpawnInEnemies());
			enemiesHaveSpawned = true; // set flag so no more enemies will spawn in
		}
	}
	#endregion

	#region Utility Methods
	private void SpawnInEnemy(GameObject enemy)
	{
		// instantiate the current enemy in the loop at a spawn point and remove that spawn point from the list of spawn points
		GameObject tempEnemy = Instantiate(enemy, enemySpawnPoints[0].transform.position, new Quaternion(0, 0, 0, 0));

		// play the teleport shader effect if there is one on the enemy
		//(it will find all effects on the enemy because some enemies have more than one like the shield enemy)
		List<_2dxFX_NewTeleportation2> enemyTeleportScripts = new List<_2dxFX_NewTeleportation2>();
		enemyTeleportScripts.AddRange(tempEnemy.GetComponentsInChildren<_2dxFX_NewTeleportation2>());

		if (enemyTeleportScripts.Count != 0) // check for empty list
		{
			foreach(_2dxFX_NewTeleportation2 enemyTeleportScript in enemyTeleportScripts)
			{
				StartCoroutine(TeleportInEnemy(enemyTeleportScript));
			}
		}

		enemySpawnPoints.Remove(enemySpawnPoints[0]);
	}
	#endregion

	#region Coroutines
	private IEnumerator SpawnInEnemies()
	{
		// plan the camera to the newly spawned in enemies
		GameObject.Find("Main Camera").GetComponent<cameraMovement>().PanCameraToLocation(enemySpawnPoints[0], 1, 1, 1f);

		foreach (GameObject enemy in enemiesToSpawn)
		{
			// make sure there are spawn points left in the spawn points list
			if (enemySpawnPoints.Count > 0)
			{
				SpawnInEnemy(enemy);
				yield return new WaitForSeconds(enemySpawnRate); // spawn in an enemy every N seconds
			}
		}
	}

	private IEnumerator TeleportInEnemy(_2dxFX_NewTeleportation2 teleportScript)
	{
		float percentageComplete = 0;

		// make the enemy invisible, this is not set by default in the prefab because
		// then the enemy would be invisible in Dev rooms because they don't have this script running in them
		teleportScript._Fade = 1;

		// teleport the player in, it does this by "sliding" a float from 0 to 1 over time
		while (percentageComplete < 1)
		{
			teleportScript._Fade = Mathf.Lerp(1f, 0f, percentageComplete);
			percentageComplete += Time.deltaTime;
			yield return null;
		}

		teleportScript._Fade = 0;
	}
	#endregion
}
