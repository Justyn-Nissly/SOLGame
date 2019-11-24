using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldGuardian : MonoBehaviour
{
    #region Public Variables
	public int changeAttackRate;
	public float moveSpeed;
	public float lockSpeed;
	public float dashSpeed;
	public GameObject tileMapGameObject;
	public float dashWindowX;
	public float dashWindowY;
    #endregion

    #region Private Variables

    private string patternType;
    private int patternAlarm;
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

    // Start is called before the first frame update
    void Start()
    {
        patternType = "MoveVertical";
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

		if (--patternAlarm == 0)
		{
			// Alarm rings; change pattern.
			switch (patternType)
			{
				case "MoveVertical":
					patternType = "LockOnVertical";
					break;

				case "MoveHorizontal":
					patternType = "LockOnHorizontal";
					break;

				case "LockOnHorizontal":
					patternType = "DashVertical";
					break;

				case "LockOnVertical":
					patternType = "DashHorzontal";
					break;
			}
		}

		// Check if in line with the Player
		if ((patternType == "LockOnVertical" && myRigidBody.position.y <= playerPosition.y + dashWindowY && myRigidBody.position.y >= playerPosition.y - dashWindowY)
			|| (patternType == "LockOnHorizontal" && myRigidBody.position.x <= playerPosition.x + dashWindowX && myRigidBody.position.x >= playerPosition.x - dashWindowX))
		{
			if (patternType == "LockOnVertical")
			{
				patternType = "DashHorizontal";
				facingX = (int)Mathf.Sign(playerPosition.x - myRigidBody.position.x);
				facingY = 0;
				prevYSpeed = ySpeed;
			}
			else if (patternType == "LockOnHorizontal")
			{
				patternType = "DashVertical";
				facingX = 0;
				facingY = (int)Mathf.Sign(playerPosition.y - myRigidBody.position.y);
				prevXSpeed = xSpeed;
			}
		}

		// Manage movement based on current pattern.
			switch (patternType)
        {
			case "MoveVertical":
				xSpeed = 0f;
				ySpeed = Mathf.Sign(ySpeed) * moveSpeed;
				break;
			case "MoveHorizontal":
				xSpeed = Mathf.Sign(xSpeed) * moveSpeed;
				ySpeed = 0f;
				break;
			case "LockOnVertical":
				xSpeed = 0f;
				ySpeed = Mathf.Sign(ySpeed) * lockSpeed;
				break;
			case "LockOnHorizontal":
				xSpeed = Mathf.Sign(xSpeed) * lockSpeed;
				ySpeed = 0f;
				break;
			case "DashVertical":
				xSpeed = 0;
				ySpeed = Mathf.Sign(facingY) * dashSpeed;
				break;
			case "DashHorizontal":
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
		myRigidBody.MovePosition(new Vector2(xSpeed, ySpeed) + myRigidBody.position);
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (tileMapGameObject != null && collision.gameObject == tileMapGameObject)
		{
			// When touching a wall, change stuff
			if (patternType == "MoveHorizontal" || patternType == "MoveVertical")
			{
				xSpeed *= -1f;
				ySpeed *= -1f;
			}
			else if (patternType == "LockOnHorizontal" || patternType == "LockOnVertical")
			{
				patternType = "DashDiagonal";
				dashTarget = playerPosition;
				facingX = (int)Mathf.Sign(playerPosition.x - myRigidBody.position.x);
				facingY = (int)Mathf.Sign(playerPosition.y - myRigidBody.position.y);
			}
			else if (patternType == "DashHorizontal" || patternType == "DashVertical")
			{
				if (patternType == "DashHorizontal")
				{
					patternType = "MoveVertical";
					facingX *= -1;
					facingY = 0;
					xSpeed = 0;
					ySpeed = prevYSpeed;
				}
				else if(patternType == "DashVertical")
				{
					patternType = "MoveHorizontal";
					facingX = 1;
					facingY *= -1;
					xSpeed = prevXSpeed;
					ySpeed = 0;
				}
				patternAlarm = changeAttackRate;
			}
			else
			{
				// Error catch-all, for now.
				patternAlarm = changeAttackRate;
			}
		}
	}
}
