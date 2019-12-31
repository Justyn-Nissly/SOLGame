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
	public float playerCurrentHealth;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Start is called before the first frame update
	void Start()
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
		playerCurrentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentHealth;
		float tempHealth = playerCurrentHealth / 2;
		for (int i = 0; i < heartContainers.initialValue; i++)
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
	#endregion

	#region Coroutines

	#endregion
}