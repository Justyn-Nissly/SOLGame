using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public Image[] hearts;
	public Sprite fullHeart;
	public Sprite halfHeart;
	public Sprite emptyHeart;
	public FloatValue heartContainers;
	public FloatValue playerCurrentHealth;

	// these game objects change the background of the health bar based on how many hearts the player has
	public GameObject 
		fourHearts,
		fiveHearts,
		SixHearts,
		SevenHearts,
		EightHearts;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Start is called before the first frame update
	private void Start()
	{
		for (int i = 0; i < heartContainers.initialValue; i++)
		{
			hearts[i].gameObject.SetActive(true);
			hearts[i].sprite = fullHeart;
		}
	}

	#endregion

	#region Utility Methods
	/// <summary> update the health HUD to the players current health</summary>
	public void UpdateHearts()
	{
		float tempHealth = playerCurrentHealth.runTimeValue / 2;
		for (int i = 0; i < heartContainers.runTimeValue; i++)
		{
			if (i <= tempHealth - 1)
			{
				hearts[i].sprite = fullHeart;
			}
			else if (i >= tempHealth)
			{
				hearts[i].sprite = emptyHeart;
			}
			else
			{
				hearts[i].sprite = halfHeart;
			}
		}
	}

	/// <summary> changed the number of hearts the players health bar has</summary>
	public void ChangeNumberOfHearts()
	{
		// increase the players hearts
		for (int i = 0; i < heartContainers.runTimeValue; i++)
		{
			hearts[i].gameObject.SetActive(true);
			hearts[i].sprite = fullHeart;
		}

		// set the health bar background to match the number of hearts
		SetBackgroundHealthBar();

		// make the health bar match the current health amount
		UpdateHearts();
	}

	private void SetBackgroundHealthBar()
	{
		switch (heartContainers.runTimeValue)
		{
			case 8:
				EightHearts.SetActive(true);
				break;
			case 7:
				SevenHearts.SetActive(true);
				break;
			case 6:
				SixHearts.SetActive(true);
				break;
			case 5:
				fiveHearts.SetActive(true);
				break;
			case 4:
				fourHearts.SetActive(true);
				break;
		}
	}
	#endregion

	#region Coroutines

	#endregion
}