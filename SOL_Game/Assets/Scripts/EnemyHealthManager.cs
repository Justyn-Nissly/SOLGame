using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
	public int enemyMaxHealth;
	public int enemyCurrentHealth;


	// Start is called before the first frame update
	void Start()
	{
		enemyCurrentHealth = enemyMaxHealth;

	}

	// Update is called once per frame
	void Update()
	{
		if (enemyCurrentHealth <= 0)
		{
			Destroy (gameObject);



		}

	}


	// How much damage the player is going to take from an enemy
	public void HurtEnemy(int damageToGive)
	{
		enemyCurrentHealth -= damageToGive;
	}


	//Set player health back to maxHealth
	public void setMaxHealth()
	{
		enemyCurrentHealth = enemyMaxHealth;
	}
}