using UnityEngine;

public class SpikeSurge : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		speed,
		activationTime, // Starting time before the arc launches
		spikeTime,      // How long it takes a blast to appear
		maxX,           // Maximum x-coordinate of the direction
		minX,           // Minimum x-coordinate of the direction
		maxY,           // Maximum y-coordinate of the direction
		minY;           // Minimum y-coordinate of the direction
	public GameObject
		spike,  // Used to instantiate the blast objects
		source; // Where the blast launches from
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
		origin,    // The starting location of the first arc
		target;    // Position the arc will move towards
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set up the blasts to appear </summary>
	void Start()
	{
		// Seed the RNG and assign the proper starting values to various members
		new Random();
		activationTimer = activationTime;
		arcLaunching    = false;
		spikeTimer      = spikeTime;

		// If no follow object is set the blasts arc from the same permanent location
		if(source == null)
		{
			this.tag = "Enemy";
			source   = GameObject.FindGameObjectWithTag(this.tag);
			origin   = transform.position;
		}

		AssignDirection();
	}

	/// <summary> Count down to when the arc launches then launch it </summary>
	void FixedUpdate()
	{
		// Keep the arc from rotating with its source
		this.transform.Rotate(0.0f, 0.0f, 0.0f);

		ArcTimer();

		// The arc will launch once
		if (arcLaunching)
		{
			LaunchSpikes();
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Make a new blast appear along the arc </summary>
	void CreateBlast()
	{
		Instantiate(spike, transform.position, Quaternion.identity);
	}

	/// <summary> Launch the arc </summary>
	void LaunchSpikes()
	{
		// Find the launch angle and move in that direction
		SetAngle();
		transform.position = Vector2.MoveTowards(transform.position, target,
		                                         speed * Time.deltaTime);

		// Check if a new blast should appear
		spikeTimer -= Time.deltaTime;
		if (spikeTimer <= 0)
		{
			spikeTimer = spikeTime;
			CreateBlast();
		}

		// Check when to stop the arc
		if (Vector2.Distance(target, transform.position) <= 0.4f)
		{
			Destroy(gameObject);
		}
	}

	/// <summary> Set the arc angle </summary>
	void SetAngle()
	{
		// The missile angles towards a distance from and perpendicular to the target
		angle = Mathf.Atan2(target.y - target.y, target.x - target.x) + 90 * Mathf.Deg2Rad;

		// The missile angles closer to the target as it approaches
		direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) *
		                                          Vector2.Distance(target, transform.position);
	}

	/// <summary> Count down to the arc launch </summary>
	void ArcTimer()
	{
		// Count down to the arc launching
		if (activationTimer > 0 && arcLaunching == false)
		{
			activationTimer -= Time.deltaTime;

			// When time is up launch the arc
			if (activationTimer <= 0)
			{
				arcLaunching = true;
				activationTimer = activationTime;
			}
		}
	}

	void AssignDirection()
	{
		target.x = Random.Range(minX, maxX);
		target.y = Random.Range(minY, maxY);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}