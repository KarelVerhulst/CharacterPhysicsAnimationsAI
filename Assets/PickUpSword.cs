using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSword : StateMachineBehaviour {

    public Transform WeaponHandle;
    public Transform RightHand;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        WeaponHandle = GameObject.Find("SwordGrabHandle").transform;
        RightHand = GameObject.Find("mixamorig:RightHandIndex1").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (WeaponHandle != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, WeaponHandle.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, WeaponHandle.rotation);

           // Debug.Log(stateInfo.normalizedTime);

            if (stateInfo.normalizedTime >= 0.4971169f)
            {
                WeaponHandle.parent = RightHand.transform;
            }
            //check time normalized time
            //
        }
    }
}
