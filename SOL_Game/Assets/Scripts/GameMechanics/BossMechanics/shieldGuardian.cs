using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldGuardian : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public int changeAttackRate;
	public float moveSpeed;
	public float lockSpeed;
	public float dashSpeed;
	public GameObject tileMapGameObject;
	public float dashWindowX;
	public float dashWindowY;
	public int backupTime;
	public float backupSpeed;
	public int cooldownTime;
	#endregion

	#region Private Variables
	private string patternType;
	private float patternAlarm;
	private float xSpeed;
	private float ySpeed;
	private float prevXSpeed;
	private float prevYSpeed;
	private int facingX; // -1 = facing left, 1 = facing right, 0 = facing up/down
	private int facingY; // -1 = facing down, 1 = facing up, 0 = facing left/right
	private Rigidbody2D myRigidBody;
	private Vector2 playerPosition; // current position of the Player
	private Vector2 dashTarget; // Where to dash to
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		patternType = "MoveY";
		patternAlarm = changeAttackRate;
		xSpeed = 0f;
		ySpeed = moveSpeed;
		facingX = 1;
		facingY = 0;
		myRigidBody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		// get the Player's position
		playerPosition = GameObject.FindWithTag("Player").transform.position;

		Debug.Log("ShieldGuardianState=" + patternType);

		patternAlarm -= Time.deltaTime;
		if (patternAlarm <= 0)
		{
			// Alarm rings; change pattern.
			switch (patternType)
			{
				case "MoveY":
					patternType = "LockOnY";
					break;

				case "MoveX":
					patternType = "LockOnX";
					break;

				case "LockOnX":
					patternType = "DashY";
					break;

				case "LockOnY":
					patternType = "DashX";
					break;

				case "CooldownX":
					patternType = "BackupX";
					xSpeed = Mathf.Sign(prevXSpeed) * backupSpeed * -1f;
					patternAlarm = backupTime;
					break;

				case "CooldownY":
					patternType = "BackupY";
					ySpeed = Mathf.Sign(prevYSpeed) * backupSpeed * -1f;
					patternAlarm = backupTime;
					break;

				case "BackupX":
					patternType = "MoveY";
					facingX *= -1;
					facingY = 0;
					xSpeed = 0;
					ySpeed = prevYSpeed;
					patternAlarm = changeAttackRate;
					break;

				case "BackupY":
					patternType = "MoveX";
					facingX = 1;
					facingY *= -1;
					xSpeed = prevXSpeed;
					ySpeed = 0;
					patternAlarm = changeAttackRate;
					break;
			}
			Debug.Log(">ShieldGuardianStateChange=" + patternType);
		}

		// Check if in line with the Player
		if ((patternType == "LockOnY" && myRigidBody.position.y <= playerPosition.y + dashWindowY && myRigidBody.position.y >= playerPosition.y - dashWindowY)
			|| (patternType == "LockOnX" && myRigidBody.position.x <= playerPosition.x + dashWindowX && myRigidBody.position.x >= playerPosition.x - dashWindowX))
		{
			if (patternType == "LockOnY")
			{
				patternType = "DashX";
				facingX = (int)Mathf.Sign(playerPosition.x - myRigidBody.position.x);
				facingY = 0;
				prevYSpeed = ySpeed;
			}
			else if (patternType == "LockOnX")
			{
				patternType = "DashY";
				facingX = 0;
				facingY = (int)Mathf.Sign(playerPosition.y - myRigidBody.position.y);
				prevXSpeed = xSpeed;
			}
		}

		// Manage movement based on current pattern.
		switch (patternType)
		{
			case "MoveY":
				xSpeed = 0f;
				ySpeed = Mathf.Sign(ySpeed) * moveSpeed;
				break;
			case "MoveX":
				xSpeed = Mathf.Sign(xSpeed) * moveSpeed;
				ySpeed = 0f;
				break;
			case "LockOnY":
				xSpeed = 0f;
				ySpeed = Mathf.Sign(ySpeed) * lockSpeed;
				break;
			case "LockOnX":
				xSpeed = Mathf.Sign(xSpeed) * lockSpeed;
				ySpeed = 0f;
				break;
			case "DashY":
				xSpeed = 0;
				ySpeed = Mathf.Sign(facingY) * dashSpeed;
				break;
			case "DashX":
				xSpeed = Mathf.Sign(facingX) * dashSpeed;
				ySpeed = 0;
				break;

			default:
				// error case; start freaking out now
				xSpeed = 0;
				ySpeed = 0;
				break;
		}

		// Update speed.
		//if (!myRigidBody.SweepTest(new Vector3(movement, 0, 0), out hit, new Vector3(movement, 0, 0).magnitude))
		myRigidBody.MovePosition(new Vector2(xSpeed, ySpeed) + myRigidBody.position);
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Walls"))
		{
			// When touching a wall, change stuff
			if (patternType == "MoveX" || patternType == "MoveY" || patternType == "BackupX" || patternType == "BackupY")
			{
				xSpeed *= -1f;
				ySpeed *= -1f;
			}
			else if (patternType == "LockOnX" || patternType == "LockOnY")
			{
				patternType = "DashDiagonal";
				dashTarget = playerPosition;
				facingX = (int)Mathf.Sign(playerPosition.x - myRigidBody.position.x);
				facingY = (int)Mathf.Sign(playerPosition.y - myRigidBody.position.y);
			}
			else if (patternType == "DashX" || patternType == "DashY")
			{
				if (patternType == "DashX")
				{
					patternType = "CooldownX";
					prevXSpeed = xSpeed;
					xSpeed = 0;
					ySpeed = 0;
				}
				else if (patternType == "DashY")
				{
					patternType = "CooldownY";
					prevYSpeed = ySpeed;
					xSpeed = 0;
					ySpeed = 0;
				}
				patternAlarm = cooldownTime;
			}
			else
			{
				// Error catch-all, for now.
				patternAlarm = changeAttackRate;
			}
		}
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion
}
