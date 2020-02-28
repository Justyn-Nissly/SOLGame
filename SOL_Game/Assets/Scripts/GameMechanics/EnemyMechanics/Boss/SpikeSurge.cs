using UnityEngine;

public class SpikeSurge : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		speed,          // How fast the spike trail moves
		activationTime, // Starting time before the arc launches
		spikeTime,      // How long it takes a blast to appear
		maxX,           // Maximum x-coordinate of the direction
		minX,           // Minimum x-coordinate of the direction
		maxY,           // Maximum y-coordinate of the direction
		minY,           // Minimum y-coordinate of the direction
		spikeTimeTillDestroyed = 5; // how long the instantiated spike will exist for
	public GameObject
		spike,  // Used to instantiate the blast objects
		source; // Where the blast launches from
	public Vector2
		target;    // Position the arc will move towards
	public bool
		stopWhenHitWall = false; // destroy this spike surge if it hits a wall(the wall needs a rigid body 2d to detect this)
	#endregion

	#region Private Variables
	private float
		activationTimer, // Time before the arc launches
		angle,           // Angle at which the arc travels
		spikeTimer;      // Time until another blast appears
	private bool
		arcLaunching; // The arc is being launched
	private Vector2
		direction, // Where the arc is moving
		origin;    // The starting location of the first arc

	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> destroy this spike surge if it hits a wall if the flag to stop when hit wall is set </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Wall") && stopWhenHitWall)
		{
			Destroy(gameObject);
		}
	}

	/// <summary> Set up the blasts to appear </summary>
	void Start()
	{
		// Seed the RNG and assign the proper starting values to various members
		new Random();
		activationTimer = activationTime;
		arcLaunching    = false;
		spikeTimer      = spikeTime;

		// If no follow object is set the spikes arc from the same permanent location
		if(source == null)
		{
			this.tag = "Enemy";
			source   = GameObject.FindGameObjectWithTag(this.tag);
			origin   = transform.position;
		}

		AssignDirection();
	}

	/// <summary> Count down to when the spikes launch then launch them </summary>
	void FixedUpdate()
	{
		// Keep the spikes from rotating with their source
		this.transform.Rotate(0.0f, 0.0f, 0.0f);

		SpikesTimer();

		// The spikes will launch once
		if (arcLaunching)
		{
			LaunchSpikes();
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Make a new spike appear along the trail </summary>
	void CreateSpike()
	{
		Destroy(Instantiate(spike, transform.position, Quaternion.identity), spikeTimeTillDestroyed);
	}

	/// <summary> Launch the arc </summary>
	void LaunchSpikes()
	{
		// Find the launch angle and move in that direction
		SetAngle();
		transform.position = Vector2.MoveTowards(transform.position, target,
		                                         speed * Time.deltaTime);

		// Check if a new spike should appear
		spikeTimer -= Time.deltaTime;
		if (spikeTimer <= 0)
		{
			spikeTimer = spikeTime;
			CreateSpike();
		}

		// Check when to stop the spikes
		if (Vector2.Distance(target, transform.position) <= 0.4f)
		{
			Destroy(gameObject);
		}
	}

	/// <summary> Set the movement angle </summary>
	void SetAngle()
	{
		// The spikes move in a line
		angle = Mathf.Atan2(target.y - target.y, target.x - target.x) + 90 * Mathf.Deg2Rad;

		// Set the spike trail's direction
		direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) *
		                                          Vector2.Distance(target, transform.position);
	}

	/// <summary> Count down to the spike launch </summary>
	void SpikesTimer()
	{
		// Count down to the spikes launching
		if (activationTimer > 0 && arcLaunching == false)
		{
			activationTimer -= Time.deltaTime;

			// When time is up launch the spikes
			if (activationTimer <= 0)
			{
				arcLaunching = true;
				activationTimer = activationTime;
			}
		}
	}

	/// <summary> Assign random coordinates for the spike trail to move towards </summary>
	void AssignDirection()
	{
		if(target == null)
		{
			target.x = Random.Range(minX, maxX);
			target.y = Random.Range(minY, maxY);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
