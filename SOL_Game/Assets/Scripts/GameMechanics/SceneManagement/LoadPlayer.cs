using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayer : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Transform defaultPlayerStartingPosition;
	public Transform altStartingPosition;

	public GameObject playerPrefab; // the player prefab, it will be instantiated if there is no player in the scene already
	#endregion

	#region Private/Protected Variables
	protected Transform
		startingPosition;
	protected GameObject
		playerInScene;
	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary> Ensure the player is at the starting position </summary>
	private void Awake()
	{
		SetStartingPostion();
		PlacePlayer();
	}
	#endregion

	#region Utility Methods
	///<summary> Set the starting position </summary>
	public virtual void SetStartingPostion()
	{
		playerInScene = GameObject.FindGameObjectWithTag("Player");

		// Assign the starting position
		if (GlobalVarablesAndMethods.startInBeginingPosition == false && altStartingPosition != null)
		{
			startingPosition = altStartingPosition;
		}
		else
		{
			startingPosition = defaultPlayerStartingPosition;
		}
	}

	/// <summary> Instantiate the player if not already present at the starting position </summary>
	public virtual void PlacePlayer()
	{
		// Place the player in the starting position
		if (playerInScene != null)
		{
			playerInScene.transform.position = startingPosition.position;
		}
		// Instantiate the player in the starting position
		else
		{
			Instantiate(playerPrefab, startingPosition.position, playerPrefab.transform.rotation);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}