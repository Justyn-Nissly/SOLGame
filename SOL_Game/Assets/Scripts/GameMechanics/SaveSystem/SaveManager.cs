using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour
{
	[SerializeField]
	private SavedGame[]
		saveSlots; // The slots that contain saved games
	public Player
		player;
	public GameObject
		loadGameMenu;
	public Hud
		playerHealthHud;
	private bool
		active;

	void Awake()
	{
		//GameManager.FindObjectOfType<GameManager>();
		//player = GameObject.FindObjectOfType<Player>();
	}

	public void ShowFiles()
	{
		if(SceneManager.GetActiveScene().name == "Menu")
		{
			saveSlots[0] = GameObject.Find("Slot1").GetComponent<SavedGame>();
			saveSlots[1] = GameObject.Find("Slot2").GetComponent<SavedGame>();
			saveSlots[2] = GameObject.Find("Slot3").GetComponent<SavedGame>();
			loadGameMenu = GameObject.Find("LoadGameMenu");
			for (int index = 0; index < saveSlots.Length; index++)
			{
				ShowSavedFiles(saveSlots[index]);
			}
		}
	}

	void Update()
	{
		
	}

	public void Activate()
	{
		GameObject.Find("SaveSlots").GetComponent<Canvas>().sortingOrder = 32767;
	}
	private void ShowSavedFiles(SavedGame savedGame)
	{
		if(File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

			SaveData data = formatter.Deserialize(file) as SaveData;
			file.Close();
			
			savedGame.ShowSaveData(data);
		}
	}

	///<summary> Create a new game save </summary>
	public void NewGame(SavedGame savedGame)
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);

			SaveData data = new SaveData();

			NewSave(data);

			formatter.Serialize(file, data);

			file.Close();

			savedGame.ShowSaveData(data);
		}
		catch (System.Exception)
		{
			//Handles errors
			throw;
		}
	}

	///<summary> Save the game data </summary>
	public void Save(string savedGame)
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame + ".dat", FileMode.Create);

			SaveData data = new SaveData();

			SavePlayer(data);
			
			if (SceneManager.GetActiveScene().name != "GameOverMenu")
			{
				data.gameData.currentLevel = SceneManager.GetActiveScene().name;
			}

			formatter.Serialize(file, data);

			file.Close();
		}
		catch(System.Exception)
		{
			//Handles errors
			throw;
		}
	}

	///<summary> Populate the save data object with the default values </summary>
	private void NewSave(SaveData data)
	{
		data.gameData = new GameData(true, false, false, false, false, player.maxHealth.initialValue,
		                             player.heartContainers.initialValue, "Hub",
		                             0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
	}

	///<summary> Populate the save data object </summary>
	private void SavePlayer(SaveData data)
	{
		data.gameData = new GameData(Globals.startInBeginingPosition,           Globals.swordUnlocked,
									 Globals.hammerUnlocked,                    Globals.blasterUnlocked,
									 Globals.shieldUnlocked,                    player.maxHealth.runTimeValue,
									 player.heartContainers.runTimeValue,       Globals.sceneToLoad, 
									 Globals.bossesDefeated,                    Globals.guardiansDefeated,                 
									 Globals.hubCheckPointReached,              Globals.biolabCheckPointReached,
									 Globals.atlantisCheckPointReached,         Globals.factoryCheckPointReached,
									 Globals.factoryLevel2CheckPointReached,    Globals.geothermalCheckPointReached,
									 Globals.geothermalLevel2CheckPointReached, Globals.spacebaseCheckPointReached,
									 Globals.spacebaseLevel2CheckPointReached,  Globals.spacebaseLevel3CheckPointReached,
									 Globals.finalWyrmFightCheckPoint);
	}

	///<summary> Load the save data </summary>
	public void Load(SavedGame savedGame)
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream      file      = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
			SaveData        data      = formatter.Deserialize(file) as SaveData;

			LoadPlayer(data);

			file.Close();
			playerHealthHud = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Hud>();
			//playerHealthHud.ChangeNumberOfHearts();
			savedGame.ShowSaveData(data);
		}
		catch (System.Exception)
		{
			//Handles errors
		}
	}

	///<summary> Load the save data back into the game </summary>
	private void LoadPlayer(SaveData data)
	{
		//player.maxHealth.initialValue   = data.gameData.maxHealth; // don't change the initial value it will break stuff :)
		Globals.startInBeginingPosition           = data.gameData.beginingPosition;
		Globals.swordUnlocked                     = data.gameData.sword;
		Globals.hammerUnlocked                    = data.gameData.hammer;
		Globals.blasterUnlocked                   = data.gameData.blaster;
		Globals.shieldUnlocked                    = data.gameData.shield;
		player.maxHealth.runTimeValue             = data.gameData.currentHealth;
		player.heartContainers.runTimeValue       = data.gameData.maxHealth;
		Globals.sceneToLoad                       = data.gameData.currentLevel;
		Globals.bossesDefeated                    = data.gameData.bossesDefeated;
		Globals.guardiansDefeated                 = data.gameData.guardiansDefeated;
		Globals.hubCheckPointReached              = data.gameData.hubCheckPoint;
		Globals.biolabCheckPointReached           = data.gameData.biolabCheckPoint;
		Globals.atlantisCheckPointReached         = data.gameData.atlantisCheckPoint;
		Globals.factoryCheckPointReached          = data.gameData.factoryCheckPoint;
		Globals.factoryLevel2CheckPointReached    = data.gameData.factoryLevel2CheckPoint;
		Globals.geothermalCheckPointReached       = data.gameData.geothermalCheckPoint;
		Globals.geothermalLevel2CheckPointReached = data.gameData.geothermalLevel2CheckPoint;
		Globals.spacebaseCheckPointReached        = data.gameData.spacebaseCheckPoint;
		Globals.spacebaseLevel2CheckPointReached  = data.gameData.spacebaseLevel2CheckPoint;
		Globals.spacebaseLevel3CheckPointReached  = data.gameData.spacebaseLevel3CheckPoint;
		Globals.finalWyrmFightCheckPoint          = data.gameData.finalWyrmFightCheckPoint;
		player.SetUpInputDetection();
		playerHealthHud.ChangeNumberOfHearts();
		FindObjectOfType<loadSceneOnTrigger>().sceneToLoad = data.gameData.currentLevel;
	}

	///<summary> Delete a save file </summary>
	public void DeleteSave(SavedGame saveGame)
	{
		File.Delete(Application.persistentDataPath + "/" + saveGame.name + ".dat");
	}
}