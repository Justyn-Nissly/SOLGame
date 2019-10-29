using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPlayer : ShieldBase
{
	#region Enums
	#endregion

	#region Public Variables
	public Player player;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (Input.GetButton("B"))
		{
			EnableShield();
			ShieldIsEnabled = true;
			player.canAttack = false;
		}
		else if (Input.GetButton("B") == false && ShieldIsEnabled)
		{
			DisableShield();
			ShieldIsEnabled = false;
			player.canAttack = true;
		}
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion
}
