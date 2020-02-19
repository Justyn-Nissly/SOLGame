﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public static GameObject
		NPC;          // A reference to the NPC
	public Animator
		animator;     // The animator controler to make the dialogue box appear
	public Text
		dialogueText, // The text that the NPC is currently speaking
	    nameText;     // The name of the NPC currently Speaking
	public Queue<string> 
		sentences;    // All sentences for the characters dialogue
	public float
		textSpeed,    // The speed at which the text will be typed on the screen
		timer;        // The timer to remove the NPC from the scene
	#endregion

	#region Private Variables
	private GameObject playerMovement; // A reference to the player
	#endregion
	public int
		currentSentence; // The sentence that is currently being typed
	// Unity Named Methods
	#region Main Methods
	///<summary> Initialize the sentence queue for the NPC, find the player, and get the animator for the NPC</summary>
	void Start()
	{
		NPC = GameObject.FindGameObjectWithTag("LoadStar");
		sentences      = new Queue<string>();
		playerMovement = GameObject.FindGameObjectWithTag("Player");
		animator       = GameObject.FindObjectOfType<DialogueManager>().GetComponentInChildren<Animator>();
		currentSentence = 0;
	}

	///<summary> Check if the player has pressed the return key to move to the next sentence </summary>
	void Update()
	{
		/*Debug.Log(NPC.GetComponent<DialogueTrigger>().dialogue.sentences[currentSentence]);*/
		if (Input.GetKeyDown(KeyCode.Return) && animator.GetBool("IsOpen") == true)
		{
			
			if (dialogueText.text != NPC.GetComponent<DialogueTrigger>().dialogue.sentences[currentSentence])
			{
				StopAllCoroutines();
				if (currentSentence < NPC.GetComponent<DialogueTrigger>().dialogue.sentences.Length)
				{
					dialogueText.text = NPC.GetComponent<DialogueTrigger>().dialogue.sentences[currentSentence];
				}
			}
			else
			{
				if (currentSentence < NPC.GetComponent<DialogueTrigger>().dialogue.sentences.Length)
				{
					currentSentence += 1;
					Debug.Log(currentSentence);
				}
				DisplayNextSentence();
			}
		}
	}
	#endregion

	#region Utility Methods
	/// Start the dialogue with the player
	public void StartDialogue (Dialogue dialogue)
	{
		animator.SetBool("IsOpen", true);
		nameText.text = dialogue.name;

		sentences.Clear();

		foreach(string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		DisplayNextSentence();
	}

	/// Display the next sentence the nonplayable character will speak
	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	/// End the dialogue with the player and close the dialogue box
	public void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
		NPC.GetComponent<Animator>().SetBool("IsActive", false);
		FindObjectOfType<Player>().playerAllowedToMove = true;
		StartCoroutine(TeleportLoadstar());
		NPC.GetComponent<DialogueTrigger>().canActivate = false;
		Debug.Log("End of Conversation.");
		currentSentence = 0;
	}
	#endregion

	#region Coroutines
	/// Type each letter of a sentence into the dialogue box one at a time
	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			yield return new WaitForSeconds(textSpeed);
			dialogueText.text += letter;
			yield return null;
		}
	}
	IEnumerator TeleportLoadstar()
	{
		timer = 100;
		yield return new WaitForSeconds(timer);
	}
	#endregion
}