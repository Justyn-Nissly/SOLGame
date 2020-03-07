using UnityEngine;

public class BehemothController : Enemy
{
	#region Enums and Defined Constants
	public const int
		END_PHASE  = 3,  // The boss has this many phases
		OUTER_ORBS = 8,  // Orbs revolving far from the boss
		INNER_ORBS = 4;  // Orbs revolving near the boss
	public const float
		INNER_ORB_DIST           = 2.5f,                  // How far out the inner orbs rotate
		OUTER_ORB_DIST           = INNER_ORB_DIST * 2.0f, // How far out the outer orbs rotate
		CONVEYOR_BELT_SPEED      = 400.0f,                // The boss activates the conveyor belts
		CONVEYOR_BELT_SWITCH_MIN = 15.0f,                 // Minimum time before switching conveypr belt direction
		CONVEYOR_BELT_SWITCH_MAX = 30.0f;                 // Maximum time before switching conveypr belt direction
	#endregion

	#region Public Variables
	public float
		phaseChangeDelay,      // How long the boss takes to move to the next phase
		phaseChangeTimer,      // Count down to start the next phase
		tractorBeamDuration,   // How long the tractor beam fires
		tractorBeamChargeTime; // How long the tractor beam takes to charge
	public int
		phaseHealth; // Starting health for each phase
	public GameObject
		powerWave, // Used to instantiate power waves
		orb;       // Used to instantiate attack orbs
	public EnemyMovement
		enemyMovement; // Make the boss move
	#endregion

	#region Private Variables
	private bool
		defeated; // Check if the boss has taken fatal damage
	private float
		conveyorBeltTimer, // Time until the conveyor belt directions switch
		tractorBeamCharge, // How much longer the tractor beam will take to charge
		tractorBeamTimer,  // How much longer the tractor beam will fire
		defeatTimer;       // Time left between fatal damage and being destroyed
	private int
		phase; // Current phase of the boss fight
	private OrbController []
		allOrbs,   // Track all the orbs
		outerOrbs, // Track the outer orbs
		innerOrbs; // Track the inner orbs
	private TractorBeamEmitter
		emitter; // Use tractor beam
	private ConveyorBeltControl
		conveyorBeltControl; // Controls conveyor belts
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the boss and set up scene control </summary>
	override public void Start()
	{
		// Control conveyor belts
		new Random();
		conveyorBeltTimer         = Random.Range(CONVEYOR_BELT_SWITCH_MIN, CONVEYOR_BELT_SWITCH_MAX);
		conveyorBeltControl       = GetComponent<ConveyorBeltControl>();
		conveyorBeltControl.belts = FindObjectsOfType<ConveyorBelt>();
		foreach (ConveyorBelt belt in conveyorBeltControl.belts)
		{
			belt.speed                  = CONVEYOR_BELT_SPEED;
			conveyorBeltControl.enabled = false;
		}

		// Reference movement, tractor beam, and player scripts
		enemyMovement = GetComponent<EnemyMovement>();
		emitter       = GetComponent<TractorBeamEmitter>();
		player        = FindObjectOfType<Player>();

		// Set various members to begin the battle
		phase       = 0;
		defeatTimer = 3.0f;
		defeated    = false;
		aggro       = true;

		// Set up the arrays to control the boss's orbs
		outerOrbs = new OrbController [OUTER_ORBS];
		innerOrbs = new OrbController [INNER_ORBS];

		// Delay the boss's activation and charge the tractor beam
		phaseChangeTimer  = phaseChangeDelay * 5.0f;
		tractorBeamCharge = tractorBeamChargeTime - 5.0f;
	}

	/// <summary> Turn towards and chase down the player </summary>
	override public void FixedUpdate()
	{
		// Randomly reverse conveyor belt directions after the first phase
		if (phase > 1)
		{
			ControlConveyorBelts();
		}

		// Keep the orbs following the boss
		CenterOrbs();

		// Move unless the phase is changing, the boss is defeated, or the tractor beam is in use
		enemyMovement.enabled = (phaseChangeTimer  <= 0.0f && defeated          == false                         &&
		                        (tractorBeamCharge >  3.0f && tractorBeamCharge <  tractorBeamChargeTime - 3.0f) &&
		                         emitter.emitting == false);

		// Remain invincible unless hit by the hammer
		canTakeDamage = false;

		// Upon defeat the boss is destroyed after a short time
		if (defeated)
		{
			if ((defeatTimer -= Time.deltaTime) <= 0.0f)
			{
				Destroy(gameObject);
			}
		}
		// Charge and fire the tractor beam in the final phase
		else if (phase == END_PHASE && tractorBeamCharge <= 0.0f)
		{
			tractorBeamCharge     = tractorBeamChargeTime;
			tractorBeamTimer      = tractorBeamDuration;
		}
		else if (tractorBeamTimer > 0.0f)
		{
			FireTractorBeam();
		}
		else
		{
			// Ignore the tractor beam until the final phase
			tractorBeamCharge -= (phase == END_PHASE) ? Time.deltaTime : 0;

			// Move to the next phase if applicable
			if (phaseChangeTimer > 0.0f)
			{
				phaseChangeTimer      -= Time.deltaTime;
				if (phaseChangeTimer <= 0.0f)
				{
					AdvancePhase();
				}
			}
		}
	}

	///<summary> Deal damage to the boss </summary>
	private void OnTriggerStay2D(Collider2D collision)
	{
		// The boss takes damage if the hammer hits and all outer orbs are destroyed
		if (collision.gameObject.CompareTag("PlayerHeavyWeapon") && phaseChangeTimer <= 0.0f &&
			CheckOrbsEmpty(outerOrbs, OUTER_ORBS))
		{
			// The fight goes to the next phase if the boss runs out of health
			canTakeDamage = true;
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
	#endregion

	#region Utility Methods
	///<summary> The fight goes tothe next phase </summary>
	private void AdvancePhase()
	{
		// Halt the conveyor belts and tractor beam if the boss is defeated
		if (++phase > END_PHASE)
		{
			defeated = true;
			tractorBeamCharge = 100.0f;
			foreach (ConveyorBelt belt in conveyorBeltControl.belts)
			{
				belt.speed = 0.0f;
			}
		}
		// Get ready for the next phase
		else
		{
			currentHealth = phaseHealth;
			SpawnOrbs();

			// In the final phase double the health, start using the tractor beam, and orbs deal more damage
			if (phase == END_PHASE)
			{
				currentHealth     *= 2.0f;
				tractorBeamCharge  = tractorBeamChargeTime;
				foreach (OrbController orb in outerOrbs)
				{
					orb.revolveScript.contactDamage = 2;
				}
			}
		}

		// Pursue in the first phase then free roam after that
		enemyMovement.evasionDistance  = -enemyMovement.evasionDistance;
		enemyMovement.Type             = (phase == 1) ? EnemyMovement.MovementType.EvadePlayer
		                                              : EnemyMovement.MovementType.FreeRoam;
		moveSpeed                     *= (phase % 2 == 1) ? 0.33f : 3.0f;
		followRange                    = -followRange;
		aggroRange                     = -aggroRange;
	}

	///<summary> Spawn the boss's shield orbs </summary>
	private void SpawnOrbs()
	{
		// Setup arrays for tracking the orbs
		outerOrbs = new OrbController[OUTER_ORBS];
		innerOrbs = new OrbController[INNER_ORBS];
		allOrbs   = new OrbController[OUTER_ORBS + ((phase > 1) ? INNER_ORBS : 0)];

		// Instantiate and find the orbs
		for (int i = 0; i < OUTER_ORBS + ((phase > 1) ? INNER_ORBS : 0); i++)
		{
			Instantiate(orb, transform.position, Quaternion.identity);
		}
		allOrbs   = FindObjectsOfType<OrbController>();

		// Make the orbs target the player
		foreach (OrbController orb in allOrbs)
		{
			orb.target = GameObject.FindGameObjectWithTag("Player");
		}

		// Separate the inner and outer orbs
		for (int i = 0; i < OUTER_ORBS + ((phase > 1) ? INNER_ORBS : 0); i++)
		{
			if (phase > 1 && i < INNER_ORBS)
			{
				innerOrbs[i] = allOrbs[i];
			}
			else
			{
				outerOrbs[i - ((phase > 1) ? INNER_ORBS : 0)] = allOrbs[i];
			}
		}

		// Initialize the outer orbs
		for (int i = 0; i < OUTER_ORBS; i++)
		{
			outerOrbs[i].revolveScript.revolutionPoint       = transform.position;
			outerOrbs[i].revolveScript.maxRevolutionDistance = OUTER_ORB_DIST + 2.5f * (phase - 1);
			outerOrbs[i].revolveScript.minRevolutionDistance = OUTER_ORB_DIST;
			outerOrbs[i].revolveScript.startAngle            = 360.0f / (float) OUTER_ORBS * i;
			outerOrbs[i].attackHP                            = (phase > 1) ? phase : 0;
			outerOrbs[i].destructible.health                 = phase + 1;
			outerOrbs[i].revolveScript.clockwise             = true;
			outerOrbs[i].revolveScript.revolutionSpeed       = 1.0f;
		}

		// Initialize the inner orbs
		if (phase > 1)
		{
			for (int i = 0; i < INNER_ORBS; i++)
			{
				innerOrbs[i].revolveScript.revolutionPoint       = transform.position;
				innerOrbs[i].revolveScript.maxRevolutionDistance = INNER_ORB_DIST;
				innerOrbs[i].revolveScript.minRevolutionDistance = INNER_ORB_DIST;
				innerOrbs[i].revolveScript.startAngle            = 360.0f / (float)INNER_ORBS * i;
				innerOrbs[i].attackHP                            = 0;
				innerOrbs[i].destructible.health                 = phase * 2 - 1;
				outerOrbs[i].revolveScript.revolutionSpeed       = 1.2f;
			}
		}
	}

	///<summary> Check if an orb array is empty </summary>
	private bool CheckOrbsEmpty(OrbController[] orbs, int orbCount)
	{
		for (int i = orbCount - 1; i >= 0; i--)
		{
			orbCount -= (orbs[i] == null) ? 1 : 0;
		}
		return (orbCount == 0);
	}

	///<summary> Make the orbs follow the boss </summary>
	private void CenterOrbs()
	{
		// Move the inner orbs with the boss and keep them invincible unless all outer orbs are destroyed
		if (CheckOrbsEmpty(innerOrbs, INNER_ORBS) == false)
		{
			foreach (OrbController orb in innerOrbs)
			{
				orb.revolveScript.revolutionPoint = transform.position;
			}
			if (CheckOrbsEmpty(outerOrbs, OUTER_ORBS) == false)
			{
				foreach (OrbController orb in innerOrbs)
				{
					orb.destructible.health = phase * 2;
				}
			}
		}

		// Move the outer orbs with the boss
		if (CheckOrbsEmpty(outerOrbs, OUTER_ORBS) == false)
		{
			foreach (OrbController orb in outerOrbs)
			{
				orb.revolveScript.revolutionPoint = transform.position;
			}
		}
		else if (CheckOrbsEmpty(innerOrbs, INNER_ORBS) == false)
		{
			foreach (OrbController orb in innerOrbs)
			{
				orb.attackHP = phase * 2;
			}
		}
	}

	///<summary> Fire the tractor beam at the player </summary>
	private void FireTractorBeam()
	{
		// Prepare the tractor beam and stop moving before it activates
		emitter.emitting      = true;

		// Shut the tractor beam off after it uses up its charge
		if ((tractorBeamTimer -= Time.deltaTime) <= 0.0f)
		{
			emitter.emitting      = false;
			tractorBeamCharge     = tractorBeamChargeTime;

			// Emit a power wave
			if (defeated == false)
			{
				Instantiate(powerWave, transform.position, Quaternion.identity);
			}
		}
	}

	///<summary> Change the conveyor belts' direction </summary>
	private void ControlConveyorBelts()
	{
		if ((conveyorBeltTimer -= Time.deltaTime) <= 0.0f)
		{
			conveyorBeltControl.enabled = true;
			conveyorBeltControl.SwitchBeltDirection();
			conveyorBeltControl.enabled = false;
			conveyorBeltTimer           = Random.Range(CONVEYOR_BELT_SWITCH_MIN, CONVEYOR_BELT_SWITCH_MAX);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}