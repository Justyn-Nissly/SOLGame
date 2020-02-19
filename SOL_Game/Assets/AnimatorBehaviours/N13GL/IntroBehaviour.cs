using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBehaviour : StateMachineBehaviour
{
	public GameObject
		N13GL;     // The reference to N13GL
	public bool
		isFadedIn; // Check if N13GL has finished his fade in animation

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		// Get the the reference to N13GL
		N13GL = GameObject.FindObjectOfType<N13GLEncounter>().gameObject;

		// Set the the animator to its starting values
		N13GL.GetComponent<_2dxFX_BurnFX>().Destroyed = 1.0f;
		isFadedIn = false;
		animator.SetBool("IsActive", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		// Start the intro animation if the encounter has started
		if (animator.GetBool("IsActive") == true)
		{
			// Decrease the "Destroyed" value of the shader and then set the shader type to "Glitch"
			if (N13GL.GetComponent<_2dxFX_BurnFX>().Destroyed >= 0.009f && isFadedIn == false)
			{
				N13GL.GetComponent<_2dxFX_BurnFX>().Destroyed -= 0.007f;
			}
			else
			{
				isFadedIn = true;
				N13GL.GetComponent<Renderer>().material.shader = Shader.Find("2DxFX_Extra_Shaders/Glitch");
				N13GL.GetComponent<_2dxFX_BurnFX>().Destroyed  = 1.0f;
				N13GL.GetComponent<_2dxFX_BurnFX>().Seed       = 0.051f;

				// Start the create guardian animation and behaviours
				animator.SetTrigger("CreateGuardian");
			}
		}
	}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
 
    }
}