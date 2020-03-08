using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
	public SnakeMovement 
		FullSnake,
		HalfSnakeHead,
		HalfSnakeBody;

	public bool
		SplitTheSnake = false;
	public bool
		SnakeIsSplit = false;

	public bool canConnect = false;
	public bool CanMove = false;

	private float
		splitCountDownTimer = 5,
		TotalSplitTime = 5;

	private void Start()
	{
		FullSnake.enabled = true;
		HalfSnakeHead.enabled = false;
		HalfSnakeBody.enabled = false;
	}


	private void FixedUpdate()
	{
		if(SnakeIsSplit == false && SplitTheSnake)
		{
			SplitSnake();
			canConnect = false;
			splitCountDownTimer = TotalSplitTime;
		}
		else if(SnakeIsSplit && SplitTheSnake == false && canConnect)
		{
			ConnectSnake();
		}

		if (SnakeIsSplit && canConnect == false)
		{
			splitCountDownTimer -= Time.deltaTime;

			if (splitCountDownTimer <= 0)
			{
				canConnect = true;
			}
		}

		if (CanMove)
		{
			FullSnake.stopMoving = false;
			HalfSnakeHead.stopMoving = false;
			HalfSnakeBody.stopMoving = false;
		}
		else
		{
			FullSnake.stopMoving = true;
			HalfSnakeHead.stopMoving = true;
			HalfSnakeBody.stopMoving = true;
		}
	}

	private void SplitSnake()
	{
		SnakeIsSplit = true;
		FullSnake.enabled = false;
		HalfSnakeHead.enabled = true;
		HalfSnakeBody.enabled = true;
	}

	private void ConnectSnake()
	{
		SnakeIsSplit = false;
		FullSnake.enabled = true;
		HalfSnakeHead.enabled = false;
		HalfSnakeBody.enabled = false;
	}

	public void LookAtPlayer()
	{
		StartCoroutine(FullSnake.lookatgameobject(GameObject.FindGameObjectWithTag("Player").transform.position, 0, 0));
	}
}
