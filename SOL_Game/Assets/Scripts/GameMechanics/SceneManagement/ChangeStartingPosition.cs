using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStartingPosition : MonoBehaviour
{
	// Empty
	#region Enums
	#endregion

	// Empty
	#region Public Variables
	public LoadPlayer.Facility
		thisfacility;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			// change the players starting position
			SetCheckPointFlag();
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> sets if the player should start at the checkpoint in the current facility</summary>
	public void SetCheckPointFlag()
	{
		switch (thisfacility)
		{
			case LoadPlayer.Facility.Hub:
				// change the players starting position
				Globals.hubCheckPointReached = true;
				break;
			case LoadPlayer.Facility.BioLab:
				Globals.biolabCheckPointReached = true;
				break;
			case LoadPlayer.Facility.Atlantis:
				Globals.atlantisCheckPointReached = true;
				break;
			case LoadPlayer.Facility.Factory:
				Globals.factoryCheckPointReached = true;
				break;
			case LoadPlayer.Facility.Geothermal:
				 Globals.geothermalCheckPointReached = true;
				break;
			case LoadPlayer.Facility.SpaceBase:
				Globals.spacebaseCheckPointReached = true;
				break;
		}
	}
	#endregion

	// Empty
	#region Coroutines
	#endregion
}
