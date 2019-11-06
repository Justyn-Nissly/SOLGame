using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Signal : ScriptableObject
{

    // List of everything that is listening to the signal
	public List<SignalListener> listeners = new List<SignalListener>();

    // Raises a signal
    public void Raise()
	{
        // Making sure there is no out of range exception by checking the list backwards to make sure nothing has been removed
        for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnSignalRaised();
		}
	}

    // Adds a particular listener to our list of listeners
    // Register
    public void RegisterListener(SignalListener listener)
	{
		listeners.Add(listener);
	}

    // Removes a particular listen from our list of listeners
    // DeRegister
    public void DeRegisterListener(SignalListener listener)
	{
		listeners.Remove(listener);
	}

}
