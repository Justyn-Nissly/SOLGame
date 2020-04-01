using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianMoveBehaviour : StateMachineBehaviour
{
	public float
		speed; // The movement speed of the guardian
	int timeToWait; // The time the enemy waits before attacking

	Transform player;       // The transform of the player
	Rigidbody2D guardianRB; // The ridged body of the guardian

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		timeToWait = 0;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		guardianRB = animator.GetComponent<Rigidbody2D>();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (timeToWait == 150)
		{
			timeToWait = 0;
			/*animator.SetTrigger("Attack");*/
		}
		else
		{
			Vector2 target = new Vector2(player.position.x, player.position.y);
			Vector2 newPos = Vector2.Lerp(guardianRB.position, target, speed * Time.fixedDeltaTime);
			guardianRB.MovePosition(newPos);
			timeToWait += 1;
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger("Attack");
	}
}