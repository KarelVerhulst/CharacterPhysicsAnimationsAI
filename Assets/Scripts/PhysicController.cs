using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicController {

    /*
     * All the physics from the character is found in this script
     */

    private CharacterController _characterController;
    private float _maxRunningSpeed; // [m/s], 30km/h
    private float _jumpHeight;
    private float _dragOnGround;
    private float _acceleration; // [m/s^2]
    private bool _isGravity;
    private Transform _absoluteForward;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _movement;

    //constructor
    public PhysicController(CharacterController characterController, float maxRunningSpeed, float jumpHeight)
    {
        _characterController = characterController;
        _maxRunningSpeed = maxRunningSpeed;
        _jumpHeight = jumpHeight;
    }

    //public methods
    public void PhysicUpdate(Vector3 movement, bool isGravity)
    {
        _movement = movement;
        _isGravity = isGravity;
    }

    public void FixedPhysicUpdate(Transform absoluteForward, bool jump, float dragOnGround, float acceleration)
    {
        _absoluteForward = absoluteForward;
        _dragOnGround = dragOnGround;
        _acceleration = acceleration;

        //Locomotions
        ApplyGround();
        ApplyGravity();
        ApplyMovement();
        ApplyGroundDrag();

        LimitMaximumRunningSpeed();

        ApplyJumpPhysic(jump);

        DoMovement();
    }

    // private methods
    private void ApplyGravity()
    {
        if (_isGravity && !_characterController.isGrounded)
        {
            _velocity += Physics.gravity * Time.deltaTime; // g[m/s^2] * t[s]
        }
    }

    private void ApplyGround()
    {
        if (_characterController.isGrounded)
        {
            _velocity -= Vector3.Project(_velocity, Physics.gravity);
        }
    }

    private void ApplyMovement()
    {
        if (_characterController.isGrounded)
        {
            ResetVelocity();

            Vector3 relativeMovement = RelativeDirection(_movement);

            _velocity += relativeMovement * _acceleration * Time.deltaTime; // F(= m.a) [m/s^2] * t [s]
        }
    }

    private Vector3 RelativeDirection(Vector3 direction)
    {
        Vector3 xzForward = Vector3.Scale(_absoluteForward.forward, new Vector3(1, 0, 1));

        Quaternion relativeRotation = Quaternion.LookRotation(xzForward);

        return relativeRotation * direction;
    }

    private void ApplyGroundDrag()
    {
        if (_characterController.isGrounded)
        {
            _velocity = _velocity * (1 - Time.deltaTime * _dragOnGround);
        }
    }

    private void LimitMaximumRunningSpeed()
    {
        Vector3 yVelocity = Vector3.Scale(_velocity, new Vector3(0, 1, 0));
        Vector3 xzVelocity = Vector3.Scale(_velocity, new Vector3(1, 0, 1));

        Vector3 clampedXzVelocity = Vector3.ClampMagnitude(xzVelocity, _maxRunningSpeed);

        _velocity = yVelocity + clampedXzVelocity;
    }

    private void DoMovement()
    {
        Vector3 displacement = _velocity * Time.deltaTime;

        _characterController.Move(displacement);
    }

    private void ApplyJumpPhysic(bool jump)
    {
        if (jump)
        {
            _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
        }
    }
    
    private void ResetVelocity()
    {
        if (_movement.magnitude <= 0.1) //if joystick is not used character stop immediately set _velocity to 0
        {
            _velocity.x = 0;
            _velocity.z = 0;
        }
    }
    
}
