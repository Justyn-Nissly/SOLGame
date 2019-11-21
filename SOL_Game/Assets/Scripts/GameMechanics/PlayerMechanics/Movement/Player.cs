using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
	// Empty
	#region Enums
	#endregion

	#region Public Variables
	public bool playerAllowedToMove = true; // used to disable player movement like when the player is knocked back
	public Animator playerAnimator; // used to animate the players movement
	public Signal playerHealthSignal; // used to signal the health UI system that the player has taken damage
	#endregion

	#region Private Variables
	private float playerMovementSpeed; // the speed the player can move at
	private Vector2 playerMovementAmount; // used to store the amount that the player will move this frame
	private Rigidbody2D playerRigidbody; // the players rigid body 2d, used to apply physics to the player like movement
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Start is called before the first frame update </summary>
	void Start()
	{
		// Set the players movement speed
		playerMovementSpeed = .1f;

		// Get the players Rigidbody2D
		playerRigidbody = GetComponent<Rigidbody2D>();
	}

	/// <summary> Fixed update is called a fixed amount of times per second and if for logic that needs to be done constantly</summary>
	private void FixedUpdate()
	{
		// If the player is allowed to move, check for player movement input and apply it to the player
		if (playerAllowedToMove)
		{
			// Get the amount of movement that the player needs to move
			playerMovementAmount = new Vector2(Mathf.RoundToInt(Input.GetAxis("Horizontal")) * playerMovementSpeed, Mathf.RoundToInt(Input.GetAxis("Vertical")) * playerMovementSpeed);

			// Update the values in the Animator for the players animations
			playerAnimator.SetFloat("Horizontal", playerMovementAmount.x);
			playerAnimator.SetFloat("Vertical", playerMovementAmount.y);
			playerAnimator.SetFloat("Magnitude", playerMovementAmount.magnitude);

			// Update the Hero's position, taking note of colliders.
			playerRigidbody.MovePosition(playerMovementAmount + playerRigidbody.position);
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> this method is for the player to take damage 
	/// and send a signal to the UI to update it with the players new health </summary>
	public override void TakeDamage(int damage)
	{
		// call the parents TakeDamage()
		base.TakeDamage(damage);

		// send a signal saying that the player has taken damage so update his health UI
		playerHealthSignal.Raise();

		// print the players current heath to the console for debugging
		Debug.Log("player CurrentHealth = " + currentHealth.initialValue);
	}
	#endregion

	// Empty
	#region Coroutines
	#endregion
}