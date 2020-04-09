using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static int
		saveSlotNumber = 1;
	///<summary> Save the game data </summary>
	public static void SaveGame(Player player)
	{
		BinaryFormatter
			formatter = new BinaryFormatter(); // The formatter to format the game data into binary
		string
			path = Application.persistentDataPath + $"/slot{saveSlotNumber}.save"; // The path to save the game data
		FileStream
			stream = new FileStream(path, FileMode.Create); // The file stream to save the game data
		SaveData
			data = new SaveData(player); // The game data to be saved

		// Format the data and save it
		formatter.Serialize(stream, data);
		stream.Close();
	}

	///<summary> Load the game data </summary>
	public static SaveData LoadData()
	{
		string
			path = Application.persistentDataPath + $"/slot{saveSlotNumber}.save"; // The path to load the game data

		// Check if the file exists
		if(File.Exists(path))
		{
			// Find the game save data and load it
			BinaryFormatter
				formatter = new BinaryFormatter(); // The formatter to format the game data
			FileStream
				stream = new FileStream(path, FileMode.Open); // The file stream to load the game data
			SaveData
				data = formatter.Deserialize(stream) as SaveData; // The game data to be loaded

			stream.Close();
			return data;
		}
		else
		{
			// Give an error message if a save file cannot be found
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}
}