using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PowerUps
{
    //Variables
    public FloatValue playerHealth;
    public FloatValue heartContainers;
    public float amountToIncrease;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerHealth.runTimeValue += amountToIncrease;
            if (playerHealth.runTimeValue > heartContainers.runTimeValue * 2f)
            {
                playerHealth.runTimeValue = heartContainers.runTimeValue * 2f;
            }
         other.transform.GetComponent<Player>().playerHealthHUD.UpdateHearts();
            Destroy(this.gameObject);
        }
    }
}


