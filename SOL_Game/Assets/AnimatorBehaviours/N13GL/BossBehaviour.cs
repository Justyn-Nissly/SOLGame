using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : StateMachineBehaviour
{
	int timeBeforeAttack;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		timeBeforeAttack = 0;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (timeBeforeAttack == 150)
		{
			if (animator.GetBool("IsGun"   ) || animator.GetBool("IsHammer") ||
				animator.GetBool("IsShield") || animator.GetBool("IsSword" ) == true)
			{
				animator.SetTrigger("Attack");
			}
			else if (animator.GetBool("IsN1G3L") == true)
			{
				animator.SetTrigger("N1G3L");
			}
			else
			{
				animator.SetTrigger("Move");
			}
		}
		else
		{
			timeBeforeAttack += 1;
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.ResetTrigger("Move");
		animator.ResetTrigger("Attack");
	}
}