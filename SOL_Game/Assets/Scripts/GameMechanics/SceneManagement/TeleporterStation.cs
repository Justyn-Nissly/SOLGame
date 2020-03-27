using UnityEngine;

public class TeleporterStation : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public ConveyorBelt
		tractorField;
	public CircleCollider2D
		teleportPoint;
	#endregion

	#region Private Variables
	private float
		returnTimer;
	private bool
		canReturn;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Turn on the conveyor belt </summary>
	void Awake()
	{
		teleportPoint.enabled = false;
		canReturn = false;
		tractorField.direction = ConveyorBelt.Direction.Down;
		returnTimer = 15.0f;
	}

	/// <summary> Turn on the conveyor belt </summary>
	void FixedUpdate()
	{
		if (canReturn == false)
		{
			returnTimer -= Time.deltaTime;
			if (returnTimer <= 2.0f)
			{
				teleportPoint.enabled = true;
			}
			if (returnTimer <= 0.0f)
			{
				canReturn = true;
				tractorField.direction = ConveyorBelt.Direction.Up;
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}