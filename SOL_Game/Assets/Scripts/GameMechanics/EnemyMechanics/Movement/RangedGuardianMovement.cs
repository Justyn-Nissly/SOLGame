using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedGuardianMovement : FreeRoam
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public bool
	canFreeRoam = true; // with this you can control if the RangedGuardian free roams regardless of other conditions
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		// if the enemy is the ranged guardian roam even if aggro, but stop if the enemy cant attack(this stops the enemy from moving then spawning enemies)
		if (canFreeRoam)
		{
			Roam();
		}
		else
		{
			waiting = waitTime;
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}