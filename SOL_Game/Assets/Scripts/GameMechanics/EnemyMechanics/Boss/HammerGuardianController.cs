using UnityEngine;

public class HammerGuardianController : Enemy
{
	#region Enums and Defined Constants
	public const int
		START_PHASE = 1; // The guardian starts at phase 1
	#endregion

	#region Public Variables
	public float
		attackTime,      // How long the guardian checks for the player before attacking
		restDelay,       // How long the guardian rests after an attack
		range,           // How far away the guardian will attack from
		attackDelayTime; // How long the guardian takes to attack
	public GameObject
		shockWave, // Used to create shockwaves
		spikes;    // Used to create spikes
	public int
		maxSpikes,   // Spike lines spawned by end phase shockwaves
		phase,       // The guardian's battle phase
		phaseHealth; // The guardian's starting health at each phase
	public SpriteRenderer
		sprite; // The guardian's sprite
	#endregion

	#region Private Variables
	private HammerGuardianMovement
		guardianMove; // Reference the movement script
	private HammerGuardianWeakness
		weakness; // The guardian's weak spot
	private float
		attackDelay,      // Time left before an attack
		attackTimer,      // Time left to check if the guardian will attack
		restTimer,        // Time left resting
		weaknessRotation; // Used to rotate the weak point as the guardian moves
	private Vector2
		facing; // The general direction the guardian is facing
	private bool
		isAttacking; // Check if the guardian is attacking
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Locate the player and initialize the guardian </summary>
	override public void Start()
	{
		isAttacking   = false;
		restTimer     = 0.0f;
		attackTimer   = attackTime;
		phase         = START_PHASE;
		player        = FindObjectOfType<Player>();
		weakness      = FindObjectOfType<HammerGuardianWeakness>();
		guardianMove  = FindObjectOfType<HammerGuardianMovement>();
		currentHealth = weakness.health = phaseHealth;
	}

	/// <summary> Turn towards and chase down the player </summary>
	override public void FixedUpdate()
	{
		// Make the character lower down overlap the character higher up
		Overlap();

		// The weak point rotates with the guardian
		RotateWeakness();

		// Check if the guardian has taken enough damage to advance the phase
		AdvancePhase();

		// When attacking the player the guardian stops
		Targeting();

		// Check if the guardian should take damage
		canTakeDamage   = isAttacking;
		weakness.health = (int) currentHealth;
	}
	#endregion

	#region Utility Methods
	/// <summary> Check if the player is in position to be attacked </summary>
	public bool AttackCheck()
	{
		facing = guardianMove.GetDirection();

		return (((guardianMove.targetAngle >=  45.0f && guardianMove.targetAngle  <= 135.0f &&
		          facing                   == Vector2.up)                                   ||
		         (guardianMove.targetAngle >= 225.0f && guardianMove.targetAngle  <= 315.0f &&
		          facing                   == Vector2.down)                                 ||
		         (guardianMove.targetAngle >  135.0f && guardianMove.targetAngle  <  225.0f &&
		          facing                   == Vector2.left)                                 ||
		         (guardianMove.targetAngle >  315.0f || guardianMove.targetAngle  <   45.0f &&
		          facing                   == Vector2.right))                               &&
		         (Vector2.Distance(transform.position, player.transform.position) < range));
	}

	/// <summary> Attack the player </summary>
	private void Attack()
	{
		// Stop moving and reset the rest and attack timers
		guardianMove.canMove = false;
		restTimer            = restDelay;
		attackTimer          = attackTime;

		// Phase 1 emits 1 shockwave
		if (phase == START_PHASE)
		{
			Instantiate(shockWave, (Vector2)transform.position + guardianMove.GetDirection() * range * 0.65f,
						Quaternion.identity);
		}
		// Phase 2 emits 2 shockwaves with lines of spikes
		else
		{
			// The guardian attacks on the left and right if it is facing up or down
			if (guardianMove.GetDirection() == Vector2.up || guardianMove.GetDirection() == Vector2.down)
			{
				Instantiate(shockWave, (Vector2)transform.position + Vector2.left * range * 0.65f,
				            Quaternion.identity);
				Instantiate(shockWave, (Vector2)transform.position + Vector2.right * range * 0.65f,
				            Quaternion.identity);

				// Send out spikes from the shockwaves
				for (int spike = 1; spike <= maxSpikes; spike++)
				{
					Instantiate(spikes, (Vector2)transform.position + Vector2.left * range * 0.65f,
					            Quaternion.identity);
					Instantiate(spikes, (Vector2)transform.position + Vector2.right * range * 0.65f,
					            Quaternion.identity);
				}
			}
			// The guardian attacks on the top and bottom if it is facing left or right
			else
			{
				Instantiate(shockWave, (Vector2)transform.position + Vector2.up * range * 0.65f,
				            Quaternion.identity);
				Instantiate(shockWave, (Vector2)transform.position + Vector2.down * range * 0.65f,
				            Quaternion.identity);

				// Send out spikes from the shockwaves
				for (int spike = 1; spike <= maxSpikes; spike++)
				{
					Instantiate(spikes, (Vector2)transform.position + Vector2.up * range * 0.65f,
					            Quaternion.identity);
					Instantiate(spikes, (Vector2)transform.position + Vector2.down * range * 0.65f,
					            Quaternion.identity);
				}
			}
		}
	}

	/// <summary> Rotate the guardian's weak point to its front </summary>
	private void RotateWeakness()
	{
		// Find what direction the guardian is facing
		facing = guardianMove.GetDirection();
		if (facing == Vector2.up)
		{
			weaknessRotation = 90.0f;
		}
		else if (facing == Vector2.down)
		{
			weaknessRotation = 270.0f;
		}
		else if (facing == Vector2.left)
		{
			weaknessRotation = 180.0f;
		}
		else
		{
			weaknessRotation = 0.0f;
		}

		// Rotate the weakness in the direction the guardian is facing
		weakness.transform.localRotation = Quaternion.AngleAxis(weaknessRotation + 90.0f, Vector3.forward);
	}

	/// <summary> Attack if the player is in position and then rest </summary>
	private void Targeting()
	{
		if (isAttacking)
		{
			// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION
			sprite.color = Color.red;
			// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION

			// The guardian takes damage only when it is ready to attack
			currentHealth = weakness.health;

			// The guardian pauses before striking
			guardianMove.canMove = false;
			if (attackDelay > 0.0f)
			{
				attackDelay -= Time.deltaTime;
			}
			else
			{
				Attack();
				restTimer = restDelay;
				isAttacking = false;
			}
		}
		else
		{
			// Check if the guardian is resting
			if (restTimer > 0.0f)
			{
				restTimer -= Time.deltaTime;
				// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION
				sprite.color = Color.green;
				// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION
			}

			// The guardian pursues the player
			else
			{
				guardianMove.canMove = true;

				// If the player is in position to be attacked long enough the guardian attacks
				if (AttackCheck())
				{
					// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION
					sprite.color = new Vector4(1.0f - (attackTimer / attackTime), 0.0f, attackTimer / attackTime, 1.0f);
					// TEMPORARY COLOR CHANGE WILL BE REPLACED WITH ANIMATION

					attackTimer -= Time.deltaTime;
					if (attackTimer <= 0.0f)
					{
						isAttacking = true;
						attackDelay = attackDelayTime;
					}
				}
				else
				{
					attackTimer = attackTime;
				}
			}
		}
	}

	/// <summary> Advance the fight to the next phase </summary>
	private void AdvancePhase()
	{
		if (weakness.health <= 0)
		{
			// Advance to the next phase or destroy if already at the last phase
			if (phase == START_PHASE)
			{
				phase++;
				currentHealth = weakness.health = phaseHealth;
				restTimer     = restDelay;
				isAttacking   = false;
			}
			else
			{
				// Play death animation and die
				Destroy(gameObject);
			}
		}
	}

	/// <summary> Make the character lower down appear in front of the other character </summary>
	private void Overlap()
	{
		sprite.sortingOrder = ((player.transform.position.y < this.transform.position.y) ? 0 : 2);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}