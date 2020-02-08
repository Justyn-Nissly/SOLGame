using UnityEngine;

public class ConveryorBeltControl : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	private bool
		reversed; // The conveyor belts have been reversed
	private ConveyorBelt[]
		belts; // Keep track of all conveyor belts in the scene
	private SpriteRenderer
		sprite; // The belt control lever
	private float
		coolDownTime,  // Time left until the lever can be flipped again
		coolDownTimer; // Time until the lever can be flipped again
	private AudioSource
		soundEffect; // Played when the lever flips
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize belt control members </summary>
	void Start()
	{
		reversed      = false;
		belts         = FindObjectsOfType<ConveyorBelt>();
		sprite        = GetComponent<SpriteRenderer>();
		soundEffect   = GetComponent<AudioSource>();
		coolDownTime  = 1.0f;
		coolDownTimer = 0.0f;
	}

	/// <summary> Reverse the conveyor belts' direction when the lever is thrown </summary>
	void FixedUpdate()
	{
		// Check if the lever has been thrown
		if (reversed && coolDownTimer <= 0.0f)
		{
			SwitchBeltDirection();
			coolDownTimer = coolDownTime;
			reversed      = !reversed;
			sprite.flipX  = !sprite.flipX;
			soundEffect.Play();
		}
		else
		{
			coolDownTimer -= Time.deltaTime;
		}
	}

	/// <summary> The lever is thrown when the player touches it </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			reversed = true;
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Reverse the direction of each conveyor belt in the scene </summary>
	void SwitchBeltDirection()
	{
		foreach (ConveyorBelt conveyorBelt in belts)
		{
			switch (conveyorBelt.direction)
			{
				case ConveyorBelt.Direction.Up:
					conveyorBelt.direction = ConveyorBelt.Direction.Down;
					break;
				case ConveyorBelt.Direction.Down:
					conveyorBelt.direction = ConveyorBelt.Direction.Up;
					break;
				case ConveyorBelt.Direction.Left:
					conveyorBelt.direction = ConveyorBelt.Direction.Right;
					break;
				case ConveyorBelt.Direction.Right:
					conveyorBelt.direction = ConveyorBelt.Direction.Left;
					break;
				default:
					Debug.Log("No direction set.");
					break;
			}
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}