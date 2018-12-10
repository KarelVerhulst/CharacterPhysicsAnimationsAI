using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterBehaviour : MonoBehaviour
{
    //locomotion
    [SerializeField]
    private float _acceleration = 3; // [m/s^2]

    //dependencies
    [SerializeField]
    private Transform _absoluteForward;
    [SerializeField]
    private float _maxRunningSpeed = (30.0f * 1000) / (60 * 60); // [m/s], 30km/h

    [SerializeField]
    private float _jumpHeight = 1f;
    [SerializeField]
    private float _dragOnGround = 3f;

    private InputController _ic = new InputController();
    private CharacterController _characterController;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _movement;
    private bool _jump;
    private bool _isJumping;
    private bool _isCrouch;

    float currentHeight;
    Vector3 currentCenter;

    

    //animations
    private Animator _animator;
    private int _horizontalVelocityParam = Animator.StringToHash("HorizontalVelocity");
    private int _verticalVelocityParam = Animator.StringToHash("VerticalVelocity");
    private int _isJumpParam = Animator.StringToHash("IsJumping");
    private int _isCrouchParam = Animator.StringToHash("IsCrouch");

    //test
    private HeadTrigger _headTrigger;

    // Use this for initialization
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = this.GetComponent<Animator>();

        currentHeight = _characterController.height;
        currentCenter = _characterController.center;

        _headTrigger = FindObjectOfType<HeadTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        //_movement = _ic.KeyMove();
        _movement = _ic.GetLeftJoystickInput();
                
        if (_ic.IsButtonAPressed() && !_isCrouch)
        {
            _isJumping = true;
        }

        Debug.Log(_headTrigger.IsInTunnel);
        if (_ic.IsLeftJoystickButtonPressed() && !_headTrigger.IsInTunnel)
        {
            _isCrouch = !_isCrouch;
            _animator.SetBool(_isCrouchParam, _isCrouch);
            EditCharactControllerParams(_isCrouch);
        }
    }
    
    void FixedUpdate()
    {
        //Debug.Log(_velocity.y);
        ApplyGround();
        ApplyGravity();
        ApplyMovement();
        ApplyGroundDrag();

        LimitMaximumRunningSpeed();

        ApplyJump();
        
        MoveAnimation();
        JumpAnimation();

        DoMovement();
    }

    private void ApplyGravity()
    {
        if (!_characterController.isGrounded)
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
            if (_movement.magnitude <= 0.1) //if joystick is not used character stop immediately 
            {
                _velocity = Vector3.zero;
            }
            if (!_isCrouch)
            {
                
                if (_movement.magnitude <= 0.6)
                {
                    _acceleration = 15;
                    _dragOnGround = 10;
                }
                else if (_movement.magnitude <= 0.4)
                {
                    _acceleration = 5;
                }
                else
                {
                    _acceleration = 20;
                    _dragOnGround = 5;
                }
            }
            

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
        Vector3 displacement = _velocity  * Time.deltaTime;

        _characterController.Move(displacement);
    }

    private void ApplyJump()
    {

        if (_isJumping && _characterController.isGrounded)
        {
            _jump = true;

            _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _isJumping = false;
        }
        else
        {
            _jump = false;
        }
    }

    private void MoveAnimation()
    {
        _animator.SetFloat(_horizontalVelocityParam, _movement.x);
        _animator.SetFloat(_verticalVelocityParam, _movement.z);
    }

    private void JumpAnimation()
    {
        _animator.SetBool(_isJumpParam, _jump);
    }

    private void EditCharactControllerParams(bool controlObject)
    {
        
        
        if (controlObject)
        {
            _characterController.height = 1.3f;
            _characterController.center = new Vector3(0, .7f, 0);
        }
        else
        {
            _characterController.height = currentHeight;
            _characterController.center = currentCenter;
        }
        
    }
    
}
