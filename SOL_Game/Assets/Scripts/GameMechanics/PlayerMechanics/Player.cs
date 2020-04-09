using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : BaseCharacter
{
	#region Enums

	#endregion

	#region Public Variables
	public bool
		playerAllowedToMove = true, // used to disable player movement like when the player is knocked back
		hammerComboUnlocked, // flag for if the player has unlocked the hammer attack combo ability
		swordComboUnlocked; // flag for if the player has unlocked the sword attack combo ability
	public Animator
		playerAnimator, // used to animate the players movement
		shieldAnimator;
	public Signal
		playerHealthSignal; // used to signal the health UI system that the player has taken damage
	public
		Vector2 playerMovementAmount; // used to store the amount that the player will move this frame

	public GameObject
		playerAttackGameObject, // this is where the players weapons get instantiated
		dustEffect; // this effect is created when the player walks around
	public DialogueManager
		dialogueManager;

	// player sound effects
	public AudioSource
		audioSourcePlayerMovement;

	public float
		extraSpeed,   // Extra speed gained from a power up
		powerUpTimer; // How long power ups last
	public int
		combo = 0,
		medKits,     // How many med kits the player is holding
		extraDamage, // Extra damage dealt with a power up
		swordComboCounter, // this counts how many times the player presses the sword attack button and is for knowing if the sword attack animator should play the next attack
		hammerComboCounter; // this counts how many times the player presses the hammer attack button and is for knowing if the hammer attack animator should play the next attack
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
	public Image[]
		medKitImages;
	public SpriteRenderer
		playerShield;
	public CircleCollider2D
		shieldCollider;
	public Hud
		playerHealthHUD;
	public FloatValue
		heartContainers;
	public AudioClip
		gameOverSound; // the game over sound effect
	public Image
		canvasFadeImage; // used to Fade the whole screen to black
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
	protected override void Awake()
	{
		inputActions = new PlayerControls(); // this in the reference to the new unity input system

		godModeEnabled = false;
		SetUpInputDetection();

		canvasFadeImage.color = Color.clear; // make image transparent

		// The player starts with max health
		maxHealth.runTimeValue = maxHealth.initialValue;
		BulletShootingDelay = 0;

		// Set the players movement speed
		playerMovementSpeed = .13f;

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

		if (medKitImages != null)
		{
			for (int i = 0; i < PowerUp.MAX_MED_KITS; i++)
			{
				medKitImages[i].enabled = (i < medKits);
			}
		}

		if (playerShield != null && shieldAnimator != null)
		{
			shieldAnimator.SetBool("ShieldUnlocked", false);
			playerShield.color = new Color(playerShield.color.r, playerShield.color.g, playerShield.color.b, 0.0f);
		}
	}

	/// <summary> Fixed update is called a fixed amount of times per second and if for logic that needs to be done constantly </summary>
	private void FixedUpdate()
	{
		// Prevent the player's health from exceeding its maximum
		if (maxHealth.runTimeValue > heartContainers.runTimeValue * 2)
		{
			maxHealth.runTimeValue = heartContainers.runTimeValue * 2;
		}

		// Activate power ups
		if (usingPowerUp)
		{
			ActivatePowerUps();
			inputActions.Gameplay.LeftTrigger.canceled += _ => usingPowerUp = false;
		}
		else
		{
			inputActions.Gameplay.LeftTrigger.performed += _ => usingPowerUp =
				((playerAllowedToMove || canAttack) && usingBlasterAttack == false && usingHammerAttack == false &&
				                                       usingSwordAttack   == false && shieldIsEnabled   == false);
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
			Globals.swordUnlocked = true;
			Globals.hammerUnlocked = true;
			Globals.blasterUnlocked = true;
			Globals.shieldUnlocked = true;
			SetUpInputDetection();
		}
		if (Input.GetKeyDown(KeyCode.End))
		{
			playerMovementSpeed = oldSpeed;
			this.GetComponent<Rigidbody2D>().isKinematic = false;
			Globals.swordUnlocked = true;
			Globals.hammerUnlocked = true;
			Globals.blasterUnlocked = true;
			Globals.shieldUnlocked = true;
			SetUpInputDetection();
		}

		if (Input.GetKeyDown(KeyCode.Delete))
		{
			SceneManager.LoadScene("DevMenu");
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

		// Show the player's shield when it is active
		ActivateForceField();
	}
	#endregion

	#region Utility Methods
	/// <summary> increases the combo counter if the player presses the right attack button while attacking</summary>
	private void CheckIfShouldIncreaseComboCounter()
	{
		if (swordComboUnlocked && usingSwordAttack == true && inputActions.Gameplay.SwordAttack.triggered &&
			swordComboCounter >= 0 && canIncrementComboCounter)
		{
			swordComboCounter++;
			if (playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("swordNorth(2)") ||
				playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("swordSouth(2)") ||
				playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("swordEast(2)") ||
				playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("swordWest(2)"))
			{
				combo = 1;
			}
			else if (playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("swordNorth(3)") ||
					playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("swordSouth(3)") ||
					playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("swordEast(3)") ||
					playerAnimator.GetCurrentAnimatorStateInfo(3).IsName("swordWest(3)"))
			{
				combo = 2;
			}
		}
		else if (hammerComboUnlocked && usingHammerAttack == true && inputActions.Gameplay.HammerAttack.triggered &&
				 hammerComboCounter >= 0 && canIncrementComboCounter)
		{
			hammerComboCounter++;
			if (playerAnimator.GetCurrentAnimatorStateInfo(4).IsName("hammerNorth(2)") ||
				playerAnimator.GetCurrentAnimatorStateInfo(4).IsName("hammerSouth(2)") ||
				playerAnimator.GetCurrentAnimatorStateInfo(4).IsName("hammerEast(2)") ||
				playerAnimator.GetCurrentAnimatorStateInfo(4).IsName("hammerWest(2)"))
			{
				combo = 1;
			}
			else if (playerAnimator.GetCurrentAnimatorStateInfo(4).IsName("hammerNorth(3)") ||
					playerAnimator.GetCurrentAnimatorStateInfo(4).IsName("hammerSouth(3)") ||
					playerAnimator.GetCurrentAnimatorStateInfo(4).IsName("hammerEast(3)") ||
					playerAnimator.GetCurrentAnimatorStateInfo(4).IsName("hammerWest(3)"))
			{
				combo = 2;
			}
		}
	}

	/// <summary> this sets up the players input detection</summary>
	public void SetUpInputDetection()
	{
		inputActions.Gameplay.Enable();

		inputActions.Gameplay.Movement.performed += context => playerMovementAmount = context.ReadValue<Vector2>() * (playerMovementSpeed + extraSpeed);
		inputActions.Gameplay.Movement.canceled += _ => playerMovementAmount = Vector2.zero;

		if (Globals.shieldUnlocked)
		{
			inputActions.Gameplay.ShieldDefense.started += _ => EnableShield();
			inputActions.Gameplay.ShieldDefense.canceled += _ => DisableShield();
		}

		if (Globals.hammerUnlocked)
		{
			inputActions.Gameplay.HammerAttack.started += _ => StartHammerAnimation();
			inputActions.Gameplay.HammerAttack.canceled += _ => canIncrementComboCounter = true;
		}

		if (Globals.blasterUnlocked)
		{
			inputActions.Gameplay.BlasterAttack.started += _ => Shoot();
		}

		if (Globals.swordUnlocked)
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

		characterAnimator.SetBool("blasting", true); // set bool flag blasting to true
		FreezePlayer(); // don't let the player move
		characterAnimator.SetInteger("attackDirection", GetAnimationDirection(0)); // set the value that plays the right blaster direction animation
		characterAnimator.SetLayerWeight(2, 2); // increase the blaster layer priority
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
			characterAnimator.SetInteger("attackDirection", GetAnimationDirection(0)); // set the value that plays the right blaster direction animation
			characterAnimator.SetLayerWeight(4, 2); // increase the blaster layer priority
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
			characterAnimator.SetInteger("attackDirection", GetAnimationDirection(0)); // set the value that plays the right blaster direction animation
			characterAnimator.SetLayerWeight(3, 2); // increase the blaster layer priority
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
		characterAnimator.SetBool("isShieldUp", true); // set bool flag blasting to true
		FreezePlayer(); // don't let the player move
		playerRigidbody.isKinematic = false;
		characterAnimator.SetInteger("attackDirection", GetAnimationDirection(0)); // set the value that plays the right blaster direction animation
		characterAnimator.SetLayerWeight(5, 2); // increase the blaster layer priority
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
		characterAnimator.SetLayerWeight(2, 0); // lowers the blaster layer priority
		characterAnimator.SetLayerWeight(3, 0); // lowers the sword layer priority
		characterAnimator.SetLayerWeight(4, 0); // lowers the hammer layer priority
		characterAnimator.SetLayerWeight(5, 0); // lowers the shield layer priority

		characterAnimator.SetBool("blasting", false); // set flag blasting to false
		characterAnimator.SetBool("isSwordAttacking", false); // set flag isSwordAttacking to false
		characterAnimator.SetBool("isHammerAttacking", false); // set flag isHammerAttacking to true
		characterAnimator.SetBool("isShieldUp", false); // set flag isHammerAttacking to true

		UnFreezePlayer(); // let the player move again

		// reset the attack counters
		swordComboCounter = 0;
		hammerComboCounter = 0;
		combo = 0;

		// reset attack flags
		usingSwordAttack = false;
		usingHammerAttack = false;
		usingBlasterAttack = false;
	}

	/// <summary> this method is for the player to take damage
	/// and send a signal to the UI to update it with the players new health </summary>
	public override void TakeDamage(int damage, bool playSwordImpactSound = false)
	{
		// only take damage if the player is allowed to take damage at the moment
		if (canTakeDamage)
		{
			// call the parents TakeDamage()
			base.TakeDamage(damage, playSwordImpactSound);


			// the player has taken damage so update his health UI
			playerHealthHUD.UpdateHearts();

			// kill the player...
			if(maxHealth.runTimeValue <= 0 && Globals.playerCanDie)
			{
				StartCoroutine(PlayerDied());
			}
			// just shake the camera
			else
			{
				StartCoroutine(GameObject.Find("Main Camera").GetComponent<cameraMovement>().ShakeCamera(.1f, .25f));
			}
		}
	}

	/// <summary> Rotates the players attack game object so that the players weapons are "fired" in the right direction </summary>
	private void ApplyAttackGameObjectRotation()
	{
		float rotationValue = 0;

		// get the direction the player is facing
		switch (GetAnimationDirection(0))
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
			characterAnimator.SetLayerWeight(1, 0);
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
			characterAnimator.SetLayerWeight(1, 1);

			DoDustEffectLogic();
		}
		else
		{
			characterAnimator.SetLayerWeight(1, 0);
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
		characterAnimator.SetFloat("Horizontal", playerMovementAmount.x);
		characterAnimator.SetFloat("Vertical", playerMovementAmount.y);
		characterAnimator.SetFloat("Magnitude", playerMovementAmount.magnitude);
	}

	private void ActivatePowerUps()
	{
		inputActions.Gameplay.ShieldDefense.performed += _ => powerUpsActive[PowerUp.SHIELD] =
			usingPowerUp || powerUpsActive[PowerUp.SHIELD];
		inputActions.Gameplay.BlasterAttack.performed += _ => powerUpsActive[PowerUp.POWER] =
			usingPowerUp || powerUpsActive[PowerUp.POWER];
		inputActions.Gameplay.SwordAttack.performed += _ => powerUpsActive[PowerUp.SPEED] =
			usingPowerUp || powerUpsActive[PowerUp.SPEED];
		inputActions.Gameplay.HammerAttack.performed += _ => heal = true;
		inputActions.Gameplay.HammerAttack.canceled += _ => heal = false;
	}

	///<summary> Make the health bar show the current health </summary>
	void UpdateHud(float percentRed, float percentGreen, float percentBlue)
	{
		RedRing.fillAmount = percentRed;
		GreenRing.fillAmount = percentGreen;
		BlueRing.fillAmount = percentBlue;
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
		extraDamage = (powerUpsActive[PowerUp.POWER] && powerUpTimers[PowerUp.POWER] > 0.0f) ? PowerUp.EXTRA_PLAYER_DAMAGE : 1;

		// Apply Speed boost
		extraSpeed = (powerUpsActive[PowerUp.SPEED] && powerUpTimers[PowerUp.SPEED] > 0.0f) ? PowerUp.EXTRA_PLAYER_SPEED : 0.0f;

		// Apply healing
		healTimer -= (healTimer > 0.0f) ? Time.deltaTime : 0.0f;
		if (heal && usingPowerUp && medKits > 0 && maxHealth.runTimeValue < maxHealth.initialValue && healTimer <= 0.0f)
		{
			if ((maxHealth.runTimeValue += 2) > maxHealth.initialValue)
			{
				maxHealth.runTimeValue = maxHealth.initialValue;
			}
			playerHealthHUD.UpdateHearts();
			healTimer = 1.0f;
			medKits--;
			heal = false;
			if (medKitImages != null)
			{
				for (int i = 0; i < PowerUp.MAX_MED_KITS; i++)
				{
					medKitImages[i].enabled = (i < medKits);
				}
			}
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

		if(RedRing != null && GreenRing != null && BlueRing != null)
		{
			UpdateHud(powerUpTimers[PowerUp.POWER]  / PowerUp.POWER_UP_TIME,
			          powerUpTimers[PowerUp.SHIELD] / PowerUp.POWER_UP_TIME,
			          powerUpTimers[PowerUp.SPEED]  / PowerUp.POWER_UP_TIME);
		}
	}

	/// <summary> override the enable shield method to disable the player from taking damage while the shield is up </summary>
	public override void EnableShield()
	{
		if(canAttack == false || usingSwordAttack || usingHammerAttack || usingBlasterAttack || usingPowerUp)
		{
			return;
		}

		// Play shield sound
		base.EnableShield();

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
			base.DisableShield();

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
		characterAnimator.SetLayerWeight(1, 0);
		audioSourcePlayerMovement.volume = 0;
		playerAllowedToMove = false;
		canAttack = false;
		usingPowerUp = false;
	}

	/// <summary> this unfreezes the player so that he can move and attack</summary>
	public void UnFreezePlayer()
	{
		playerAllowedToMove = true;
		canAttack = true;
		if (playerRigidbody != null)
			playerRigidbody.isKinematic = false;
	}

	private void ActivateForceField()
	{
		if (playerShield != null)
		{
			if (shieldIsEnabled && playerShield.color.a < 1.0f)
			{
				playerShield.color = new Color(playerShield.color.r, playerShield.color.g,
				                               playerShield.color.b, playerShield.color.a + Time.deltaTime * 4.0f);
			}
			else if (shieldIsEnabled == false && playerShield.color.a > 0.0f)
			{
				playerShield.color = new Color(playerShield.color.r, playerShield.color.g,
				                               playerShield.color.b, playerShield.color.a - Time.deltaTime * 4.0f);
			}
			//shieldCollider.enabled = (playerShield.color.a > 0.0f);
		}
	}

	/// <summary> Fade slowly to black </summary>
	private void FadeToBlack()
	{
		canvasFadeImage.color = Color.Lerp(canvasFadeImage.color, Color.black, 10.0f * Time.deltaTime);
	}
	#endregion

	#region Coroutines
	/// <summary> this plays the game over sound then loads the game over menu</summary>
	private IEnumerator PlayerDied()
	{
		// save the name of the scene the player died in
		Globals.sceneToLoad = SceneManager.GetActiveScene().name;

		// stop player movement
		FreezePlayer();
		Time.timeScale = 0.1f;

		// play the game over sound
		audioSource.clip = gameOverSound;
		audioSource.Play();

		// wait till the sound has played
		yield return new WaitForSecondsRealtime(audioSource.clip.length + .5f);

		StartCoroutine(FadeToBlackCoroutine());
	}

	/// <summary> Load the scene after fading to black </summary>
	public IEnumerator FadeToBlackCoroutine()
	{
		canvasFadeImage.color = Color.clear; // make image transparent

		while (canvasFadeImage.color.a <= 0.99f)
		{
			FadeToBlack();

			yield return null; // wait to the next frame to continue
		}

		canvasFadeImage.color = Color.black; // make image transparent

		Time.timeScale = 1.0f;

		// heal the player to full health then update his health UI
		maxHealth.runTimeValue = heartContainers.runTimeValue * 2f;
		playerHealthHUD.UpdateHearts();

		SceneManager.LoadScene("GameOverMenu");
	}
	#endregion
}