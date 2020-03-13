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
		cameraIsPanning = false, // so you can pan the camera to a game object without it fighting the normal camera movement
		cameraIsShaking = false; // this if a flag for is the camera is shaking (an effect used when shooting)

	private float
		dampingSpeed = 1.0f; // A measure of how quickly the shake effect should evaporate

	// The initial position of the GameObject
	Vector3 initialPosition;

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
		if (transform.position != target.position && cameraIsPanning == false && cameraIsShaking == false)
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

	#region Coroutines
	/// <summary> This method shakes the camera for N seconds </summary>
	public IEnumerator ShakeCamera(float shakeMagnitude, float shakeDuration)
	{
		cameraIsShaking = true;
		initialPosition = transform.position;

		while (shakeDuration > 0)
		{
			transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

			shakeDuration -= Time.deltaTime * dampingSpeed;

			yield return null;
		}

			shakeDuration = 0f;
			transform.localPosition = initialPosition;
			cameraIsShaking = false;
	}

	/// <summary> Moves a game object to a location over N seconds </summary>
	public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float timeToLocation, float pauseTime, float timeBackToPlayer)
	{
		float elapsedTime = 0;
		Vector3 startingPos = objectToMove.transform.position;

		// freeze player movement, this does freeze player movement bit freezes in the running animation NEED TO FIX
		Player player = FindObjectOfType<Player>();
		if(player != null)
		{
			player.FreezePlayer();
		}

		// move the camera to the location
		while (elapsedTime < timeToLocation)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / timeToLocation));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		objectToMove.transform.position = end;

		// pause the camera at the end location for N seconds
		yield return new WaitForSeconds(pauseTime);

		elapsedTime = 0;
		startingPos = objectToMove.transform.position;

		// move the camera back to the player
		while (elapsedTime < timeBackToPlayer)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, new Vector3(target.position.x, target.position.y, transform.position.z), (elapsedTime / timeBackToPlayer));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		objectToMove.transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

		// unfreeze player movement
		if (player != null)
		{
			player.UnFreezePlayer();
		}

		cameraIsPanning = false;
	}
	#endregion
}