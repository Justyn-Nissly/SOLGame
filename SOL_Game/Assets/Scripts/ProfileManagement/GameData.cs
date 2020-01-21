using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public int saveItem;
	public float health;
	public float[] position; // This is used to replace Vector3 variabls because Vector3 variabls are not serializable
	public static string password, firstName, middleName, lastName;

	public GameData(Player player)
	{
		/*level = player.level;
		helath = player.health;*/
		saveItem = player.saveItem;
		health = Globals.currentPlayerHealth;
		firstName = Globals.firstName;
		
		
	}
}