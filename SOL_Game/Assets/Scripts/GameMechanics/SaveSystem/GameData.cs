using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
	///<summary> Save the game data </summary>
	public void SaveGame()
	{
		SaveSystem.SaveGame(FindObjectOfType<Player>());
	}

	///<summary> Load the game data </summary>
	public void LoadGame()
	{
		SaveData
			loadData = SaveSystem.LoadData(); // The game data to be loaded

		Globals.startInBeginingPosition                   = loadData.startInBeginingPosition;
		Globals.swordUnlocked                             = loadData.swordUnlocked;
		Globals.hammerUnlocked                            = loadData.hammerUnlocked;
		Globals.blasterUnlocked                           = loadData.blasterUnlocked;
		Globals.shieldUnlocked                            = loadData.shieldUnlocked;
		FindObjectOfType<Player>().maxHealth.initialValue = loadData.currentPlayerHealth;
		Globals.bossesDefeated                            = loadData.bossesDefeated;
		Globals.guardiansDefeated                         = loadData.guardiansDefeated;
	}
}