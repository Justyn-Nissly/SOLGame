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
    #endregion

    #region Private Variables

    private string patternType;
    private int patternAlarm;
	private float xSpeed;
	private float ySpeed;
	private int facingX; // -1 = facing left, 1 = facing right, 0 = facing up/down
	private int facingY; // -1 = facing down, 1 = facing up, 0 = facing left/right
	private Rigidbody2D myRigidBody;

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
			}
		}
	}
}
