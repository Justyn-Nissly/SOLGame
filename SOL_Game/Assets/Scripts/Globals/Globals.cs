using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
	#region Enums
	#endregion

	#region Public Variables
	/// <summary>
	/// this a flag for if you want the player to start at the default start position in a facility
	/// for example if you go from the hub to a facility you want to be at the start but
	/// if you are on the second level of a facility going back up you don't want to start 
	/// at the beginning you want to start at the top of the stairs
	/// </summary>
	public static bool
		startInBeginingPosition = true,  // Allow the player to start at the initial starting point
		swordUnlocked           = false, // Track if the sword is unlocked
		hammerUnlocked          = false, // Track if the hammer is unlocked
		blasterUnlocked         = false, // Track if the blaster is unlocked
		shieldUnlocked          = false; // Track if the shield is unlocked
	public static int 
		maxPlayerHealth = 8, // The maximum health the player has
	    bossesDefeated  = 0; // Which bosses have been defeated
	public static float
		currentPlayerHealth; // The player's current health
	public static string firstName;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion
}
