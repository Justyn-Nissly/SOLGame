using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
	// Empty
	#region Enums
	#endregion

	#region Public Variables
	// player movement variables
	public bool playerAllowedToMove = true; // used to disable player movement like when the player is knocked back
	public Animator playerAnimator; // used to animate the players movement
	public Signal playerHealthSignal; // used to signal the health UI system that the player has taken damage

	// player attack origination variables
	public GameObject playerAttackGameObject; // this is where the players weapons get instantiated

	// players light melee attack variables
	public Transform lightMeleAttackPosition;
	public LayerMask willDamageLayer;
	public GameObject lightMeleeWeapon;
	public FloatValue lightMeleeDamageToGive;

	// heavy attack variables
	public Transform heavyMeleAttackPosition;
	public FloatValue heavyMeleeDamageToGive;
	public GameObject heavyMeleeWeapon;

	// player shield variables
	public SpriteRenderer
		  shieldSprite; // Shield graphics
	public BoxCollider2D
		  shieldBoxCollider; // The shield itself

	// player ranged attack variables
	public Transform firePoint;
	public Transform gunSpawnPoint;
	public GameObject bulletPrefab;
	public GameObject gunPrefab;
	public FloatValue damageToGive;



	// player sound effects
	public List<AudioClip> swordSwingSoundEffects;
	public AudioClip blasterSound;
	public AudioSource audioSourcePlayerMovement;

	public AudioSource shieldSoundSource;
	#endregion

	#region Private Variables
	// player movement variables
	private float playerMovementSpeed; // the speed the player can move at
	public Vector2 playerMovementAmount; // used to store the amount that the player will move this frame
	private Rigidbody2D playerRigidbody; // the players rigid body 2d, used to apply physics to the player like movement

	// players light melee attack variables
	private MeleeAttackBase playerLightMeleeAttack;
	private float lightMeleeAttackRange = .7f;
	private float lightStartTimeBetweenAttacks = .3f;

	// heavy attack variables
	private MeleeAttackBase playerHeavyMeleeAttack;
	private float heavyMeleeAttackRange = 1f;
	private float heavyStartTimeBetweenAttacks = .6f;
	private float timeBetweenAttacks;

	// player shield variables
	private ShieldBase playerShield;

	// player ranged attack variables
	private RangedAttackBase playerRangedAttack;
	private float RangedStartTimeBetweenAttacks = .5f;
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

		// set up the players attacks with the right values
		playerLightMeleeAttack = new MeleeAttackBase(lightMeleAttackPosition, lightMeleeAttackRange, willDamageLayer, lightMeleeWeapon, lightMeleeDamageToGive, swordSwingSoundEffects, audioSource);
		playerHeavyMeleeAttack = new MeleeAttackBase(heavyMeleAttackPosition, heavyMeleeAttackRange, willDamageLayer, heavyMeleeWeapon, heavyMeleeDamageToGive, swordSwingSoundEffects, audioSource);
		playerShield           = new ShieldBase(shieldSprite, shieldBoxCollider, shieldSoundSource);
		playerRangedAttack     = new RangedAttackBase(firePoint, gunSpawnPoint, bulletPrefab, gunPrefab, damageToGive, 0, blasterSound, audioSource);
	}

	/// <summary> Fixed update is called a fixed amount of times per second and if for logic that needs to be done constantly</summary>
	private void FixedUpdate()
	{
		// If the player is allowed to move, check for player movement input and apply it to the player
		if (playerAllowedToMove)
		{
			ApplyPlayerMovement();
		}
		if (FindObjectOfType<DialogueManager>().animator.GetBool("IsOpen") == true)
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
		if (Input.GetButton("B") && playerShield.shieldIsEnabled == false)
		{
			playerShield.EnableShield();
			playerShield.shieldIsEnabled = true;
			canAttack = false;
		}
		// On release of input deactivate the player's shield
		else if (Input.GetButton("B") == false && playerShield.shieldIsEnabled)
		{
			playerShield.DisableShield();
			playerShield.shieldIsEnabled = false;
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
					playerRangedAttack.Shoot();
				}
				// X is up arrow based on the SNES controller layout; attack with heavy weapon and reset the cooldown
				else if (Input.GetButtonDown("X"))
				{
					timeBetweenAttacks = heavyStartTimeBetweenAttacks; // reset the time between attacks
					playerHeavyMeleeAttack.Attack();
				}
				// A is right arrow based on the SNES controller layout; attack with light weapon and reset the cooldown
				else if (Input.GetButtonDown("A"))
				{
					timeBetweenAttacks = lightStartTimeBetweenAttacks; // reset the time between attacks
					playerLightMeleeAttack.Attack();
				}
			}
		}
		// The attack cool down has not finished yet
		else
		{
			timeBetweenAttacks -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> this method is for the player to take damage
	/// and send a signal to the UI to update it with the players new health </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		// call the parents TakeDamage()
		base.TakeDamage(damage, playSwordImpactSound);

		// send a signal saying that the player has taken damage so update his health UI
		playerHealthSignal.Raise();

		// print the players current heath to the console for debugging
		Debug.Log("player CurrentHealth = " + currentHealth);
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

	/// <summary>
	/// check for player movement input and apply it to the player
	/// </summary>
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
		if (playerAllowedToMove)
		{
			playerRigidbody.MovePosition(playerMovementAmount + playerRigidbody.position);
		}
	}

	/// <summary>
	/// Get the amount of movement that the player needs to move
	/// </summary>
	private Vector2 GetPlayerMovementAmount()
	{
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * playerMovementSpeed;
	}

	/// <summary>
	/// Update the values in the Animator for the players animations
	/// </summary>
	private void SetPlayerAnimatorValues()
	{
		playerAnimator.SetFloat("Horizontal", playerMovementAmount.x);
		playerAnimator.SetFloat("Vertical", playerMovementAmount.y);
		playerAnimator.SetFloat("Magnitude", playerMovementAmount.magnitude);
	}

	#endregion

	// Empty
	#region Coroutines
	#endregion
}
