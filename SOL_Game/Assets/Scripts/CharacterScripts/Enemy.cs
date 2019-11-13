using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Enemy : BaseCharacter
{
    #region Public Variables
    public Image healthBar;
    public string
        enemyName; // The enemy's name
    public float
        aggroRange,  // The max range where the enemy can detect the player
		followRange, // How far away the player must get for the enemy to deaggro
		attackDmg,   // Base damage from an intentional attack
        contactDmg,  // Base damage from making contact with the player
		moveSpeed;   // Base movement speed
    public bool
        aggro; // The enemy has detected the player
    public Vector2[]
        patrol; // The enemy's patrol points
    public Vector2
        playerPos;  // The player's position
    public Rigidbody2D
		sprite; // The enemy's sprite
    #endregion

	#region Private Variables
  #endregion

	void Start()
	{
		sprite = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		playerPos = GameObject.FindWithTag("Player").transform.position;
		if (aggro == false && Vector2.Distance(transform.position, playerPos) <= aggroRange)
		{
			aggro = true;
		}
		else if (Vector2.Distance(transform.position, playerPos) >= followRange)
		{
			aggro = false;
		}
        if (aggro)
        {
            canAttack = true;
        }
	}

	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);

        float percentHealth = currentHealth.runTimeValue / maxHealth.initialValue;
        SetHealth(percentHealth);

        Debug.Log("enemy CurrentHealth = " + currentHealth.initialValue);

		if (currentHealth.runTimeValue <= 0)
		{
			Destroy(gameObject);
		}
	}

    void SetHealth(float percentHelth)
    {
        healthBar.fillAmount = percentHelth;
    }
}
