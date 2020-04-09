using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public const int
		MAX_PLAYER_HEALTH = 8,
		HEALTH_INCREASES = MAX_PLAYER_HEALTH - 3;

	public bool
		startInBeginingPosition,  // Allow the player to start at the initial starting point
		swordUnlocked,            // Track if the sword is unlocked
		hammerUnlocked,           // Track if the hammer is unlocked
		blasterUnlocked,          // Track if the blaster is unlocked
		shieldUnlocked,           // Track if the shield is unlocked
		playerCanDie;             // This flag enables and disables the players ability to die when getting to zero health
	public bool[]
		acquiredHealthIncrease = new bool[HEALTH_INCREASES];
	public int
		maxPlayerHealth, // The maximum health the player has
		bossesDefeated, // Which bosses have been defeated
		guardiansDefeated;
	public static string
		sceneToLoad; // this is the name of the scene that will be loaded if you click continue in the game over scene (and the scene that will be loaded when clicking load in main menu?)
	public float
		currentPlayerHealth; // The player's current health
	public SaveData(Player player)
	{
		startInBeginingPosition = Globals.startInBeginingPosition;
		swordUnlocked           = Globals.swordUnlocked;
		hammerUnlocked          = Globals.hammerUnlocked;
		blasterUnlocked         = Globals.blasterUnlocked;
		shieldUnlocked          = Globals.shieldUnlocked;
		currentPlayerHealth     = player.maxHealth.runTimeValue;
		bossesDefeated          = Globals.bossesDefeated;
		guardiansDefeated       = Globals.guardiansDefeated;
		//sceneToLoad = 
	}
}