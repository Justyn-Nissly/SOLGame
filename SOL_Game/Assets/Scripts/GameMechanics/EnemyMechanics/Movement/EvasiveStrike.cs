using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveStrike : MonoBehaviour
{
	#region Enums (Empty)
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
        waiting; // The enemy is briefly waiting to start moving again
    #endregion

    // Unity Named Methods
    #region Main Methods

	/// <summary> Initialize the enemy's attack pattern </summary>
    void Start()
    {
        new Random();
        enemy     = GetComponent<Enemy>();
        evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
        charging  = false;
        waiting   = false;
    }

	/// <summary> The enemy alternates evading and charging at the player </summary>
	void FixedUpdate()
    {
		// The enemy cannot deaggro when charging
        if (charging)
        {
            enemy.aggro = true;
        }

		// If the enemy has detected the player it starts its attack pattern
        if (enemy.aggro)
        {
			// The enemy is evading the player
            if (charging == false && waiting == false)
            {
                playerPos = GameObject.FindWithTag("Player").transform.position;
                Evade();
                evadeTime -= Time.deltaTime;

				// Check if the enemy will charge
                if (evadeTime <= 0.0f)
                {
                    waiting    = true;
                    waitTime   = 0.4f;
                    charging   = true;
                    chargeTime = Vector2.Distance(enemy.rb2d.position, playerPos);
                }
            }
			// The enemy is waiting
            else if (waiting)
            {
                Wait();
            }

			// The enemy is charging
            else
            {
                ChargePlayer();
                evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
            }
        }

		// If the enemy is not aggroed, reset the evasion time
        else
        {
            evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
        }
    }
    #endregion

    #region Utility Methods
	/// <summary> The enemy evades the player when aggroed </summary>
    public void Evade()
    {
		// Calculate the angle between the enemy and the player
        angle = Mathf.Atan2(playerPos.y - enemy.transform.position.y,
                            playerPos.x - enemy.transform.position.x);

		// The enemy evades the player if it is too close
        if (Vector2.Distance(playerPos, transform.position) <= evasionDistance)
        {
            angle += Mathf.PI;
        }

		// The enemy tries to stay a set distance away from the player and moves accordingly
        if (Vector2.Distance(playerPos, transform.position) <= evasionDistance - 0.5f ||
            Vector2.Distance(playerPos, transform.position) >= evasionDistance + 0.5f)
        {
            enemy.rb2d.position =
				Vector2.MoveTowards((Vector2)transform.position,
                                    (Vector2)transform.position + new Vector2(Cos(angle), Sin(angle)),
                                    enemy.moveSpeed * Time.deltaTime);
        }
    }

	/// <summary> The enemy eventually charges the player when aggroed </summary>
	public void ChargePlayer()
    {
		// Make the enemy move directly towards the player
        enemy.rb2d.position = Vector2.MoveTowards(enemy.rb2d.position,
                                                    playerPos,
                                                    chargeSpeed * Time.deltaTime);

		// If the enemy stops moving or hits an obstacle it stops trying to charge
        if (Vector2.Distance(enemy.rb2d.position, lastPos) < 0.000000001f * enemy.moveSpeed * Time.deltaTime)
        {
            chargeTime = -1.0f;
        }

		// Check if the enemy stops trying to charge
        chargeTime -= Time.deltaTime;
        if (chargeTime <= 0.0f)
        {
            charging = false;
            waiting = true;
            waitTime = 1.0f;
            enemy.aggro = false;
        }
		// Set the enemy's previous position to see if it has stopped moving
        else
        {
            lastPos = enemy.rb2d.position;
        }
    }

	/// <summary> The enemy waits for a short time </summary>
	public void Wait()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0.0f)
        {
            waiting   = false;
            evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
        }
    }

	/// <summary> / Calculate an angle's sine; custom-made and kept local to run faster </summary>
	public static float Sin(float angle)
    {
        float
            sine = 0.0f, // Sine is a calculated sum
			part;        // Track the current part being added to the sum
        int
            counter1, // Track the current iteration
            counter2; // 

		// Approximate sine by finite sum: n = 0, Σ (-1)^n[x^(2n+1)]/[(2n+1)!]
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

	/// <summary> / Calculate an angle's cosine; custom-made and kept local to run faster </summary>
	public static float Cos(float angle)
    {
		// sine(x) = cos(pi/2 - x)
        return Sin(Mathf.PI * 0.5f - angle);
    }
	#endregion

	#region Coroutines (Empty)
	#endregion
}