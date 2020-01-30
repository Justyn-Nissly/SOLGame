using UnityEngine;

public class BossBlastAttack : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public int
		blastCompactness, // Affects how  many blasts appear; lower numbers cause more blasts
		maxHeight,        // How high a blast can appear
		minHeight,        // How low a blast can appear
		maxWidth,         // How far right a blast can appear
		minWidth;         // How far left a blast can appear
	public float
		activationTime; // Time before the blasts start
	public GameObject
		blast; // Used to instantiate the blast objects
	#endregion

	#region Private Variables
	private float
		activationTimer; // Track how much time remains before the blasts appear
	private int
		blasts; // How many blasts will appear
	private bool
		isBlasting; // Track if the blasts are appearing or not
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set up the blasts to appear </summary>
	void Start()
	{
		new Random();
		activationTimer = activationTime;
		isBlasting      = false;
	}

	/// <summary> Count down to when the blasts will appear then create them </summary>
	void FixedUpdate()
	{
		// Count down to the blasts being created
		if (activationTimer > 0 && isBlasting == false)
		{
			activationTimer -= Time.deltaTime;

			// When time is up start creating the blasts
			if (activationTimer <= 0)
			{
				activationTimer = activationTime;
				blasts          = (maxHeight - minHeight) * (maxWidth - minWidth) / blastCompactness;
				isBlasting      = true;
			}
		}

		// Create the specified number of blasts
		if (blasts > 0)
		{
			CreateBlast();
			blasts--;
		}
		else
		{
			isBlasting = false;
		}
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
	#endregion
}