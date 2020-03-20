using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefeat : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	// player movement variables
	public Player
		player;
	#endregion

	#region Private Variables (Empty)
	private SpriteRenderer
		sprite;
	private bool
		defeated;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void Start()
	{
		sprite   = GetComponent <SpriteRenderer>();
		defeated = false;
	}

	private void FixedUpdate()
	{
		// uncommenting this debug print statement will kill performance
		//Debug.Log("Player HP == " + player.currentHealth);
		defeated     = (player.currentHealth <= 0.0f || defeated);
		sprite.color = (defeated) ? Color.black : sprite.color;
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}