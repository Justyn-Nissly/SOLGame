using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; // This allows you to access the binary Formatters

public static class SaveSystem
{
	public static void SaveGame(Player player)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/player.savestuff";

		FileStream stream = new FileStream(path, FileMode.Create);



		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);
		writer.WriteLine("Test");
		writer.Close();



		GameData data = new GameData(player);
		//formatter.Serialize(stream, data);
		stream.Close();
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