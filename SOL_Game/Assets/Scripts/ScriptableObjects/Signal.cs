using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : ScriptableObject
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public List<SignalListener>
		listeners = new List<SignalListener>(); // All objects listening to the signal
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary> Raise a signal </summary>
	public void Raise()
	{
        // Prevent incorrect memory access outside the list bounds
        for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnSignalRaised();
		}
	}

	///<summary> Add a listener to the list </summary>
	public void RegisterListener(SignalListener listener)
	{
		listeners.Add(listener);
	}

	///<summary> Remove a listener from the list </summary>
	public void DeRegisterListener(SignalListener listener)
	{
		listeners.Remove(listener);
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}