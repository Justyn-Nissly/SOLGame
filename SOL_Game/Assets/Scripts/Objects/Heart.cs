using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PowerUp
{
    //Variables
    public FloatValue playerHealth;
    public FloatValue heartContainers;
    public float amountToIncrease;

	public override void Awake()
	{
		powerUpSprite = GetComponent<SpriteRenderer>();
		spinTimer = 0.0f;
	}

	public override void FixedUpdate()
	{
		spinTimer += Time.deltaTime * 8.0f;
		powerUpSprite.sprite = powerUps[(int)(spinTimer % 4.0f)];
	}

	public override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerHealth.runTimeValue += amountToIncrease;
            if (playerHealth.runTimeValue > heartContainers.runTimeValue * 2f)
            {
                playerHealth.runTimeValue = heartContainers.runTimeValue * 2f;
            }
            other.transform.GetComponent<Player>().playerHealthHUD.UpdateHearts();
            Destroy(gameObject);
        }
    }
}