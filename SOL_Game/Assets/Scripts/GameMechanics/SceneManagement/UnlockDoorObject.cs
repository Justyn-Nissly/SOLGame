using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorObject : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Sprite unusedSprite, // the default sprite
					  usedSprite; // the sprite that is changed to when the player interacts with this object
	public DoorLogic connectedDoor; // the door connected to this object that is unlocked when this object is used
	public SpriteRenderer intractableSpriteRenderer; // this is the ! that is signals an intractable object
	#endregion

	#region Private Variables
	private SpriteRenderer objectRenderer; // a reference to this objects sprite renderer for changing the sprite later
	private DoorManager doorManager = new DoorManager();
	bool canUseObject = false, // for knowing if you are allowed to use this object at this time
		  playerHasUsedObject = false; // used to make sure the player cant use this object more than once
	#endregion

	// Unity Named Methods
	#region Main Methods

	private void Start()
	{
		// assign the right sprite to this object
		objectRenderer = GetComponent<SpriteRenderer>();
		objectRenderer.sprite = unusedSprite;

		// add the connected door to the door manager
		doorManager.doors.Add(connectedDoor);
	}

	private void Update()
	{
		if(playerHasUsedObject == false && canUseObject && (Input.GetKey(KeyCode.E) || CheckForAttackInput()))
		{
			UseObject();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// check if its the player next to this object
		if (collision.gameObject.CompareTag("Player"))
		{
			// enable the ability to use this object
			canUseObject = true;

			// enable the ! above this object
			intractableSpriteRenderer.enabled = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// check if its the player next to this object
		if (collision.gameObject.CompareTag("Player"))
		{
			// disable the ability to use this object
			canUseObject = false;

			// disable the ! above this object
			intractableSpriteRenderer.enabled = false;
		}
	}
	#endregion

	#region Utility Methods

	/// use this object to unlock connected door
	private void UseObject()
	{
		// set bool so that you cant use this any more
		playerHasUsedObject = true;

		// print debug message
		Debug.Log("you unlocked the door connected to this obj");

		// unlock connected door
		doorManager.UnlockAllDoors();

		// play sound effect (it will play whatever sound is in the audio source on this game object no sound will play if there is no audio source)
		AudioSource audioSource = GetComponent<AudioSource>();
		if(audioSource != null)
		{
			audioSource.Play();
		}

		// update the sprite being used on this object
		objectRenderer.sprite = usedSprite;

		// disable the ! above this object
		intractableSpriteRenderer.enabled = false;
	}

	private bool CheckForAttackInput()
	{
		return Input.GetButton("B") || Input.GetButton("X") || Input.GetButton("A") || Input.GetButton("Y");
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}