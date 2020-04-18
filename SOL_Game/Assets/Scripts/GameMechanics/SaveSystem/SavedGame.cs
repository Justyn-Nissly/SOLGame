using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
	[SerializeField]
	private Image
		swordChip,   // Sword   item image
		blasterChip, // Blaster item image
		shieldChip,  // Shield  item image
		hammerChip;  // Hammer  item image

	[SerializeField]
	private GameObject
		visuals; // Save slot visuals

	[SerializeField]
	private GameObject[]
		heartContainers;

	[SerializeField]
	private int
		saveSlotIndex; // Index of the save slot
	public Hud
		saveHealthHud; // The display for the players current and max health for each save
	public Text
		btnText,      // The text for the save/load button
		currentLevel; // The current level the player is on
	public MainMenu
		mainMenu; // The reference to the main menu

	///<summary> Accessor to get the save slot index </summary>
	public int GetIndex
	{
		get
		{
			return saveSlotIndex;
		}
	}

	///<summary> Set all visuals to false if there is no save data </summary>
	private void Awake()
	{
		visuals.SetActive(false);
	}

	///<summary> Load all saved files upon entering the file menu </summary>
	public void ShowAllFiles()
	{
		GameObject.FindObjectOfType<SaveManager>().ShowFiles();
	}

	///<summary> Show the saved data for a specified file </summary>
	public void ShowSaveData(SaveData data)
	{
		// Set the visuals for the save slot to active
		visuals.SetActive(true);
		if (File.Exists(Application.persistentDataPath + "/" + this.gameObject.name + ".dat"))
		{
			btnText.text = "Load";
		}
		else
		{
			btnText.text = "Save";
		}

		switch (data.gameData.currentLevel)
		{
			case "Hub":
				currentLevel.text = "Hub";
				break;

			case "Biolab (first facility)":
				currentLevel.text = "Biolab";
				break;

			case "Atlantis (second facility)":
				currentLevel.text = "Atlantis";
				break;

			case "Factory (third facility level 1)":
			case "Factory (third facility level 2)":
			case "Factory (third facility level 3)":
				currentLevel.text = "Factory";
				break;

			case "Geo-thermal plant (fourth facility level 1)":
			case "Geo-thermal plant (fourth facility level 2)":
				currentLevel.text = "Geo-thermal Plant";
				break;

			case "Adrics lab (fifth facility level 1)":
			case "Adrics lab (fifth facility level 2)":
			case "Adrics lab (fifth facility level 3)":
			case "FinalWyrmFight":
				currentLevel.text = "Adric's Lab";
				break;
		}
		
		// Display the players current health on a save slot
		float tempHealth = data.gameData.currentHealth * 0.5f;
		for (int i = 0; i < Globals.MAX_PLAYER_HEALTH && i < data.gameData.maxHealth; i++)
		{
			if (i <= tempHealth - 1)
			{
				saveHealthHud.hearts[i].sprite = saveHealthHud.fullHeart;
			}
			else if (i >= tempHealth)
			{
				saveHealthHud.hearts[i].sprite = saveHealthHud.emptyHeart;
			}
			else
			{
				saveHealthHud.hearts[i].sprite = saveHealthHud.halfHeart;
			}
		}

		for (int i = 0; i < Globals.MAX_PLAYER_HEALTH; i++)
		{
			if(i < data.gameData.maxHealth)
			{
				heartContainers[i].SetActive(true);
			}
			else
			{
				heartContainers[i].SetActive(false);
			}
		}

		// Show if the sword has been unlocked or not
		if (data.gameData.sword == true)
		{
			swordChip.color = new Color32(255,255,255,255);
		}
		else
		{
			swordChip.color = new Color32(255, 255, 255, 40);
		}

		// Show if the blaster has been unlocked or not
		if (data.gameData.blaster == true)
		{
			blasterChip.color = new Color32(255, 255, 255, 255);
		}
		else
		{
			blasterChip.color = new Color32(255, 255, 255, 40);
		}

		// Show if the shield has been unlocked or not
		if (data.gameData.shield == true)
		{
			shieldChip.color = new Color32(255, 255, 255, 255);
		}
		else
		{
			shieldChip.color = new Color32(255, 255, 255, 40);
		}

		// Show if the hammer has been unlocked or not
		if (data.gameData.hammer == true)
		{
			hammerChip.color = new Color32(255, 255, 255, 255);
		}
		else
		{
			hammerChip.color = new Color32(255, 255, 255, 40);
		}
	}

	///<summary> Call the save or load functions in the save manager </summary>
	public void SaveLoadGame(SavedGame savedGame)
	{
		if(btnText.text == "Save" && !File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
		{
			GameObject.FindObjectOfType<SaveManager>().NewGame(savedGame);
			btnText.text = "Load";
		}
		else
		{
			GameObject.FindObjectOfType<SaveManager>().Load(savedGame);
			Globals.currentSaveFile = savedGame.name;
			mainMenu.ContinueGame();
		}
	}

	///<summary> Hide visuals when a save has been deleted </summary>
	public void DeleteGame(SavedGame saveGame)
	{
		visuals.SetActive(false);
		GameObject.FindObjectOfType<SaveManager>().DeleteSave(saveGame);
		btnText.text = "Save";
	}
}