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
		player = GameObject.FindObjectOfType<Player>();
	}

	void Start()
	{
		for (int index = 0; index < saveSlots.Length; index++)
		{
			ShowSavedFiles(saveSlots[index]);
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

	///<summary> Save the game data </summary>
	public void Save(SavedGame savedGame)
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);

			SaveData data = new SaveData();

			SavePlayer(data);

			data.currentLevel = SceneManager.GetActiveScene().name;

			formatter.Serialize(file, data);

			file.Close();

			savedGame.ShowSaveData(data);
		}
		catch(System.Exception)
		{
			//Handles errors
			throw;
		}
	}

	///<summary> Populate the save data object </summary>
	private void SavePlayer(SaveData data)
	{
		data.gameData = new GameData(Globals.startInBeginingPosition,     Globals.swordUnlocked,
									 Globals.hammerUnlocked,              Globals.blasterUnlocked,
									 Globals.shieldUnlocked,              player.maxHealth.runTimeValue,
									 player.heartContainers.runTimeValue, Globals.bossesDefeated,
									 Globals.guardiansDefeated);
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
			playerHealthHud.UpdateHearts();
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
		Globals.startInBeginingPosition = data.gameData.beginingPosition;
		Globals.swordUnlocked           = data.gameData.sword;
		Globals.hammerUnlocked          = data.gameData.hammer;
		Globals.blasterUnlocked         = data.gameData.blaster;
		Globals.shieldUnlocked          = data.gameData.shield;
		player.maxHealth.runTimeValue   = data.gameData.currentHealth;
		player.maxHealth.initialValue   = data.gameData.maxHealth;
		Globals.bossesDefeated          = data.gameData.bossesDefeated;
		Globals.guardiansDefeated       = data.gameData.guardiansDefeated;
		player.SetUpInputDetection();
	}
}