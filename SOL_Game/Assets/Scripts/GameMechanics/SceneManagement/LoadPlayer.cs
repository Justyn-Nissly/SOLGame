using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script on start will check if there is a player and move it to the starting position if there is one
/// if it doesn't find a player it will instantiate the player prefab in the starting position
/// </summary>
public class LoadPlayer : MonoBehaviour
{
	public Transform defaultPlayerStartingPosition;
	public Transform altStartingPosition;

	public GameObject playerPrefab; // the player prefab, it will be instantiated if there is no player in the scene already

	private Transform startingPosition;

	private void Awake()
	{
		GameObject playerInScene = GameObject.FindGameObjectWithTag("Player");

		// pick the right starting position
		if (GlobalVarablesAndMethods.startInBeginingPosition == false && altStartingPosition != null)
		{
			startingPosition = altStartingPosition;
		}
		else
		{
			startingPosition = defaultPlayerStartingPosition;
		}


		if (playerInScene != null)
		{
			// just move the player scene its in the scene
			playerInScene.transform.position = startingPosition.position;
		}
		else
		{
			// instantiate the player prefab because there is no player in this scene
			Instantiate(playerPrefab, startingPosition.position, playerPrefab.transform.rotation);
		}
	}
}
