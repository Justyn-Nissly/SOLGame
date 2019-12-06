using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedGuardian : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject
	enemyToSpawn; // the enemy type that gets spawned in when the ranged guardian gets to half health

	public List<GameObject>
		teleporterPositions;

	public float
		maxTimeBetweenAttacks = 2f,
		minTimeBetweenAttacks = 1f;
	#endregion

	#region Private Variables
	private bool
	shouldAttack = true,
	running = false;

	private float
		countDownTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (shouldAttack && Vector2.Distance(transform.position, playerPos) <= aggroRange)
		{
			if (countDownTimer <= 0)
			{
				Shoot();
				countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
			}
			else
			{
				countDownTimer -= Time.deltaTime;
			}
		}
	}
	#endregion

	#region Utility Methods
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);


		if (currentHealth == maxHealth.initialValue / 2)
		{
			// spawn enemies
			SpawnRangedEnemies();
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

	private void SpawnRangedEnemies()
	{
		foreach (GameObject teleporterGameObject in teleporterPositions)
		{
			Instantiate(enemyToSpawn, teleporterGameObject.transform.position, teleporterGameObject.transform.rotation);
		}
	}

	/// <summary> returns a random position for the ranged guardian to teleport to but will never pick the passed in parameter</summary>
	private Vector3 getRandomPosition(Vector3 notThisPosition)
	{
		Vector3 randomPosition = Vector3.zero;

		if (teleporterPositions.Count == 1)
		{
			randomPosition = teleporterPositions[0].transform.position;
		}
		else if (teleporterPositions.Count > 1)
		{
			List<GameObject> validPositions = new List<GameObject>();
			validPositions.AddRange(teleporterPositions.FindAll(t => t.transform.position != notThisPosition));

			randomPosition = validPositions[Random.Range(0, validPositions.Count)].transform.position;
		}

		return randomPosition;
	}
	#endregion

	#region Coroutines
	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
	{
		float elapsedTime = 0;
		Vector3 startingPos = objectToMove.transform.position;

		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		objectToMove.transform.position = getRandomPosition(end);
		running = false;
	}
	#endregion
}
