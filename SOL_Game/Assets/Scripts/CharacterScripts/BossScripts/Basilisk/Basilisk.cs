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
		bomb; // bomb prefab that with be instantiated

	public Transform
		upperLeftSpawnPointLimit, // used to get a random position between these two limits
		lowerRightSpawnPointLimit, // used to get a random position between these two limits
		bombSpawnPoint; // the point that bombs with be instantiated

	public CircleCollider2D
		basiliskCollider; // referance to the basilisk's Collider

	public FloatValue
		basiliskDamageToGive; // basilisk's damage

	public float
		secondsAboveGround = 2f; // the number of second the basilisk stays above ground for

	public EncounterManager
		basiliskEncounterManager; // this reference is used to send a signal when the basilisk dies
	public Animator
		animator; // animations controller
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
			DamagePlayer(collision.gameObject.GetComponent<Player>(), (int)basiliskDamageToGive.initialValue);
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

	/// <summary> starts moving the basilisk to a random point if the basilisk is not moving</summary>
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

	/// <summary> Instantiate a bomb prefab</summary>
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
		basiliskCollider.enabled = false; // disable the collider so it wont hit the player while under ground

		// trigger the under ground animation
		animator.SetTrigger("UnderGround");
		GetComponent<SpriteRenderer>().sortingOrder--; // change that the basilsik is rendered under other sprites like the player while under ground

		yield return new WaitForSeconds(secondsAboveGround / 2); // wait for the animation to fully play

		// spawn some bombs
		SpawnBomb();

		float elapsedTime = 0;
		Vector3 startingPosition = objectToMove.transform.position; // save the starting position

		// move the basilisk a little each frame based on how many seconds it should take to get the ending position
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPosition, endingPosition, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// trigger pop out of ground animation
		animator.SetTrigger("PopUp");
		GetComponent<SpriteRenderer>().sortingOrder++; // change the render layer
		basiliskCollider.enabled = true; // enable the collider so it will hit the player while not under ground

		// wait for N seconds above the ground
		yield return new WaitForSeconds(secondsAboveGround/ 2);

		moving = false;
	}

	/// <summary> override teleport script so that the basilisk pops out of the ground instead of teleporting in </summary>
	protected override IEnumerator TeleportInEnemy(_2dxFX_NewTeleportation2 teleportScript)
	{
		// trigger pop out of ground animation
		animator.SetTrigger("PopUp");

		// dont let the basilisk move, attack, or take damage
		canAttack = false; 
		canTakeDamage = false;

		yield return new WaitForSeconds(secondsAboveGround);

		// let the basilisk move, attack, or take damage
		canAttack = true;
		canTakeDamage = true;
	}


	#endregion
}