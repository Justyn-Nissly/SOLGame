using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBomb : StateMachineBehaviour
{
	// State machine finishes this state and the bomb is destroyed
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Destroy(animator.transform.gameObject, 1.0f);
	}
}