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

	public void ChangeNumberOfHearts()
	{
		for(int i = 0; i < heartContainers.runTimeValue; i++)
		{
			hearts[i].gameObject.SetActive(true);
			hearts[i].sprite = fullHeart;
		}

		playerCurrentHealth.runTimeValue = heartContainers.runTimeValue * 2;
		playerCurrentHealth.initialValue = heartContainers.runTimeValue * 2;
	}
	#endregion

	#region Coroutines

	#endregion
}