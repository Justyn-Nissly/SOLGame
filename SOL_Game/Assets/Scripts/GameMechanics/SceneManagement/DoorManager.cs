using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public List<DoorLogic> doors = new List<DoorLogic>(); // A list of doors in the scene
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods (Empty)
	#endregion

	#region Utility Methods
	/// <summary> Unlocks all doors in a list </summary>
	public void UnlockAllDoors()
	{
		foreach (DoorLogic door in doors)
		{
			door.doorIsLocked = false;

			// Change the door sprite to unlocked
			door.UpdateSprite();
		}
	}

	/// <summary> Locks all doors in a list </summary>
	public void LockAllDoors()
	{
		foreach (DoorLogic door in doors)
		{
			door.doorIsLocked = true;
			door.CloseDoor();

			// Change the door sprite to locked
			door.UpdateSprite();
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}