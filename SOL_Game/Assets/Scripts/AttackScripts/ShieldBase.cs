using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBase : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public SpriteRenderer shieldSprite;
	public BoxCollider2D shieldBoxCollider;
	#endregion

	#region Private Variables
	// so that you don't call disable shield if the shield is already disabled
	protected bool ShieldIsEnabled = false;
	#endregion

	// Unity Named Methods
	#region Main Methods

	#endregion

	#region Utility Methods
	/// <summary>
	/// Enable the shield sprite
	/// </summary>
	public void EnableShield()
	{
		shieldSprite.enabled = true;
		shieldBoxCollider.enabled = true;
	}

	/// <summary>
	/// Disable the shield sprite
	/// </summary>
	public void DisableShield()
	{
		shieldSprite.enabled = false;
		shieldBoxCollider.enabled = false;
	}
	#endregion

	#region Coroutines
	#endregion
}
