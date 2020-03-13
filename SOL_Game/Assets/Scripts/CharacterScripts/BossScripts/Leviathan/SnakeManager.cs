using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public SnakeMovement
		FullSnake, // reference to the full snake movement logic
		HalfSnakeHead, // reference to the Half Snake with a Head movement logic
		HalfSnakeBody; // reference to the Half Snake with only a Body movement logic

	public bool
		SplitTheSnake = false, // flag to start spliting the snake
		SnakeIsSplit = false; // flag for if the snake is split or not

	public Animator
		animator; // reference to the snakes head animation

	public bool
		canConnect = false, // can reconnect flag
		canMove = false; // can move flag that set the can move value on each snake
	#endregion

	#region Private Variables
	private float
		splitCountDownTimer = 5, // count down timer, so that the snake doest reconnect till after this timer is done
		TotalSplitTime = 5; // count down time interval, so that the snake doest reconnect till after this timer is done
	#endregion

	#region Main Methods
	private void Start()
	{
		// set the snake to be whole by default
		FullSnake.enabled = true;
		HalfSnakeHead.enabled = false;
		HalfSnakeBody.enabled = false;
	}

	private void FixedUpdate()
	{
		// Split the snake if the flag is set and the snake is not split yet
		if (SnakeIsSplit == false && SplitTheSnake)
		{
			SplitSnake();
			canConnect = false;
			splitCountDownTimer = TotalSplitTime;
		}
		// Connect the snake if the flag is set and the snake is not connected yet
		else if (SnakeIsSplit && SplitTheSnake == false && canConnect)
		{
			ConnectSnake();
		}

		// Start the countdown timer till the snake can re connect
		if (SnakeIsSplit && canConnect == false)
		{
			splitCountDownTimer -= Time.deltaTime;

			if (splitCountDownTimer <= 0)
			{
				canConnect = true;
			}
		}

		// Set all the movement flags in the snake movement script to the can move flag
		if (canMove)
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
	#endregion

	#region Utility Methods
	/// <summary> Logic to split the snake </summary>
	private void SplitSnake()
	{
		SnakeIsSplit = true;
		FullSnake.enabled = false;
		HalfSnakeHead.enabled = true;
		HalfSnakeBody.enabled = true;
		animator.SetBool("SnakeIsSplit", true);
	}

	/// <summary> Logic to connect the snake </summary>
	private void ConnectSnake()
	{
		SnakeIsSplit = false;
		FullSnake.enabled = true;
		HalfSnakeHead.enabled = false;
		HalfSnakeBody.enabled = false;
		animator.SetBool("SnakeIsSplit", false);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
