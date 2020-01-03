using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public void SavePlayer()
	{
		SaveSystem.SaveGame(FindObjectOfType<Player>());
		Debug.Log("Saved Game");
	}

	public void LoadPlayer()
	{
		GameData data = SaveSystem.LoadGame();

		FindObjectOfType<Player>().saveItem = data.saveItem;
		GlobalVarablesAndMethods.currentPlayerHealth = data.health;
		FindObjectOfType<Hud>().UpdateHearts();
		Debug.Log(Application.persistentDataPath);
		Debug.Log("Loaded Game");
	}
}
