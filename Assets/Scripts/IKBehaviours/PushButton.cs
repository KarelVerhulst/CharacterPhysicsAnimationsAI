using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : StateMachineBehaviour {

    private Transform _button;
    private float _iKWeight;
    private SwitchAction _sa;
    private bool _isClosed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _iKWeight = 0;
        _button = GameObject.Find("button").transform;
        _sa = _button.GetComponentInParent<SwitchAction>();
        _isClosed = !_isClosed;
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
        //Debug.Log(stateInfo.normalizedTime);
        if (stateInfo.normalizedTime < .9f)
        {
            _iKWeight = Mathf.Lerp(_iKWeight, 1, 5f);
        }
        else
        {
            _sa.OpenCloseGates(_isClosed);
            _iKWeight = Mathf.Lerp(_iKWeight, 0, .5f);
        }

        //IK
        animator.SetIKPosition(AvatarIKGoal.RightHand, _button.position);
        //animator.SetIKRotation(AvatarIKGoal.RightHand, _button.rotation); 
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _iKWeight);
        //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _iKWeight);

        //animator.SetIKHintPosition(AvatarIKHint.RightElbow,Vector3.zero);
    }
}
