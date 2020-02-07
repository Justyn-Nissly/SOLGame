using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
	#region Enums
	public enum Direction
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
		direction;
	public Vector2
		movement;
	#endregion

	#region Private Variables
	private bool 
		isMoving;
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		isMoving = true;
	}

	void Update()
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

	private void OnTriggerStay2D(Collider2D collider)
	{
		// if the player collides with this trigger spawn in enemies
		if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Enemy") && isMoving)
		{
			collider.attachedRigidbody.AddRelativeForce(movement * speed, ForceMode2D.Force);
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}