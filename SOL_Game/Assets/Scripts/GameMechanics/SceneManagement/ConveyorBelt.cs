using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
	#region Enums
	public enum Direction // Controls where the conveyor belt pushes objects on it
	{
		Up,
		Down,
		Left,
		Right
	}
	#endregion

	#region Public Variables
	[Header("Keep at least 300.")]
	public float
		speed; // How fast the conveyor belt pushes things
	public Direction
		direction; // The conveyor belt's direction to push
	public Vector2
		movement; // The force vector to push objects
	#endregion

	#region Private Variables
	private bool
		isMoving; // Check if the conveyor belt is actually moving
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Turn on the conveyor belt </summary>
	void Start()
	{
		isMoving = true;
	}

	/// <summary> Turn on the conveyor belt </summary>
	void Update()
	{
		GetDirection();
	}

	/// <summary> Push objects on the conveyor belt </summary>
	private void OnTriggerStay2D(Collider2D collider)
	{
		// if the player collides with this trigger spawn in enemies
		if (isMoving)
		{
			collider.attachedRigidbody.AddRelativeForce(movement * speed, ForceMode2D.Force);
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Get the conveyor belt's direction </summary>
	void GetDirection()
	{
		switch (direction)
		{
			case Direction.Up:
				movement = Vector2.up;
				break;
			case Direction.Down:
				movement = Vector2.down;
				break;
			case Direction.Left:
				movement = Vector2.left;
				break;
			case Direction.Right:
				movement = Vector2.right;
				break;
			default:
				Debug.Log("No direction set.");
				break;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}