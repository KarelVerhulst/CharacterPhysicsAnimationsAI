using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSword : StateMachineBehaviour {

    [SerializeField]
    private Transform _weaponHandle;
    [SerializeField]
    private Transform _rightHand;

    private AnimationController _ac;
    private ActionController _actionC;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _weaponHandle = GameObject.Find("SwordGrabHandle").transform;
        _rightHand = GameObject.Find("mixamorig:RightHand").transform;
        _actionC = new ActionController();
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
        if (_weaponHandle != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, _weaponHandle.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, _weaponHandle.rotation);

            

            Debug.Log(stateInfo.normalizedTime);

            if (stateInfo.normalizedTime >= 0.45f)
            {
                _weaponHandle.parent = _rightHand.transform;
                _actionC.IsSwordInHand = true;

                _ac = new AnimationController(animator);
                _ac.UseSwordLocomotionAnimation(_actionC.IsSwordInHand);
            }
        }
    }
}
