using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGuardian : Enemy
{

	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Animator
		anim; // Reference to change animation states
	public Transform
		upperLeftSpawnPointLimit,  // used to get a random position between these two limits
		lowerRightSpawnPointLimit; // Location you wish the enemy to move to
	public bool
	    moving = false; // Check if the character is currently moving
	public FloatValue
		meleeDamageToGive;
	public Material
        damagedShaderMaterial,
        fishEyeMaterial;

	public EncounterManager EncounterManager; 
	#endregion

	#region Private Variables
	private float
        movemntSpeed = 1f; // The higher the number the slower he moves

	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		base.FixedUpdate();

        // Stage 2 after half health
		if (currentHealth == maxHealth.initialValue / 2)
		{
			StateEnrage();
		}

		// The guardian is constantly moving
		if (moving == false && canAttack)
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
	public override void TakeDamage(int damage, bool playSwordImpactSound, bool fireBreathAttack = false)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if (currentHealth <= 0)
		{
			EncounterManager.EndEncounter();
		}
	}

	/// <summary> The method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage((int)meleeDamageToGive.initialValue, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}

	/// <summary> Sets trigger to know boss is moving then moves</summary>
	private void Move()
	{
		// Prevents coroutine from running again if the boss is already running
		if (moving == false)
		{
			moving = true;
			StartCoroutine(MoveOverSeconds(gameObject, GetRandomPositionBeweenLimits(), movemntSpeed));
		}
	}

	/// <summary> Gets a random gameobject from the list of pop up positions</summary>
	private Vector2 GetRandomPositionBeweenLimits()
	{
		Vector2 randomPosition = new Vector2();

		// Set the random psition to be in the range of the set limits
		randomPosition.x = Random.Range(upperLeftSpawnPointLimit.position.x, lowerRightSpawnPointLimit.position.x);
		randomPosition.y = Random.Range(upperLeftSpawnPointLimit.position.y, lowerRightSpawnPointLimit.position.y);

		return randomPosition;
	}

	/// <summary> Changes animator states to enraged</summary>
	private void StateEnrage()
    {
        anim.SetTrigger("Enrage");
    }

	/// <summary> Add fish eye shader to enemy</summary>
	private void AddFishEye()
	{
		moving = true;
		float percentageComplete = 0;

		// freeze the enemy because they are dead...
		canAttack = false;

		if (fishEyeMaterial != null)
		{
			foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
			{
				renderer.material = fishEyeMaterial;
			}

			fishEyeMaterial.SetFloat("Shader_Value", 0); // Set starting value

			// play the pixel die effect from start(0) to finish(1)
			while (percentageComplete < 1)
			{
				fishEyeMaterial.SetFloat("Shader_Value", Mathf.Lerp(0f, 1f, percentageComplete));
				percentageComplete += Time.deltaTime / 2;
			}

			fishEyeMaterial.SetFloat("Shader_Value", 1); // Set ending value
		}
		moving = false;
		anim.SetTrigger("Patrol");
	}

	/// <summary> Add fish eye shader to enemy</summary>
	private void AddDamagedShader()
	{
		moving = true;
		float percentageComplete = 0;


		if (damagedShaderMaterial != null)
		{
			foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
			{
				renderer.material = damagedShaderMaterial;
			}

			damagedShaderMaterial.SetFloat("Shader_Value", 0); // Set starting value

			// play the pixel die effect from start(0) to finish(1)
			while (percentageComplete < 1)
			{
				damagedShaderMaterial.SetFloat("Shader_Value", Mathf.Lerp(0f, 1f, percentageComplete));
				percentageComplete += Time.deltaTime / 2;
			}

			damagedShaderMaterial.SetFloat("Shader_Value", 1); // Set ending value
		}

		moving = false;
	}
	#endregion

	#region Coroutines

	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 endingPosition, float seconds)
	{
		float elapsedTime = 0; // Amount of time an enemy is waiting in one position
	    Vector3 startingPosition = objectToMove.transform.position; // Save the starting position

		// Move the guardian a little each frame based on how many seconds it should take to get the ending position
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPosition, endingPosition, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

        // Wait in one spot then go to attack
        yield return new WaitForSeconds(3);
		anim.SetTrigger("Attack");
	}


	#endregion
}