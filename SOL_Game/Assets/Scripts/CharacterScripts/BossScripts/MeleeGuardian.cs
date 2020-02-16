using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGuardian : Enemy
{

	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject
		homingSword,
		staticArmLeft;
		

	public Transform
		upperLeftSpawnPointLimit,  // used to get a random position between these two limits
		lowerRightSpawnPointLimit, // Location you wish the enemy to move to
		swordSawnPoint;            // The obejct that the enemy will throw at the player

	public FloatValue
		meleeDamageToGive;

	public EncounterManager EncounterManager; // this reference is used to send a signal when the basilisk dies
	#endregion

	#region Private Variables
	private bool
		moving = false;
    private Vector3 targetGameObject;
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
			EncounterManager.EndEncounter();
		}
	}

	/// <summary> the method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage((int)meleeDamageToGive.initialValue, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}

	private void Move()
	{
		// Prevents coroutine from running again if the boss is already running
		if (moving == false)
		{
			// Move the ranged guardian to the closest teleporter location
			moving = true;
			StartCoroutine(MoveOverSeconds(gameObject, GetRandomPositionBeweenLimits(), 1f));
			StartCoroutine(DoSpinAttack());

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


	private Vector2 CreateTarget()
	{
		targetGameObject = GameObject.FindGameObjectWithTag("Player").transform.position;

		return targetGameObject;
	}

	#endregion

	#region Coroutines
	/// <summary> this does a spinning the arms type attack</summary>
	private IEnumerator DoSpinAttack()
	{
		homingSword.SetActive(true);

		staticArmLeft.SetActive(false);

		StartCoroutine(MoveOverSeconds(homingSword, CreateTarget(), 1f));

		yield return new WaitForSeconds(2);

		homingSword.SetActive(false);

		staticArmLeft.SetActive(true);

		yield return new WaitForSeconds(1);

		canAttack = true;
	}

	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 endingPosition, float seconds)
	{

        float elapsedTime = 0; // Amount of time an enemy is waiting in one position
	    Vector3 startingPosition = objectToMove.transform.position; // save the starting position

		// Move the guardian a little each frame based on how many seconds it should take to get the ending position
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPosition, endingPosition, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(4);
		moving = false;
	}


	#endregion
}