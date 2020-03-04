using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGuardianBehaviour : StateMachineBehaviour
{
	private MonoBehaviour monoBehaviour = new MonoBehaviour();

	public GameObject n1g3lControler;
	N13GL n13glControl;
	private Color32 guardianColour = new Color32(0x3C, 0x71, 0x6F, 0xFF);
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		n13glControl = animator.GetComponent<N13GL>();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		/*// check if the boss should start charging at the player
		//if (n13glControl.canShoot && n13glControl.canAttack && n13glControl.PlayerInShootingLane())
		//{
		//	n13glControl.canAttack = false;
		//	n13glControl.canShoot = false;
		//	n13glControl.RandomlySetShootingPoint();
		//	animator.SetTrigger("shootBlaster");
		//}
		if (n13glControl.isCharging == false && n13glControl.isStunned == false && n13glControl.canAttack)
		{
			// make the boss charge at the player
			n13glControl.startthing();
		}



		// if the enemy should be shacking start shacking the enemy
		if (n13glControl.enemyIsShacking)
		{
			animator.transform.position = new Vector2(animator.transform.position.x + (Mathf.Sin(Time.time * n13glControl.shackSpeed) * n13glControl.shackAmount), 
				                                      animator.transform.position.y + (Mathf.Sin(Time.time * n13glControl.shackSpeed) * n13glControl.shackAmount));
		}
		Debug.Log("doing the thing");*/
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
	{
		
	}
}