using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public playerMovement playerMovement;
	public Dialogue dialogue; // The dialogue for the nonplayable character
	public GameObject NPC;
	#endregion

	#region Private Variables

	#endregion

	// Unity Named Methods
	#region Main Methods
	void OnTriggerEnter2D(Collider2D NPC)
	{
		if (NPC.CompareTag("Player"))
		{
			FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
		}
		
	}
	#endregion

	#region Utility Methods

	#endregion

	#region Coroutines

	#endregion
}
