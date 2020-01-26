using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActionItem : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public string
		destroyTag; // The tag of the object(s) the player must destroy
	#endregion

	#region Private Variables
	public bool
		objectiveComplete = false;
	#endregion

	// Unity Named Methods
	#region Main Methods (Empty)
	/// <summary> Check if all objects to be destroyed are destroyed </summary>
	void FixedUpdate()
	{
		if (GameObject.FindWithTag(destroyTag) == null && objectiveComplete == false)
		{
			Debug.Log("Objective complete!");
			objectiveComplete = true;
		}
	}

/*	public bool IsTouching(Collider2D collider, ContactFilter2D contactFilter)
	{
		return (collider.Tag != destroyTag);
	}*/
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}