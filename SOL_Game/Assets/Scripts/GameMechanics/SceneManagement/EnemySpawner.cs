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

	public GameObject
		spawnInAnimation; // the animation that is played where the enemy spawns in
	#endregion

	#region Private Variables (Empty)
	private bool 
		enemiesHaveSpawned = false;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// if the player collides with this trigger spawn in enemies
		if (collision.gameObject.CompareTag("Player") && enemiesHaveSpawned == false)
		{
			SpawnInEnemies();
			enemiesHaveSpawned = true; // set flag so no more enemies will spawn in
		}
	}
	#endregion

	#region Utility Methods
	private void SpawnInEnemies()
	{
		// plan the camera to the newly spawned in enemies
		GameObject.Find("Main Camera").GetComponent<cameraMovement>().PanCameraToLocation(enemySpawnPoints[0], 1, 1, 1f);

		foreach (GameObject enemy in enemiesToSpawn)
		{
			// make sure there are spawn points left in the spawn points list
			if (enemySpawnPoints.Count > 0)
			{
				// instantiate the current enemy in the loop at a spawn point and remove that spawn point from the list of spawn points
				Instantiate(enemy, enemySpawnPoints[0].transform.position, new Quaternion(0, 0, 0, 0));
				Destroy(Instantiate(spawnInAnimation, enemySpawnPoints[0].transform.position, new Quaternion(0, 0, 0, 0)), 1);

				enemySpawnPoints.Remove(enemySpawnPoints[0]);
			}
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
