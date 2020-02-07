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

	public List<DoorLogic>
		doors = new List<DoorLogic>(); // all doors that will be locked when the enemies spawn in(and unlock when those enemies are killed)

	public bool
		panCamera = true; // flag for if you want the camera to pan to the first spawn point when the enemies spawn in
	#endregion

	#region Private Variables (Empty)
	private bool
		enemiesHaveSpawned = false,
		doorsHaveBeenUnlocked = false;
	private float
		enemySpawnRate = .5f, // the amount of time before the next enemy (from the list if enemies to spawn) will spawn in
		timer,
		CheckInterval = .5f; // the interval time for how often the code will check if the enemies are defeated
	private DoorManager
		doorManager = new DoorManager(); // this manager has the logic to control a list of door (in this case used to unlock them all)
	private List<int>
		enemyIDs = new List<int>(); // this is a list of all the game object ID's of each enemy that has been spawned at this spawner
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void Start()
	{
		// add all doors to the door manager
		doorManager.doors.AddRange(doors);

		// set the timer
		timer = CheckInterval;
	}

	private void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// if the player collides with this trigger spawn in enemies
		if (collision.gameObject.CompareTag("Player") && enemiesHaveSpawned == false)
		{
			// spawn in enemies
			StartCoroutine(SpawnInEnemies());
			enemiesHaveSpawned = true; // set flag so no more enemies will spawn in

			// lock any doors
			doorManager.LockDoors();

			// start checking if the enemies have been defeated(for unlocking doors so dont check this if there are no doors)
			if(doorManager.doors.Count > 0)
			{
				InvokeRepeating("CheckEnemies", 1f, .5f);  //1s delay, repeat every .5s
			}
		}
	}
	#endregion

	#region Utility Methods
	private void SpawnInEnemy(GameObject enemy)
	{
		// instantiate the current enemy in the loop at a spawn point and remove that spawn point from the list of spawn points
		GameObject tempEnemy = Instantiate(enemy, enemySpawnPoints[0].transform.position, new Quaternion(0, 0, 0, 0));

		// saves this enemy's ID number
		enemyIDs.Add(tempEnemy.GetInstanceID());

		// play the teleport shader effect if there is one on the enemy
		//(it will find all effects on the enemy because some enemies have more than one like the shield enemy)
		PlayTeleportEffect(tempEnemy);

		// remove this enemies spawn point from the list because its been used
		enemySpawnPoints.Remove(enemySpawnPoints[0]);
	}

	private void PlayTeleportEffect(GameObject enemy)
	{
		List<_2dxFX_NewTeleportation2> enemyTeleportScripts = new List<_2dxFX_NewTeleportation2>();
		enemyTeleportScripts.AddRange(enemy.GetComponentsInChildren<_2dxFX_NewTeleportation2>());

		if (enemyTeleportScripts.Count != 0) // check for empty list
		{
			foreach (_2dxFX_NewTeleportation2 enemyTeleportScript in enemyTeleportScripts)
			{
				StartCoroutine(TeleportInEnemy(enemyTeleportScript));
			}
		}
	}

	/// <summary> this method unlocks any locked doors linked to this spawner if all this spawer's enemies have been defeated</summary>
	private void CheckEnemies()
	{
		print("check");

		if (CheckIfEnemiesDefeated())
		{
			// unlock any doors
			doorsHaveBeenUnlocked = true;
			doorManager.UnlockDoors();

			// stop checking if enemies have been defeated
			CancelInvoke();
		}
		
	}

	/// <summary> this method returns true if all this spawner's enemies have been defeated</summary>
	private bool CheckIfEnemiesDefeated()
	{
		List<GameObject> enemies = new List<GameObject>();
		enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

		// check if any of the enemies in the scene have any of the ID numbers of the enemies spawned by this spawner
		foreach(GameObject enemy in enemies)
		{
			foreach(int ID in enemyIDs)
			{
				if(ID == enemy.GetInstanceID())
				{
					return false;
				}
			}
		}

		return true;
	}
	#endregion

	#region Coroutines
	/// <summary> method to spawn in this spawner's enemies </summary>
	private IEnumerator SpawnInEnemies()
	{
		// pan the camera to the newly spawned in enemies
		if (panCamera)
		{
			GameObject.Find("Main Camera").GetComponent<cameraMovement>().PanCameraToLocation(enemySpawnPoints[0], 1, 1, 1f);
		}

		// freeze the player
		FindObjectOfType<Player>().FreezePlayer();

		// spawn in each enemy in the enemies to spawn list
		foreach (GameObject enemy in enemiesToSpawn)
		{
			// make sure there are spawn points left in the spawn points list
			if (enemySpawnPoints.Count > 0)
			{
				SpawnInEnemy(enemy);
				yield return new WaitForSeconds(enemySpawnRate); // spawn in an enemy every N seconds
			}
		}

		// unfreeze the player
		FindObjectOfType<Player>().UnFreezePlayer();
	}

	private IEnumerator TeleportInEnemy(_2dxFX_NewTeleportation2 teleportScript)
	{
		float percentageComplete = 0;

		// make the enemy invisible, this is not set by default in the prefab because
		// then the enemy would be invisible in Dev rooms because they don't have this script running in them
		teleportScript._Fade = 1;

		// teleport the enemy in, it does this by "sliding" a float from 0 to 1 over time
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