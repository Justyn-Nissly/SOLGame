using UnityEngine;

public class BehemothController : Enemy
{
	#region Enums and Defined Constants
	public const int
		END_PHASE       = 3,     // The boss has this many phases
		FIRST_PHASE     = 1,     // The first phase after the boss is initialized
		OUTER_ORBS      = 8,     // Orbs revolving far from the boss
		INNER_ORBS      = 4,     // Orbs revolving right by the boss
		INNER_ORB_GUARD = 10000, // Prevent damage to inner orbs before outer orbs are destroyed
		HEALTH_GUARD    = 10;    // Prevent the boss from being destroyed before its final stage
	public const float
		ORB_CHECK_TIME = 5.0f; // How often to check if the orbs are destroyed
	#endregion

	#region Public Variables
	public float
		phaseChangeDelay; // How long the boss takes to move to the next phase
	public int
		phaseHealth; // Starting health for each phase
	#endregion

	#region Private Variables
	private bool
		tractorBeamReady; // Check if the boss can use the tractor beam
	private float
		phaseChangeTimer; // Count down to start the next phase
	private int
		phase; // Current phase of the boss fight
	private OrbController[]
		outerOrbs,
		innerOrbs,
		totalOrbs;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Locate the player and start the guardian facing down and moving </summary>
	override public void Start()
	{
		currentHealth    = phaseHealth + HEALTH_GUARD;
		player           = FindObjectOfType<Player>();
		outerOrbs        = new OrbController [OUTER_ORBS];
		innerOrbs        = new OrbController [INNER_ORBS];
		phase            = FIRST_PHASE;
		tractorBeamReady = true;
		AdvancePhase();
	}

	/// <summary> Turn towards and chase down the player </summary>
	override public void FixedUpdate()
	{
		if (currentHealth <= HEALTH_GUARD)
		{
			AdvancePhase();
		}

		if (phaseChangeTimer > 0.0f)
		{
			phaseChangeTimer -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Methods
	private void AdvancePhase()
	{
		if (phase++ > END_PHASE)
		{
			Debug.Log("he ded");
		}
		else
		{
			currentHealth    = phaseHealth + HEALTH_GUARD;
			phaseChangeTimer = phaseChangeDelay;
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


	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}