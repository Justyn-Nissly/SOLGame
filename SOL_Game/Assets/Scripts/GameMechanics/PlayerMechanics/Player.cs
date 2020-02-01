using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	// player movement variables
	public bool playerAllowedToMove = true; // used to disable player movement like when the player is knocked back
	public Animator playerAnimator; // used to animate the players movement
	public Signal playerHealthSignal; // used to signal the health UI system that the player has taken damage

	// player attack origination variables
	public GameObject playerAttackGameObject; // this is where the players weapons get instantiated
	public DialogueManager dialogueManager;

	// player sound effects
	public AudioSource audioSourcePlayerMovement;

	public AudioSource shieldSoundSource;

	public float
		extraSpeed,   // Extra speed gained from a power up
		powerUpTimer; // How long power ups last
	public int
		extraDamage; // Extra damage dealt with a power up
	public float[]
		powerUpTimers; // Make power ups last for a set time
	#endregion

	#region Private Variables
	// player movement variables
	private float playerMovementSpeed; // the speed the player can move at
	public Vector2 playerMovementAmount; // used to store the amount that the player will move this frame
	private Rigidbody2D playerRigidbody; // the players rigid body 2d, used to apply physics to the player like movement

	// player attack timer variables
	private float
		timeBetweenAttacks, // the timer that cotrols if the player can use any of their attacks
		lightStartTimeBetweenAttacks = .3f,
		heavyStartTimeBetweenAttacks = .6f,
		RangedStartTimeBetweenAttacks = .5f;
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

		dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
		powerUpTimers = new float[PowerUp.SPEED + 1];
	}

	/// <summary> Fixed update is called a fixed amount of times per second and if for logic that needs to be done constantly </summary>
	private void FixedUpdate()
	{
		// Apply power ups to the player
		ApplyPowerUps();

		// If the player is allowed to move, check for player movement input and apply it to the player
		if (playerAllowedToMove)
		{
			ApplyPlayerMovement();
		}
		if (dialogueManager.GetComponentInChildren<Animator>().GetBool("IsOpen") == true)
		{
			playerMovementAmount = Vector2.zero;
			playerAnimator.SetLayerWeight(1, 0);
			audioSourcePlayerMovement.volume = 0;
		}

		// rotate the players attack object if there is input
		// but don't rotate if the freeze rotation button is down
		if (Input.GetAxis("FreezeRotation") == 0)
		{
			ApplyAttackGameObjectRotation();
		}

		// On input activate the player's shield
		if (Input.GetButton("B") && shieldIsEnabled == false)
		{
			EnableShield();
			shieldIsEnabled = true;
			canAttack = false;
		}
		// On release of input deactivate the player's shield
		else if (Input.GetButton("B") == false && shieldIsEnabled)
		{
			DisableShield();
			shieldIsEnabled = false;
			canAttack = true;
		}

		// The player can use a weapon on cool down
		if (timeBetweenAttacks <= 0.0f)
		{
			// if the player is allowed to attack
			if (canAttack)
			{
				// Y is left arrow based on the SNES controller layout; fire and reset the cooldown
				if (Input.GetButtonUp("Y"))
				{
					timeBetweenAttacks = RangedStartTimeBetweenAttacks;
					Shoot();
				}
				// X is up arrow based on the SNES controller layout; attack with heavy weapon and reset the cooldown
				else if (Input.GetButtonDown("X"))
				{
					timeBetweenAttacks = heavyStartTimeBetweenAttacks; // reset the time between attacks
					MeleeAttack(heavyMeleeWeapon, heavyMeleeAttackPosition, heavyMeleeAttackRange, heavyMeleeDamageToGive, false);
				}
				// A is right arrow based on the SNES controller layout; attack with light weapon and reset the cooldown
				else if (Input.GetButtonDown("A"))
				{
					timeBetweenAttacks = lightStartTimeBetweenAttacks; // reset the time between attacks
					MeleeAttack(lightMeleeWeapon, lightMeleeAttackPosition, lightMeleeAttackRange, lightMeleeDamageToGive, false);
				}
			}
		}
		// The attack cool down has not finished yet
		else
		{
			timeBetweenAttacks -= Time.deltaTime;
		}
	}

	/// <summary> The player picks up a power up </summary>
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "PowerUp")
		{
			Debug.Log("Picked up power up.");
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> this method is for the player to take damage
	/// and send a signal to the UI to update it with the players new health </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		// only take damage if the player is allowed to take damage at the moment
		if (canTakeDamage)
		{
			// call the parents TakeDamage()
			base.TakeDamage(damage, playSwordImpactSound);

			// send a signal saying that the player has taken damage so update his health UI
			playerHealthSignal.Raise();

			// print the players current heath to the console for debugging
			Debug.Log("player CurrentHealth = " + currentHealth);
		}
	}

	/// <summary> Rotates the players attack game object so that the players weapons are "fired" in the right direction </summary>
	private void ApplyAttackGameObjectRotation()
	{
		// If the player clicks the left key set rotation to -90 degrees
		if (Input.GetAxis("Horizontal") < 0)
		{
			playerAttackGameObject.transform.rotation = Quaternion.Euler(0, 0, -90f);
		}
		// If the player clicks the right key set rotation to 90 degrees
		else if (Input.GetAxis("Horizontal") > 0)
		{
			playerAttackGameObject.transform.rotation = Quaternion.Euler(0, 0, 90f);
		}
		// If the player clicks the up key set rotation to 180 degrees
		else if (Input.GetAxis("Vertical") > 0)
		{
			playerAttackGameObject.transform.rotation = Quaternion.Euler(0, 0, 180f);
		}
		// If the player clicks the down key set rotation to zero degrees
		else if (Input.GetAxis("Vertical") < 0)
		{
			playerAttackGameObject.transform.rotation = Quaternion.Euler(0, 0, 0f);
		}
	}

	/// <summary> check for player movement input and apply it to the player </summary>
	private void ApplyPlayerMovement()
	{
		// Get the amount of movement that the player needs to move
		playerMovementAmount = GetPlayerMovementAmount();

		// play or stop the player movement sound
		if(playerMovementAmount != Vector2.zero)
		{
			audioSourcePlayerMovement.volume = 1;
		}
		else
		{
			audioSourcePlayerMovement.volume = 0;
		}

		// Check if the player is moving or is idle
		if (playerMovementAmount.x != 0 || playerMovementAmount.y != 0)
		{
			playerAnimator.SetLayerWeight(1, 1);
		}
		else
		{
			playerAnimator.SetLayerWeight(1, 0);
		}
		// Update the values in the Animator for the players animation
		SetPlayerAnimatorValues();
		// Update the Hero's position, taking note of colliders.
		playerRigidbody.MovePosition(playerMovementAmount + playerRigidbody.position);
	}

	/// <summary> Get the amount of movement that the player needs to move </summary>
	private Vector2 GetPlayerMovementAmount()
	{
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * (playerMovementSpeed + extraSpeed);
	}

	/// <summary> Update the values in the Animator for the players animations </summary>
	private void SetPlayerAnimatorValues()
	{
		playerAnimator.SetFloat("Horizontal", playerMovementAmount.x);
		playerAnimator.SetFloat("Vertical", playerMovementAmount.y);
		playerAnimator.SetFloat("Magnitude", playerMovementAmount.magnitude);
	}

	/// <summary> Apply any power ups the player has picked up </summary>
	private void ApplyPowerUps()
	{
		// Not a for loop because different power ups work differently
		// Apply heal
		currentHealth += (powerUpTimers[PowerUp.HEAL] > 0.0f) ? 2 : 0;

		// Apply damage boost
		extraDamage = (powerUpTimers[PowerUp.POWER] > 0.0f) ? 1 : 0;

		// Apply Speed boost
		extraSpeed = (powerUpTimers[PowerUp.SPEED] > 0.0f) ? 0.05f : 0.0f;

		// Decrease each power up timer
		for (int counter = PowerUp.HEAL; counter <= PowerUp.SPEED; counter++)
		{
			powerUpTimers[counter] -= (powerUpTimers[counter] > 0.0f) ? Time.deltaTime : 0.0f;
		}
	}

	/// <summary> override the enable shield method to disable the player from taking damage while the shield is up </summary>
	public override void EnableShield()
	{
		base.EnableShield();
		canTakeDamage = false;
	}

	/// <summary> override the disable shield method to re able the player to take damage because the shield is down </summary>
	public override void DisableShield()
	{
		base.DisableShield();
		canTakeDamage = true;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}