using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		maxHealth; // Maximum possible health
	public bool
		canAttack = true,     // Toggle ability to attack
		canTakeDamage = true; // Toggle vulnerability
	public float
		currentHealth; // Current health

	public List<AudioClip>
		takeDamageSounds,       // Play injured sound
		meleeSwingSoundEffects; // Play melee attack sound

	public AudioSource
		audioSource; // Plays character sounds

	// Light melee attack variables (fill in the inspector only if using light melee attack)
	public Transform
		lightMeleeAttackPosition;
	public float
		lightMeleeAttackRange;
	public GameObject
		lightMeleeWeapon;
	public FloatValue
		lightMeleeDamageToGive;

	// Heavy melee attack variables (fill in the inspector only if using heavy melee attack)
	public Transform
		heavyMeleeAttackPosition;
	public float
		heavyMeleeAttackRange;
	public GameObject
		heavyMeleeWeapon;
	public FloatValue
		heavyMeleeDamageToGive;

	public LayerMask
		willDamageLayer; // Layer that light and heavy melee will damage

	// Ranged attack variables (fill in the inspector only if using ranged attack)
	public Transform
		firePoint,
		gunSpawnPoint;
	public GameObject
		bulletPrefab,
		gunPrefab;
	public FloatValue
		rangedAttackDamageToGive;
	public float
		BulletShootingDelay = .2f;
	public AudioClip
		blasterSound;

	// Shield variables
	public SpriteRenderer
		shieldSprite; // Shield graphics
	public BoxCollider2D
		shieldBoxCollider; // The shield itself
	public AudioSource
		shieldSound; // Sound of the shield while active
	#endregion

	#region Private Variables
	protected float
		thrust = 7,      // Apply knock back
		knockTime = .2f; // Time of knock back
	protected bool
		characterHasKnockback = false, // Used for knock back
		shieldIsEnabled = false;       // Check if shield is on or off
	// NOT NEEDED; REMOVE LATER
	// NOT NEEDED; REMOVE LATER
	// NOT NEEDED; REMOVE LATER
	protected Player player;
	// NOT NEEDED; REMOVE LATER
	// NOT NEEDED; REMOVE LATER
	// NOT NEEDED; REMOVE LATER
	#endregion

	// Unity Named Methods
	#region Main Methods (Empty)
	#endregion

	#region Utility Methods
	/// <summary> Make the character take damage and become temporarily invincible </summary> 
	public virtual void TakeDamage(int damage, bool playSwordImpactSound)
	{
		// The character must be vulnerable to take damage
		if (canTakeDamage == true)
		{
			// Play a damaged sound
			if (takeDamageSounds.Count > 0 && playSwordImpactSound && audioSource != null)
			{
				audioSource.clip = GetRandomSoundEffect(takeDamageSounds);
				audioSource.Play();
			}

			// Reduce the character's health and grant it temporary invincibility
			currentHealth -= damage;
			StartCoroutine("StartBlinking");
		}
	}

	/// <summary> Make a character use a melee attack</summary>
	public void MeleeAttack(GameObject meleeWeapon, Transform attackPosition, float attackRange, FloatValue damageToGive, bool characterHasKnockback)
	{
		// Enable the attack to damage multiple objects
		Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, willDamageLayer);

		// Each object hit can take damage
		foreach (Collider2D collider in enemiesToDamage)
		{
			// Damage and knock back hit objects
			BaseCharacter characterBeingAtacked = collider.GetComponent<BaseCharacter>();
			if (characterBeingAtacked != null)
			{
				characterBeingAtacked.TakeDamage((int)damageToGive.initialValue, true);

				if (characterHasKnockback)
				{
					ApplyKnockBack(collider.gameObject);
				}
			}
		}

		// Play a damage sound
		if (meleeSwingSoundEffects.Count > 0)
		{
			audioSource.clip = GetRandomSoundEffect(meleeSwingSoundEffects);
			audioSource.Play();
		}

		// VISUALS HANDLED BY ANIMATOR; CHANGE THIS LATER
		// VISUALS HANDLED BY ANIMATOR; CHANGE THIS LATER
		// VISUALS HANDLED BY ANIMATOR; CHANGE THIS LATER
		GameObject weaponInstance = Instantiate(meleeWeapon, attackPosition.transform);
		Destroy(weaponInstance, .5f);
		// VISUALS HANDLED BY ANIMATOR; CHANGE THIS LATER
		// VISUALS HANDLED BY ANIMATOR; CHANGE THIS LATER
		// VISUALS HANDLED BY ANIMATOR; CHANGE THIS LATER
	}

	/// <summary> Apply knock back to an object </summary>
	// NEEDS EDITING
	// NEEDS EDITING
	// NEEDS EDITING
	private void ApplyKnockBack(GameObject characterBeingAtacked)
	{
		// Find the rigidbody of the target
		Rigidbody2D rigidbody2D = characterBeingAtacked.GetComponent<Rigidbody2D>();

		// Apply knock back to the target
		if (rigidbody2D != null)
		{
			Vector2
				difference; // The direction to apply knock back

			// Find the direction to knock back the object
			if (GetPlayer(characterBeingAtacked) != null)
			{
				GetPlayer(characterBeingAtacked).playerAllowedToMove = false;
				rigidbody2D.isKinematic = true;
				rigidbody2D.isKinematic = false;
				difference              =
					characterBeingAtacked.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
			}
			else
			{
				difference = characterBeingAtacked.transform.position - transform.position;
			}

			// Apply force to knock back the target
			difference = difference.normalized * thrust;
			rigidbody2D.AddForce(difference, ForceMode2D.Impulse);
			StartCoroutine(KnockBackCoroutine(characterBeingAtacked));
		}
	}
	// NEEDS EDITING
	// NEEDS EDITING
	// NEEDS EDITING

	/// <summary> Find the player (not needed after fixing the knock back effect)</summary>
	private Player GetPlayer(GameObject gameObject)
	{
		return gameObject.GetComponent<Player>();
	}

	/// <summary> Gets random sound effect from a list </summary>
	private AudioClip GetRandomSoundEffect(List<AudioClip> SoundEffectList)
	{
		return SoundEffectList[Random.Range(0, SoundEffectList.Count - 1)];
	}

	/// <summary> Instantiate gun and bullet (both must be assigned in inspector) </summary>
	public virtual void Shoot()
	{
		InstantiateAndDestroyGun();

		// Play the blaster firing sound
		if (audioSource != null && blasterSound != null)
		{
			audioSource.clip = blasterSound;
			audioSource.Play();
		}

		// Launch the bullet
		StartCoroutine(InstantiateBullet());
	}

	/// <summary> Temporarily spawn the gun </summary>
	private void InstantiateAndDestroyGun()
	{
		GameObject gunInstance = Instantiate(gunPrefab, gunSpawnPoint.position, gunSpawnPoint.rotation);
		gunInstance.transform.SetParent(gunSpawnPoint);

		Destroy(gunInstance, .5f);
	}

	/// <summary> Activate shield </summary>
	public virtual void EnableShield()
	{
		// Make the shield visible and able to block
		shieldSprite.enabled      = true;
		shieldBoxCollider.enabled = true;

		// Play shield sound
		if (shieldSound != null)
		{
			shieldSound.Play();
		}
	}

	/// <summary> Deactivate shield </summary>
	public virtual void DisableShield()
	{
		// Make the shield invisible and unable to block
		shieldSprite.enabled = false;
		shieldBoxCollider.enabled = false;

		// Stop playing shield sound
		if (shieldSound != null)
		{
			shieldSound.Stop();
		}
	}

	#endregion

	#region Coroutines
	/// <summary> Make a character blink for a short time after taking damage </summary>
	IEnumerator StartBlinking()
	{
		SpriteRenderer
			spriteRenderer = GetComponent<SpriteRenderer>(); // Control character's sprite
		float
			timer = 0.5f;  // Time to blink after taking damage

		canTakeDamage = false; // Character becomes temporarily invulnerable

		// Toggle sprite's visibility to make it blink
		while (timer >= 0.0f)
		{
			spriteRenderer.enabled = !spriteRenderer.enabled;
			timer -= Time.deltaTime + 0.1f;
			yield return new WaitForSeconds(0.1f);
		}

		// Ensure the sprite is visible and the character can take damage after blinking stops
		spriteRenderer.enabled = true;
		canTakeDamage = true;
	}

	/// <summary> Apply knock back </summary>
	// NEEDS EDITING
	// NEEDS EDITING
	// NEEDS EDITING
	private IEnumerator KnockBackCoroutine(GameObject characterBeingAtacked)
	{
		Enemy
			enemy = characterBeingAtacked.GetComponent<Enemy>(); // Reference an enemy
		Rigidbody2D
			rigidbody2D = characterBeingAtacked.GetComponent<Rigidbody2D>(); // Apply physics to character

		// Damaged enemies deaggro while being knocked back
		if (enemy != null)
		{
			enemy.aggro = false;
		}

		// Apply knock back
		if (rigidbody2D != null)
		{
			yield return new WaitForSeconds(knockTime);
			if (rigidbody2D != null)
			{
				rigidbody2D.velocity    = Vector2.zero;
				rigidbody2D.isKinematic = true;
				rigidbody2D.isKinematic = false;

				if (GetPlayer(characterBeingAtacked) != null)
				{
					GetPlayer(characterBeingAtacked).playerAllowedToMove = true;
				}

				// Damaged enemies automatically aggro after knock back dissipates
				if (enemy != null)
				{
					enemy.aggro = true;
				}
			}
		}
	}
	// NEEDS EDITING
	// NEEDS EDITING
	// NEEDS EDITING

	/// <summary> Instantiate bullet </summary>
	// NEEDS EDITING
	// NEEDS EDITING
	// NEEDS EDITING
	private IEnumerator InstantiateBullet()
	{
		yield return new WaitForSeconds(BulletShootingDelay);

		GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		BulletLogic bulletLogic   = bulletInstance.GetComponent<BulletLogic>();
		bulletLogic.bulletDamage  = (int)rangedAttackDamageToGive.initialValue;
	}
	// NEEDS EDITING
	// NEEDS EDITING
	// NEEDS EDITING
	#endregion
}