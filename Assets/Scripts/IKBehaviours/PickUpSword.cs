﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSword : StateMachineBehaviour {

    private Transform _rightHand;
    private AnimationController _ac;
    private GameObject _sword;
    private float _iKWeight;

    private HUDPanelTriggers _hudPaneltrigger;
    private SwordController _sc;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _iKWeight = 0;
        _sc = _sword.GetComponent<SwordController>();
        _hudPaneltrigger.HideTriggerPanels();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
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
        //before taking sword
        if (stateInfo.normalizedTime < .4f)
            _iKWeight = Mathf.Lerp(_iKWeight, 1, .5f);
        else
        {
            //take the sword
            if (stateInfo.normalizedTime < .9f)
            {
                _sc.TakeSword(_rightHand);
            }
            // use the correct animation
            _ac = new AnimationController(animator);
            _ac.UseSwordLocomotionAnimation(_sc.IsSwordInHand);
            
            _iKWeight = Mathf.Lerp(_iKWeight, 0, .5f);
        }

        //IK
        animator.SetIKPosition(AvatarIKGoal.RightHand, _sc.RightHand.position);
        //animator.SetIKRotation(AvatarIKGoal.RightHand, _sc.RightHand.rotation); //-> hand doet raar als dit aanstaat
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _iKWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _iKWeight);
    }

    public void SetBehaviourFields(Transform rightHand, GameObject sword, HUDPanelTriggers hudPaneltrigger)
    {
        _rightHand = rightHand;
        _sword = sword;
        _hudPaneltrigger = hudPaneltrigger;
    }
}
