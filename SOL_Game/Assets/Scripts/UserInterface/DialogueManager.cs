using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public Text nameText;
	public Text dialogueText;
	public Animator animator;
	public Queue<string> sentences; // All sentences for the characters dialogue
	#endregion

	#region Private Variables

	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		sentences = new Queue<string>();
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

	public void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
		Debug.Log("End of Conversation.");
	}
	#endregion

	#region Coroutines
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
