using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	#region Enums(Empty)
	#endregion

	#region Public Variables
	public GameObject interactableIndicator; // Keep interactable objects locked until being unlocked
	#endregion

	#region Private Variables(Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary> On player approach indicate the object is interactable </summary>
	public void OnTriggerEnter2D(Collider2D player)
	{
		if (player.CompareTag("Player"))
		{
			interactableIndicator.SetActive(true);
		}
	}

	///<summary> When the player leaves disable the interactable indicator </summary>
	public void OnTriggerExit2D(Collider2D player)
	{
		if (player.CompareTag("Player"))
		{
			interactableIndicator.SetActive(false);
		}
	}
	#endregion

	#region Utility Methods(Empty)
	#endregion

	#region Coroutines(Empty)
	#endregion
}