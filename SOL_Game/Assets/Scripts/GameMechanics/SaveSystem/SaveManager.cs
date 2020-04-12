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
		/*foreach (SavedGame saved in saveSlots)
		{
			ShowSavedFiles(saved);
		}*/
	}

	void Start()
	{
		active = false;
	}

	void Update()
	{
		if (loadGameMenu.activeInHierarchy == true && active == true)
		{
			foreach (SavedGame saved in saveSlots)
			{
				ShowSavedFiles(saved);
			}
			active = false;
		}
	}

	public void Activate()
	{
		active = true;
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

			ShowSavedFiles(savedGame);
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
		data.myPlayerData = new PlayerData(Globals.startInBeginingPosition, Globals.swordUnlocked,  Globals.hammerUnlocked,
		                                   Globals.blasterUnlocked,         Globals.shieldUnlocked, player.maxHealth.runTimeValue,
		                                   Globals.bossesDefeated,          Globals.guardiansDefeated);
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
		Globals.startInBeginingPosition = data.myPlayerData.beginingPosition;
		Globals.swordUnlocked = true;//data.myPlayerData.sword;
		Globals.hammerUnlocked          = data.myPlayerData.hammer;
		Globals.blasterUnlocked         = data.myPlayerData.blaster;
		Globals.shieldUnlocked          = data.myPlayerData.shield;
		player.maxHealth.runTimeValue   = data.myPlayerData.playerHealth;
		Globals.bossesDefeated          = data.myPlayerData.bossesDefeated;
		Globals.guardiansDefeated       = data.myPlayerData.guardiansDefeated;
	}
}