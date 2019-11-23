using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour
{
    #region Enums
    public enum MovementType
    {
        MoveTowards,
        LerpTowards
    }
    #endregion

    #region Public Variables
    public MovementType
		type; // Movement type used
	public MovementPath
		movePath;
	public float
		speed;
    #endregion

    #region Private Variables
    private IEnumerator<Transform>
		pointInPath; // Reference movePath.GetNextPathPoint
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Assign the movement path </summary>
	public void Start()
    {
		type = MovementType.MoveTowards; // Movement type used

        // Make sure there is a path assigned
        if (movePath == null)
        {
            Debug.LogError("ERROR: movement path null. ", gameObject);
            return;
        }

        // Set reference to GetNextPathPoint
        pointInPath = movePath.GetNextPathPoint();

        // Get the next path point
        pointInPath.MoveNext();

        //Make sure there is a point to move to
        if (pointInPath.Current == null)
        {
            Debug.LogError("A path must have points in it to follow. ", gameObject);
            return;
        }

        // Set this object to the starting point
        transform.position = pointInPath.Current.position;
    }
     
    // Update is called by Unity every frame
    public void Update()
    {
        // Validate the movement path
        if (pointInPath == null || pointInPath.Current == null)
        {
            return;
        }

		// Move to the next point smoothly
		if (type == MovementType.MoveTowards)
        {
            transform.position = Vector3.MoveTowards(transform.position,
			                                         pointInPath.Current.position,
			                                         speed * Time.deltaTime);
        }
		// Lerp to the next point
		else if (type == MovementType.LerpTowards)
        {
            transform.position = Vector3.Lerp(transform.position,
			                                  pointInPath.Current.position,
			                                  speed * Time.deltaTime);
        }

		// If the enemy has reached the next point get the following point in MovementPath
		if (Vector2.Distance(transform.position, pointInPath.Current.position) < 0.1f)
        {
            pointInPath.MoveNext();
        }
    }
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}