using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : BaseCharacter
{
	#region Enums (Empty)
	private const int
		WEST = 0,
		NORTH = 1,
		EAST = 2,
		SOUTH = 3;
	#endregion

	#region Public Variables
	// player movement variables
	public bool
		playerAllowedToMove = true, // used to disable player movement like when the player is knocked back
		hammerComboUnlocked, // flag for if the player has unlocked the hammer attack combo ability
		swordComboUnlocked; // flag for if the player has unlocked the sword attack combo ability
	public Animator
		playerAnimator; // used to animate the players movement
	public Signal
		playerHealthSignal; // used to signal the health UI system that the player has taken damage
	public
		Vector2 playerMovementAmount; // used to store the amount that the player will move this frame
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
		medKits,     // How many med kits the player is holding
		extraDamage; // Extra damage dealt with a power up
	public float[]
		powerUpTimers; // Make power ups last for a set time
	public bool[]
		powerUpsActive; // Check which power ups are active
	public bool
		usingPowerUp = false;
	public Image
		RedRing,
		GreenRing,
		BlueRing;
	#endregion

	#region Private Variables
	// player movement variables
	private float
		playerMovementSpeed; // the speed the player can move at
	private Rigidbody2D
		playerRigidbody; // the players rigid body 2d, used to apply physics to the player like movement

	// player attack timer variables
	private float
		dustCountdownTimer = 1.0f, // timer for placing dust
		dustTimeInterval = 1.0f,
		healTimer = 1.0f;          // Time between being able to heal

	private int
		swordComboCounter, // this counts how many times the player presses the sword attack button and is for knowing if the sword attack animator should play the next attack
		hammerComboCounter; // this counts how many times the player presses the hammer attack button and is for knowing if the hammer attack animator should play the next attack
	private bool
		usingSwordAttack = false, // flags for whether the player is using an attack so that it only plays once
		usingHammerAttack = false,
		usingBlasterAttack = false,
		canPowerUp = false,
		canIncrementComboCounter = false,
		heal;

	public PlayerControls
		inputActions;
	#endregion
	//////////////////////////////////THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION
	private float fast, oldSpeed; private bool godModeEnabled;
	//////////////////////////////////THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION
	// Unity Named Methods
	#region Main Methods
	/// <summary> Start is called before the first frame update </summary>
	void Awake()
	{
		inputActions = new PlayerControls(); // this in the reference to the new unity input system

		godModeEnabled = false;
		SetUpInputDetection();

		// The player starts with max health
		currentHealth = maxHealth.initialValue;
		BulletShootingDelay = 0;

		// Set the players movement speed
		playerMovementSpeed = .1f;

		// Get the players Rigidbody2D
		playerRigidbody = GetComponent<Rigidbody2D>();

		medKits = 0;
		dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
		powerUpTimers = new float[PowerUp.SPEED + 1];
		powerUpsActive = new bool[PowerUp.SPEED + 1];
		fast = playerMovementSpeed * 2;
		oldSpeed = playerMovementSpeed;

		swordComboUnlocked = false;
		hammerComboUnlocked = false;
	}

	/// <summary> Fixed update is called a fixed amount of times per second and if for logic that needs to be done constantly </summary>
	private void FixedUpdate()
	{
		// Prevent the player's health from exceeding its maximum
		if (currentHealth > maxHealth.initialValue)
		{
			currentHealth = maxHealth.initialValue;
		}

		// Activate power ups
		if (usingPowerUp)
		{
			ActivatePowerUps();
			inputActions.Gameplay.LeftTrigger.canceled += _ => usingPowerUp = false;
		}
		else
		{
			inputActions.Gameplay.LeftTrigger.performed += _ => usingPowerUp = true;
		}

		// Apply power ups to the player
		ApplyPowerUps();



		/*************************************************************************************************************************************************************
		 *******THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION**************THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION**************************
		 *************************************************************************************************************************************************************
		 *******THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION**************THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION**************************
		 *************************************************************************************************************************************************************
		 *************************************************************************************************************************************************************/
		if (Input.GetKeyDown(KeyCode.Space))
		{
			playerMovementSpeed = fast;
			this.GetComponent<Rigidbody2D>().isKinematic = true;
			GlobalVarablesAndMethods.swordUnlocked = true;
			GlobalVarablesAndMethods.hammerUnlocked = true;
			GlobalVarablesAndMethods.blasterUnlocked = true;
			GlobalVarablesAndMethods.shieldUnlocked = true;
			SetUpInputDetection();
		}
		if (Input.GetKeyDown(KeyCode.End))
		{
			playerMovementSpeed = oldSpeed;
			this.GetComponent<Rigidbody2D>().isKinematic = false;
			GlobalVarablesAndMethods.swordUnlocked = true;
			GlobalVarablesAndMethods.hammerUnlocked = false;
			GlobalVarablesAndMethods.blasterUnlocked = true;
			GlobalVarablesAndMethods.shieldUnlocked = true;
			SetUpInputDetection();
		}
		/*************************************************************************************************************************************************************
		 *******THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION**************THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION**************************
		 *************************************************************************************************************************************************************
		 *******THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION**************THIS IS DEBUG CODE!!!!!! REMOVE BEFORE FINAL PRODUCTION**************************
		 *************************************************************************************************************************************************************
		 *************************************************************************************************************************************************************/





		// If the player is allowed to move, check for player movement input and apply it to the player
		if (playerAllowedToMove)
		{
			ApplyPlayerMovement();
		}
		CheckIfShouldIncreaseComboCounter();

		// Update the power ups HUD
		UpdateHud(powerUpTimers[PowerUp.POWER] / PowerUp.POWER_UP_TIME,
		          powerUpTimers[PowerUp.SHIELD] / PowerUp.POWER_UP_TIME,
		          powerUpTimers[PowerUp.SPEED] / PowerUp.POWER_UP_TIME);
	}
	#endregion

	#region Utility Methods
	/// <summary> increases the combo counter if the player presses the right attack button while attacking</summary>
	private void CheckIfShouldIncreaseComboCounter()
	{
		if (swordComboUnlocked && usingSwordAttack == true && inputActions.Gameplay.SwordAttack.triggered &&
		    swordComboCounter >= 0 && canIncrementComboCounter)
			swordComboCounter++;
		else if (hammerComboUnlocked && usingHammerAttack == true && inputActions.Gameplay.HammerAttack.triggered &&
		         hammerComboCounter >= 0 && canIncrementComboCounter)
			hammerComboCounter++;
	}

	/// <summary> this sets up the players input detection</summary>
	public void SetUpInputDetection()
	{
		inputActions.Gameplay.Enable();

		inputActions.Gameplay.Movement.performed += context => playerMovementAmount = context.ReadValue<Vector2>() * (playerMovementSpeed + extraSpeed);
		inputActions.Gameplay.Movement.canceled += _ => playerMovementAmount = Vector2.zero;

		if (GlobalVarablesAndMethods.shieldUnlocked)
		{
			inputActions.Gameplay.ShieldDefense.started += _ => EnableShield(false);
			inputActions.Gameplay.ShieldDefense.canceled += _ => DisableShield();
		}

		if (GlobalVarablesAndMethods.hammerUnlocked)
		{
			inputActions.Gameplay.HammerAttack.started += _ => StartHammerAnimation();
			inputActions.Gameplay.HammerAttack.canceled += _ => canIncrementComboCounter = true;
		}

		if (GlobalVarablesAndMethods.blasterUnlocked)
		{
			inputActions.Gameplay.BlasterAttack.started += _ => Shoot();
		}

		if (GlobalVarablesAndMethods.swordUnlocked)
		{
			inputActions.Gameplay.SwordAttack.started += _ => StartSwordAnimation();
			inputActions.Gameplay.SwordAttack.canceled += _ => canIncrementComboCounter = true;
		}
	}

	/// <summary> shoot the players blaster</summary>
	public override void Shoot()
	{
		// dont attack if the player is not allowed to
		if (canAttack == false || usingBlasterAttack || shieldIsEnabled || usingPowerUp)
		{
			return;
		}

		base.Shoot();

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

	/// <summary> this method starts playing the hammer animation </summary>
	private void StartHammerAnimation()
	{
		// dont attack if the player is not allowed to
		if(canAttack == false || shieldIsEnabled || usingPowerUp)
		{
			return;
		}

		// start the first hammer attack animation
		if (usingHammerAttack == false)
		{
			// set combo flags
			hammerComboCounter++;
			usingHammerAttack = true;

			// set bool flag blasting to true
			playerAnimator.SetBool("isHammerAttacking", hammerComboUnlocked || hammerComboCounter <= 1);
			FreezePlayer(); // don't let the player move
			playerAnimator.SetInteger("attackDirection", GetAnimationDirection()); // set the value that plays the right blaster direction animation
			playerAnimator.SetLayerWeight(4, 2); // increase the blaster layer priority
			canIncrementComboCounter = false;
		}
	}

	/// <summary>This deals damage at a certain frame in the hammer attack animation using an event</summary>
	public void DealPlayerHammerDamage()
	{
		// Deal Player hammer Damage to enemies in range
		MeleeAttack(heavyMeleeWeapon, heavyMeleeAttackPosition, heavyMeleeAttackRange, heavyMeleeDamageToGive, true);
	}

	/// <summary> shake the screen (called from in event in the hammer animation) </summary>
	public void ShakeScreen()
	{
		StartCoroutine(GameObject.Find("Main Camera").GetComponent<cameraMovement>().ShakeCamera(0.2f, 0.35f));
	}

	/// <summary> this method starts playing the sword animation </summary>
	private void StartSwordAnimation()
	{
		// dont attack if the player is not allowed to
		if (canAttack == false || shieldIsEnabled || usingPowerUp)
		{
			return;
		}

		swordComboCounter++;

		// increase the sword combo counter if the sword is already being used
		if (usingSwordAttack == false)
		{
			// set combo flags
			usingSwordAttack = true;

			// set bool flag blasting to true
			playerAnimator.SetBool("isSwordAttacking", swordComboUnlocked || swordComboCounter <= 1);
			FreezePlayer(); // don't let the player move
			playerAnimator.SetInteger("attackDirection", GetAnimationDirection()); // set the value that plays the right blaster direction animation
			playerAnimator.SetLayerWeight(3, 2); // increase the blaster layer priority
			canIncrementComboCounter = false;
		}
	}

	/// <summary>This deals damage at a certain frame in the sword attack animation using an event</summary>
	public void DealPlayerSwordDamage()
	{
		// Deal Player Sword Damage to enemies in range
		MeleeAttack(lightMeleeWeapon, lightMeleeAttackPosition, lightMeleeAttackRange, lightMeleeDamageToGive, true);
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
			hammerComboCounter--; // decrement counter

			if (hammerComboCounter <= 0)
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
		hammerComboCounter = 0;

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
				animationDirection = WEST;
				break;
			case "IdleUp":
				animationDirection = NORTH;
				break;
			case "IdleRight":
				animationDirection = EAST;
				break;
			case "IdleDown":
				animationDirection = SOUTH;
				break;
		}

		return animationDirection;
	}

	/// <summary> this method is for the player to take damage
	/// and send a signal to the UI to update it with the players new health </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound, bool fireBreathAttack = false)
	{
		if (fireBreathAttack && safeFromFireAttack)
		{
			return;
		}

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
		float rotationValue = 0;

		// get the direction the player is facing
		switch (GetAnimationDirection())
		{
			case WEST:
				rotationValue = -90f;
				break;
			case NORTH:
				rotationValue = 180f;
				break;
			case EAST:
				rotationValue = 90f;
				break;
			case SOUTH:
				rotationValue = 0f;
				break;
		}

		// apply rotation
		playerAttackGameObject.transform.rotation = Quaternion.Euler(0, 0, rotationValue);
	}

	/// <summary> check for player movement input and apply it to the player </summary>
	public void ApplyPlayerMovement()
	{


		if (dialogueManager != null && dialogueManager.GetComponentInChildren<Animator>().GetBool("IsOpen") == true)
		{
			playerMovementAmount = Vector2.zero;
			playerAnimator.SetLayerWeight(1, 0);
			audioSourcePlayerMovement.volume = 0;
			return;
		}

		// play or stop the player movement sound
		if (playerMovementAmount != Vector2.zero)
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

		// rotate the point at which the player attacks will be determined from (blaster bullet direction)
		ApplyAttackGameObjectRotation();
	}

	/// <summary> This creates a dust effect every N seconds</summary>
	private void DoDustEffectLogic()
	{
		if (dustCountdownTimer <= 0)
		{
			Instantiate(dustEffect, transform.position, new Quaternion(0, 0, 0, 0));
			dustCountdownTimer = dustTimeInterval;
		}
		else
		{
			dustCountdownTimer -= Time.deltaTime;
		}
	}

	/// <summary> Update the values in the Animator for the players animations </summary>
	private void SetPlayerAnimatorValues()
	{
		playerAnimator.SetFloat("Horizontal", playerMovementAmount.x);
		playerAnimator.SetFloat("Vertical", playerMovementAmount.y);
		playerAnimator.SetFloat("Magnitude", playerMovementAmount.magnitude);
	}

	private void ActivatePowerUps()
	{
		inputActions.Gameplay.ShieldDefense.performed += _ => powerUpsActive[PowerUp.SHIELD] =
			usingPowerUp || powerUpsActive[PowerUp.SHIELD];
		inputActions.Gameplay.BlasterAttack.performed += _ => powerUpsActive[PowerUp.POWER] =
			usingPowerUp || powerUpsActive[PowerUp.POWER];
		inputActions.Gameplay.SwordAttack.performed += _ => powerUpsActive[PowerUp.SPEED] =
			usingPowerUp || powerUpsActive[PowerUp.SPEED];
		inputActions.Gameplay.HammerAttack.performed += _ => heal = usingPowerUp;
	}

	///<summary> Make the health bar show the current health </summary>
	void UpdateHud(float percentRed, float percentGreen, float percentBlue)
	{
	//	RedRing.fillAmount = percentRed;
	//	GreenRing.fillAmount = percentGreen;
	//	BlueRing.fillAmount = percentBlue;
	}

	/// <summary> Apply any power ups the player has picked up </summary>
	private void ApplyPowerUps()
	{
		// Apply shield power up
		if (powerUpsActive[PowerUp.SHIELD])
		{
			canTakeDamage = (powerUpTimers[PowerUp.SHIELD] <= 0.0f && shieldIsEnabled == false);
		}

		// Apply damage boost
		extraDamage = (powerUpsActive[PowerUp.POWER] && powerUpTimers[PowerUp.POWER] > 0.0f) ? 2 : 1;

		// Apply Speed boost
		extraSpeed = (powerUpsActive[PowerUp.SPEED] && powerUpTimers[PowerUp.SPEED] > 0.0f) ? 0.05f : 0.0f;

		// Apply healing
		healTimer -= (healTimer > 0.0f) ? Time.deltaTime : 0.0f;
		if (heal && medKits > 0 && currentHealth < maxHealth.initialValue && healTimer <= 0.0f)
		{
			maxHealth.runTimeValue = (currentHealth += 2);
			playerHealthSignal.Raise();
			healTimer = 1.0f;
			medKits--;
			heal = false;
		}

		// Decrease each power up timer
		for (int i = PowerUp.SHIELD; i <= PowerUp.SPEED; i++)
		{
			if ((powerUpsActive[i] && powerUpTimers[i] > 0.0f))
			{
				powerUpTimers[i]  -= Time.deltaTime;
				powerUpsActive[i]  = true;
			}
			else
			{
				powerUpsActive[i] = false;
			}
		}

		/*if(RedRing != null && BlueRing != null && GreenRing != null)
		{
			SetPowerUpFillAmounts(powerUpTimers[PowerUp.POWER]  / PowerUp.POWER_UP_TIME,
								  powerUpTimers[PowerUp.SPEED]  / PowerUp.POWER_UP_TIME,
								  powerUpTimers[PowerUp.SHIELD] / PowerUp.POWER_UP_TIME);
		}*/
	}

	/// <summary> override the enable shield method to disable the player from taking damage while the shield is up </summary>
	public override void EnableShield(bool createShield)
	{
		if(canAttack == false || usingSwordAttack || usingHammerAttack || usingBlasterAttack || usingPowerUp)
		{
			return;
		}

		// Play shield sound
		if (shieldSound != null)
		{
			shieldSound.Play();
		}

		canTakeDamage = false;
		StartShieldAnimation();
		shieldIsEnabled = true;
		canAttack = false;
	}

	/// <summary> override the disable shield method to re able the player to take damage because the shield is down </summary>
	public override void DisableShield()
	{
		if (shieldIsEnabled == true)
		{
			// Stop playing shield sound
			if (shieldSound != null)
			{
				shieldSound.Stop();
			}
			canTakeDamage = true;

			EndAttackAnimation();
			shieldIsEnabled = false;
			canAttack = true;
		}
	}

	/// <summary> this freezes the player so that he can't move or attack</summary>
	public void FreezePlayer()
	{
		playerMovementAmount = Vector2.zero;
		if(playerRigidbody != null)
			playerRigidbody.isKinematic = true;
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
		if (playerRigidbody != null)
			playerRigidbody.isKinematic = false;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
