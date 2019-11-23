using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuePlayer : MonoBehaviour
{
    #region Enums and Defined Constants
    #endregion

    #region Public Variables
    public float
        maxChaseTime; // Time left before the enemy might deaggro
	public bool
		canMoveAtPlayer; // The aggroed enemy pursues the player until it reaches the player
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
	/// <summary> Initialize the enemy movement </summary>
	void Start()
    {
        enemy           = GetComponent<Enemy>();
        chaseTime       = maxChaseTime;
		canMoveAtPlayer = true;
	}

	/// <summary> The enemy keeps track of the player's position </summary>
	void FixedUpdate()
    {
        if (enemy.aggro && canMoveAtPlayer)
        {
            playerPos = GameObject.FindWithTag("Player").transform.position;
            Pursue();
        }
    }

	/// <summary> The enemy stops moving if it reaches the player </summary>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
			canMoveAtPlayer = false;
	}

	/// <summary> The enemy pursues if the player moves away </summary>
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
			canMoveAtPlayer = true;
	}
	#endregion

	#region Utility Methods
	/// <summary> The enemy chases the player </summary>
	public void Pursue()
    {
		// After a short time the enemy stops chasing the player if outside of the aggro range
        if (chaseTime <= 0.0f)
        {
            chaseTime = maxChaseTime;
            if (Vector2.Distance((Vector2)transform.position, playerPos) > enemy.aggroRange)
            {
                enemy.aggro = false;
            }
        }

		// Move the enemy towards the player
        enemy.rb2d.position = Vector2.MoveTowards(enemy.rb2d.position,
                                                    playerPos,
                                                    enemy.moveSpeed * Time.deltaTime);
        chaseTime -= Time.deltaTime;
    }

	/// <summary> Calculate an angle's sine; custom-made and kept local to run faster </summary>
	public static float Sin(float angle)
    {
        float
            sine = 0.0f, // Sine is a calculated sum
			part;        // Track the current part being added to the sum
        int
			counter1, // Track which part of the sum is being calculated
			counter2; // The power and factorial functions must be done iteratively

		// Approximate sine by finite sum: n = 0, Σ (-1)^n * (x^(2n+1)) / ((2n+1)!)
		for (counter1 = 1; counter1 < 100; counter1 += 2)
		{
			part = 1.0f;
			for (counter2 = 1; counter2 <= counter1; counter2++)
			{
				part *= angle;
				part /= counter2;
			}
			if (counter1 % 4 == 1)
			{
				sine += part;
			}
			else
			{
				sine -= part;
			}
		}

        return sine;
    }

	/// <summary> Calculate an angle's cosine; custom-made and kept local to run faster </summary>
	public static float Cos(float angle)
    {
		// cos(x) = sin(pi/2 - x)
        return Sin(Mathf.PI * 0.5f - angle);
    }
	#endregion

	#region Coroutines (Empty)
	#endregion
}