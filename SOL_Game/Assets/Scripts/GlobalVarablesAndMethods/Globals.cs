using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
	#region Enums and Defined Constants
	public const int
		MAX_PLAYER_HEALTH = 8,
		HEALTH_INCREASES  = MAX_PLAYER_HEALTH - 3;
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
		swordUnlocked = false, // Track if the sword is unlocked
		hammerUnlocked = false, // Track if the hammer is unlocked
		blasterUnlocked = false, // Track if the blaster is unlocked
		shieldUnlocked = false, // Track if the shield is unlocked
		playerCanDie = false, // This flag enables and disables the players ability to die when getting to zero health

		// these are for staring the player at a check point (passed bosses if the player died)
		hubCheckPointReached = false,
		biolabCheckPointReached = false,
		atlantisCheckPointReached = false,
		factoryCheckPointReached = false,
		geothermalCheckPointReached = false,
		spacebaseCheckPointReached = false,
		wyrmDefeated = false;


	public static bool[]
		acquiredHealthIncrease = new bool[HEALTH_INCREASES];
	public static int
		currentPlayerHealth,   // The player's current health
		maxPlayerHealth = 8, // The maximum health the player has
		bossesDefeated = 0, // Which bosses have been defeated
		guardiansDefeated = 0;
	public static string
		sceneToLoad; // this is the name of the scene that will be loaded if you click continue in the game over scene (and the scene that will be loaded when clicking load in main menu?)

	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods (Empty)
	#endregion

	#region Utility Methods
	/// <summary>
	/// resets any saved data from the last play through (don't call this method if you are loading a saved game!!!!)
	/// </summary>
	public static void StartNewGame()
	{
		startInBeginingPosition = true;
		swordUnlocked = false;
		hammerUnlocked = false;
		blasterUnlocked = false;
		shieldUnlocked = false;

		hubCheckPointReached = false;
		biolabCheckPointReached = false;
		atlantisCheckPointReached = false;
		factoryCheckPointReached = false;
		geothermalCheckPointReached = false;
		spacebaseCheckPointReached = false;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
