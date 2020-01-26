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
			File.AppendAllText(path, "Username: " + data.firstName + "\n");
			File.AppendAllText(path, "Password: " + data.password + "\n");
			File.AppendAllText(path, data.ToString());
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