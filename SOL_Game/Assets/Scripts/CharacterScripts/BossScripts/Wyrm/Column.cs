using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			PlayerSafeFromFire(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			PlayerSafeFromFire(false);
		}
	}

	private void PlayerSafeFromFire(bool isPlayerSafeFromFire)
	{
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		if (player != null)
		{
			player.safeFromFireAttack = isPlayerSafeFromFire;
		}
	}
}
