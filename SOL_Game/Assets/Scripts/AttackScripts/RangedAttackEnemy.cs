using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackEnemy : RangedAttackBase
{
	#region Enums
	#endregion

	#region Public Variables
	public int rangeAttackDamage;

	public float maxTimeBetweenAttacks = 2f;
	public float minTimeBetweenAttacks = 1f;
	#endregion

	#region Private Variables
	private float countDownTimer;
	private Enemy enemy;
    #endregion


    #region Before Start
    private void Awake()
    {
        rangeAttackDamage = (int)damageToGive.initialValue;
    }
    #endregion

    // Unity Named Methods
    #region Main Methods
    public void Start()
	{
        enemy          = GetComponent<Enemy>();
        countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
	}

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
	#endregion

	#region Coroutines
	#endregion
}
