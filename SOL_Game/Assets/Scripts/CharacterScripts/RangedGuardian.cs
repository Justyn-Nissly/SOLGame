using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedGuardian : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject
	enemyToSpawn, // the enemy type that gets spawned in when the ranged guardian gets to half health
	teleportAnimaiton;

	public List<GameObject>
		teleporterPositions;

	public float
		maxTimeBetweenAttacks = 2f,
		minTimeBetweenAttacks = 1f;

	public EncounterManager
		encounterManager;
	#endregion

	#region Private Variables
	private bool
	running = false,
	canSpawnEnemies = true;

	private float
		attackCountDownTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (canAttack)
		{
			if (attackCountDownTimer <= 0)
			{
				Shoot(true);
				attackCountDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
			}
			else
			{
				attackCountDownTimer -= Time.deltaTime;
			}
		}
	}
	#endregion

	#region Utility Methods
	public override void TakeDamage(int damage, bool playSwordImpactSound, bool fireBreathAttack = false)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if(currentHealth <= 0)
		{
			encounterManager.EndEncounter();
		}

		if (currentHealth <= maxHealth.initialValue / 2 && canSpawnEnemies)
		{
			// spawn enemies
			canSpawnEnemies = false;
			StartCoroutine(SpawnRangedEnemies());
		}
		else
		{
			Run();
		}

	}

	private void Run()
	{
		// so that you don't call the coroutine again if the ranged guardian is already running
		if (running == false)
		{
			// Move the ranged guardian to the closest teleporter location
			running = true;
			StartCoroutine(MoveOverSeconds(gameObject, GetClosestGameobject(teleporterPositions).transform.position, 1f));
		}
	}

	/// <summary> gets the closest game object from the passed in list in relation to this game object</summary>
	private GameObject GetClosestGameobject(List<GameObject> gameObjects)
	{
		GameObject closestGameObject = null;
		float distance;

		if (gameObjects.Count >= 1)
		{
			// set the first game object as the closest so that we have something to compare with
			distance = Vector2.Distance(transform.position, gameObjects[0].transform.position);
			closestGameObject = gameObjects[0];

			// loop through each game object searching for the closest one
			foreach (GameObject gameObject in gameObjects)
			{
				if (Vector2.Distance(transform.position, gameObject.transform.position) < distance)
				{
					distance = Vector2.Distance(transform.position, gameObject.transform.position);
					closestGameObject = gameObject;
				}
			}
		}

		return closestGameObject;
	}

	/// <summary> returns the next teleport in the list of teleporters, so to work right the list needs to be in clockwise order</summary>
	private Vector3 GetNextClockWiseTeleporter(Vector3 CurrentTeleporter)
	{
		int nextTeleporterIndex = 0;

		if (teleporterPositions.Count <= 0)
		{
			// error, no game objects in the teleporter list
			Debug.LogError("There are no telporter locations to teleport too!");
			return Vector3.zero; 
		}
		else if (teleporterPositions.Count > 0)
		{
			nextTeleporterIndex = teleporterPositions.FindIndex(teleporter => teleporter.transform.position == CurrentTeleporter) + 1;

			// check if the index is greater then the index range, if it is set index to zero
			if (nextTeleporterIndex == teleporterPositions.Count)
			{
				nextTeleporterIndex = 0;
			}
		}

		return teleporterPositions[nextTeleporterIndex].transform.position;
	}
	#endregion

	#region Coroutines
	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
	{
		float oldMoveSpeed = moveSpeed; // save the old move speed
		moveSpeed = 0; // stops the free roam script from effecting the run movement

		float elapsedTime = 0;
		Vector3 startingPos = objectToMove.transform.position;


		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		Destroy(Instantiate(teleportAnimaiton, end, new Quaternion(0, 0, 0, 0)), 1);
		objectToMove.transform.position = GetNextClockWiseTeleporter(end);

		running = false;
		moveSpeed = oldMoveSpeed; // set move speed back
	}

	private IEnumerator SpawnRangedEnemies()
	{
		float oldMoveSpeed = moveSpeed; // save the old move speed

		// this stops attacks, and movement, and makes invulnerable while spawning enemies
		canAttack = false;
		moveSpeed = 0;
		canTakeDamage = false;

		// spawn in an enemy for each teleporter ever N seconds
		foreach (GameObject teleporterGameObject in teleporterPositions)
		{
			yield return new WaitForSeconds(.5f);
			Instantiate(enemyToSpawn, teleporterGameObject.transform.position, teleporterGameObject.transform.rotation);
			Destroy(Instantiate(teleportAnimaiton, teleporterGameObject.transform.position, new Quaternion(0, 0, 0, 0)), 1);
		}

		// this resumes attacks, and movement, and stops invulnerably
		canAttack = true;
		moveSpeed = oldMoveSpeed;
		canTakeDamage = true;
		yield return null;
	}
	#endregion
}