using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterBehaviour : MonoBehaviour
{
    //locomotion
    //[SerializeField]
    //private float _mass = 75; // [kg]
    [SerializeField]
    private float _acceleration = 3; // [m/s^2]

    //dependencies
    [SerializeField]
    private Transform _absoluteForward;

    [SerializeField]
    private float _maxRunningSpeed = (30.0f * 1000) / (60 * 60); // [m/s], 30km/h

    private InputController _ic = new InputController();

    private CharacterController _characterController;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _movement;
    private bool _jump;
    private bool _isJumping;
    [SerializeField]
    private float _jumpHeight = 1f;
    

    [SerializeField]
    private float _dragOnGround = 3f;


    //animations
    private Animator _animator;
    private int _horizontalVelocityParam = Animator.StringToHash("HorizontalVelocity");
    private int _verticalVelocityParam = Animator.StringToHash("VerticalVelocity");
    private int _isJumpParam = Animator.StringToHash("IsJumping");

    // Use this for initialization
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //_movement = _ic.KeyMove();
        _movement = _ic.GetLeftJoystickInput();

        Debug.Log(_movement);
        if (_ic.IsButtonAPressed())
        {
            _isJumping = true;
        }
    }

    void FixedUpdate()
    {
        ApplyGravity();
        if (_characterController.isGrounded)
        {
            _jump = false;
            ApplyGround();
            ApplyMovement();
            ApplyGroundDrag();
            
            LimitMaximumRunningSpeed();

            ApplyJump();
        }

        MoveAnimation();
        JumpAnimation();

        DoMovement();

    }

    private void ApplyGravity()
    {
        _velocity += Physics.gravity * Time.fixedDeltaTime; // g[m/s^2] * t[s]
    }

    private void ApplyGround()
    {
        _velocity -= Vector3.Project(_velocity, Physics.gravity);
    }

    private void ApplyMovement()
    {
        Vector3 relativeMovement = RelativeDirection(_movement);

        _velocity += relativeMovement * _acceleration * Time.fixedDeltaTime; // F(= m.a) [m/s^2] * t [s]
    }

    private Vector3 RelativeDirection(Vector3 direction)
    {
        Vector3 xzForward = Vector3.Scale(_absoluteForward.forward, new Vector3(1, 0, 1));

        Quaternion relativeRotation = Quaternion.LookRotation(xzForward);

        return relativeRotation * direction;
    }

    private void ApplyGroundDrag()
    {
        _velocity = _velocity * (1 - Time.deltaTime * _dragOnGround);
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
        Vector3 displacement = _velocity * Time.fixedDeltaTime;

        _characterController.Move(displacement);
    }

    private void ApplyJump()
    {
        if (_isJumping && _characterController.isGrounded)
        {
            _jump = true;
            _characterController.center = new Vector3(0, 1.4f, 0);
            _characterController.height = 1.4f;
            _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _isJumping = false;
        }
        else
        {
            _characterController.center = new Vector3(0, 1, 0);
            _characterController.height = 2f;
        }
    }

    private void MoveAnimation()
    {
        Vector3 relativeMovement = RelativeDirection(_movement);

        Vector3 localVelocityXZ = gameObject.transform.InverseTransformDirection(relativeMovement);
        
        
        _animator.SetFloat(_horizontalVelocityParam, _movement.x);
        _animator.SetFloat(_verticalVelocityParam, _movement.z);
    }

    public void JumpAnimation()
    {
        _animator.SetBool(_isJumpParam, _jump);
    }
}
