using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveStrike : MonoBehaviour
{
    #region Enums and Defined Constants
    #endregion

    #region Public Variables
    public float
        evasionDistance, // How far the enemy tries to stay from the player
        minEvadeTime,    // Maximum time the enemy evades the player before charging
        maxEvadeTime,    // Minimum time the enemy evades the player before charging
        chargeSpeed,     // How fast the enemy charges at the player
        waitTime;        // How long the enemy waits to move again after charging
    public bool
        charging; // The enemy is charging at the player
    #endregion

    #region Private Variables
    private float
        angle,      // The angle from the enemy to the player
        x,          // The enemy's x-axis movement
        y,          // The enemy's y-axis movement
        evadeTime,  // How long the enemy will evade the player before charging
        chargeTime; // How long the enemy charges
    private Vector2
        playerPos, // The player's position
        lastPos;   // The enemy's last position
    private Enemy
        enemy; // Access the enemy's members
    private bool
        waiting;  // The enemy is briefly waiting to start moving again
    #endregion

    // Unity Named Methods
    #region Main Methods

    void Start()
    {
        new Random();
        enemy     = GetComponent<Enemy>();
        evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
        charging  = false;
        waiting   = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (charging)
        {
            enemy.aggro = true;
        }
        if (enemy.aggro)
        {
            if (charging == false && waiting == false)
            {
                playerPos = GameObject.FindWithTag("Player").transform.position;
                Evade();
                evadeTime -= Time.deltaTime;
                if (evadeTime <= 0.0f)
                {
                    waiting    = true;
                    waitTime   = 0.4f;
                    charging   = true;
                    chargeTime = Vector2.Distance(enemy.sprite.position, playerPos);
                }
            }
            else if (waiting)
            {
                Wait();
            }
            else
            {
                ChargePlayer();
                evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
            }
        }
        else
        {
            evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
        }
    }
    #endregion

    #region Utility Methods
    public void Evade()
    {
        angle = Mathf.Atan2(playerPos.y - enemy.transform.position.y,
                            playerPos.x - enemy.transform.position.x);
        if (Vector2.Distance(playerPos, transform.position) <= evasionDistance)
        {
            angle += Mathf.PI;
        }
        x = Cos(angle);
        y = Sin(angle);
        if (Vector2.Distance(playerPos, transform.position) <= evasionDistance - 0.5f ||
            Vector2.Distance(playerPos, transform.position) >= evasionDistance + 0.5f)
        {
            enemy.sprite.position = Vector2.MoveTowards((Vector2)transform.position,
                                                        (Vector2)transform.position + new Vector2(x, y),
                                                         enemy.moveSpeed * Time.deltaTime);
        }
    }

    public void ChargePlayer()
    {
        enemy.sprite.position = Vector2.MoveTowards(enemy.sprite.position,
                                                    playerPos,
                                                    chargeSpeed * Time.deltaTime);

        if (Vector2.Distance(enemy.sprite.position, lastPos) < 0.000000001f * enemy.moveSpeed * Time.deltaTime)
        {
            chargeTime = -1.0f;
        }

        chargeTime -= Time.deltaTime;

        if (chargeTime <= 0.0f)
        {
            charging = false;
            waiting = true;
            waitTime = 1.0f;
            enemy.aggro = false;
        }
        else
        {
            lastPos = enemy.sprite.position;
        }
    }

    public void Wait()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0.0f)
        {
            waiting   = false;
            evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
        }
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