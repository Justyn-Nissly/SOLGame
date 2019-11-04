using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackEnemy : MeleeAttackBase
{
	#region Enums
	#endregion

	#region Public Variables
	public Enemy enemy;
	public float maxTimeBetweenAttacks = 1.2f;
	public float minTimeBetweenAttacks = 0.7f;
	#endregion

	#region Private Variables
	private float countDownTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (countDownTimer <= 0 && enemy.CanAttack)
		{
			countDownTimer = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks); // reset the time between attacks

			Attack();
		}
		else
		{
			countDownTimer -= Time.deltaTime;
		}
	}
    #endregion

    #region Utility Methods
    #endregion

    #region Coroutines
    #endregion

    // Inflict dagame function
    #region Utility Methods
    public void Attack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, willDamageLayer);

        foreach (Collider2D collider in enemiesToDamage)
        {
            BaseCharacter characterBeingAtacked = collider.GetComponent<BaseCharacter>();
            if (characterBeingAtacked != null)
            {
                characterBeingAtacked.TakeDamage(damageToGive.enemyLightDamage);


                //ApplyKnockBack(collider.gameObject); MAKE SURE TO UNCOMMENT THIS BEFORE MERGIMNG TO TEST

            }
        }

        GameObject weaponInstance = Instantiate(weapon, attackPosition.transform);
        Destroy(weaponInstance, .3f);
    }
    #endregion

}
