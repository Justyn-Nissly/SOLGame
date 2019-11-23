using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadePlayer : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
        evasionDistance; // How far the enemy tries to stay from the player
    #endregion

    #region Private Variables
    private float
        angle, // The angle from the enemy to the player
        x,     // The enmy's x-axis movement
        y;     // The enmy's y-axis movement
    private Vector2
        playerPos, // The player's position
        direction; // The enemy's movement direction
    private Enemy
        enemy; // Access the enemy's members
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the enemy </summary>
	void Start()
    {
        enemy = GetComponent<Enemy>();
    }

	/// <summary> Evade the player if aggroed </summary>
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
	/// <summary> Try to stay a set distance from the player </summary>
	public void Evade()
    {
		// Get the angle between the enemy and the player
        angle = Mathf.Atan2(playerPos.y - enemy.transform.position.y, playerPos.x - enemy.transform.position.x);

		// If the enemy is too close to the player it evades; otherwise it pursues the player
        if (Vector2.Distance(playerPos, transform.position) <= evasionDistance)
        {
            angle += Mathf.PI;
        }

		// If the enemy is not the desired distance from the player it moves to the desired distance
        if (Vector2.Distance(playerPos, transform.position) <= evasionDistance - 0.5f ||
            Vector2.Distance(playerPos, transform.position) >= evasionDistance + 0.5f)
        {
            enemy.rb2d.position = Vector2.MoveTowards((Vector2)transform.position,
                                                      (Vector2)transform.position + new Vector2(Cos(angle), Sin(angle)),
                                                       enemy.moveSpeed * Time.deltaTime);
        }
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