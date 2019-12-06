using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		maxHealth;     // The character's highest health possible
	public bool
		canAttack = true,     // Toggle the character's ability to attack
		canTakeDamage = true; // Toggle the character's ability to take damage
	public float
		currentHealth; // The character's current health

	public List<AudioClip>
		takeDamageSounds,
		meleeSwingSoundEffects;

	public AudioSource
		audioSource; // used to player sounds from

	// light melee attack variables (only need to be filled in the inspector if you use the light melee attack)
	public Transform
		lightMeleeAttackPosition;
	public float
		lightMeleeAttackRange;
	public GameObject
		lightMeleeWeapon;
	public FloatValue
		lightMeleeDamageToGive;

	// heavy melee attack variables (only need to be filled in the inspector if you use the heavy melee attack)
	public Transform
		heavyMeleeAttackPosition;
	public float
		heavyMeleeAttackRange;
	public GameObject
		heavyMeleeWeapon;
	public FloatValue
		heavyMeleeDamageToGive;

	public LayerMask
		willDamageLayer; // the layer that the light and heavy attack will damage

	// ranged attack variables (only need to be filled in the inspector if you use the ranged attack)
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

	// shield variables
	public SpriteRenderer
		shieldSprite; // Shield graphics
	public BoxCollider2D
		shieldBoxCollider; // The shield itself
	public AudioSource
		shieldSound; // the sound of the shield while its active

	#endregion

	#region Private Variables
	protected float
		thrust = 7, // used for the knock back effect
		knockTime = .2f; // used for the knock back effect
	protected bool
		characterHasKnockback = false, // used for the knock back effect
		shieldIsEnabled = false; // flag for whether the shield is on/off
	protected Player player; // not needed ?
	#endregion

	// Unity Named Methods
	#region Main Methods (Empty)
	#endregion

	#region Utility Methods
	/// <summary> Make the character receive damage and then become temporarily invincible </summary> 
	public virtual void TakeDamage(int damage, bool playSwordImpactSound)
	{
		if (canTakeDamage == true)
		{
			if (takeDamageSounds.Count > 0 && playSwordImpactSound && audioSource != null)
			{
				audioSource.clip = GetRandomSoundEffect(takeDamageSounds);
				audioSource.Play();
			}


			currentHealth -= damage;
			StartCoroutine("StartBlinking");
		}
	}

	/// <summary> the attack method used for the enemy and the player to swing light/heavy melee weapons</summary>
	public void MeleeAttack(GameObject meleeWeapon, Transform attackPosition, float attackRange, FloatValue damageToGive, bool characterHasKnockback)
	{
		Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, willDamageLayer);

		foreach (Collider2D collider in enemiesToDamage)
		{
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

		if (meleeSwingSoundEffects.Count > 0)
		{
			audioSource.clip = GetRandomSoundEffect(meleeSwingSoundEffects);
			audioSource.Play();
		}

		GameObject weaponInstance = Instantiate(meleeWeapon, attackPosition.transform);
		Destroy(weaponInstance, .5f);
	}

	/// <summary> needs to be changed to always work</summary>
	private void ApplyKnockBack(GameObject characterBeingAtacked)
	{
		Rigidbody2D rigidbody2D = characterBeingAtacked.GetComponent<Rigidbody2D>();

		// add knock back
		if (rigidbody2D != null)
		{
			Vector2 difference;

			if (GetPlayer(characterBeingAtacked) != null)
			{
				GetPlayer(characterBeingAtacked).playerAllowedToMove = false;
				rigidbody2D.isKinematic = true;
				rigidbody2D.isKinematic = false;

				difference = characterBeingAtacked.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
			}
			else
			{
				difference = characterBeingAtacked.transform.position - transform.position;
			}

			difference = difference.normalized * thrust;

			rigidbody2D.AddForce(difference, ForceMode2D.Impulse);
			StartCoroutine(KnockBackCoroutine(characterBeingAtacked));
		}

	}

	// 
	/// <summary> gets the player script from the game object if it has one (not needed after fixing the knock back effect)</summary>
	private Player GetPlayer(GameObject gameObject)
	{
		return gameObject.GetComponent<Player>();
	}

	/// <summary>gets a random sound effect from a list of sound effects and returns it</summary>
	private AudioClip GetRandomSoundEffect(List<AudioClip> SoundEffectList)
	{
		return SoundEffectList[Random.Range(0, SoundEffectList.Count - 1)];
	}

	/// <summary> Instantiates the gun and a bullet (both need to be assigned in the inspector to work)</summary>
	public virtual void Shoot()
	{
		InstantiateAndDestroyGun();

		if (audioSource != null && blasterSound != null)
		{
			audioSource.clip = blasterSound;
			audioSource.Play();
		}

		if (BulletShootingDelay == 0)
		{
			InstantiateBullet();
		}
		else
		{
			Invoke("InstantiateBullet", BulletShootingDelay);
		}
	}

	/// <summary> instantiates the gun game object variable then destroys it N seconds later </summary>
	private void InstantiateAndDestroyGun()
	{
		GameObject gunInstance = Instantiate(gunPrefab, gunSpawnPoint.position, gunSpawnPoint.rotation);
		gunInstance.transform.SetParent(gunSpawnPoint);

		Destroy(gunInstance, .5f);
	}

	/// <summary> instantiates the bullet game object variable </summary>
	private void InstantiateBullet()
	{
		GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

		BulletLogic bulletLogic = bulletInstance.GetComponent<BulletLogic>();
		bulletLogic.bulletDamage = (int)rangedAttackDamageToGive.initialValue; // is this right
	}

	/// <summary> Turn on the shield </summary>
	public void EnableShield()
	{
		shieldSprite.enabled = true;
		shieldBoxCollider.enabled = true;

		if (shieldSound != null)
		{
			shieldSound.Play();
		}
	}

	/// <summary> Turn off the shield </summary>
	public void DisableShield()
	{
		shieldSprite.enabled = false;
		shieldBoxCollider.enabled = false;

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
			spriteRenderer = GetComponent<SpriteRenderer>();
		float
			timer = 0.5f;  // The character blinks for a short time after taking damage
		canTakeDamage = false; // The character cannot take more damage immediately after taking damage

		// make the character blink
		while (timer >= 0.0f)
		{
			spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle the sprite's visibility to make it blink
			timer -= Time.deltaTime + 0.1f;
			yield return new WaitForSeconds(0.1f);
		}

		// Ensure the sprite is visible and the character can take damage after blinking stops
		spriteRenderer.enabled = true;
		canTakeDamage = true;
	}

	/// <summary> needs to be fixed </summary>
	private IEnumerator KnockBackCoroutine(GameObject characterBeingAtacked)
	{
		Enemy enemy = characterBeingAtacked.GetComponent<Enemy>();
		Rigidbody2D rigidbody2D = characterBeingAtacked.GetComponent<Rigidbody2D>();


		if (enemy != null)
		{
			enemy.aggro = false;
		}

		if (rigidbody2D != null)
		{
			yield return new WaitForSeconds(knockTime);
			if (rigidbody2D != null)
			{
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.isKinematic = true;
				rigidbody2D.isKinematic = false;

				if (GetPlayer(characterBeingAtacked) != null)
				{
					GetPlayer(characterBeingAtacked).playerAllowedToMove = true;
				}

				if (enemy != null)
				{
					enemy.aggro = true;
				}
			}
		}
	}
	#endregion
}