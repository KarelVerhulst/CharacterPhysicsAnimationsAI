using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterBehaviour : MonoBehaviour
{
    /*
     * the main script for the character
     * in this script the main for the character is used
     * like the different animation, the locomotion physics, the input controller (joystick)
     * 
     * todo 
     *  - refactor the phsyics in another script
     * 
     */
     
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
    
    private CharacterController _characterController;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _movement;
    private bool _jump;
    private bool _isJumping;
    private bool _isCrouch;

    float currentHeight;
    Vector3 currentCenter;

    //externe scripts
    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    private HeadTrigger _headTrigger;

    // Use this for initialization
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _ac = new AnimationController(this.GetComponent<Animator>());

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

        //Debug.Log(_headTrigger.IsInTunnel);
        if (_ic.IsLeftJoystickButtonPressed() && !_headTrigger.IsInTunnel)
        {
            _isCrouch = !_isCrouch;
            EditCharactControllerParams(_isCrouch);
        }

        //animations
        _ac.MoveAnimation(_movement);
        _ac.JumpAnimation(_jump);
        _ac.CrouchAnimation(_isCrouch);
    }
    
    void FixedUpdate()
    {
        //Locomotions
        ApplyGround();
        ApplyGravity();
        ApplyMovement();
        ApplyGroundDrag();

        LimitMaximumRunningSpeed();

        ApplyJump();

        EditMovementFields();

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
    
    private void EditCharactControllerParams(bool controlObject)
    {
        // edit the the charactercontroller collider 
        /*
         * todo change the if else to switch because the values are different for example crouch and stand
         */

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

    private void EditMovementFields()
    {
        /*
         * edit the acceleration and dragonground if you in an other locomotion speed
         * 
         * todo
         *  code overzichtelijker maken
         */
        if (_characterController.isGrounded)
        {
            ResetVelocity();
            
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
            else //you are in the crouch statement
            {
                _acceleration = 15;
                _dragOnGround = 10;
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Cube" && _ic.IsButtonXPressed())
        {
            Debug.Log("trigger cue");
        }
    }
}
