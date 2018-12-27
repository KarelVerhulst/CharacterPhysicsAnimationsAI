using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController {

    private Animator _animator;

    public AnimationController(Animator animator)
    {
        _animator = animator;
    }

    // animation params
    private int _horizontalVelocityParam = Animator.StringToHash("HorizontalVelocity");
    private int _verticalVelocityParam = Animator.StringToHash("VerticalVelocity");
    private int _isJumpParam = Animator.StringToHash("IsJumping");
    private int _isCrouchParam = Animator.StringToHash("IsCrouch");
    private int _horizontalRotationParam = Animator.StringToHash("HorizontalRotation");
    private int _isPickUpObjectParam = Animator.StringToHash("IsPickUpObject");
    private int _isSwordInHandParam = Animator.StringToHash("IsSwordInHand");
    private int _IsWeaponArmedParam = Animator.StringToHash("IsWeaponArmed");



    public void MoveAnimation(Vector3 movement)
    {
        _animator.SetFloat(_horizontalVelocityParam, movement.x);
        _animator.SetFloat(_verticalVelocityParam, movement.z);
    }

    public void UseSwordLocomotionAnimation(bool isSwordInHand)
    {
        _animator.SetBool(_isSwordInHandParam, isSwordInHand);
    }

    public void JumpAnimation(bool isJump)
    {
        _animator.SetBool(_isJumpParam, isJump);
    }

    public void CrouchAnimation(bool isCrouch)
    {
        _animator.SetBool(_isCrouchParam, isCrouch);
    }

    public void RotateCameraAnimation(float camHorRotation)
    {
        _animator.SetFloat(_horizontalRotationParam, camHorRotation);
    }

    public void PickupObjectAnimation(bool isPickup)
    {
        _animator.SetBool(_isPickUpObjectParam, isPickup);
    }

    public void DisArmWeaponWeapon(bool isDisarm)
    {
        _animator.SetBool(_IsWeaponArmedParam, isDisarm);
    }
}
