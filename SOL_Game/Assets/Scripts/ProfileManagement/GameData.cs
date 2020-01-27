using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public bool
		blasterUnlocked,
		hammerUnlocked,
		shieldUnlocked,
		swordUnlocked;
	public int
		bossesDefeated, // Which bosses have been defeated
		saveItem;
	public float
		health;
	public float[]
		position; // This is used to replace Vector3 variabls because Vector3 variabls are not serializable
	public string
		firstName,
		lastName,
		middleName,
		password;
	public GameData()
	{
		blasterUnlocked = Globals.blasterUnlocked;
		bossesDefeated	= Globals.bossesDefeated;
		firstName		= Globals.firstName;
		hammerUnlocked	= Globals.hammerUnlocked;
		health			= Globals.currentPlayerHealth;
		lastName		= Globals.lastName;
		middleName		= Globals.middleName;
		password		= Globals.password;
		shieldUnlocked	= Globals.shieldUnlocked;
		swordUnlocked	= Globals.swordUnlocked;
		// possition to spawn in on load
	}
}