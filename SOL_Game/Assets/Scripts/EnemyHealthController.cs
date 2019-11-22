using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : Enemy
{

    float amountHealed = 0;
    public float healPerLoop;
    public float healAmount;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        healPerLoop = healAmount / duration;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Heal Over Time increase health over time as designed in the variables passed to it
    public void HealOverTime(int healAmount, int duration)
    {
        StartCoroutine(HealOverTimeCoroutine(healAmount, duration));
    }


    //Heal over time
    IEnumerator HealOverTimeCoroutine(float healAmount, float duration)
    {

        while (amountHealed < healAmount)
        {
            currentHealth.runTimeValue += healPerLoop;
            amountHealed += healPerLoop;
            yield return new WaitForSeconds(1f);
        }
    }

}
