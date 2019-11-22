using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPatrol : MonoBehaviour
{
    #region Enums and Defined Constants
    #endregion

    #region Public Variables
    public bool
        clockwise; // The enemy is patrolling clockwise
    #endregion

    #region Private Variables
    private float
        angle; // Current angle of rotation

    private Enemy
        enemy; // Reference the enemy
    #endregion

    // Unity Named Methods
    #region Unity Main Methods
    void Start()
    {
        enemy = GetComponent<Enemy>();
        angle = 0.0f;
    }

    void FixedUpdate()
    {
        if (enemy.aggro == false)
        {
            CirclePatrol(ref angle);
        }
    }
    #endregion

    #region Utility Functions
    // Patrol  in a circular pattern
    public void CirclePatrol(ref float angle)
    {
        enemy.rb2d.MovePosition(enemy.rb2d.position + new Vector2(Cos(angle), Sin(angle))
                                    * Time.deltaTime * enemy.moveSpeed * 2.0f);

        // Update the movement angle
        if (clockwise)
        {
            angle -= (Mathf.PI / 180.0f);
        }
        else
        {
            angle += (Mathf.PI / 180.0f);
        }
        if (angle >= 2.0f * Mathf.PI)
        {
            angle -= 2.0f * Mathf.PI;
        }
        if (angle < 0.0f)
        {
            angle += 2.0f * Mathf.PI;
        }
    }

    // Sine helps determine the drone's y-location
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

    // Cosine helps determine the drone's x-location
    public static float Cos(float angle)
    {
        return Sin(Mathf.PI / 2.0f - angle);
    }
    #endregion

    #region Coroutines
    #endregion
}