using UnityEngine;

public class HammerGuardianWeakness : DestructibleObject
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	public AudioSource
		damaged;
	#endregion

	// Unity Named Methods
	#region Main Methods
	protected override void Start()
	{
		damaged = GetComponent<AudioSource>();
	}

	/// <summary> Check if the player has hit the guardian </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// If the right weapon hit the weak point deal the guardian damage
		if (collision.CompareTag("PlayerLightWeapon") && FindObjectOfType<HammerGuardianController>().isAttacking)
		{
			damaged.Play();
			if (--health <= 0)
			{
				DestroyObject();
				FindObjectOfType<HammerGuardianController>().AdvancePhase();
			}
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Change sprite to destroyed if applicable </summary>
	public override void DestroyObject()
	{
		if (destroyedSprite != null)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}