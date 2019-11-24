using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBase : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public SpriteRenderer
		  shieldSprite; // Shield graphics
	public BoxCollider2D
		  shieldBoxCollider; // The shield itself
	public bool
	  shieldIsEnabled; // For toggling the shield on/off
	public AudioSource
		shieldSound;

	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		shieldIsEnabled = false;
	}
	#endregion

	#region Utility Methods
	public ShieldBase()
	{

	}

	public ShieldBase(SpriteRenderer _shieldSprite, BoxCollider2D _shieldBoxCollider, AudioSource _shieldSound)
	{
		shieldSprite = _shieldSprite;
		shieldBoxCollider = _shieldBoxCollider;
		shieldSound = _shieldSound;
	}

	// Turn on the shield
	public void EnableShield()
	{
		shieldSprite.enabled      = true;
		shieldBoxCollider.enabled = true;

		if(shieldSound != null)
		{
			shieldSound.Play();
		}
	}

	// Turn off the shield
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
	#endregion
}