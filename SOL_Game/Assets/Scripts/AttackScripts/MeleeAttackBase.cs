using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBase : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public Transform attackPosition;
	public float attackRange;
	public LayerMask willDamageLayer;
	public GameObject weapon;
	public FloatValue damageToGive;

	public List<AudioClip> soundEffects;
	public AudioSource audioSource;
	#endregion

	#region Private Variables
	protected float thrust = 7;
	protected float knockTime = .2f;
	protected bool characterHasKnockback = false;
	protected Player player;

	#endregion

	// Unity Named Methods
	#region Main Methods
	#endregion

	// Inflict damage function
	#region Utility Methods

	// constructor
	public MeleeAttackBase(Transform _attackPosition, float _attackRange, LayerMask _willDamageLayer,
						GameObject _weapon, FloatValue _damageToGive, List<AudioClip> _soundEffects, AudioSource _audioSource)
	{
		attackPosition = _attackPosition;
		attackRange = _attackRange;
		willDamageLayer = _willDamageLayer;
		weapon = _weapon;
		damageToGive = _damageToGive;
		soundEffects = _soundEffects;
		audioSource = _audioSource;
	}

	public MeleeAttackBase()
	{

	}

	public void Attack()
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

		if (soundEffects.Count > 0)
		{
			audioSource.clip = GetRamdomSoundEffect();
			audioSource.Play();
		}

		GameObject weaponInstance = Instantiate(weapon, attackPosition.transform);
		Destroy(weaponInstance, .5f);
    }
    #endregion

	#region Utility Methods

	private void ApplyKnockBack(GameObject characterBeingAtacked)
	{
		Rigidbody2D rigidbody2D = characterBeingAtacked.GetComponent<Rigidbody2D>();

		// add knock back
		if(rigidbody2D != null)
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

	private IEnumerator KnockBackCoroutine(GameObject characterBeingAtacked)
	{
		Enemy enemy = characterBeingAtacked.GetComponent<Enemy>();
		Rigidbody2D rigidbody2D = characterBeingAtacked.GetComponent<Rigidbody2D>();


		if(enemy != null)
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

				if(GetPlayer(characterBeingAtacked) != null)
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
	

	// gets the player script from the game object if it has one
	private Player GetPlayer(GameObject gameObject)
	{
		return gameObject.GetComponent<Player>();
	}

	private AudioClip GetRamdomSoundEffect()
	{
		int index = Random.Range(0, soundEffects.Count - 1);

		return soundEffects[index];
	}
	#endregion

	#region Coroutines
	#endregion
}
