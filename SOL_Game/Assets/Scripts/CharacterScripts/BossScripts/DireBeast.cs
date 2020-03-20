using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireBeast : Enemy
{

	#region Enums (Empty)
	#endregion

	#region Public Variables
	// light melee attack variables (only need to be filled in the inspector if you use the light melee attack)
	public Transform
		armAttackPosition;
	public float
		armAttackRange;
	public GameObject
		armWeapon;
	public FloatValue
		armDamageToGive;

	//public Animator
		//anim; // Reference to change animation states
	public GameObject
		campLocation; // Location you wish the enemy to move to
	public FloatValue
		meleeDamageToGive;
	public float
		countDownTimer;

	public EncounterManager EncounterManager; // this reference is used to send a signal when the basilisk dies
	#endregion

	#region Private Variables
	private float
		maxTimeBetweenAttacks = 1.2f,
		minTimeBetweenAttacks = 0.7f;
	private BossBlastAttack
		bossBlastAttack;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		base.FixedUpdate();

		bossBlastAttack = FindObjectOfType<BossBlastAttack>();

		// Stage 2 after half health
		if (currentHealth == maxHealth.initialValue / 2)
		{
			//StateEnrage();
			Move();
			armAttackRange = 2f;
		}

        if (Vector2.Distance(playerPos, gameObject.transform.position) < 5 &&
			(countDownTimer <= 0 && canAttack && aggro))
        {
			bossBlastAttack.enabled = false;
			countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks
			MeleeAttack(armWeapon, armAttackPosition, armAttackRange, armDamageToGive, false);
		}
		else
		{
			countDownTimer -= Time.deltaTime;
			bossBlastAttack.enabled = true;
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
	/// <summary> Changes defeat functionality to Encounter Manager</summary>
	public virtual void TakeDamage(int damage, bool playSwordImpactSound, bool fireBreathAttack = false)
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
	
	    StartCoroutine(MoveOverSeconds(gameObject, campLocation.transform.position, 1f));

	}

	/// <summary> Changes animator states to enraged</summary>
	private void StateEnrage()
	{
		//anim.SetTrigger("Enrage");
	}

	/// <summary> Find the player</summary>
	private GameObject CreateTarget()
	{
		GameObject targetGameObject = new GameObject("target game object");
		targetGameObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;

		return targetGameObject;
	}

	/// <summary> the attack method used for the enemy and the player to swing light/heavy melee weapons</summary>
	public override void MeleeAttack(GameObject meleeWeapon, Transform attackPosition, float attackRange, FloatValue damageToGive, bool createWeapon)
	{
		Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, willDamageLayer);

		foreach (Collider2D collider in enemiesToDamage)
		{
			BaseCharacter characterBeingAtacked = collider.GetComponent<BaseCharacter>();
			if (characterBeingAtacked != null)
			{
				characterBeingAtacked.TakeDamage((int)damageToGive.initialValue, true);
			}
		}

		GameObject weaponInstance = Instantiate(meleeWeapon, attackPosition.transform);
		Destroy(weaponInstance, 2f);
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
	}


	#endregion
}