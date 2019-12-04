using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public variables
	public Signal
		signal;
    public UnityEvent
		signalEvent;
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Raise a signal </summary>
	public void OnSignalRaised()
	{
		signalEvent.Invoke();
	}

	/// <summary> Enable a signal listener </summary>
	private void OnEnable()
	{
		signal.RegisterListener(this);
	}

	/// <summary> Disable a signal listener </summary>
	private void OnDisable()
	{
		signal.DeRegisterListener(this);
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines (Empty)
	#endregion
}