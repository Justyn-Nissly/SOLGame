using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : PowerUps
{
    public FloatValue heartContainers;
    public FloatValue playerHealth;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            heartContainers.runTimeValue += 1;
            playerHealth.runTimeValue = heartContainers.runTimeValue * 2f;
            powerUpSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
