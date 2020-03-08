using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{

	public List<Transform> bodyParts = new List<Transform>();

	public float minDistance = .3f;

	public Transform LeftUpperLimit;
	public Transform RightLowerLimit;
	public Transform centerLeftUpperLimit;
	public Transform centerRightLowerLimit;
	public Transform target;

	public int beginsize;

	public float speed = 3;
	public float rotationSpeed = 15;
	public float rotSPPPEddd = 8;
	public float uperLimit = 16;
	public bool rotateBackAndForth = true;
	public bool stopMoving = false;

	private float rotationAmount = 1;
	private bool countingUp = true;
	private bool coRoutineRunning = false;
	private float dis;
	private Transform curBodyPart;
	private Transform prevbodyPart;

	// Update is called once per frame
	private void FixedUpdate()
	{
		if(stopMoving == false)
			Move();

		if((bodyParts[0].position.x < LeftUpperLimit.position.x ||
			bodyParts[0].position.x > RightLowerLimit.position.x ||
			bodyParts[0].position.y > LeftUpperLimit.position.y ||
			bodyParts[0].position.y < RightLowerLimit.position.y) && coRoutineRunning == false)
		{
			rotateBackAndForth = false;
		}
	}

	private void RotateBackAndForth()
	{
		if (rotationAmount <= -uperLimit && countingUp == false)
		{
			rotationAmount += Time.deltaTime * rotationSpeed;
			countingUp = true;
		}
		else if (rotationAmount >= uperLimit && countingUp == true)
		{
			rotationAmount -= Time.deltaTime * rotationSpeed;
			countingUp = false;
		}
		else if (countingUp == true)
		{
			rotationAmount += Time.deltaTime * rotationSpeed;
		}
		else if (countingUp == false)
		{
			rotationAmount -= Time.deltaTime * rotationSpeed;
		}

		bodyParts[0].Rotate(new Vector3(0, 0, rotationAmount) * rotSPPPEddd * Time.deltaTime);
	}

	public void Move()
	{
		// move a little bit forward
		bodyParts[0].Translate(bodyParts[0].up * speed * Time.smoothDeltaTime, Space.World);

		if(rotateBackAndForth)
		{
			RotateBackAndForth();
		}

		if (rotateBackAndForth == false && coRoutineRunning == false)
		{
			//StartCoroutine(Rotate(180));
			StartCoroutine(lookatgameobject());
		}

		for (int i = 1; i < bodyParts.Count; i++)
		{
			curBodyPart = bodyParts[i];
			prevbodyPart = bodyParts[i - 1];

			dis = Vector2.Distance(prevbodyPart.position, curBodyPart.position);

			Vector2 newPos = prevbodyPart.position;

			float T = Time.deltaTime * dis / minDistance * speed;

			if(T > .5f)
			{
				T = .5f;
			}

			curBodyPart.position = Vector2.Lerp(curBodyPart.position, newPos, T);
			curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, prevbodyPart.rotation, T);
		}
	}

	public void LookAtTarget()
	{
		// Get the angle between the enemy and player
		Vector3 dir = target.transform.position - bodyParts[0].position;
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

	/// <summary> gets a random gameobject from the list of pop up positions</summary>
	private Vector2 GetRandomPositionBeweenLimits()
	{
		Vector2 randomPosition = new Vector2();

		// set the random psition to be in the range of the set limits
		randomPosition.x = Random.Range(centerLeftUpperLimit.position.x, centerRightLowerLimit.position.x);
		randomPosition.y = Random.Range(centerLeftUpperLimit.position.y, centerRightLowerLimit.position.y);

		return randomPosition;
	}

	private IEnumerator lookatgameobject()
	{
		coRoutineRunning = true;
		target.position = GetRandomPositionBeweenLimits();
		LookAtTarget();

		yield return new WaitForSeconds(1);

		coRoutineRunning = false;
		rotateBackAndForth = true;
	}

	private IEnumerator Rotate(float degrees)
	{
		coRoutineRunning = true;
		float currentRotationAmount = 0;

		while (currentRotationAmount <= degrees)
		{
			bodyParts[0].Rotate(new Vector3(0, 0, 2));
			currentRotationAmount += 2;
			yield return new WaitForSeconds(.01f);
		}

		coRoutineRunning = false;
		rotateBackAndForth = true;

	}
}
