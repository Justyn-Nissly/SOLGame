using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPatrollingSpeed : StateMachineBehaviour
{
    public float
        movementSpeed;

    private MeleeGuardian
        meleeGuardian;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meleeGuardian = GameObject.Find("SwordG_V2").GetComponent<MeleeGuardian>();
        meleeGuardian.moveSpeed = movementSpeed;
    }

}