using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> this script is for destroying a game object from an event in a animation</summary>
public class DestroyThisObject : MonoBehaviour
{
	/// <summary> call this method to destroy this game object</summary>
	public void DestroyThisGameObject()
	{
		Destroy(gameObject);
	}
}
