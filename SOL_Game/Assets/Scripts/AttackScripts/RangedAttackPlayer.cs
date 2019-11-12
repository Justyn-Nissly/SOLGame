using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackPlayer : RangedAttackBase
{
	#region Enums and Defined Constants
	#endregion

	#region Public Variables
	public Player
        player; // Reference player script
	public int
        rangeAttackDamage = 2;
	public float
        startTimeBetweenAttacks = 3.0f;
	#endregion

	#region Private Variables
	private float
        timeBetweenAttacks;
	#endregion

	// Unity Named Methods
	#region Main Methods
	public void FixedUpdate()
	{
        // The player can fire the blaster on cooldown
		if (timeBetweenAttacks <= 0.0f)
		{
            // Y is left arrow based on the SNES controller layout; fire and reset the cooldown
            if (Input.GetButtonUp("Y") && player.canAttack)
			{
                timeBetweenAttacks = startTimeBetweenAttacks;
				Shoot();
			}
		}
        // The cooldown has not finished yet
		else
		{
			timeBetweenAttacks -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Methods
    // Fire the blaster
	public override void Shoot()
	{
		base.Shoot();

        // Create and launch blaster bullet
		GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		BulletLogic bulletLogic = bulletInstance.GetComponent<BulletLogic>();
		bulletLogic.bulletDamage = rangeAttackDamage;
	}
	#endregion

	#region Coroutines
	#endregion
}