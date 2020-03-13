using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGuardianBehaviour : StateMachineBehaviour
{
	GameObject
		currentArm, // The current arm being used by the guardian
		nextArm;    // The next arm to be spawned in
	N13GL
		guardian; 
	private Color32 guardianColour = new Color32(0x3C, 0x71, 0x6F, 0xFF);
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		currentArm = guardian.currentGuardian;
		nextArm    = guardian.shieldGuardianArm;
		guardian.currentGuardianPattern = N13GL.AttackPattern.shieldGuardianPattern;
		guardian.typeIsChanged = true;
		currentArm.GetComponent<_2dxFX_NewTeleportation2>().TeleportationColor = guardianColour;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
	{
		float percentageComplete = 0; // The percentage of completion the teleport animation is at

		// Set the arm that is to be teleported in to invisable
		nextArm.GetComponent<_2dxFX_NewTeleportation2>()._Fade = 1;

		// Teleport the current arm away
		while (percentageComplete < 1)
		{
			currentArm.GetComponent<_2dxFX_NewTeleportation2>()._Fade = Mathf.Lerp(0f, 1f, percentageComplete);
			percentageComplete += Time.deltaTime;
		}

		// Teleport the new arm in
		percentageComplete = 0;
		while (percentageComplete < 1)
		{
			nextArm.GetComponent<_2dxFX_NewTeleportation2>()._Fade = Mathf.Lerp(1f, 0f, percentageComplete);
			percentageComplete += Time.deltaTime;
		}
		currentArm.SetActive(false);
		nextArm.GetComponent<_2dxFX_NewTeleportation2>()._Fade = 0;
	}
}
