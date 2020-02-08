using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerGuardianMovement : MonoBehaviour
{
	#region Enums and Defined Constants (Empty)
	public const float
		MAX_REFRESH_TIME = 0.65f, // Maximum amount of time before checking player position
		MIN_REFRESH_TIME = 0.2f;  // Minimum amount of time before checking player position
	#endregion

	#region Public Variables (Empty)
	public float
		moveSpeed; // How fast the guardian moves
	#endregion

	#region Private Variables
	private Vector2
		playerPos; // The player's position
	private float
		refreshTargetTime,  // 
		refreshTargetTimer; // 
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Initalize the enemy path
	void Start()
	{
		refreshTargetTime = MIN_REFRESH_TIME;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		refreshTargetTime  = MAX_REFRESH_TIME - 0.05f * Vector2.Distance(transform.position, playerPos);
		refreshTargetTime  = (refreshTargetTime < MIN_REFRESH_TIME) ? MIN_REFRESH_TIME : refreshTargetTime;

		if (refreshTargetTimer > 0.0f)
		{
			refreshTargetTimer -= Time.deltaTime;
		}
		else
		{
			playerPos = GameObject.FindWithTag("Player").transform.position;
			transform.rotation = new Quaternion(0.0f, 0.0f, Mathf.Atan2(playerPos.y - transform.position.y,
																		playerPos.x - transform.position.x), 3.0f);
			refreshTargetTimer = refreshTargetTime;
		}
		Pursue();
	}
	#endregion

	#region Utility Methods
	public void Pursue()
	{
		transform.position = Vector2.MoveTowards(transform.position,
		                                         playerPos,
		                                         moveSpeed * Time.deltaTime);
	}
	#endregion
}