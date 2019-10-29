using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthManager : MonoBehaviour
{

	public int playerMaxHealth;
	public int playerCurrentHealth;


    // Start is called before the first frame update
    void Start()
    {
		playerCurrentHealth = playerMaxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
		if (playerCurrentHealth <= 0)
		{
			gameObject.SetActive(false);



		}

	}


    // How much damage the player is going to take from an enemy
    public void HurtPlayer(int damageToGive)
	{
		playerCurrentHealth -= damageToGive;
	}


    //Set player health back to maxHealth
    public void setMaxHealth()
	{
		playerCurrentHealth = playerMaxHealth;
	}
}
