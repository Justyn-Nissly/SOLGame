using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackEnemy : RangedAttackBase
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public int
		rangeAttackDamage;
	public float
		maxTimeBetweenAttacks = 2f,
		minTimeBetweenAttacks = 1f;
	#endregion

	#region Private Variables
	private float
		countDownTimer; // Track how long before the next attack
	private Enemy
		enemy;
	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary> Assign the attack's damage </summary>
	private void Awake()
	{
		rangeAttackDamage = (int)damageToGive.initialValue;
	}

	///<summary> Make the enemy ready to attack </summary>
	public void Start()
	{
        enemy          = GetComponent<Enemy>();
        countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
	}

	///<summary> If the enemy is aggro it fires at the player </summary>
	public void FixedUpdate()
    {
        if (enemy.aggro)
        {
            if (countDownTimer <= 0)
            {
                Shoot();
                countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
            }
            else
            {
                countDownTimer -= Time.deltaTime;
            }
        }
    }
	#endregion

	#region Utility Methods
	///<summary> Fire a projectile after a short delay </summary>
	public override void Shoot()
	{
		base.Shoot();
		// Create and launch blaster bullet
		GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		BulletLogic bulletLogic = bulletInstance.GetComponent<BulletLogic>();
		bulletLogic.bulletDamage = rangeAttackDamage;
	}

	///<summary> Create the projectile </summary>
  	public void InstantiateBullet()
	{
		// Create and launch blaster bullet
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}