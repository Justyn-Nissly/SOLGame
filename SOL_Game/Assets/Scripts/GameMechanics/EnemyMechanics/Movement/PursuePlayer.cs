using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuePlayer : MonoBehaviour
{
    #region Enums and Defined Constants (Empty)
    #endregion

    #region Public Variables
    public float
		attackReach,  // How far the enemy attack can reach
        maxChaseTime; // Time left before the enemy might deaggro
	public bool
		canMoveAtPlayer = true; // Stop enemy when it collides with the player
	#endregion

	#region Private Variables
	private float
        chaseTime; // Time left before the enemy might deaggro
    private Vector2
        playerPos; // The player's position
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
    void FixedUpdate()
    {
		playerPos = GameObject.FindWithTag("Player").transform.position;
		if (enemy.aggro && Vector2.Distance(enemy.transform.position, playerPos) >= attackReach)
        {
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
        enemy.rb2d.position = Vector2.MoveTowards(enemy.rb2d.position,
                                                    playerPos,
                                                    enemy.moveSpeed * Time.deltaTime);
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

	#region Coroutines (Empty)
	#endregion
}
