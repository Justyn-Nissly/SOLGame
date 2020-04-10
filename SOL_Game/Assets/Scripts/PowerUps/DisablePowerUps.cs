using System.Collections;
using UnityEngine;

public class DisablePowerUps : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	private Player
		player;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the power up type </summary>
	void Awake()
	{
		player = FindObjectOfType<Player>();
		player.canPowerUp = player.usingPowerUp = false;
	}

	void EarlyFixedUpdate()
	{
		player.canPowerUp = player.usingPowerUp = false;
	}

	void FixedUpdate()
	{
		player.canPowerUp = player.usingPowerUp = false;
	}

	void LateFixedUpdate()
	{
		player.canPowerUp = player.usingPowerUp = false;
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}