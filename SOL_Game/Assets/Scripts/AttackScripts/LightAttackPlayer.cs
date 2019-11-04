using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackPlayer : MeleeAttackBase
{
	#region Enums
	#endregion

	#region Public Variables
	public Player player;
	public float startTimeBetweenAttacks = .3f;
	#endregion

	#region Private Variables
	private float timeBetweenAttacks;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void FixedUpdate()
	{
		if (timeBetweenAttacks <= 0 && Input.GetButtonDown("A") && player.CanAttack)
		{
			timeBetweenAttacks = startTimeBetweenAttacks; // reset the time between attacks

			Attack();
		}
		else
		{
			timeBetweenAttacks -= Time.deltaTime;
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
                characterBeingAtacked.TakeDamage(damageToGive.playerLightDamage);


                //ApplyKnockBack(collider.gameObject); MAKE SURE TO UNCOMMENT THIS BEFORE MERGIMNG TO TEST

            }
        }

        GameObject weaponInstance = Instantiate(weapon, attackPosition.transform);
        Destroy(weaponInstance, .3f);
    }
    #endregion

}
