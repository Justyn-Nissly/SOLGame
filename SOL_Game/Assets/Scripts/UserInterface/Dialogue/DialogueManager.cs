using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public Animator      animator;     // The animator controler to make the dialogue box appear
	public Text          dialogueText; // The text that the NPC is currently speaking
	public Text          nameText;     // The name of the NPC currently Speaking
	public Queue<string> sentences;    // All sentences for the characters dialogue
	#endregion

	#region Private Variables
	private Player playerMovement;
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		sentences = new Queue<string>();
		playerMovement = FindObjectOfType<Player>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			DisplayNextSentence();
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
		playerMovement.playerAllowedToMove = true;
		Debug.Log("End of Conversation.");
	}
	#endregion

	#region Coroutines
	/// Type each letter of a sentence into the dialogue box one at a time
	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}
	#endregion
}
