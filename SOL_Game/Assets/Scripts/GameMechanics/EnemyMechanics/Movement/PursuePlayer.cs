using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuePlayer : MonoBehaviour
{
    #region Enums and Defined Constants
    #endregion

    #region Public Variables
    public float
        speed,
        maxChaseTime; // Time left before the enemy might deaggro
    #endregion

    #region Private Variables
    private float
        angle,     // The angle from the enemy to the player
        x,         // The enmy's x-axis movement
        y,         // The enmy's y-axis movement
        chaseTime; // Time left before the enemy might deaggro
    private Vector2
        playerPos, // The player's position
        direction; // The enemy's movement direction
    private Enemy
        enemy; // Access the enemy's members
    #endregion

    // Unity Named Methods
    #region Main Methods
    // Initalize the enemy path
    void Start()
    {
        enemy     = GetComponent<Enemy>();
        chaseTime = maxChaseTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.aggro)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            Pursue();
        }
    }
    #endregion

    #region Utility Methods
    public void Pursue()
    {
        if (chaseTime <= 0.0f)
        {
            chaseTime = maxChaseTime;
            if (Vector2.Distance((Vector2)transform.position, playerPos) > enemy.aggroRange)
            {
                enemy.aggro = false;
            }
        }
        angle      = Mathf.Atan2(playerPos.y - enemy.transform.position.y, playerPos.x - enemy.transform.position.x);
        x          = Cos(angle) * speed;
        y          = Sin(angle) * speed;
        direction  = new Vector2(x, y);
        enemy.sprite.MovePosition(enemy.sprite.position + direction);
        chaseTime -= Time.deltaTime;
    }

    // Calculate an angle's sine; helps determine the enemy's y-location
    public static float Sin(float angle)
    {
        float
            sine = 0.0f,
            part;
        int
            counter1,
            counter2;

        for (counter1 = 1; counter1 < 100; counter1 += 2)
        {
            part = 1.0f;
            for (counter2 = 1; counter2 <= counter1; counter2++)
            {
                part *= angle;
                part /= counter2;
            }
            if (counter1 % 4 == 1)
                sine += part;
            else
                sine -= part;
        }

        return sine;
    }

    // Calculate an angle's cosine; helps determine the enemy's x-location
    public static float Cos(float angle)
    {
        return Sin(Mathf.PI * 0.5f - angle);
    }
    #endregion

    #region Coroutines
    #endregion
}