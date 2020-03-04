using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	// player movement variables
	public bool
		playerAllowedToMove = true, // used to disable player movement like when the player is knocked back
		hammerComboUnlocked = true, // flag for if the player has unlocked the hammer attack combo ability
		swordComboUnlocked = true; // flag for if the player has unlocked the sword attack combo ability
	public Animator
		playerAnimator; // used to animate the players movement
	public Signal
		playerHealthSignal; // used to signal the health UI system that the player has taken damage

	// player attack origination variables
	public GameObject 
		playerAttackGameObject, // this is where the players weapons get instantiated
		dustEffect; // this effect is created when the player walks around
	public DialogueManager
		dialogueManager;

	// player sound effects
	public AudioSource
		audioSourcePlayerMovement;

	public AudioSource
		shieldSoundSource;

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
		dustCountdownTimer = 1, // timer for placing dust
		dustTimeInterval = 1;

	private int
		swordComboCounter, // this counts how many times the player presses the sword attack button and is for knowing if the sword attack animator should play the next attack
		hammerComboCouter; // this counts how many times the player presses the hammer attack button and is for knowing if the hammer attack animator should play the next attack
	private bool
		usingSwordAttack = false, // flags for whether the player is using an attack so that it only plays once
		usingHammerAttack = false,
		usingBlasterAttack = false;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Start is called before the first frame update </summary>
	void Start()
	{
		// The player starts with max health
		currentHealth = maxHealth.initialValue;
		BulletShootingDelay = 0;

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
			StartShieldAnimation();
			EnableShield(false);
			shieldIsEnabled = true;
			canAttack = false;
		}
		// On release of input deactivate the player's shield
		else if (Input.GetButton("B") == false && shieldIsEnabled)
		{
			EndAttackAnimation();
			DisableShield();
			shieldIsEnabled = false;
			canAttack = true;
		}

		// check if the player is trying use an attack
		DetectAttackInput();
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
	private void DetectAttackInput()
	{
		// The player can use a weapon on cool down
		if (usingBlasterAttack == false && usingSwordAttack == false && usingHammerAttack == false)
		{
			// if the player is allowed to attack
			if (canAttack)
			{
				// Y is left arrow based on the SNES controller layout; fire and reset the cooldown
				if (Input.GetButtonDown("Y"))
				{
					Shoot(false);
				}
				// X is up arrow based on the SNES controller layout; attack with heavy weapon and reset the cooldown
				else if (Input.GetButtonDown("X"))
				{
					StartHammerAnimation();
				}
				// A is right arrow based on the SNES controller layout; attack with light weapon and reset the cooldown
				else if (Input.GetButtonDown("A"))
				{
					StartSwordAnimation();
				}
			}
		}
		// The attack cool down has not finished yet
		else
		{
			IncreaseComboCounter();
		}
	}

	/// <summary> This method increases the counters for the combos (used when the player has already started attacking)</summary>
	private void IncreaseComboCounter()
	{
		// X is up arrow based on the SNES controller layout; check if the hammer attack has been used again
		if (Input.GetButtonDown("X") && hammerComboUnlocked)
		{
			hammerComboCouter++;
		}
		// A is right arrow based on the SNES controller layout; check if the sword attack has been used again
		if (Input.GetButtonDown("A") && swordComboUnlocked)
		{
			swordComboCounter++;
		}
	}

	public override void Shoot(bool createGun)
	{
		base.Shoot(createGun);

		StartShootAnimation();

		StartCoroutine(GameObject.Find("Main Camera").GetComponent<cameraMovement>().ShakeCamera(0.05f, 0.25f));
	}

	/// <summary> this method starts playing the shooting animation </summary>
	private void StartShootAnimation()
	{
		usingBlasterAttack = true;

		playerAnimator.SetBool("blasting", true); // set bool flag blasting to true
		FreezePlayer(); // don't let the player move
		playerAnimator.SetInteger("attackDirection", GetAnimationDirection()); // set the value that plays the right blaster direction animation
		playerAnimator.SetLayerWeight(2, 2); // increase the blaster layer priority
	}

	/// <summary> this method starts playing the sword animation </summary>
	private void StartHammerAnimation()
	{
		// set combo flags
		hammerComboCouter++;
		usingHammerAttack = true;

		playerAnimator.SetBool("isHammerAttacking", true); // set bool flag blasting to true
		FreezePlayer(); // don't let the player move
		playerAnimator.SetInteger("attackDirection", GetAnimationDirection()); // set the value that plays the right blaster direction animation
		playerAnimator.SetLayerWeight(4, 2); // increase the blaster layer priority
	}

	/// <summary>This deals damage at a certain frame in the hammer attack animation using an event</summary>
	public void DealPlayerHammerDamage()
	{
		// Deal Player hammer Damage to enemies in range
		MeleeAttack(heavyMeleeWeapon, heavyMeleeAttackPosition, heavyMeleeAttackRange, heavyMeleeDamageToGive, false);
	}

	/// <summary> shake the screen (called from in event in the hammer animation) </summary>
	public void ShakeScreen()
	{
		StartCoroutine(GameObject.Find("Main Camera").GetComponent<cameraMovement>().ShakeCamera(0.2f, 0.35f));
	}

	/// <summary> this method starts playing the sword animation </summary>
	private void StartSwordAnimation()
	{
		// set combo flags
		swordComboCounter++;
		usingSwordAttack = true;

		playerAnimator.SetBool("isSwordAttacking", true); // set bool flag blasting to true
		FreezePlayer(); // don't let the player move
		playerAnimator.SetInteger("attackDirection", GetAnimationDirection()); // set the value that plays the right blaster direction animation
		playerAnimator.SetLayerWeight(3, 2); // increase the blaster layer priority
	}

	/// <summary>This deals damage at a certain frame in the sword attack animation using an event</summary>
	public void DealPlayerSwordDamage()
	{
		// Deal Player Sword Damage to enemies in range
		MeleeAttack(lightMeleeWeapon, lightMeleeAttackPosition, lightMeleeAttackRange, lightMeleeDamageToGive, false);
	}

	/// <summary> this method starts playing the sword animation </summary>
	private void StartShieldAnimation()
	{
		playerAnimator.SetBool("isShieldUp", true); // set bool flag blasting to true
		FreezePlayer(); // don't let the player move
		playerAnimator.SetInteger("attackDirection", GetAnimationDirection()); // set the value that plays the right blaster direction animation
		playerAnimator.SetLayerWeight(5, 2); // increase the blaster layer priority
	}

	/// <summary> ends an attack animation (called with an event in the attack animation)</summary>
	public void CheckIfShouldEndAttackAnimation()
	{
		if (usingSwordAttack)
		{
			swordComboCounter--; // decrement counter

			if (swordComboCounter <= 0)
			{
				EndAttackAnimation();
			}
		}
		else if (usingHammerAttack)
		{
			hammerComboCouter--; // decrement counter

			if (hammerComboCouter <= 0)
			{
				EndAttackAnimation();
			}
		}

	}

	/// <summary> ends an attack animation (called with an event in the attack animation)</summary>
	public void EndAttackAnimation()
	{
		playerAnimator.SetLayerWeight(2, 0); // lowers the blaster layer priority
		playerAnimator.SetLayerWeight(3, 0); // lowers the sword layer priority
		playerAnimator.SetLayerWeight(4, 0); // lowers the hammer layer priority
		playerAnimator.SetLayerWeight(5, 0); // lowers the shield layer priority

		playerAnimator.SetBool("blasting", false); // set flag blasting to false
		playerAnimator.SetBool("isSwordAttacking", false); // set flag isSwordAttacking to false
		playerAnimator.SetBool("isHammerAttacking", false); // set flag isHammerAttacking to true
		playerAnimator.SetBool("isShieldUp", false); // set flag isHammerAttacking to true

		UnFreezePlayer(); // let the player move again

		// reset the attack counters
		swordComboCounter = 0;
		hammerComboCouter = 0;

		// reset attack flags
		usingSwordAttack = false;
		usingHammerAttack = false;
		usingBlasterAttack = false;
	}

	/// <summary> this gets the direction that an animations should play based on the players idle animation state</summary>
	private int GetAnimationDirection()
	{
		int animationDirection = 0; // return value for the animations direction

		AnimatorClipInfo[] animatorStateInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);

		switch (animatorStateInfo[0].clip.name)
		{
			case "IdleLeft":
				animationDirection = 0; // west
				break;
			case "IdleUp":
				animationDirection = 1; // north
				break;
			case "IdleRight":
				animationDirection = 2; // east
				break;
			case "IdleDown":
				animationDirection = 3; // south
				break;
		}

		return animationDirection;
	}

	/// <summary> this method is for the player to take damage
	/// and send a signal to the UI to update it with the players new health </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound)
	{
		// only take damage if the player is allowed to take damage at the moment
		if (canTakeDamage)
		{
			// call the parents TakeDamage()
			base.TakeDamage(damage, playSwordImpactSound);

			StartCoroutine(GameObject.Find("Main Camera").GetComponent<cameraMovement>().ShakeCamera(.1f, .25f));

			// set the float value varable to the players current health after taking damage
			// the float value is used for updating the players health bar
			maxHealth.runTimeValue = currentHealth;

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

			DoDustEffectLogic();
		}
		else
		{
			playerAnimator.SetLayerWeight(1, 0);
			dustCountdownTimer = -.1f; // set the countdown timer less than zero so that dust will be created right when the player next moves
		}
		// Update the values in the Animator for the players animation
		SetPlayerAnimatorValues();
		// Update the Hero's position, taking note of colliders.
		playerRigidbody.MovePosition(playerMovementAmount + playerRigidbody.position);
	}

	/// <summary> This creates a dust effect every N seconds</summary>
	private void DoDustEffectLogic()
	{
		if(dustCountdownTimer <= 0)
		{
			Instantiate(dustEffect, transform.position, new Quaternion(0, 0, 0, 0));
			dustCountdownTimer = dustTimeInterval;
		}
		else
		{
			dustCountdownTimer -= Time.deltaTime;
		}
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
	public override void EnableShield(bool createShield)
	{
		base.EnableShield(createShield);
		canTakeDamage = false;
	}

	/// <summary> override the disable shield method to re able the player to take damage because the shield is down </summary>
	public override void DisableShield()
	{
		base.DisableShield();
		canTakeDamage = true;
	}

	/// <summary> this freezes the player so that he can't move or attack</summary>
	public void FreezePlayer()
	{
		playerMovementAmount = Vector2.zero;
		playerAnimator.SetLayerWeight(1, 0);
		audioSourcePlayerMovement.volume = 0;
		playerAllowedToMove = false;
		canAttack = false;
	}

	/// <summary> this unfreezes the player so that he can move and attack</summary>
	public void UnFreezePlayer()
	{
		playerAllowedToMove = true;
		canAttack = true;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
