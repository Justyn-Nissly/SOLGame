using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public int saveItem;
	public float health;
	public float[] position; // This is used to replace Vector3 variabls because Vector3 variabls are not serializable
	public string firstName, middleName, lastName;
	public static string password;
	public GameData()
	{
		/*level = player.level;
		helath = player.health;*/
		health = Globals.currentPlayerHealth;
		password = GameObject.FindGameObjectWithTag("PasswordText").GetComponent<TMPro.TextMeshProUGUI>().text;
		firstName = Globals.firstName;
	}
}