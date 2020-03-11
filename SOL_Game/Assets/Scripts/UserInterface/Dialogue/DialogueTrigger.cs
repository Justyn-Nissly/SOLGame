using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public Dialogue dialogue; // The dialogue for the nonplayable character
	public GameObject NPC;    // The NonPlayable Character that is currently speaking
	#endregion

	#region Private Variables
	public bool canActivate = true;
	#endregion
	
	// Unity Named Methods
	#region Main Methods
	/// Trigger the dialogue between the player and the NPC
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			if (canActivate == true)
			{
				DialogueManager.NPC = NPC;
				if (FindObjectOfType<Player>().playerAllowedToMove == true)
				{
					// Pause the enemy movement
					FindObjectOfType<Player>().playerAllowedToMove = false;
					FindObjectOfType<Player>().playerMovementAmount = Vector2.zero;
					FindObjectOfType<Player>().ApplyPlayerMovement();
				}
				NPC.GetComponent<Animator>().SetBool("IsActive", true);
				FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
			}
		}
		
	}
	#endregion

	#region Utility Methods

	#endregion

	#region Coroutines
	#endregion
}