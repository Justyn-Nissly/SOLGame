using System.Collections;
using System.Collections.Generic;
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
		saveHealthHud;

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
		//visuals.SetActive(false);
	}

	public void ShowSaveData(SaveData data)
	{
		Color item; // The colour of the unlockable item

		// Set the visuals for the save slot to active
		visuals.SetActive(true);
		saveHealthHud.heartContainers.runTimeValue = data.gameData.maxHealth;

		/*for(int i = 0; i < 4; i++)
		{
			heartContainers[i].SetActive(true);
		}*/


		// Display the players current health on a save slot
		float tempHealth = data.gameData.currentHealth * 0.5f;
		for (int i = 0; i < Globals.MAX_PLAYER_HEALTH && i < saveHealthHud.heartContainers.runTimeValue; i++)
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

		// Show if the sword has been unlocked or not
		item = swordChip.color;
		if (data.gameData.sword == true)
		{
			item.a = 255.0f;
		}
		else
		{
			item.a = 40.0f;
		}

		// Show if the blaster has been unlocked or not
		item = blasterChip.color;
		if (data.gameData.blaster == true)
		{
			item.a = 255.0f;
		}
		else
		{
			item.a = 40.0f;
		}

		// Show if the shield has been unlocked or not
		item = shieldChip.color;
		if (data.gameData.shield == true)
		{
			item.a = 255.0f;
		}
		else
		{
			item.a = 40.0f;
		}

		// Show if the hammer has been unlocked or not
		item = hammerChip.color;
		if (data.gameData.hammer == true)
		{
			item.a = 255.0f;
		}
		else
		{
			item.a = 40.0f;
		}
	}
}