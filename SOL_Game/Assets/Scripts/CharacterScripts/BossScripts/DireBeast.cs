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
		armAttackRange = 2f;
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

	public EncounterManager
		EncounterManager; // this reference is used to send a signal when the basilisk dies

	public Animator
		armAnimator; // the wall attack animation
	#endregion

	#region Private Variables
	private float
		maxTimeBetweenAttacks = 2f,
		minTimeBetweenAttacks = 1.5f;
	private BossBlastAttack
		bossBlastAttack;

	private bool
		inWall = true,
		deadLogicHappened = false;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void Start()
	{
		base.Start();

		bossBlastAttack = FindObjectOfType<BossBlastAttack>();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		playerPos = GameObject.FindWithTag("Player").transform.position;

		// Stage 2 after half health
		if (maxHealth.runTimeValue == maxHealth.initialValue * 0.5f)
		{
			Move();

			Destroy(GetComponent<PolygonCollider2D>());
			gameObject.AddComponent<PolygonCollider2D>();

			inWall = false;
			characterAnimator.SetTrigger("ExitWall");
		}

        if (Vector2.Distance(playerPos, gameObject.transform.position) < 7 &&
			countDownTimer <= 0 && canAttack && aggro)
        {
			//bossBlastAttack.enabled = false;
			countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks
			MeleeAttack(armWeapon, armAttackPosition, armAttackRange, armDamageToGive, false);
		}
		else if (Vector2.Distance(playerPos, gameObject.transform.position) >= 7 &&
			countDownTimer <= 0 && canAttack && aggro && inWall == false)
		{
			countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks
			bossBlastAttack.enabled = true;
			characterAnimator.SetBool("shoot", true);
		}
		 else
		{
			countDownTimer -= Time.deltaTime;
		}



		if (isDead && deadLogicHappened == false)
		{
			deadLogicHappened = true;
			canAttack = false;
			EndBlastAttack();
			EncounterManager.EndEncounter();
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
	}

	/// <summary> The method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage((int)meleeDamageToGive.initialValue, false);
		}
	}

	/// <summary> Sets trigger to know boss is moving then moves</summary>
	private void Move()
	{
	    StartCoroutine(MoveOverSeconds(gameObject, campLocation.transform.position, 1f));
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

		if (inWall)
		{
			armAnimator.SetTrigger("Scratch");
		}
		else
		{
			characterAnimator.SetBool("tailAttack", true);
		}
	}

	/// <summary>
	///  called when the animation fully plays
	/// </summary>
	public void EndAttack()
	{
		characterAnimator.SetBool("tailAttack", false);
		characterAnimator.SetBool("shoot", false);
	}

	public void EndBlastAttack()
	{
		bossBlastAttack.enabled = false;
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