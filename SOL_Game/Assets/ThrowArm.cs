using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowArm : StateMachineBehaviour
{
    #region Enums (Empty)
    #endregion

    #region Public Variables
    private GameObject
        origin;
    public bool
        shouldThrow,
        returnOrigin;
    public Vector2
        destination;
    #endregion

    #region Private Variables
    private bool
        moving = false;

    private float
        timerCountdown;
    #endregion

    //  OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timerCountdown = 3;
        returnOrigin = false;
        shouldThrow = true;
        origin = animator.transform.Find("StaticLeftArm").gameObject;
        destination = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //timerCountdown <= 0


        //timerCountdown -= Time.deltaTime;


        /*  if (shouldThrow)
          {
              if (animator.transform.Find("HomingSword").transform.localRotation.z < -0.7f)
              {
                  if ((Vector2)animator.transform.Find("HomingSword").transform.position == destination)
                  {
                      returnOrigin = true;
                  }
                  else if (animator.transform.Find("HomingSword").transform.position == origin.transform.position)
                  {
                      destination = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
                      returnOrigin = false;
                  }
                  animator.transform.Find("HomingSword").transform.position = (returnOrigin == true) ?
                          Vector2.Lerp(animator.transform.Find("HomingSword").transform.position, origin.transform.position, 10 * Time.deltaTime) :
                          Vector2.Lerp(animator.transform.Find("HomingSword").transform.position, destination, 10 * Time.deltaTime);
              }
          }

       */



        if (shouldThrow)
        {
            animator.transform.Find("HomingSword").transform.position = (returnOrigin == true) ?
                    Vector2.Lerp(animator.transform.Find("HomingSword").transform.position, origin.transform.position, 10 * Time.deltaTime) :
                    Vector2.Lerp(animator.transform.Find("HomingSword").transform.position, destination, 10 * Time.deltaTime);

            if(Vector2.Distance(animator.transform.Find("HomingSword").transform.position, destination) < 0.05f)
            {
                returnOrigin = true;
            }
            else if(Vector2.Distance(animator.transform.Find("HomingSword").transform.position, origin.transform.position) < 0.05f)
            {
                animator.transform.Find("HomingSword").transform.position = origin.transform.position;
                returnOrigin = false;
                shouldThrow = false;
            }
        }





        /*

        if (animator.transform.Find("HomingSword").transform.localRotation.z < -0.7f &&
            animator.transform.Find("HomingSword").transform.position == origin.transform.position)
        {
            animator.SetTrigger("Patrol");
        }
        */
    }

    //  OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}