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
		swordUnlocked           = false, // Track if the sword   is unlocked
		hammerUnlocked          = false, // Track if the hammer  is unlocked
		blasterUnlocked         = false, // Track if the blaster is unlocked
		shieldUnlocked          = false, // Track if the shield  is unlocked
		playerCanDie            = true,  // This flag enables and disables the players ability to die when getting to zero health
		wyrmDefeated            = false; // Track if the Wyrm was defeated

		
		


	public static bool[]
		acquiredHealthIncrease = new bool[HEALTH_INCREASES];
	public static int
		currentPlayerHealth,   // The player's current health
		maxPlayerHealth   = 8, // The maximum health the player has
		bossesDefeated    = 0, // Which bosses have been defeated
		guardiansDefeated = 0, // Which guardians have been defeated
		// These are for staring the player at a check point the current check point
		hubCheckPointReached              = 0,
		biolabCheckPointReached           = 0,
		atlantisCheckPointReached         = 0,
		factoryCheckPointReached          = 0,
		factoryLevel2CheckPointReached    = 0,
		geothermalCheckPointReached       = 0,
		geothermalLevel2CheckPointReached = 0,
		spacebaseCheckPointReached        = 0,
		spacebaseLevel2CheckPointReached  = 0, 
		spacebaseLevel3CheckPointReached  = 0,
		finalWyrmFightCheckPoint          = 0;
	public static string
		sceneToLoad,     // This is the name of the scene that will be loaded if you click continue in the game over scene (and the scene that will be loaded when clicking load in main menu?)
		currentSaveFile; // The current save file that is being played on

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
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		if(player != null)
		{
			player.maxHealth.runTimeValue = player.maxHealth.initialValue;
		}


		startInBeginingPosition = true;
		swordUnlocked = false;
		hammerUnlocked = false;
		blasterUnlocked = false;
		shieldUnlocked = false;

		hubCheckPointReached = 0;
		biolabCheckPointReached = 0;
		atlantisCheckPointReached = 0;
		factoryCheckPointReached = 0;
		factoryLevel2CheckPointReached = 0;
		geothermalCheckPointReached = 0;
		geothermalLevel2CheckPointReached = 0;
		spacebaseCheckPointReached = 0;
		spacebaseLevel2CheckPointReached = 0;
		spacebaseLevel3CheckPointReached = 0;
		finalWyrmFightCheckPoint = 0;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}