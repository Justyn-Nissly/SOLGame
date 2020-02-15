using UnityEngine;

public class BehemothController : Enemy
{
	#region Enums and Defined Constants
	public const int
		END_PHASE       = 3,     // The boss has this many phases
		OUTER_ORBS      = 8,     // Orbs revolving far from the boss
		INNER_ORBS      = 4,     // Orbs revolving right by the boss
		INNER_ORB_GUARD = 10000; // Prevent damage to inner orbs before outer orbs are destroyed
	public const float
		ORB_CHECK_TIME = 5.0f; // How often to check if the orbs are destroyed
	#endregion

	#region Public Variables
	public float
		phaseChangeDelay; // How long the boss takes to move to the next phase
	public int
		phaseHealth; // Starting health for each phase
	public GameObject
		orb; // Used to instantiate attack orbs
	#endregion

	#region Private Variables
	private bool
		tractorBeamReady, // Check if the boss can use the tractor beam
		defeated;         // Check if the boss has taken fatal damage
	private float
		defeatTimer,      // Time left between fatal damage and being destroyed
		phaseChangeTimer; // Count down to start the next phase
	private int
		playerDamage, // Damage from the player
		phase;        // Current phase of the boss fight
	private OrbController[]
		outerOrbs, // Track the outer orbs
		innerOrbs; // Track the inner orbs
	private EnemyMovement
		enemyMovement; // Make the boss move
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Locate the player and start the guardian facing down and moving </summary>
	override public void Start()
	{
		enemyMovement    = FindObjectOfType<EnemyMovement>();
		player           = FindObjectOfType<Player>();
		outerOrbs        = new OrbController [OUTER_ORBS];
		innerOrbs        = new OrbController [INNER_ORBS];
		phase            = 0;
		tractorBeamReady = false;
		defeated         = false;
		defeatTimer      = 3.0f;
		playerDamage     = 0;
		aggro            = true;
		AdvancePhase();
		phaseChangeTimer = phaseChangeDelay * 4.0f;
	}

	/// <summary> Turn towards and chase down the player </summary>
	override public void FixedUpdate()
	{
		// The boss is invincible to eveything but the hammer
		canTakeDamage = false;

		if (defeated)
		{
			if ((defeatTimer -= Time.deltaTime) <= 0.0f)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			if (phaseChangeTimer > 0.0f)
			{
				canTakeDamage     = false;
				phaseChangeTimer -= Time.deltaTime;
				if (phaseChangeTimer <= 0.0f)
				{
					AdvancePhase();
				}
			}
			; // CODE
		}
	}
	#endregion

	#region Utility Methods
	///<summary> Deal damage to the boss </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// check if the right weapon is whats hitting this object
		if (collision.gameObject.CompareTag("PlayerHeavyWeapon") && phaseChangeTimer <= 0.0f)
		{
			canTakeDamage = true;
			// The fight goes to the next phase if the boss runs out of health
			if (currentHealth <= 1)
			{
				phaseChangeTimer = phaseChangeDelay;
				currentHealth    = phaseHealth;
				base.TakeDamage(0, false);
			}
			else
			{
				base.TakeDamage(1, false);
			}
		}
	}

	private void AdvancePhase()
	{
		if (phase++ > END_PHASE)
		{
			defeated = true;
		}
		else
		{
			currentHealth = phaseHealth;
		}

		// Spawn outer orbs (oscillating with for-each)
		// Spawn inner orbs

		if (phase >= END_PHASE - 1)
		{
			// PHASE 2 
			// (lock-on outer orbs with for-each)
		}

		if (phase >= END_PHASE)
		{
			// Phase 3
			tractorBeamReady = true;
		}
		canTakeDamage = true;
	}

	private void SpawnOuterOrbs()
	{
		outerOrbs = new OrbController[OUTER_ORBS];

		// Instantiate outer orbs
		for (int i = 0; i < OUTER_ORBS; i++)
		{
			Instantiate(orb, transform.position, Quaternion.identity);
		}

		// Initialize the outer orbs
		outerOrbs = FindObjectsOfType<OrbController>();
		foreach (OrbController orb in outerOrbs)
		{
			;
		}
	}
	#endregion


	#region Coroutines (Empty)
	#endregion
}