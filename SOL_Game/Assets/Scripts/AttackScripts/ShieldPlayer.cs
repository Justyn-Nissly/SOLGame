using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPlayer : ShieldBase
{
	#region Enums and Defined Constants
	#endregion

	#region Public Variables
	public Player
        player; // Reference player script
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
        // On input activate the player's shield
		if (Input.GetButton("B"))
		{
			EnableShield();
			shieldIsEnabled = true;
			player.canAttack = false;
		}
        // On release of input deactivate the player's shield
        else if (Input.GetButton("B") == false && shieldIsEnabled)
		{
			DisableShield();
			shieldIsEnabled = false;
			player.canAttack = true;
		}
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion
}
