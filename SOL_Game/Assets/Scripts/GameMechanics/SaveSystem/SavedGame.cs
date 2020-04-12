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
		visuals.SetActive(false);
	}



	public void ShowSaveData(SaveData data)
	{
		visuals.SetActive(true);
		saveHealthHud.UpdateHearts();
	}
}