using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttactOrientationControllerEnemy : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public GameObject gameObjectToLookAt;
    #endregion

    #region Private Variables
    private Enemy
        enemy; // Access the enemy's members
    #endregion

    // Unity Named Methods
    #region Main Methods
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void FixedUpdate()
	{
		if (gameObjectToLookAt != null && enemy.aggro)
		{
			LookInEightDirectionOfGameObject();
		}
	}
	#endregion

	#region Utility Methods
	public void LookInEightDirectionOfGameObject()
	{
		// This makes the game object that this script is attached to rotate on the z axis to look at the game object to look at
		// and that game object "looks" in only orthogonal and diagonal directions
		Vector3 dir = gameObjectToLookAt.transform.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 22.5f;

		// 0 <= angle < 360
		if (angle < 0.0f)
		{
			angle += 360.0f;
		}
        if (angle >= 360.0f)
        {
            angle -= 360.0f;
        }
        angle = (float) (((int) angle) / 45) * 45.0f;

        // apply rotation
        transform.rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
	}
	#endregion

	#region Coroutines
	#endregion
}