using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject virtualCamera;
	#endregion

	#region Private/Protected Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        // If the player enters the room activaates the vCam
        if(other.CompareTag("Player") && !other.isTrigger)
        {
			virtualCamera.SetActive(true);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
		// If the player exits the room deactives the vCam
		if (other.CompareTag("Player") && !other.isTrigger)
		{
			virtualCamera.SetActive(false);
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}
