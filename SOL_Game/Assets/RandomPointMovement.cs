using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointMovement : StateMachineBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		 speed;

	public Transform
		upperLeftSpawnPointLimit,  // used to get a random position between these two limits
		lowerRightSpawnPointLimit; // Location you wish the enemy to move to
	#endregion

	#region Private Variables
	private bool
		moving = true;
	Vector2
        randomPosition;
	private Vector3
        targetGameObject;
	#endregion

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		randomPosition = new Vector2
            (Random.Range(upperLeftSpawnPointLimit.position.x, lowerRightSpawnPointLimit.position.x),
            Random.Range(upperLeftSpawnPointLimit.position.y, lowerRightSpawnPointLimit.position.y));
	}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Chnage animations after movement is complete
		if ((Vector2)animator.transform.position == randomPosition)
		{
			animator.SetTrigger("Attack");
		}

		// Move character to a random position on between to desired markers
		animator.transform.position = Vector2.MoveTowards(animator.transform.position, randomPosition, speed * Time.deltaTime);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
