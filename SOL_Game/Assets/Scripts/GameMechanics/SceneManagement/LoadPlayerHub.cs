using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sets the players starting position in the hub
public class LoadPlayerHub : LoadPlayer
{
	// Empty
	#region Enums
	#endregion

	#region Public Variables
	public static bool UseBeginningPostion = true;
	#endregion

	// Unity Named Methods
	// Empty
	#region Main Methods
	#endregion

	#region Utility Methods
	public override void SetStartingPostion()
	{
		base.SetStartingPostion();

		if (UseBeginningPostion)
		{
			startingPosition = defaultPlayerStartingPosition;
		}
		else
		{
			startingPosition = altStartingPosition;
		}
	}
	#endregion

	// Empty
	#region Coroutines
	#endregion
}
