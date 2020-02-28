using UnityEngine;

public class HammerGuardianWeakness : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public int
		health; // Only the weak point can take damage
	public Sprite
		destroyedSprite; // Sprite displayed when weak point takes sufficient damage
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Check if the player has hit the guardian </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// If the right weapon hit the weak point deal the guardian damage
		if (collision.gameObject.CompareTag("PlayerLightWeapon"))
		{
			if (health-- < 0)
			{
				DestroyObject();
			}
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Change sprite to destroyed if applicable </summary>
	private void DestroyObject()
	{
		if (destroyedSprite != null)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}