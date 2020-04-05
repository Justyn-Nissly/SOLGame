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
	private const int
		SPRITES = 5;
	#endregion

	#region Public Variables
	[Header("250 stops the player.")]
	public float
		speed; // How fast the conveyor belt pushes things
	public Direction
		direction; // The conveyor belt's direction to push
	public Vector2
		movement; // The force vector to push objects
	public Sprite []
		sprites;
	public bool
		isMoving; // Check if the conveyor belt is actually moving
	#endregion

	#region Private Variables
	private SpriteRenderer
		sprite;
	private float
		moveTime;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Turn on the conveyor belt </summary>
	void Start()
	{
		moveTime = 0.0f;
		isMoving = true;
		sprite = GetComponent<SpriteRenderer>();
	}

	/// <summary> Turn on the conveyor belt </summary>
	void Update()
	{
		if (isMoving)
		{
			GetDirection();
			if (sprites.Length > 0)
			{
				moveTime = (moveTime + speed * 0.08f * Time.deltaTime) % SPRITES;
				sprite.sprite = sprites[(int)((movement == Vector2.up || movement == Vector2.left) ? SPRITES - moveTime - 1 : moveTime)];
			}
		}
	}

	/// <summary> Push objects on the conveyor belt </summary>
	private void OnTriggerStay2D(Collider2D collider)
	{
		// Only characters and items get affected by conveyor belts
		if (isMoving && (collider.tag == "Player" || collider.tag == "Enemy" || collider.tag == "PuzzleItem"))
		{
			collider.attachedRigidbody.AddRelativeForce(movement * speed * (Mathf.Log(collider.attachedRigidbody.mass + 1.0f) + 1.0f),
			                                            ForceMode2D.Force);
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