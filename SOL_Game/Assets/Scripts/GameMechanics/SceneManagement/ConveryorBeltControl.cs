using UnityEngine;
using System.Collections.Generic;

public class ConveryorBeltControl : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	private bool
		reversed;
	private ConveyorBelt[]
		belts;
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
	// Initalize the missile
	void Start()
	{
		reversed      = false;
		belts         = FindObjectsOfType<ConveyorBelt>();
		sprite        = GetComponent<SpriteRenderer>();
		soundEffect   = GetComponent<AudioSource>();
		coolDownTime  = 0.0f;
		coolDownTimer = 1.0f;
	}

	/// <summary> Lock on to the target and arc towards it </summary>
	void FixedUpdate()
	{
		if (reversed && coolDownTime <= 0.0f)
		{
			SwitchBeltDirection();
			coolDownTime = coolDownTimer;
			reversed     = !reversed;
			sprite.flipX = !sprite.flipX;
			soundEffect.Play();
		}
		else
		{
			coolDownTime -= Time.deltaTime;
		}
	}

	/// <summary> Damage the player on contact and destroy the missile </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			reversed = true;
		}
	}
	#endregion

	#region Utility Methods
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