using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public SnakeManager
		snakeManager; // reference to the snake manager

	public List<Transform>
		bodyParts = new List<Transform>(); // a list of all the snakes body parts

	public float
		minDistance = .3f; // the distance between each snake body part

	public Transform
		LeftUpperLimit, // the LeftUpperLimit that the snake will turn around at
		RightLowerLimit; // the RightLowerLimit that the snake will turn around at

	public float
		movementSpeed = 3, // the snakes movement speed
		HeadRotationSpeedPerCount = 15, // the speed that the head rotates by while counting up and down
		HeadRotationSpeedBase = 8, // a base head rotation speed
		uperLowerDegreeRotationLimit = 16; // the degree number that the head rotates back and forth with (-N to N and back down to -N ex.)
	public bool
		rotateBackAndForth = true, // flag for if the head should rotate Back And Forth
		stopMoving = false; // a flag used to stop movement
	#endregion

	#region Private Variables
	private float
		rotationAmount = 1, // the degree amount that the snakes head will rotate this frame
		distance; // distance, used to calculate the snake movement
	private bool
		countingUp = true, // flag for knowing what direction the head is rotating left or right(counting up/counting down)
		coRoutineRunning = false;

	private Transform
		currentBodyPart, // current body part used to calculate the snake movement
		previousBodyPart;  // previous body part used to calculate the snake movement
	#endregion

	#region Main Methods
	// Update is called once per frame
	private void FixedUpdate()
	{
		// if the snake should split and the split coroutine is not running yet start split logic
		if (snakeManager.SnakeIsSplit && coRoutineRunning == false)
		{
			rotateBackAndForth = false; // stop rotating back and forth
			StartCoroutine(CoSetHeadToLookAtTarget(GameObject.FindGameObjectWithTag("Player").transform.position, .25f, 1));
		}
		// apply normal movement if stop moving flag is false
		else if (stopMoving == false)
			Move();


		// turn the snake around because its leaving the outer limits
		if ((bodyParts[0].position.x < LeftUpperLimit.position.x ||
			bodyParts[0].position.x > RightLowerLimit.position.x ||
			bodyParts[0].position.y > LeftUpperLimit.position.y ||
			bodyParts[0].position.y < RightLowerLimit.position.y) && coRoutineRunning == false)
		{
			rotateBackAndForth = false;
			StartCoroutine(CoSetHeadToLookAtTarget(GameObject.FindGameObjectWithTag("Player").transform.position, 0, 1));
		}
	}

	/// <summary> if the snake collides with the other split snake reconnect</summary>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// check what it collided with an enemy and that that enemy was a part of the snake
		if (collision.transform.CompareTag("Enemy") && collision.transform.GetComponent<LeviadrinCollisionUnit>() != null)
		{
			if (snakeManager.canConnect) // check if it is allowed to re connect
			{
				snakeManager.SplitTheSnake = false;
			}
		}
	}
	#endregion

	#region Utility Methods
	///<summary> rotates the head back and forth to get a snake movement effect </summary>
	private void RotateBackAndForth()
	{
		if (rotationAmount <= -uperLowerDegreeRotationLimit && countingUp == false)
		{
			rotationAmount += Time.deltaTime * HeadRotationSpeedPerCount;
			countingUp = true;
		}
		else if (rotationAmount >= uperLowerDegreeRotationLimit && countingUp == true)
		{
			rotationAmount -= Time.deltaTime * HeadRotationSpeedPerCount;
			countingUp = false;
		}
		else if (countingUp == true)
		{
			rotationAmount += Time.deltaTime * HeadRotationSpeedPerCount;
		}
		else if (countingUp == false)
		{
			rotationAmount -= Time.deltaTime * HeadRotationSpeedPerCount;
		}

		bodyParts[0].Rotate(new Vector3(0, 0, rotationAmount) * HeadRotationSpeedBase * Time.deltaTime);
	}

	/// <summary> moves the snake based on the set varables</summary>
	public void Move()
	{
		// move the head a little bit forward
		bodyParts[0].Translate(bodyParts[0].up * movementSpeed * Time.smoothDeltaTime, Space.World);

		//rotate back and forth
		if (rotateBackAndForth)
		{
			RotateBackAndForth();
		}

		// move all the body parts forward
		for (int i = 1; i < bodyParts.Count; i++)
		{
			currentBodyPart = bodyParts[i];
			previousBodyPart = bodyParts[i - 1];

			distance = Vector2.Distance(previousBodyPart.position, currentBodyPart.position);

			Vector2 newPos = previousBodyPart.position;

			float T = Time.deltaTime * distance / minDistance * movementSpeed;

			if (T > .5f)
			{
				T = .5f;
			}

			currentBodyPart.position = Vector2.Lerp(currentBodyPart.position, newPos, T);
			currentBodyPart.rotation = Quaternion.Slerp(currentBodyPart.rotation, previousBodyPart.rotation, T);
		}
	}

	/// <summary>makes the snake look at the target game object</summary>
	public void SetHeadToLookAtTarget(Vector3 target)
	{
		// Get the angle between the enemy and player
		Vector3 dir = target - bodyParts[0].position;
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

		bodyParts[0].rotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
	}
	#endregion

	#region Coroutines
	/// <summary> looks at the target and after N seconds stops so that normal movement can happen</summary>
	public IEnumerator CoSetHeadToLookAtTarget(Vector3 lookAtTarget, float startDelay, float endDelay)
	{
		coRoutineRunning = true;

		yield return new WaitForSeconds(startDelay);

		SetHeadToLookAtTarget(lookAtTarget); // look at the new target

		yield return new WaitForSeconds(endDelay);

		coRoutineRunning = false;
		rotateBackAndForth = true;
	}
	#endregion
}
