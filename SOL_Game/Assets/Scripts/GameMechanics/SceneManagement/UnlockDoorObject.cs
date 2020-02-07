using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorObject : MonoBehaviour
{
	#region Enums
	public enum DoorType
	{
		door,         // A door
		preasurePlate // A hatch over a preasure plate
	}
	#endregion

	#region Public Variables
	public DoorType
		doorType; // The type of door that is being unlocked
	public Sprite
		unusedSprite, // the default sprite
		usedSprite;   // the sprite that is changed to when the player interacts with this object
	public DoorLogic
		connectedDoor; // the door connected to this object that is unlocked when this object is used
	public PuzzleLogic
		preasurePlate; // The preasure plate connected to this object that is unlocked when this object is used
	public SpriteRenderer
		intractableSpriteRenderer; // this is the ! that is signals an intractable object
	public bool
		isDoor,         // A flag checking if the the item being activated is a door or a preasure plate
		isPowerSwitch,  // Check to see if the switch is for enabling power
		isUnlockSwitch; // Check to see if the switch if for unlocking a door

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
		// Check to see if the switch has been flipped
		if(playerHasUsedObject == false && canUseObject && (Input.GetKey(KeyCode.E) || CheckForAttackInput()))
		{
			if (doorType == DoorType.door)
			{
				UseObject();
			}
			else
			{
				OpenPreasurePlate();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Check if its the player next to this object
		if (collision.gameObject.CompareTag("Player"))
		{
			// Enable the ability to use this object
			canUseObject = true;

			// Enable the ! above this object
			intractableSpriteRenderer.enabled = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// Check if its the player next to this object
		if (collision.gameObject.CompareTag("Player"))
		{
			// Disable the ability to use this object
			canUseObject = false;

			// Disable the ! above this object
			intractableSpriteRenderer.enabled = false;
		}
	}
	#endregion

	#region Utility Methods

	/// <summary> Unlock connected door </summary>
	private void UseObject()
	{
		// Set bool so that you cant use this any more
		playerHasUsedObject = true;

		// Print debug message
		Debug.Log("you unlocked the door connected to this obj");

		// Unlock connected door
		doorManager.UnlockAllDoors();

		// Play sound effect (it will play whatever sound is in the audio source on this game object no sound will play if there is no audio source)
		AudioSource audioSource = GetComponent<AudioSource>();
		if(audioSource != null)
		{
			audioSource.Play();
		}

		// Update the sprite being used on this object
		objectRenderer.sprite = usedSprite;

		// Disable the ! above this object
		intractableSpriteRenderer.enabled = false;
	}

	/// <summary> Check if the player has pressed an attack button to flip the switch </summary>
	private bool CheckForAttackInput()
	{
		return Input.GetButton("B") || Input.GetButton("X") || Input.GetButton("A") || Input.GetButton("Y");
	}


	private void OpenPreasurePlate()
	{

		if (isPowerSwitch)
		{
			preasurePlate.UpdateDoorState();
			preasurePlate.isPowered = true;
		}
		else if (isUnlockSwitch)
		{
			preasurePlate.UpdateDoorState();
			preasurePlate.isLocked = false;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
