using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; // This allows you to access the binary Formatters

public static class SaveSystem
{
	/// <summary> Save all game data to a file </summary>
	public static void SaveGame()
	{

		GameData data = new GameData(); // The game data to save
		
		//Debug.Log(data.firstName);
		string path = Application.dataPath + "/" + Globals.firstName + "-" + "Profile" + ".data";
		if(!File.Exists(path))
		{
			File.WriteAllText (path, "Profile Information:\n\n");
			File.AppendAllText(path, "Username: "         + data.firstName + " " 
				                                          + data.middleName + " " + data.lastName + " string string string" + "\n");
			File.AppendAllText(path, "Password: "         + data.password + " string" + "\n");
			File.AppendAllText(path, "Sword Unlocked: "   + data.swordUnlocked + " bool" + "\n");
			File.AppendAllText(path, "Shield Unlocked: "  + data.shieldUnlocked + " bool" + "\n");
			File.AppendAllText(path, "Blaster Unlocked: " + data.blasterUnlocked + " bool" + "\n");
			File.AppendAllText(path, "Hammer Unlocked: "  + data.hammerUnlocked + " bool" + "\n");
			File.AppendAllText(path, "Player Health: "    + data.health + " float"+ "\n");
			File.AppendAllText(path, "Bosses Defeated: "  + data.bossesDefeated + " int");

			///<summary> The file needs to be formatted into a table so that we can read in all the data and correctly</summary>
		}
		else
		{
			File.AppendAllText(path, data.firstName + "\n");
			
		}
		/*BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/" + Globals.firstName + "-" + 
			          Globals.middlename + "-" + Globals.lastname + ".data";

		FileStream stream = new FileStream(path, FileMode.Truncate);

		GameData data = new GameData(player);
		formatter.Serialize(stream, data);
		stream.Close();*/
	}

	/// <summary> Load all game data for the player's profile </summary>
	public static GameData LoadGame()
	{
		string path = Application.persistentDataPath + "/player.savestuff";

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			GameData data = formatter.Deserialize(stream) as GameData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("Save file not found in" + path);
			return null;
		}
	}
}