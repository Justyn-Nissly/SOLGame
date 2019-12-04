using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	#region Enums(Empty)
	#endregion

	#region Public Variables
	/*public bool isInteractable;*/ // This variable is here if we need to have certian things be interactable but locked to be unlocked later
	public GameObject interactableIndicator; // The characters indicater that an item is interactable
	#endregion

	#region Private Variables(Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary>If the player is close enought to the interactable object, display an indicator that the object is interactable</summary>
	public void OnTriggerEnter2D(Collider2D player)
	{
		if (player.CompareTag("Player"))
		{
			interactableIndicator.SetActive(true);
		}
	}
	///<sumary>If the player moves away from the interactable object, disable the indicator that the object is interactable</sumary>
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