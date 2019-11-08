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
	#endregion

	#region Private Variables
	protected bool
        shieldIsEnabled; // For toggling the shield on/off
    #endregion

    // Unity Named Methods
    #region Main Methods
    void Start()
    {
        shieldIsEnabled = false;
    }
	#endregion

	#region Utility Methods
	// Turn on the shield
	public void EnableShield()
	{
		shieldSprite.enabled      = true;
		shieldBoxCollider.enabled = true;
	}

	// Turn off the shield
	public void DisableShield()
	{
		shieldSprite.enabled = false;
		shieldBoxCollider.enabled = false;
	}
	#endregion

	#region Coroutines
	#endregion
}