using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	private Transform
		target; // Center the camera on the player
	private float
		smoothing = 0.05f; // Make camera movement smooth
	private bool
		cameraIsPanning = false; // so you can pan the camera to a game object without it fighting the normal camera movement
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Assign the camera to follow the player </summary>
	private void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	/// <summary> Make the camera follow the player at the end of a frame </summary>
	private void LateUpdate()
	{
		// The camera doesn't move if already centered on the player
		if (transform.position != target.position && cameraIsPanning == false)
		{
			transform.position = Vector3.Lerp(transform.position,
											  new Vector3(target.position.x, target.position.y, transform.position.z),
											  smoothing);
		}
	}
	#endregion

	#region Utility Methods
	public void PanCameraToLocation(GameObject location, float timeToLocation, float pauseTime, float timeBackToPlayer)
	{
		cameraIsPanning = true;
		StartCoroutine(MoveOverSeconds(gameObject, new Vector3(location.transform.position.x, location.transform.position.y, transform.position.z), timeToLocation, pauseTime, timeBackToPlayer));
	}
	#endregion

	#region Coroutines (Empty)
	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float timeToLocation, float pauseTime, float timeBackToPlayer)
	{
		float elapsedTime = 0;
		Vector3 startingPos = objectToMove.transform.position;

		while (elapsedTime < timeToLocation)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / timeToLocation));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		objectToMove.transform.position = end;

		yield return new WaitForSeconds(pauseTime);


		elapsedTime = 0;
		startingPos = objectToMove.transform.position;

		while (elapsedTime < timeBackToPlayer)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, new Vector3(target.position.x, target.position.y, transform.position.z), (elapsedTime / timeBackToPlayer));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		objectToMove.transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

		cameraIsPanning = false;
	}
	#endregion
}