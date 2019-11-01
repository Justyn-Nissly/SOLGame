using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : BaseCharacter
{
	#region Enums

	#endregion

	#region Public Variables
	#endregion

	#region Private Variables

	#endregion
	// Unity Named Methods
	#region Main Methods
	private void Awake()
	{
		CurrentHealth = MaxHealth.StartingPlayerHP;
	}
	#endregion

	#region Utility Methods
	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);

		Debug.Log("player CurrentHealth = " + CurrentHealth);
	}
	#endregion

	#region Coroutines

	#endregion
}