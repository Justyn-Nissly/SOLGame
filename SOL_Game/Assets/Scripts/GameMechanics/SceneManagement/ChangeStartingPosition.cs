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
	public int CheckPointIndex;
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
				Globals.hubCheckPointReached = CheckPointIndex;
				break;
			case LoadPlayer.Facility.BioLab:
				Globals.biolabCheckPointReached = CheckPointIndex;
				break;
			case LoadPlayer.Facility.Atlantis:
				Globals.atlantisCheckPointReached = CheckPointIndex;
				break;
			case LoadPlayer.Facility.FactoryLevel1:
				Globals.factoryCheckPointReached = CheckPointIndex;
				break;
			case LoadPlayer.Facility.FactoryLevel2:
				Globals.factoryLevel2CheckPointReached = CheckPointIndex;
				break;
			case LoadPlayer.Facility.GeothermalLevel1:
				 Globals.geothermalCheckPointReached = CheckPointIndex;
				break;
			case LoadPlayer.Facility.GeothermalLevel2:
				Globals.geothermalLevel2CheckPointReached = CheckPointIndex;
				break;
			case LoadPlayer.Facility.SpaceBaseLevel1:
				Globals.spacebaseCheckPointReached = CheckPointIndex;
				break;
			case LoadPlayer.Facility.SpaceBaseLevel2:
				Globals.spacebaseLevel2CheckPointReached = CheckPointIndex;
				break;
			case LoadPlayer.Facility.SpaceBaseLevel3:
				Globals.spacebaseLevel3CheckPointReached = CheckPointIndex;
				break;
		}
	}
	#endregion

	// Empty
	#region Coroutines
	#endregion
}
