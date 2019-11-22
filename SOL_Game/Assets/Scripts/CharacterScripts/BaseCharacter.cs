using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public FloatValue
		currentHealth, // The character's current health
		maxHealth;     // The character's highest health possible
	public bool
		canAttack = true,     // Toggle the character's ability to attack
		canTakeDamage = true; // Toggle the character's ability to take damage
	#endregion

	#region Private Variables (Empty)
	#endregion

	// Unity Named Methods
	#region Main Methods (Empty)
	#endregion

	#region Utility Methods
	/// <summary> Make the character receive damage and then become temporarily invincible </summary> 
	public virtual void TakeDamage(int damage)
	{
		if (canTakeDamage == true)
		{
			currentHealth.runTimeValue -= damage;
			StartCoroutine("StartBlinking");
		}
	}
	#endregion

	#region Coroutines
	/// <summary> Make a character blink for a short time after taking damage </summary>
	IEnumerator StartBlinking()
	{
		SpriteRenderer
			spriteRenderer = GetComponent<SpriteRenderer>();
		float
			timer = 0.5f;  // The character blinks for a short time after taking damage
		canTakeDamage = false; // The character cannot take more damage immediately after taking damage

		// make the character blink
		while (timer >= 0.0f)
		{
			spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle the sprite's visibility to make it blink
			timer -= Time.deltaTime + 0.1f;
			yield return new WaitForSeconds(0.1f);
		}

		// Ensure the sprite is visible and the character can take damage after blinking stops
		spriteRenderer.enabled = true;
		canTakeDamage = true;
	}
	#endregion
}