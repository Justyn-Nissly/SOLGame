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
		panCamera = true, // flag for if you want the camera to pan to the first spawn point when the enemies spawn in
		connectQuestItem;
	#endregion

	#region Private Variables
	private bool
		enemiesHaveSpawned = false,
		doorsHaveBeenUnlocked = false,
		questItemFound;
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

		questItemFound = false;
	}

	private void FixedUpdate()
	{
		if (questItemFound == false)
		{
			
		}
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
				StartCheckingIfEnemiesDefeated();  //1s delay, repeat every .5s
			}
		}
	}
	#endregion

	#region Utility Methods
	private void SpawnInEnemy(GameObject enemy, int spawnPoint)
	{
		// instantiate the current enemy in the loop at a spawn point and remove that spawn point from the list of spawn points
		GameObject tempEnemy = Instantiate(enemy, enemySpawnPoints[spawnPoint].transform.position, new Quaternion(0, 0, 0, 0));

		// saves this enemy's ID number
		enemyIDs.Add(tempEnemy.GetInstanceID());

		// freeze enemy movement
		Enemy enemyScript = tempEnemy.GetComponent<Enemy>();
		if (enemyScript != null)
		{
			enemyScript.maxMoveSpeed = enemyScript.moveSpeed; // save old move speed
			enemyScript.moveSpeed = 0;

			// play the teleport shader effect if there is one on the enemy
			//(it will find all effects on the enemy because some enemies have more than one like the shield enemy)
			enemyScript.PlayTeleportEffect();
		}
	}

	/// <summary> this starts the logic to check if all this spawner's enemies are defeated (this is a separate method so that other scripts can call this)</summary>
	public void StartCheckingIfEnemiesDefeated()
	{
		InvokeRepeating("CheckEnemies", 1f, .5f);  //1s delay, repeat every .5s
	}

	/// <summary> this method unlocks any locked doors linked to this spawner if all this spawner's enemies have been defeated</summary>
	private void CheckEnemies()
	{
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

	/// <summary> for adding a new enemy that needs to die before the doors unlock (mainly used for boss battles)</summary>
	public void AddNewEnemyID(int enemyID)
	{
		enemyIDs.Add(enemyID);
	}
	#endregion

	#region Coroutines
	/// <summary> method to spawn in this spawner's enemies </summary>
	/// <param name="numberOfEnemiesToSpawn">only fill in if you want to limit the number of spawned in enemies
	/// (the reason the default value is 100 is because that is a limit that we will never reach)</param>
	public IEnumerator SpawnInEnemies(bool freezePlayer = true, int numberOfEnemiesToSpawn = 100)
	{
		int spawnPointIndex = 0;

		// pan the camera to the newly spawned in enemies
		if (panCamera)
		{
			GameObject.Find("Main Camera").GetComponent<cameraMovement>().PanCameraToLocation(enemySpawnPoints[0], 1, 1, 1f);
		}
		else
		{
			freezePlayer = false;
		}

		// freeze the player, there is a bool check because we don't want to freeze movement in the middle of a boss fight
		if (freezePlayer)
		{
			FindObjectOfType<Player>().FreezePlayer();
		}

		// spawn in each enemy in the enemies to spawn list
		foreach (GameObject enemy in enemiesToSpawn)
		{
			// make sure there are spawn points left in the spawn points list
			if (enemySpawnPoints.Count > 0 && numberOfEnemiesToSpawn >= 1)
			{
				SpawnInEnemy(enemy, spawnPointIndex); // spawn in the enemy

				// change counters
				numberOfEnemiesToSpawn--; // decrement the counter
				if (spawnPointIndex < enemySpawnPoints.Count - 1)
				{
					spawnPointIndex++;
				}

				yield return new WaitForSeconds(enemySpawnRate); // spawn in an enemy every N seconds
			}
		}

		// unfreeze the player
		if (freezePlayer)
		{
			FindObjectOfType<Player>().UnFreezePlayer();
		}

		// unfreeze the enemies
		foreach(Enemy enemy in FindObjectsOfType<Enemy>()) // find all enemies in the scene
		{
			if(enemyIDs.Contains(enemy.gameObject.GetInstanceID())) // check if this enemy is from this spawner
			{
				enemy.moveSpeed = enemy.maxMoveSpeed; // set move speed back to what it was
			}
		}
	}
	#endregion
}
