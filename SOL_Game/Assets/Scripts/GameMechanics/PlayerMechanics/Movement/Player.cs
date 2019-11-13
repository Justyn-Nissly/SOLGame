using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : BaseCharacter
{
    public Signal playerHealthSignal;

	#region Enums

	#endregion

	#region Public Variables
	public bool canMove = true;
	#endregion

	#region Private Variables

	#endregion
	// Unity Named Methods
	#region Main Methods

	#endregion

	#region Utility Methods
	public override void TakeDamage(int damage)
	{

		base.TakeDamage(damage);
        playerHealthSignal.Raise();

        Debug.Log("player CurrentHealth = " + currentHealth.initialValue);
	}
	#endregion

	#region Coroutines

	#endregion
}