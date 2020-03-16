using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVarablesAndMethods
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	/// <summary>
	/// this a flag for if you want the player to start at the default start position in a facility
	/// for example if you go from the hub to a facility you want to be at the start but
	/// if you are on the second level of a facility going back up you don't want to start 
	/// at the beginning you want to start at the top of the stairs
	/// </summary>
	public static bool
		startInBeginingPosition = true, // Allow the player to start at the initial starting point
		swordUnlocked           = true, // Track if the sword is unlocked
		hammerUnlocked          = true, // Track if the hammer is unlocked
		blasterUnlocked         = true, // Track if the blaster is unlocked
		shieldUnlocked          = true; // Track if the shield is unlocked
	public static int 
		currentPlayerHealth, // The player's current health
		maxPlayerHealth = 8, // The maximum health the player has
	    bossesDefeated  = 0; // Which bosses have been defeated
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods (Empty)
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}