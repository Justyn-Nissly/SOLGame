using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public Dialogue dialogue;     // The dialogue for the nonplayable character
	public Animator NPC; // The NonPlayable Character that is currently speaking
	#endregion

	#region Private Variables

	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		
	}
	/// Trigger the dialogue between the player and the NPC
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (NPC.CompareTag("Player"))
		{
			if (FindObjectOfType<Player>().playerAllowedToMove == true)
			{
				// Pause the enemy movement
				FindObjectOfType<Player>().playerAllowedToMove = false;
				FindObjectOfType<Player>().playerMovementAmount = Vector2.zero;
			}
			NPC.SetBool("IsActive", true);
			FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
		}
		
	}
	void OnTriggerExit2D(Collider2D collider)
	{
		NPC.SetBool("IsActive", false);
		NPC.enabled = false;
	}
	#endregion

	#region Utility Methods

	#endregion

	#region Coroutines

	#endregion
}
