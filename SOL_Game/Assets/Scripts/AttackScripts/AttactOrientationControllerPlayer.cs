using UnityEngine;

public class AttactOrientationControllerPlayer : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public GameObject objectToRotate;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	public void FixedUpdate()
	{
		// don't rotate if the freeze rotation button is down
		if (Input.GetAxis("FreezeRotation") == 0)
		{
			// If the player clicks the left key set rotation to -90 degrees
			if (Input.GetAxis("Horizontal") < 0)
			{
				objectToRotate.transform.rotation = Quaternion.Euler(0, 0, -90f);
			}
			// If the player clicks the right key set rotation to 90 degrees
			else if (Input.GetAxis("Horizontal") > 0)
			{
				objectToRotate.transform.rotation = Quaternion.Euler(0, 0, 90f);
			}
			// If the player clicks the up key set rotation to 180 degrees
			else if (Input.GetAxis("Vertical") > 0)
			{
				objectToRotate.transform.rotation = Quaternion.Euler(0, 0, 180f);
			}
			// If the player clicks the down key set rotation to zero degrees
			else if (Input.GetAxis("Vertical") < 0)
			{
				objectToRotate.transform.rotation = Quaternion.Euler(0, 0, 0f);
			}
		}
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion

}
