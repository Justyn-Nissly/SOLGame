using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGuardianBehaviour : StateMachineBehaviour
{
	N13GL n13glControl;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		n13glControl = animator.GetComponent<N13GL>();
		if (animator.GetBool("IsShield"))
		{
			n13glControl.currentGuardianPattern = N13GL.AttackPattern.shieldGuardianPattern;
		}
		else if (animator.GetBool("IsGun"))
		{
			n13glControl.currentGuardianPattern = N13GL.AttackPattern.gunGuardianPattern;
		}
		else if (animator.GetBool("IsHammer"))
		{
			n13glControl.currentGuardianPattern = N13GL.AttackPattern.hammerGuardianPattern;
		}
		else if (animator.GetBool("IsSword"))
		{
			n13glControl.currentGuardianPattern = N13GL.AttackPattern.swordGuardianPattern;
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponent<N13GL>().currentGuardianPattern = N13GL.AttackPattern.finalGuardianPattern;
		if (animator.GetBool("IsShield"))
		{
			animator.SetBool("IsShield", false);
		}
		else if (animator.GetBool("IsGun"))
		{
			animator.SetBool ("IsGun", false);
		}
		else if (animator.GetBool("IsHammer"))
		{
			animator.SetBool ("IsHammer", false);
		}
		else if (animator.GetBool("IsSword"))
		{
			animator.SetBool ("IsSword", false);
		}
		animator.SetTrigger  ("Idle");
		animator.ResetTrigger("Idle");
	}
}