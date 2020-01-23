using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; // This allows you to access the binary Formatters

public static class SaveSystem
{
	public static void SaveGame(Player player)
	{

		GameData data = new GameData();
		
		//Debug.Log(data.firstName);
		string path = Application.dataPath + "/" + Globals.firstName + "-" + "Profile" + ".data";
		if(!File.Exists(path))
		{
			File.WriteAllText(path, "Profile Information:\n\n");
		}
		else
		{
			File.AppendAllText(path, data.firstName + "\n");
			
		}
		//File.AppendAllText(path, data.password); // does not work for some reason. need to be able to access all the data and print it to a file so we can see it.







		/*BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/" + Globals.firstName + "-" + 
			          Globals.middlename + "-" + Globals.lastname + ".data";

		FileStream stream = new FileStream(path, FileMode.Truncate);

		GameData data = new GameData(player);
		formatter.Serialize(stream, data);
		stream.Close();*/
	}

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