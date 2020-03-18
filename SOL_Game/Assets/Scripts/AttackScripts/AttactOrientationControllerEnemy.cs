using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttactOrientationControllerEnemy : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public bool
		shouldLookAtPlayer = true, // this is for toggling this Attack Orientation Controller on/off
		lookDirectlyAtPlayer = false;
	#endregion

	#region Private Variables
	private GameObject
		target; // The object to rotate towards
	private Enemy
		enemy; // Reference en enemy
	private float
		offsetVaule = -30; // offset to make it aim in the right direction if its to look directly at the player
	#endregion

	// Unity Named Methods
	#region Main Methods
	///<summary> The enemy tracks the player's position </summary>
	private void Start()
	{
		enemy  = GetComponent<Enemy>();
		target = GameObject.FindGameObjectWithTag("Player").gameObject;
	}

	///<summary> Make aggroed enemies face the player </summary>
	private void FixedUpdate()
	{
		// Make sure a target is assigned before trying to face it
		if(shouldLookAtPlayer && target != null) // make sure a target is assigned
		{
			// Face the player if he is within detection range
			if (enemy != null && enemy.aggro)
			{
				LookInEightDirectionOfGameObject();
			}
			else if (enemy == null)
			{
				LookInEightDirectionOfGameObject();
			}
		}
	}
	#endregion

	#region Utility Methods
	///<summary> Turn an object to an orthogonal or diagonal direction </summary>
	public void LookInEightDirectionOfGameObject()
	{
		// Get the angle between the enemy and player
		Vector3
			dir = target.transform.position - transform.position;
		float
			angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 22.5f;

		// Make sure 0 <= angle < 360
		if (angle < 0.0f)
		{
			angle += 360.0f;
		}
		else if (angle >= 360.0f)
		{
			angle -= 360.0f;
		}

		// Make the object face orthagonally or diagonally towards the target
		if(lookDirectlyAtPlayer == false)
		{
			angle = (float)(((int)angle) / 45) * 45.0f;
		}
		else
		{
			angle += offsetVaule;
		}

		transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
