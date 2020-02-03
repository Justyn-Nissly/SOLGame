using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFix : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public Transform
		player; // The transform of the player object
	public float
		moveSpeed; // The movement speed of the enemy
	#endregion

	#region Private Variables
	private Rigidbody2D 
		enemyRidgedBody; // The ridged body of the enemy
	private Vector2
		movement; // The direction that the enemy will move
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		enemyRidgedBody = this.GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		Vector3 direction = player.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		enemyRidgedBody.rotation = angle;
		direction.Normalize();
		movement = direction;
	}

	void FixedUpdate()
	{
		moveEnemy(movement);
	}
	#endregion

	#region Utility Methods
	/// <summary> Move the enemy to its next locaiton </summary>
	public void moveEnemy(Vector2 direction)
	{
		enemyRidgedBody.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
	}
	#endregion

	#region Coroutines

	#endregion
}
