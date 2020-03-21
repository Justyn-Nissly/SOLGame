using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	public bool
		spinObject = true;
	public FloatValue
		DamageToGive;
	#endregion

	#region Private Variables
	private float
		rotationAmount = 720; // degrees to rotate per second
	#endregion

	// Unity Named Methods
	#region Unity Main Methods
	/// <summary> constantly spin this game object by rotating it </summary>
	void FixedUpdate()
	{
		if (spinObject)
		{
			transform.Rotate(0, 0, rotationAmount * Time.deltaTime); //rotates 50 degrees per second around z axis
		}
		else
		{
			transform.rotation = new Quaternion(0, 0, 0, 0);
		}

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && DamageToGive != null)
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
		}
	}
	#endregion

	#region Utility Functions
	/// <summary> the method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage((int)DamageToGive.initialValue, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.maxHealth.runTimeValue);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
