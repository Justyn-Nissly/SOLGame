using UnityEngine;

public class ArcBlast : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		activationTime, // Starting time before the arc launches
		blastTime,      // How long it takes a blast to appear
		arcOffset,      // Curve of the blast arc
		speed;
	public GameObject
		blast,  // Used to instantiate the blast objects
		source; // Where the blast launches from
	public bool
		veerLeft; // The arc will curve left
	#endregion

	#region Private Variables
	private Player
		player; // Reference the player
	private float
		activationTimer, // Time before the arc launches
		angle,           // Angle at which the arc travels
		blastTimer;      // Time until another blast appears
	private bool
		arcLaunching; // The arc is being launched
	private Vector2
		direction, // Where the arc is moving
		origin;    // The starting location of the first arc
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set up the blasts to appear </summary>
	void Start()
	{
		// Seed the RNG and assign the proper starting values to various members
		new Random();
		player          = FindObjectOfType<Player>();
		activationTimer = activationTime;
		arcLaunching    = false;
		blastTimer      = blastTime;

		// If no follow object is set the blasts arc from the same permanent location
		if(source == null)
		{
			this.tag = "Enemy";
			source   = GameObject.FindGameObjectWithTag(this.tag);
			origin   = transform.position;
		}
	}

	/// <summary> Count down to when the arc launches then launch it </summary>
	void FixedUpdate()
	{
		// Independent arcs always launch from the original position
		if (arcLaunching == false && source.transform.position == this.transform.position)
		{
			transform.position = origin;
		}

		// Keep the arc from rotating with its source
		this.transform.Rotate(0.0f, 0.0f, 0.0f);

		ArcTimer();

		// The arc will launch once
		if (arcLaunching)
		{
			LaunchArc();
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Make a new blast appear along the arc </summary>
	void CreateBlast()
	{
		Instantiate(blast, transform.position, Quaternion.identity);
	}

	/// <summary> Launch the arc </summary>
	void LaunchArc()
	{
		// Find the launch angle and move in that direction
		SetAngle();
		transform.position = Vector2.MoveTowards(transform.position, (Vector2) (player.transform.position) + direction,
		                                         speed * Time.deltaTime);

		// Check if a new blast should appear
		blastTimer -= Time.deltaTime;
		if (blastTimer <= 0)
		{
			blastTimer = blastTime;
			CreateBlast();
		}

		// Check when to stop the arc
		if (Vector2.Distance(player.transform.position, transform.position) <= 0.4f)
		{
			transform.position = source.transform.position;
			arcLaunching = false;
		}
	}

	/// <summary> Set the arc angle </summary>
	void SetAngle()
	{
		// The missile angles towards a distance from and perpendicular to the target
		angle = Mathf.Atan2(player.transform.position.y - transform.position.y,
		                    player.transform.position.x - transform.position.x) +
		                  ((veerLeft) ? 90 : -90) * Mathf.Deg2Rad;

		// The missile angles closer to the target as it approaches
		// Offset affects arc width and effective range determines how easily the missile follows the target
		direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * arcOffset *
		                                          Vector2.Distance(player.transform.position, transform.position);
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

				// There is a chance the arc will curve the other way
				veerLeft = (Random.Range(0, 10) >= 7) ? !veerLeft : veerLeft;
			}
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}