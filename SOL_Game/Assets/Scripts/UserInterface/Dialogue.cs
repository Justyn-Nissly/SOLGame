using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue

{
	#region Enums

	#endregion

	#region Public Variables
	public string name;        // The name of teh nonplayable character

	[TextArea(3,10)]
	public string[] sentences; // The sentences the nonplayable character will speak
	#endregion

	#region Private Variables

	#endregion

	// Unity Named Methods
	#region Main Methods

	#endregion

	#region Utility Methods

	#endregion

	#region Coroutines
	#endregion
}
