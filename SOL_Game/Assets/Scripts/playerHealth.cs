using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//****************************************************//
//                                                    //
// Don't forget to comment tasks so we know what they //
// do and comment any variables that need commented   //
//                                                    //
//****************************************************//
public class PlayerHealth : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	// Variables used for helath
	public int 
		health,
	    maxHealth,
	    numOfhearts;

	// Image-sprites used for index containers
	public Image[] hearts;
	public Sprite
		fullindex,
		halfindex,
		emptyindex;
	public int 
		heartsLost;
	#endregion

	#region Private Variables
	private int 
		index;
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		index = numOfhearts - 1;
		heartsLost = 0;
	}
	void Update()
	{
		if (index >= 1)
		{
			if (heartsLost == 1)
			{
				hearts[index].sprite = halfindex;
			}
			else if (heartsLost == 2)
			{
				hearts[index].sprite = emptyindex;
				heartsLost = 0;
				index -= 1;
			}
			else
			{
				hearts[index].sprite = fullindex;
			}
			/*for (int i = 0; i < hearts.Length; i++)
			{
				if (i < health && i % 2 == 0)
				{
					hearts[i].sprite = fullindex;
				}
				else if (i < health && i % 2 == 1)
				{
					hearts[i].sprite = halfindex;
				}
				else
				{
					hearts[i].sprite = emptyindex;
				}
			}
			*/
		}
		else if (index < 1)
		{
			if (heartsLost == 1)
			{
				hearts[index].sprite = halfindex;
			}
			else if (heartsLost == 2)
			{
				hearts[index].sprite = emptyindex;
			}
			else
			{
				hearts[index].sprite = fullindex;
			}
		}

		if (index < numOfhearts)
		{
			hearts[index].enabled = true;
		}
		else
		{
			hearts[index].enabled = false;
		}
	}
	#endregion

	#region Utility Methods

	#endregion

	#region Coroutines

	#endregion
}