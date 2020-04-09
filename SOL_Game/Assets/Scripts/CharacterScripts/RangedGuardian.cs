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
		teleportAnimaiton,
		canonBullet,
		shotGunBullet;

	public List<GameObject>
		teleporterPositions;

	public float
		maxTimeBetweenAttacks = 2f,
		minTimeBetweenAttacks = 1f;

	public EncounterManager
		encounterManager;

	public EnemyMovement
	enemyMovement;
	public Transform // the point that a bullet will be created at
		firePointNorth,
		firePointEast,
		firePointSouth,
		firePointWest,
		canonFirePointNorth,
		canonFirePointEast,
		canonFirePointSouth,
		canonFirePointWest,
		parentShotgunFirePointNorth, // a bullet will fire from all transforms that are a child of this game obj
		parentShotgunFirePointEast,
		parentShotgunFirePointWest,
		parentShotgunFirePointSouth;
	#endregion

	#region Private Variables
	private Transform
		currentFirePointCanon, // the current point that the canon will shot from
		currentFirePointShotgun; // the current parent shotgun transform (a bullet will fire from all transforms that are a child of this game obj)
	private float
		attackCountDownTimer = 3,
		attackInterval = 1;

	private bool
		running = false,
		canSpawnEnemies = true,
		usingBlaster = false;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public override void FixedUpdate()
	{
		base.FixedUpdate();


		if (canAttack && usingBlaster == false && attackCountDownTimer <= 0 && maxHealth.runTimeValue > 0)
		{
			startRandomAttack();
		}
		else
		{
			attackCountDownTimer -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Methods
	private void startRandomAttack()
	{
		switch (Random.Range(0, 4))
		{
			case 0:
				StartBlasterAttackAnimation();
				break;
			case 1:
				StartShotGunAttackAnimation();
				break;
			default:
				StartCanonAttackAnimation();
				break;
		}
	}


	/// <summary> starts the attack animations (bullet created with an animation event and the shoot method)</summary>
	private void StartBlasterAttackAnimation()
	{
		// set flags
		usingBlaster = true;
		enemyMovement.canMove = false;

		// set up the attack animation to play
		characterAnimator.SetBool("blasting", true); // set bool flag blasting to true
		characterAnimator.SetInteger("attackDirection", GetAnimationDirection(1)); // set the value that plays the right blaster direction animation
		characterAnimator.SetLayerWeight(2, 2); // increase the blaster layer priority
	}

	/// <summary> starts the attack animations (bullet created with an animation event and the shoot method)</summary>
	private void StartShotGunAttackAnimation()
	{
		// set flags
		usingBlaster = true;
		enemyMovement.canMove = false;

		// set up the attack animation to play
		characterAnimator.SetBool("blasting", true); // set bool flag blasting to true
		characterAnimator.SetInteger("attackDirection", GetAnimationDirection(1)); // set the value that plays the right blaster direction animation
		
		characterAnimator.SetLayerWeight(4, 2); // increase the blaster layer priority
	}

	/// <summary> starts the attack animations (bullet created with an animation event and the shoot method)</summary>
	private void StartCanonAttackAnimation()
	{
		// set flags
		usingBlaster = true;
		enemyMovement.canMove = false;

		// set up the attack animation to play
		characterAnimator.SetBool("blasting", true); // set bool flag blasting to true
		characterAnimator.SetInteger("attackDirection", GetAnimationDirection(1)); // set the value that plays the right blaster direction animation
		characterAnimator.SetLayerWeight(3, 2); // increase the blaster layer priority
	}

	/// <summary>
	/// shoot big canon attack (called with an event in the attack animation)
	/// </summary>

	public void ShootCanon()
	{
		// shoot
		GameObject bulletInstance = Instantiate(canonBullet, currentFirePointCanon.position, currentFirePointCanon.rotation);
		BulletLogic bulletLogic = bulletInstance.GetComponent<BulletLogic>();
		bulletLogic.bulletDamage = (int)rangedAttackDamageToGive.initialValue;

		print("canon");
	}

	/// <summary>
	/// shoot the shot gun (called with an event in the attack animation)
	/// </summary>
	public void ShootShotGun()
	{
		foreach (Transform shotgunFirePoint in currentFirePointShotgun.GetComponentsInChildren<Transform>())
		{
			if(shotgunFirePoint == currentFirePointShotgun)
			{
				continue;
			}
			else
			{
				GameObject bulletInstance = Instantiate(shotGunBullet, shotgunFirePoint.position, shotgunFirePoint.rotation);
				BulletLogic bulletLogic = bulletInstance.GetComponent<BulletLogic>();
				bulletLogic.bulletDamage = (int)rangedAttackDamageToGive.initialValue;
			}
		}
		print("shot gun");
	}

	/// <summary> ends an attack animation (called with an event in the attack animation)</summary>
	public void EndAttackAnimation()
	{
		// set flag that the enemy is not using the blaster anymore
		usingBlaster = false;
		enemyMovement.canMove = true;

		// end the attack animation
		characterAnimator.SetLayerWeight(2, 0); // lowers the blaster layer priority
		characterAnimator.SetLayerWeight(3, 0); // lowers the blaster layer priority
		characterAnimator.SetLayerWeight(4, 0); // lowers the blaster layer priority
		characterAnimator.SetBool("blasting", false); // set flag blasting to false

		attackCountDownTimer = attackInterval;
	}


	/// <summary> this gets the direction that an animations should play based on the characters idle animation state</summary>
	public override int GetAnimationDirection(int idleLayerIndex)
	{
		int animationDirection = 0; // return value for the animations direction

		AnimatorClipInfo[] animatorStateInfo = characterAnimator.GetCurrentAnimatorClipInfo(idleLayerIndex);

		switch (animatorStateInfo[0].clip.name)
		{
			case "IdleLeft":
				animationDirection = WEST;
				firePoint = firePointWest;
				currentFirePointCanon = canonFirePointWest;
				currentFirePointShotgun = parentShotgunFirePointWest;
				break;
			case "IdleUp":
				animationDirection = NORTH;
				firePoint = firePointNorth;
				currentFirePointCanon = canonFirePointNorth;
				currentFirePointShotgun = parentShotgunFirePointNorth;
				break;
			case "IdleRight":
				animationDirection = EAST;
				firePoint = firePointEast;
				currentFirePointCanon = canonFirePointEast;
				currentFirePointShotgun = parentShotgunFirePointEast;
				break;
			case "IdleDown":
				animationDirection = SOUTH;
				firePoint = firePointSouth;
				currentFirePointCanon = canonFirePointSouth;
				currentFirePointShotgun = parentShotgunFirePointSouth;
				break;
		}

		return animationDirection;
	}
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		base.TakeDamage(damage, playSwordImpactSound);

		if(maxHealth.runTimeValue <= 0)
		{
			encounterManager.EndEncounter();
		}

		if (maxHealth.runTimeValue <= maxHealth.initialValue / 2 && canSpawnEnemies)
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
			Debug.LogError("There are no teleporter locations to teleport too!");
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