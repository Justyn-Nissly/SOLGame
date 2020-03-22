using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Animator
		animator;
	public int
		bombDamage = 2;
	public float
		timeTillBlowUp = 1;
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
		// call the blow up method
		StartCoroutine(BlowUp(timeTillBlowUp));
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


	/// <summary> the method deals damage to the passed in player</summary>
	private void DamagePlayer(Player player)
	{
		if (player != null)
		{
			player.TakeDamage(bombDamage, false);

			// DEBUG CODE, REMOVE LATER
			Debug.Log("players CurrentHealth = " + player.maxHealth.runTimeValue);
		}
	}
	#endregion

	#region Coroutines
	private IEnumerator BlowUp(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);

		animator.SetTrigger("BlowUpBomb"); // start playing the animation
		yield return new WaitForSeconds(.5f);
		canDamage = true;
	}
	#endregion
}