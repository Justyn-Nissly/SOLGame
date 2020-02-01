using UnityEngine;

public class BossWaveAttack : MonoBehaviour
{
/*	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Player
		player; // Reference the player
	public float
		activationTime; // Starting time before the wave launches
	public GameObject
		blast; // Used to instantiate the blast objects
	#endregion

	#region Private Variables
	private float
		angle,           // Angle at which the wave travels
		activationTimer; // Time before the wave launches
	private bool
		waveLaunching; // The wave is being launched
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set up the blasts to appear </summary>
	void Start()
	{
		activationTimer = activationTime;
		waveLaunching   = false;
	}

	/// <summary> Count down to when the wave launches then launch it </summary>
	void FixedUpdate()
	{
		// Count down to the wave launching
		if (activationTimer > 0 && waveLaunching == false)
		{
			activationTimer -= Time.deltaTime;

			// When time is up start creating the blasts
			if (activationTimer <= 0)
			{
				//LaunchWave();
				waveLaunching = true;
			}
		}

		// Create the specified number of blasts
		waveLaunching = false;
	}

	#endregion

	#region Utility Methods
	/// <summary> Make a new blast appear in a random location within the range </summary>
	void CreateBlast()
	{
		Instantiate(blast, new Vector2(Random.Range(minWidth, maxWidth),
		                               Random.Range(minHeight, maxHeight)), Quaternion.identity);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion*/
}