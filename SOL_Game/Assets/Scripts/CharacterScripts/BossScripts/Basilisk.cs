using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basilisk : Enemy
{

	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject
		enemyToSpawn, // the enemy type that gets spawned in when the ranged guardian gets to half health
		popUpAnimaiton; // the animation that will be played when the basilisk pops up

	public List<GameObject>
		popUpPositions; // a list of locations that the basilisk can pop up and attack from

	public SpriteRenderer
		mainSpriteRenderer, // the main sprite for when the basilisk is above ground
		underGoundSpriteRenderer; // the sprite that is shown when the basilisk is under ground

	public FloatValue
		basiliskDamageToGive;

	public float
		secondsAboveGround = 2f; // the number of second the basilisk stays above ground for
	#endregion

	#region Private Variables
	private bool
		running = false;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (running == false) // the basilisk is constantly moving
		{
			Run();
		}

		if (canAttack)
		{
			// enable the weapon box collider so that if the boss hits the player it damages the player
		}
		else
		{
			// dis able the weapon box collider so that if the boss hits the player it doesnt damages the player
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();
			player.TakeDamage((int)basiliskDamageToGive.initialValue, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}
	#endregion

	#region Utility Methods
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			GameObject.Find("rangedGuardianEncounterManager").gameObject.GetComponent<RangedGuardianEncounterManager>().EndRangedGuardianEncounter();
		}

		if (currentHealth == maxHealth.initialValue / 2)
		{
			// spawn enemies
			//StartCoroutine(SpawnRangedEnemies());
		}
	}

	private void Run()
	{
		// so that you don't call the coroutine again if the boss is already running
		if (running == false)
		{
			// Move the ranged guardian to the closest teleporter location
			running = true;
			StartCoroutine(MoveOverSeconds(gameObject, GetRandomGameobject().transform.position, 1f));
		}
	}

	/// <summary> gets a random gameobject from the list of pop up positions</summary>
	private GameObject GetRandomGameobject()
	{
		return popUpPositions[Random.Range(0, popUpPositions.Count)];
	}
	#endregion

	#region Coroutines
	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
	{
		// change to the under ground sprite
		mainSpriteRenderer.enabled = false;
		underGoundSpriteRenderer.enabled = true;

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

		Destroy(Instantiate(popUpAnimaiton, end, new Quaternion(0, 0, 0, 0)), 1);
		//objectToMove.transform.position = popUpPositions[Random.Range(0, popUpPositions.Count)].transform.position; // new code to get random location


		// change to the main sprite
		mainSpriteRenderer.enabled = true;
		underGoundSpriteRenderer.enabled = false;

		// wait for N seconds 
		yield return new WaitForSeconds(secondsAboveGround);

		running = false;
		moveSpeed = oldMoveSpeed; // set move speed back
	}


	#endregion
}

