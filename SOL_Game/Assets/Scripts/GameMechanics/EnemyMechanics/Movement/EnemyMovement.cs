using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	#region Enums and Constants
	public enum MovementType
	{
		EvadePlayer,
		FreeRoam,
		PersuePlayer,
		EvasiveStrike,
		CircularPatrol
	}
	const int
		UP = 1,
		LEFT = 2,
		DOWN = 3,
		RIGHT = 4;
	#endregion

	#region Public Variables

	#region Shared Variables
	public MovementType
		Type; // The movement pattern that the enemy uses
	public Animator
		enemyAnimator; // The animator controller for the enemy
	public float
		evasionDistance, // How far the enemy tries to stay from the player
		waitTime;        // Waiting time before moving in new direction and how long the enemy waits to move again after charging
	public bool
		canMove = true; // The enemy can move
	#endregion

	#region Evade Player Variables
	/// <summary> Uses evasionDistance </summary>
	#endregion

	#region Persue Player Variables
	public float
		maxChaseTime; // Time left before the enemy might deaggro
	public bool
		canMoveAtPlayer = true; // Stop enemy when it collides with the player
	#endregion

	#region Free Roam Variables
	public float
	   maxTime, // Maximum time the enemy moves before choosing new direction
	   minTime; // Minimum time the enemy moves before choosing new direction
	#endregion

	#region Evasive Strike Variables
	public float
		minEvadeTime, // Maximum time the enemy evades the player before charging
		maxEvadeTime, // Minimum time the enemy evades the player before charging
		chargeSpeed;  // How fast the enemy charges at the player
	public bool
		charging; // The enemy is charging at the player
	#endregion

	#endregion

	#region Private Variables

	#region Shared Variables
	private Vector2
		enemyMovementAmount, // The distance the enemy will move
		lastPos,             // The enemy's previous position
		playerPos;           // The player's position
	private Enemy
		enemy; // Access the enemy's members
	private Rigidbody2D
		enemyRidgedBody; // The ridged body of the enemy
	private float
		angle,    // The angle from the enemy to the player
		x = 0.0f, // Horizontal movement
		y = 0.0f; // Vertical movement
	#endregion

	#region Evade Player Variables
	/// <summary> Uses angle </summary>
	#endregion

	#region Persue Player Variables
	private float
		chaseTime, // Time left before the enemy might deaggro
		waiting;   // The amount of time left for enemy to wait
	#endregion

	#region Free Roam Variables
	private float
		moveTime; // How long the enemy will move before choosing a new direction
	private int
		direction,     // The enemy's current direction (for choosing where to go)
		lastDirection; // The enemy's last direction
	private Vector2
		path; // The enemy's path: up, down, left, or right (for actually moving)
	private bool
		stopped; // An obstacle is hindering the enemy
	#endregion

	#region Evasive Strike Variables
	private float
		evadeTime,  // How long the enemy will evade the player before charging
		chargeTime; // How long the enemy charges
	private bool
		isWaiting; // The enemy is briefly waiting to start moving again
	#endregion
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the enemy </summary>
	void Start()
	{
		enemy           = GetComponent<Enemy>();
		enemyRidgedBody = GetComponent<Rigidbody2D>();
		new Random();
		if (Type == MovementType.FreeRoam)
		{
			stopped = false;
			ChooseNewPath();
			moveTime = Random.Range(minTime, maxTime);
		}
		else if (Type == MovementType.EvasiveStrike)
		{
			evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
			charging = false;
			isWaiting = false;
		}
	}

	/// <summary> Evade the player if aggroed </summary>
	void FixedUpdate()
	{
		// Update the values in the Animator for the players animation
		if (enemyAnimator != null)
		{
			SetEnemyAnimatorValues();
		}

		// Check which movement type is being used
		switch (Type)
		{
			case MovementType.EvadePlayer:
			{
				if (enemy.aggro)
				{
					playerPos = GameObject.FindWithTag("Player").transform.position;
					Evade();
				}
				break;
			}
			case MovementType.PersuePlayer:
			{
				if (enemy.aggro && canMoveAtPlayer)
				{
					playerPos = GameObject.FindWithTag("Player").transform.position;
					Pursue();
				}
				break;
			}
			case MovementType.FreeRoam:
			{
				if (canMove)
				{
					if (enemy.aggro == false && GameObject.FindObjectOfType<DialogueManager>().GetComponentInChildren<Animator>().GetBool("IsOpen") == false)
					{
						Roam();
					}
					else
					{
						waiting = waitTime;
					}

					// if the enemy is the ranged guardian roam even if aggro
					if (enemy is RangedGuardian)
					{
						Roam();
					}
					if (enemy.aggro && canMoveAtPlayer)
					{
						playerPos = GameObject.FindWithTag("Player").transform.position;
						Pursue();
					}
				}
				
				break;
			}
			case MovementType.EvasiveStrike:
			{
				if (charging)
				{
					enemy.aggro = true;
				}

				// If the enemy has detected the player it starts its attack pattern
				if (enemy.aggro)
				{
					// The enemy is evading the player
					if (charging == false && isWaiting == false)
					{
						playerPos = GameObject.FindWithTag("Player").transform.position;
						Evade();
						evadeTime -= Time.deltaTime;

						// Check if the enemy will charge at the player
						if (evadeTime <= 0.0f)
						{
							isWaiting = true;
							waitTime = 0.4f;
							charging = true;
							chargeTime = Vector2.Distance(enemy.rb2d.position, playerPos);
							enemyAnimator.SetLayerWeight(0, 1);
							enemyAnimator.SetLayerWeight(1, 0);
							}
					}
					// The enemy is waiting
					else if (isWaiting)
					{
						Wait();
					}

					// The enemy is charging at the player
					else
					{
						ChargeAtPlayer();
						evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
					}
				}

				// If the enemy is not aggroed, reset the evasion time
				else
				{
					evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
				}
				break;
			}
		};

		if (canMove)
		{
			enemyAnimator.SetLayerWeight(1, 0);
			enemyAnimator.SetLayerWeight(0, 1);
		}
		else
		{
			enemyAnimator.SetLayerWeight(1, 1);
			enemyAnimator.SetLayerWeight(0, 0);
		}
	}
	#region Persue Player Unity Methods

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			canMove = false;
			canMoveAtPlayer = false;
		}
			
	}

	private void OnTriggerExit2D(Collider2D collision)
	{

		if (collision.gameObject.CompareTag("Player"))
		{
			canMove = true;
			canMoveAtPlayer = true;
		}
	}
	#endregion
	#endregion

	#region Utility Methods

	#region Shared Methods
	/// <summary> Try to stay a set distance away from the player </summary>
	public void Evade()
	{
		// Get the angle between the enemy and the player
		angle = Mathf.Atan2(playerPos.y - enemy.transform.position.y, playerPos.x - enemy.transform.position.x);

		// If the enemy is too close to the player it evades
		if (Vector2.Distance(playerPos, transform.position) <= evasionDistance)
		{
			angle += Mathf.PI;
		}

		// If the enemy is not the desired distance from the player it moves to the desired distance
		if ((Vector2.Distance(playerPos, transform.position) <= evasionDistance - 0.5f ||
			Vector2.Distance(playerPos, transform.position) >= evasionDistance + 0.5f) && canMove)
		{
			enemy.rb2d.position = Vector2.MoveTowards((Vector2)transform.position,
													  (Vector2)transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)),
													   enemy.moveSpeed * Time.deltaTime);
			enemyAnimator.SetLayerWeight(1, 0);
			enemy.canAttack = false;
		}
		else
		{
			enemyAnimator.SetLayerWeight(1, 1);
			enemy.canAttack = true;
		}
	}

	/// <summary> Update the values in the Animator for the enemy animations </summary>
	private void SetEnemyAnimatorValues()
	{
		enemyMovementAmount = playerPos - enemy.rb2d.position;

		if (Type == MovementType.FreeRoam && enemy.aggro == false)
		{
			switch (direction)
			{
				case UP:
				{
					enemyAnimator.SetFloat("Vertical", 0.1f);
					enemyAnimator.SetFloat("Horizontal", 0.0f);
					break;
				}
				case LEFT:
				{
					enemyAnimator.SetFloat("Vertical", 0.0f);
					enemyAnimator.SetFloat("Horizontal", -0.1f);
					break;
				}
		        case DOWN:
				{
					enemyAnimator.SetFloat("Vertical", -0.1f);
					enemyAnimator.SetFloat("Horizontal", 0.0f);
					break;
				}
				case RIGHT:
				{
					enemyAnimator.SetFloat("Vertical", 0.0f);
					enemyAnimator.SetFloat("Horizontal", 0.1f);
					break;
				}
			}
		}
		else
		{
			enemyAnimator.SetLayerWeight(1, 0);
			if (Mathf.Abs(enemyMovementAmount.y) > Mathf.Abs(enemyMovementAmount.x))
			{
				enemyAnimator.SetFloat("Vertical", (enemyMovementAmount.y > 0) ? 0.1f : -0.1f);
				enemyAnimator.SetFloat("Horizontal", 0.0f);
			}
			else
			{
				enemyAnimator.SetFloat("Vertical", 0.0f);
				enemyAnimator.SetFloat("Horizontal", (enemyMovementAmount.x > 0) ? 0.1f : -0.1f);
			}
		}

	}
	#endregion

	#region EvadePlayer Methods
	/// <summary> Uses the Evade () function </summary>
	#endregion

	#region PersuePlayer Methods
	/// <summary> Find the player and persue </summary>
	public void Pursue()
	{

		//enemyMovementAmount = GetNextVector();
		if (chaseTime <= 0.0f)
		{
			chaseTime = maxChaseTime;
			if (Vector2.Distance((Vector2)transform.position, playerPos) > enemy.aggroRange)
			{
				enemy.aggro = false;
			}

		}
		else
		{
			// Update the values in the Animator for the players animation
			if (enemyAnimator != null)
			{
				SetEnemyAnimatorValues();
				enemyAnimator.SetLayerWeight(1, 0);
				enemyAnimator.SetLayerWeight(0, 1);
			}
		}

		enemy.rb2d.position = Vector2.MoveTowards(enemy.rb2d.position,
													playerPos,
													enemy.moveSpeed * Time.deltaTime);
		//enemyRidgedBody.MovePosition(GetNextVector() + enemyRidgedBody.position);
		chaseTime -= Time.deltaTime;
	}
	#endregion



	/*private Vector2 GetNextVector()
	{
		return Vector2.Distance(playerPos, enemyRidgedBody.position) * enemy.moveSpeed;
	}*/




	#region FreeRoam Methods
	/// <summary> Move in a new direction </summary>
	public void ChooseNewPath()
	{
		lastDirection = direction;

		// If the enemy hit an obstacle it will not try to move in the same direction
		direction = Random.Range(UP, RIGHT + 1);
		if (stopped)
		{
			if (direction == lastDirection)
			{
				direction = RIGHT - direction / 2;
			}
		}

		// Set the new movement direction
		x = 0.0f;
		y = 0.0f;
		switch (direction)
		{
			case UP:
			y = 1.0f;
			break;
			case DOWN:
			y = -1.0f;
			break;
			case LEFT:
			x = -1.0f;
			break;
			case RIGHT:
			x = 1.0f;
			break;
			default: // This should never occur
			Debug.Log("ERROR: random number outside accepted bounds.");
			break;
		}

		// Set the new path and movement time
		path = new Vector2(x * enemy.moveSpeed, y * enemy.moveSpeed);
		moveTime = Random.Range(minTime, maxTime);
	}


	/// <summary> Roam around randomly </summary>
	public void Roam()
	{
		// If the enemy stops moving it chooses a new direction
		if (moveTime <= 0.0f)
		{
			if(enemyAnimator != null)
			{
				enemyAnimator.SetLayerWeight(1, 0);
				enemyAnimator.SetLayerWeight(0, 1);
			}

			waiting = waitTime;
			ChooseNewPath();
			stopped = false;
		}

		// The enemy delays before moving in a new direction
		else if (waiting > 0.0f)
		{
			if (enemyAnimator != null)
			{
				enemyAnimator.SetLayerWeight(1, 1);
				enemyAnimator.SetLayerWeight(0, 0);
			}

			waiting -= Time.deltaTime;
		}

		// The enemy is moving
		else
		{
			if(enemyAnimator != null)
			{
				enemyAnimator.SetLayerWeight(1, 0);
				enemyAnimator.SetLayerWeight(0, 1);
			}

			enemy.rb2d.MovePosition((Vector2)transform.position + path * Time.deltaTime);
			moveTime -= Time.deltaTime;
			// If the enemy hits an obstacle it stops and chooses a different direction
			if (Vector2.Distance(transform.position, lastPos) < 0.01f * enemy.moveSpeed * Time.deltaTime)
			{
				moveTime = -0.1f;
				waiting = waitTime * 0.4f;
				stopped = true;
				lastDirection = direction;
			}
			// The enemy keeps moving
			else
			{
				lastPos = transform.position;
				lastDirection = -1;
			}
		}
		if(enemyAnimator != null)
		{
			SetEnemyAnimatorValues();
		}
	}
	#endregion

	#region EvasiveStrike Methods
	/// <summary> Charge at the player when aggro </summary>
	public void ChargeAtPlayer()
	{
		// Make the enemy move directly towards the player
		enemy.rb2d.position = Vector2.MoveTowards(enemy.rb2d.position, playerPos, chargeSpeed * Time.deltaTime);

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
			enemyAnimator.SetLayerWeight(0, 0);
			enemyAnimator.SetLayerWeight(1, 1);
			enemyAnimator.SetLayerWeight(2, 2);
			enemy.canTakeDamage = false;
			isWaiting = true;
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
			isWaiting = false;
			enemyAnimator.SetLayerWeight(2, 0);
			enemy.canTakeDamage = true;
			evadeTime = Random.Range(minEvadeTime, maxEvadeTime);
		}
	}
	#endregion

	#endregion

	#region Coroutines (Empty)
	#endregion
}