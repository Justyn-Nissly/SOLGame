using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadePlayer : MonoBehaviour
{
    #region Enums and Defined Constants
    #endregion

    #region Public Variables
    public float
        evasionDistance; // How far the enemy tries to stay from the player
    #endregion

    #region Private Variables
    private float
        angle,         // The angle from the enemy to the player
        x,             // The enmy's x-axis movement
        y;             // The enmy's y-axis movement
    private Vector2
        playerPos,     // The player's position
        direction;     // The enemy's movement direction
    private Enemy
        enemy;         // Access the enemy's members
    #endregion

    // Unity Named Methods
    #region Main Methods
    // Initalize the enemy path
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemy.aggro)
        {
            playerPos = GameObject.FindWithTag("Player").transform.position;
            Evade();
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
            enemy.enemyRigidbody.position = Vector2.MoveTowards((Vector2)transform.position,
                                                        (Vector2)transform.position + new Vector2(x, y),
                                                         enemy.moveSpeed * Time.deltaTime);
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