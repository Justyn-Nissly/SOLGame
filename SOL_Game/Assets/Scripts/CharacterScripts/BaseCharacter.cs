using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public int health;

	public bool canAttack = true; // for disabling the ability to attack, like when the players shield is up the player cant attack
	public bool canTakeDamage = true;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods

	#endregion

	#region Utility Methods
	public BaseCharacter()
	{
		health = 10;
	}

	public virtual void TakeDamage(int damage)
	{
		if(canTakeDamage == true)
		{
			health -= damage;
			StartCoroutine("StartBlinking");
		}
	}
	#endregion

	#region Coroutines
	IEnumerator StartBlinking()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		float timer = .5f;

		canTakeDamage = false; // cant take damage while blinking

		while (timer >= 0)
		{
			spriteRenderer.enabled = !spriteRenderer.enabled; //This toggles it
			timer -= Time.deltaTime + .1f;
			yield return new WaitForSeconds(.1f); //However many seconds you want
		}

		spriteRenderer.enabled = true; //This toggles it back on in the end
		canTakeDamage = true; // can take damage again
	}
	#endregion
}
