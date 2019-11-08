using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : BaseCharacter
{
	#region Enums

	#endregion

	#region Public Variables
	public bool canMove = true;
	#endregion

	#region Private Variables

	#endregion
	// Unity Named Methods
	#region Main Methods
	private void Awake()
	{
		health = 10;
	}
	#endregion

	#region Utility Methods
	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);

		Debug.Log("player health = " + health);
	}
	#endregion

	#region Coroutines

	#endregion
}