using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedGuardian : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject
		enemyToSpawn,      // Enemy to spawn when the ranged guardian's health falls to half
		teleportAnimaiton; // Makes enemies appear when they spawn

	public List<GameObject>
		teleporterPositions; // Locations where enemies spawn

	public float
		maxTimeBetweenAttacks = 2f, // Longest time between attacks
		minTimeBetweenAttacks = 1f; // Shortest time between attacks

	public EncounterManager
		encounterManager; // Control the guardian fight
	#endregion

	#region Private Variables
	private bool
		retreat         = false, // Check if the guardian is retreating to a teleporter
		canSpawnEnemies = true;  // Check if enemies can spawn

	private float
		attackCountDownTimer; // Time left before the next attack
	#endregion

	// Unity Named Methods
	/// <summary> Control attacking </summary>
	#region Main Methods
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		// Count down to the next attack then attack
		if (canAttack)
		{
			if (attackCountDownTimer <= 0)
			{
				Shoot();
				attackCountDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
			}
			else
			{
				attackCountDownTimer -= Time.deltaTime;
			}
		}
	}
	#endregion

	/// <summary> Deal damage to the guardian </summary>
	#region Utility Methods
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		// The guardian is defeated when it runs out of health
		if (currentHealth <= 0)
		{
			encounterManager.EndEncounter();
		}

		// Additional enemies spawn when the guardian's health falls to half
		if (currentHealth <= maxHealth.initialValue * 0.5f && canSpawnEnemies)
		{
			canSpawnEnemies = false;
			StartCoroutine(SpawnRangedEnemies());
		}
		else
		{
			Retreat();
		}

	}

	private void Retreat()
	{
		// Don't call the coroutine again if the ranged guardian is retreating
		if (retreat == false)
		{
			// Move the ranged guardian to the closest teleporter location
			retreat = true;
			StartCoroutine(MoveOverSeconds(gameObject, GetClosestGameobject(teleporterPositions).transform.position, 1f));
		}
	}

	/// <summary> Find the closest game object in a list </summary>
	private GameObject GetClosestGameobject(List<GameObject> gameObjects)
	{
		GameObject
			closestGameObject = null; // Find the distance to this
		float
			distance; // Distance to the nearest object

		if (gameObjects.Count >= 1)
		{
			// Set the first game object as the closest to start comparing distances
			distance = Vector2.Distance(transform.position, gameObjects[0].transform.position);
			closestGameObject = gameObjects[0];

			// Find the nearest object in the list
			foreach (GameObject gameObject in gameObjects)
			{
				if (Vector2.Distance(transform.position, gameObject.transform.position) < distance)
				{
					distance          = Vector2.Distance(transform.position, gameObject.transform.position);
					closestGameObject = gameObject;
				}
			}
		}

		return closestGameObject;
	}

	/// <summary> Find the next teleporter clockwise </summary>
	private Vector3 GetNextClockWiseTeleporter(Vector3 CurrentTeleporter)
	{
		int
			nextTeleporterIndex = 0; // Where in the list the next teleporter is

		// The guardian does not retreat if no teleporters are available
		if (teleporterPositions.Count > 0)
		{
			nextTeleporterIndex =
				teleporterPositions.FindIndex(teleporter => teleporter.transform.position == CurrentTeleporter) + 1;

			// Loop the index to zero if needed
			if (nextTeleporterIndex == teleporterPositions.Count)
			{
				nextTeleporterIndex = 0;
			}
		}
		else
		{
			return Vector3.zero;
		}

		return teleporterPositions[nextTeleporterIndex].transform.position;
	}
	#endregion

	#region Coroutines
	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
	{
		float
			oldMoveSpeed = moveSpeed, // Save the old move speed
			elapsedTime  = 0.0f;      // Time that has passed
		Vector3
			startingPos = objectToMove.transform.position; // Where the object to move starts

		// Manually move an object towards a specified position
		moveSpeed = 0.0f;
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// Teleport the guardian
		Destroy(Instantiate(teleportAnimaiton, end, new Quaternion(0, 0, 0, 0)), 1);
		objectToMove.transform.position = GetNextClockWiseTeleporter(end);

		// Reset the move speed
		retreat                         = false;
		moveSpeed                       = oldMoveSpeed;
	}

	/// <summary> Spawn additional ranged enemies to aid the guardian </summary>
	private IEnumerator SpawnRangedEnemies()
	{
		float
			oldMoveSpeed = moveSpeed; // Save the old move speed

		// Spawning enemies start deactivated
		canTakeDamage = false;
		canAttack     = false;
		moveSpeed     = 0;

		// Spawn an enemy from each teleporter
		foreach (GameObject teleporterGameObject in teleporterPositions)
		{
			yield return new WaitForSeconds(0.5f);
			Instantiate(enemyToSpawn, teleporterGameObject.transform.position, teleporterGameObject.transform.rotation);
			Destroy(Instantiate(teleportAnimaiton, teleporterGameObject.transform.position,
			                    new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)), 1.0f);
		}

		// Activate spawned enemies
		canTakeDamage = true;
		canAttack     = true;
		moveSpeed     = oldMoveSpeed;
		yield return null;
	}
	#endregion
}