using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGuardian : Enemy
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Animator
		anim; // Reference the animator
	public Vector2
		destination; // Where the guardian is going
	public Transform
		upperLeftSpawnPointLimit,  // Get a random position between two limits
		lowerRightSpawnPointLimit, // Get a random position between two limits
		swordSpawnPoint;           // Object the enemy will throw at the player
	public GameObject
		origin, // Where the sword is thrown from
		sword;  // Reference the sword being thrown
	public TestThrow
		throwSword; // Control throwing the sword
	public FloatValue
		meleeDamageToGive; // Boss's damage to the player
	public EncounterManager
		encounterManager; // Raises a signal upon guardian defeat
	#endregion

	#region Private Variables
	public bool
		moving       = false,
		returnOrigin = false;
	private Vector3
		targetGameObject;
	private SwordThrow
		shouldThrow;
	#endregion

	// Unity Named Methods
	#region Main Methods
   /* public void Awake()  
    {
		moving = true;

	}
    */
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
	/// <summary> Deal the guardian damage </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			encounterManager.EndEncounter();
		}
	}

	/// <summary> Damage the player </summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage((int)meleeDamageToGive.initialValue, false);
		}
	}

	/// <summary> Move the guardian </summary>
	private void Move()
	{
		// Prevents the coroutine from running again while already running
		if (moving == false)
		{
			moving = true;
			StartCoroutine(MoveOverSeconds(gameObject, GetRandomPositionBeweenLimits(), 1.0f));
		}
	}

	/// <summary> Return a random position within a specified range </summary>
	private Vector2 GetRandomPositionBeweenLimits()
	{
		return new Vector2(Random.Range(upperLeftSpawnPointLimit.position.x, lowerRightSpawnPointLimit.position.x),
		                   Random.Range(upperLeftSpawnPointLimit.position.y, lowerRightSpawnPointLimit.position.y));
	}

	/// <summary> Throw the sword </summary>
/*	private void Throw()
	{
		//StartCoroutine(HomingSword());
		//StartCoroutine(HomingSword());

		if ((Vector2)sword.transform.position == destination)
		{
			returnOrigin = true;
		}
		else if (sword.transform.position == origin.transform.position)
		{
			destination = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
			returnOrigin = false;
		}
		sword.transform.position = (returnOrigin == true) ? Vector2.Lerp(sword.transform.position, origin.transform.position, 10 * Time.deltaTime) :
													  Vector2.Lerp(sword.transform.position, destination, 10 * Time.deltaTime);

		moving = false;
		anim.SetTrigger("Patrol");
	}*/
	#endregion

	#region Coroutines
	/// <summary> Move towards a specified position </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 endingPosition, float seconds)
	{
		shouldThrow = FindObjectOfType<SwordThrow>();

		float
			elapsedTime = 0.0f; // Time an enemy is waiting
		Vector3
			startingPosition = objectToMove.transform.position; // Save the starting position

		// Move the guardian to a specified position
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position  = Vector3.Lerp(startingPosition, endingPosition, (elapsedTime / seconds));
			elapsedTime                     += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		//anim.SetTrigger("Attack");
		//yield return new WaitForSeconds(1.5f);
		//throwSword.shouldThrow = true;
		//anim.SetTrigger("Patrol");
		yield return new WaitForSeconds(3);
		shouldThrow.findTarget = true;
		//moving                 = false;
	}
	#endregion
}