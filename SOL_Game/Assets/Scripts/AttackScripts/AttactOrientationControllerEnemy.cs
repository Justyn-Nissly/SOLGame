using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttactOrientationControllerEnemy : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public bool
		shouldLookAtPlayer = true; // this is for toggling this Attack Orientation Controller on/off
	#endregion

	#region Private Variables
	private GameObject
		target;
	private Enemy
		enemy;
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
		if(shouldLookAtPlayer && target != null) // make sure a target is assigned
		{
			if (enemy != null && enemy.aggro) // if there is an enemy check if aggro
			{
				LookInEightDirectionOfGameObject();
			}
			else if (enemy == null) // if there is no enemy just rotate
			{
				LookInEightDirectionOfGameObject();
			}
		}
	}
	#endregion

	#region Utility Methods
	///<summary> Make the enemy look at the player </summary>
	public void LookInEightDirectionOfGameObject()
	{
		// Get the angle between the enemy and player and keep it within 0 and 360 degrees
		Vector3 dir = target.transform.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 22.5f;
		if (angle < 0.0f)
		{
			angle += 360.0f;
		}
		else if (angle >= 360.0f)
		{
			angle -= 360.0f;
		}

		// Make the enemy look orthagonally or diagonally at the player
		angle = (float)(((int)angle) / 45) * 45.0f;
		transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}