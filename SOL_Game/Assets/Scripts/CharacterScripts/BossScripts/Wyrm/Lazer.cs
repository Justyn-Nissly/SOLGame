using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public Transform
		lazerHitPoisiton; // the end location of the lazer
	public LayerMask
		layerMask, // the layer that the lazer will hit
		playerLayer; // the layers used to damage the player
	#endregion

	#region Private Variables
	private LineRenderer
		lineRenderer; // the lazers renderer
	private bool
		lazerHitPLayer = false; // used so that the player is only damaged once
	#endregion

	// Unity Named Methods
	#region Unity Main Methods
	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void FixedUpdate()
	{
		// cast the lazer that will be used to draw the lazer
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 100, layerMask);

		// cast the lazer that will be used to damage the player
		RaycastHit2D playerhit = Physics2D.Raycast(transform.position, transform.up, 100, playerLayer);

		// check if the player should be damaged
		if (playerhit == true && playerhit.transform.gameObject.CompareTag("Player") && lazerHitPLayer == false)
		{
			DamagePlayer(playerhit.transform.GetComponent<Player>(), 1, false);
			lazerHitPLayer = true;
		}

		// set the end position to the ending raycast position
		lazerHitPoisiton.position = hit.point;

		// draw the lazer
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, lazerHitPoisiton.position);
	}
	#endregion

	#region Utility Functions
	/// <summary> deal damage to the passed in player </summary>
	protected virtual void DamagePlayer(Player player, int damageToGive, bool playSwordSoundEffect = false)
	{
		if (player != null)
		{
			player.TakeDamage(damageToGive, playSwordSoundEffect);
		}
	}

	/// <summary>
	/// called in the animator
	/// </summary>
	public void DestroyGameObject()
	{
		Destroy(gameObject.transform.parent.gameObject);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
