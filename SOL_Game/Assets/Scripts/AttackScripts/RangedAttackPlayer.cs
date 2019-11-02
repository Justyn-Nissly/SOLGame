using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackPlayer : RangedAttackBase
{
	#region Enums
	#endregion

	#region Public Variables
	public Player player;
	public int overRideBulletDamage; //Overrides The damage given to the bullet
	public float startTimeBetweenAttacks = 3f;
	#endregion

	#region Private Variables
	private float timeBetweenAttacks;
    #endregion

    //Initialize Values
    private void Awake()
    {
        overRideBulletDamage = damageToGive.enemyRangedDamage;
    }

    // Unity Named Methods
    #region Main Methods
    public void FixedUpdate()
	{
		if (timeBetweenAttacks <= 0)
		{
			if (Input.GetButtonUp("Y") && player.CanAttack) // Y is the left arrow button and is based on the SNES controller button layout
			{
				timeBetweenAttacks = startTimeBetweenAttacks; // reset the time between attacks
				Shoot();
			}
		}
		else
		{
			timeBetweenAttacks -= Time.deltaTime;
		}
	}
	#endregion

	#region Utility Methods
	public override void Shoot()
	{
		base.Shoot();

		GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

		BulletLogic bulletLogic = bulletInstance.GetComponent<BulletLogic>();
		bulletLogic.bulletDamage = overRideBulletDamage;
	}
	#endregion

	#region Coroutines
	#endregion
}
