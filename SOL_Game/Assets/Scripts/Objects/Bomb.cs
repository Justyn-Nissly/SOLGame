using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject 
		BlowUpAnimaiton;
	public int
		bombDamage = 2;
	public float
		timeTillBlowUp = 1;
	public SpriteRenderer
		bombSpriteRenderer; // so that we can disable it when the bomb blows up
	#endregion

	#region Private Variables
	private float
		explosionDuration = 1; // this game object will be destroyed after this timer
	private bool
		canDamage = false; // this is because the collider is also used for movement and we dont want it damaging the player before it explodes
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void Start()
	{
		// call the blow up method is N seconds
		Invoke("BlowUp", timeTillBlowUp);
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && canDamage)
		{
			DamagePlayer(collision.gameObject.GetComponent<Player>());
			canDamage = false;
		}
	}
	#endregion

	#region Utility Methods
	private void BlowUp()
	{
		bombSpriteRenderer.enabled = false;
		canDamage = true;

		GameObject explotionEffect = Instantiate(BlowUpAnimaiton, gameObject.transform.position, new Quaternion(0, 0, 0, 0));
		explotionEffect.transform.localScale = new Vector3(50, 50, 0);

		Destroy(explotionEffect, explosionDuration);
		Destroy(gameObject, explosionDuration); // destroy this bomb game object after N seconds
	}

	/// <summary> the method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage(bombDamage, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.currentHealth);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}