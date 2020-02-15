using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorObject : MonoBehaviour
{
	#region Enums
	public enum SwitchType
	{
		doorSwitch,         // A switch for opening a door
		spawnerSwitch,      // A switch is for spawning enemies
		pressurePlateSwitch // A switch for opening a hatch over a pressure plate
	}
	#endregion

	#region Public Variables
	public SwitchType
		switchType; // The type of switch that is being used
	public Sprite
		unusedSprite, // The default sprite
		usedSprite;   // The sprite that is changed to when the player interacts with this object
	public DoorLogic
		connectedDoor; // The door connected to this object that is unlocked when this object is used
	public DoorManager
		allDoors = new DoorManager(); // The door manager to unlock several doors at once
	public EnemySpawner
		spawner; // The enemies to spawn
	public PuzzleLogic
		pressurePlate; // The pressure plate connected to this object that is unlocked when this object is used
	public SpriteRenderer
		intractableSpriteRenderer; // This is the ! that is signals an intractable object

	[Header("For use with pressure plates")]
	[Tooltip("Set the lever to power the pressure plate")]
	public bool isPowerSwitch;   // Check to see if the switch is for enabling power

	[Tooltip("Set the lever to unlock the pressure plate")]
	public bool isUnlockSwitch;  // Check to see if the switch if for unlocking a door

	#endregion

	#region Private Variables
	private SpriteRenderer
		objectRenderer; // A reference to this objects sprite renderer for changing the sprite later
	private DoorManager
		doorManager = new DoorManager();
	private bool
		canUseObject        = false, // For knowing if you are allowed to use this object at this time
		playerHasUsedObject = false, // Used to make sure the player cant use this object more than once
		isSwitchFlipped     = false; // The state of the switch
	#endregion

	// Unity Named Methods
	#region Main Methods

	private void Start()
	{
		// Assign the right sprite to this object
		objectRenderer = GetComponent<SpriteRenderer>();
		objectRenderer.sprite = unusedSprite;

		// Add the connected door to the door manager
		if(connectedDoor != null)
		{
			doorManager.doors.Add(connectedDoor);
		}
	}

	private void Update()
	{
		// Check to see if the switch has been flipped
		if(playerHasUsedObject == false && canUseObject && (Input.GetKeyDown(KeyCode.E) || CheckForAttackInput()))
		{
			if (switchType == SwitchType.doorSwitch)
			{
				UseObject();
			}
			else if (switchType == SwitchType.spawnerSwitch && isSwitchFlipped == false)
			{
				SpawnEnemies();
			}
			else
			{
				OpenPressurePlate();
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

		// Unlock all doors
		if(allDoors != null)
		{
			allDoors.UnlockDoors();
		}
		else
		{
			// Unlock connected door
			doorManager.UnlockDoors();
		}

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

	///<summary> Spawn in enemies if the switch is flipped </summary>
	private void SpawnEnemies()
	{
		// Set the switch's state to on
		isSwitchFlipped = true;

		// Start spawning in enemies
		spawner.StartCoroutine(spawner.SpawnInEnemies());

		// Lock any doors
		doorManager.LockDoors();

		// Start checking if the enemies have been defeated(for unlocking doors so dont check this if there are no doors)
		if (doorManager.doors.Count > 0)
		{
			spawner.StartCheckingIfEnemiesDefeated();  //1s delay, repeat every .5s
		}
	}

	///<summary> Open or power the pressure plate when the lever is flipped </summary>
	private void OpenPressurePlate()
	{

		if (isPowerSwitch)
		{
			pressurePlate.UpdateDoorState();
			pressurePlate.isPowered = true;
		}
		else if (isUnlockSwitch)
		{
			pressurePlate.UpdateDoorState();
			pressurePlate.isLocked = false;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}