using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basilisk : Enemy
{

	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject
		popUpAnimaiton, // the animation that will be played when the basilisk pops up
		bomb;

	public Transform
		upperLeftSpawnPointLimit, // used to get a random position between these two limits
		lowerRightSpawnPointLimit,
		bombSpawnPoint;

	public CircleCollider2D
		basiliskCollider;

	public SpriteRenderer
		mainSpriteRenderer, // the main sprite for when the basilisk is above ground
		underGoundSpriteRenderer; // the sprite that is shown when the basilisk is under ground

	public FloatValue
		basiliskDamageToGive;

	public float
		secondsAboveGround = 2f; // the number of second the basilisk stays above ground for

	public BasiliskEncounterManager basiliskEncounterManager; // this reference is used to send a signal when the basilisk dies
	#endregion

	#region Private Variables
	private bool
		moving = false;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (moving == false && canAttack) // the basilisk is constantly moving
		{
			Move();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
		}
	}
	#endregion

	#region Utility Methods
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			basiliskEncounterManager.EndEncounter();
		}
	}

	/// <summary> the method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if(player != null)
		{
			player.TakeDamage((int)basiliskDamageToGive.initialValue, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}

	private void Move()
	{
		// so that you don't call the coroutine again if the boss is already running
		if (moving == false)
		{
			// Move the ranged guardian to the closest teleporter location
			moving = true;
			StartCoroutine(MoveOverSeconds(gameObject, GetRandomPositionBeweenLimits(), 1f));
		}
	}

	/// <summary> gets a random gameobject from the list of pop up positions</summary>
	private Vector2 GetRandomPositionBeweenLimits()
	{
		Vector2 randomPosition = new Vector2();

		// set the random psition to be in the range of the set limits
		randomPosition.x = Random.Range(upperLeftSpawnPointLimit.position.x, lowerRightSpawnPointLimit.position.x);
		randomPosition.y = Random.Range(upperLeftSpawnPointLimit.position.y, lowerRightSpawnPointLimit.position.y);

		return randomPosition;
	}

	private void SpawnBomb()
	{
		GameObject bombGameObject = Instantiate(bomb, gameObject.transform.position, new Quaternion(0, 0, 0, 0));
		bombGameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f)));
	}
	#endregion

	#region Coroutines
	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 endingPosition, float seconds)
	{
		// change to the under ground sprite
		mainSpriteRenderer.enabled = false;
		underGoundSpriteRenderer.enabled = true;
		basiliskCollider.enabled = false; // disable the collider so it wont hit the player while under ground

		//float oldMoveSpeed = moveSpeed; // save the old move speed
		//moveSpeed = 0; // stops the free roam script from effecting the run movement

		float elapsedTime = 0;
		Vector3 startingPosition = objectToMove.transform.position; // save the starting position

		// move the basilisk a little each frame based on how many seconds it should take to get the ending position
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPosition, endingPosition, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// start the animation that plays when the basilisk pops out of the ground
		if(popUpAnimaiton != null)
		{
			Destroy(Instantiate(popUpAnimaiton, endingPosition, new Quaternion(0, 0, 0, 0)), 1);
		}

		// change to the main sprite
		mainSpriteRenderer.enabled = true;
		underGoundSpriteRenderer.enabled = false;
		basiliskCollider.enabled = true; // enable the collider so it will hit the player while not under ground

		// wait for N seconds
		yield return new WaitForSeconds(secondsAboveGround);

		// spawn some bombs
		SpawnBomb();

		moving = false;
	}


	#endregion
}

