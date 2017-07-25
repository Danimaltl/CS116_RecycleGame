using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyesController : StateMachineBehaviour {
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
		//  Random blinking
		int blinkFrequency = 250; //  bigger number = fewer blinks
		if (Random.Range(0, blinkFrequency) <= 1) {
			animator.SetTrigger ("blinkTrigger");
		}

		//  chenge expression to match current mood
		//  These bins have like 8 levels of grumpy 2 neutral and 1 happy 

		//if (animator != null && 
		//	animator.gameObject != null) 
		//{
			bin_controller bin = animator.gameObject.transform.root.GetComponent<bin_controller> ();
			//Debug.Log ("mood: " + bin.getMood () / 11f);
			animator.Play (stateInfo.fullPathHash, -1, bin.getMood () / 11f);
		//}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
	/*
	//  Messy utility function
	private GameObject getHighestParent (GameObject child){
		//  Get parent's parent's parent's... parent etc
		GameObject nextParent = child.transform.parent.gameObject;
		if (nextParent != null) {
			return getHighestParent (nextParent);
		} else {
			//  Child is already the highest parent
			return child;
		}
	} */
}
