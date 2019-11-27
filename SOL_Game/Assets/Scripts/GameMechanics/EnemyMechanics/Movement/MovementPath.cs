using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MovementPath : MonoBehaviour
{
	#region Enums
	public enum PathTypes //Types of movement paths
	{
		linear,
		loop
	}
	#endregion

	#region Public Variables
	public PathTypes
		pathType; // The path is either linear or looping
	public int
		movingTo = 0; // Point to move to
	public Transform[]
		pathSequence; // All points in the path
	#endregion

	#region Private Variables (Empty)
	#endregion

	// (Unity Named Methods)
	#region Main Methods
	/// <summary>  Draw the path the enemy follows </summary>
	public void OnDrawGizmos()
	{
		// Check if the path exists and has at least two points in it
		if (pathSequence == null || pathSequence.Length < 2)
		{
			return;
		}

		// Draw a line between each point and the one after
		for (var point = 1; point < pathSequence.Length; point++)
		{
			Gizmos.DrawLine(pathSequence[point - 1].position, pathSequence[i].position);
		}

		// Make the last point in a looping path return to the first point
		if (pathType == PathTypes.loop)
		{
			Gizmos.DrawLine(pathSequence[0].position, pathSequence[pathSequence.Length - 1].position);
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines
	/// <summary>  Get the next path point </summary>
	public IEnumerator<Transform> GetNextPathPoint()
	{
		// Exit if the path doesn't exist
		if (pathSequence == null || pathSequence.Length < 1)
		{
			yield break;
		}

		// Make the enemy continually follow its set path
		while (true)
		{
			// Get current path point and wait for next enumerator call
			yield return pathSequence[movingTo];

			// If there is only one point exit the coroutine
			if (pathSequence.Length == 1)
			{
				continue;
			}

			// A linear path goes start to end then back
			if (pathType == PathTypes.linear)
			{
				// At path beginning move forward and at end path move backward
				movingTo += (movingTo > 0 || movingTo < pathSequence.Length - 1) ? 1 : -1;
			}

			// A looping returns from the last point directly to the first point
			if (pathType == PathTypes.loop)
			{
				// Moving forward past path end (to path beginning)
				if (movingTo >= pathSequence.Length)
				{
					movingTo = 0;
				}
				// Moving backward past path beginning (to path end)
				if (movingTo < 0)
				{
					movingTo = pathSequence.Length - 1;
				}
			}
		}
	}
	#endregion
}