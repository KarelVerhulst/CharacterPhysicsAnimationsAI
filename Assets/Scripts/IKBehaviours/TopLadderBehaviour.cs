﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLadderBehaviour : StateMachineBehaviour {

    private Transform _rightHandLadder;
    private GameObject _character;
    private AnimationController _ac;
    private Transform _characterTopPosition;
    private LadderAction _la;
    
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rightHandLadder = GameObject.Find("RightHandLadder").transform;
        _character = GameObject.Find("Character");
        _ac = new AnimationController(animator);
        _characterTopPosition = GameObject.Find("TopLadderEnd").transform;
        _la = GameObject.Find("Ladder Trigger box 01").GetComponent<LadderAction>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _ac.ClimbAnimation(false, 0);
        _character.GetComponent<CharacterBehaviour>().IsGravity = true;
        
        _la.IsCharacterReadyToClimb = false;
        _character.transform.position = _characterTopPosition.position;
        //Debug.Log("onStateExit");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //IK
        //animator.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Euler(new Vector3(20, 0, 0)));

        animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandLadder.position);
        //animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandLadder.rotation); //-> hand doet raar als dit aanstaat
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

        _ac.EndClimbAnimation(false);
    }
}