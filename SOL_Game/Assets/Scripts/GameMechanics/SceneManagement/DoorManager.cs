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

	// Unity Named Methods
	#region Main Methods (Empty)
	#endregion

	#region Utility Methods
	/// <summary> unlocks all doors in the doors list </summary>
	public void UnlockAllDoors()
	{
		foreach (DoorLogic door in doors)
		{
			door.doorIsLocked = false;
			door.UpdateSprite(); // this changes the door sprite to the unlocked sprite
		}
	}

	/// <summary>
	/// locks all doors in the doors list
	/// </summary>
	public void LockAllDoors()
	{
		foreach (DoorLogic door in doors)
		{
			door.doorIsLocked = true;
			door.CloseDoor();
			door.UpdateSprite(); // this changes the door sprite to the unlocked sprite
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}