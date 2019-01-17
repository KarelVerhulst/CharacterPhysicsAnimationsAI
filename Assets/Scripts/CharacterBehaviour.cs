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
     */
     
    public bool IsGravity { get; set; }
    public bool IsDead { get; set; }

    [SerializeField]
    private float _acceleration = 3; // [m/s^2]
    [SerializeField]
    private Transform _absoluteForward;
    [SerializeField]
    private float _maxRunningSpeed = (30.0f * 1000) / (60 * 60); // [m/s], 30km/h
    [SerializeField]
    private float _jumpHeight = 1f;
    [SerializeField]
    private float _dragOnGround = 3f;
    [SerializeField]
    private SwordController _sc;
    [SerializeField]
    private HUD _hudHealth;
    [SerializeField]
    private LayerMask _mapLayerMask;
    [SerializeField]
    private BoxCollider _swordCollider;
    [SerializeField]
    private Transform _respawnPoint;

    private CharacterController _characterController;
    private AnimatorStateInfo _aStateInfo;
    private Vector3 _movement;
    private bool _jump;
    private bool _isJumping;
    private bool _isCrouch;
    private float _currentHeight;
    private Vector3 _currentCenter;
    private float _deathTimer = 0;

    //externe scripts
    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    private HeadTrigger _headTrigger;
    private PhysicController _pc;
    
    // Use this for initialization
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _ac = new AnimationController(this.GetComponent<Animator>());

        _currentHeight = _characterController.height;
        _currentCenter = _characterController.center;

        _headTrigger = GetComponent<HeadTrigger>();
        IsGravity = true;
        IsDead = false;

        _pc = new PhysicController(_characterController, _maxRunningSpeed, _jumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
        //if animation is playing you can't walk otherwise it looks weird if your character is pushing a button but can move away from it
        if (_ac.CheckIfAnimationIsPlaying(0, "PushAtSwitch") || _ac.CheckIfAnimationIsPlaying(0, "PickUpObject") || _ac.CheckIfAnimationIsPlaying(4, "Climb Layer.BlendTreeClimb"))
            return;

        ApplyDeath();
        ApplyButtonsInput();

        _movement = _ic.GetLeftJoystickInput();
        
        _pc.PhysicUpdate(_movement, IsGravity);

        //animations
        _ac.MoveAnimation(_movement);
        _ac.JumpAnimation(_jump, GetJumpDistanceToGround());
        _ac.CrouchAnimation(_isCrouch);
        _ac.DeathAnimation(IsDead);
    }
    
    void FixedUpdate()
    {
        _pc.FixedPhysicUpdate(_absoluteForward, _jump, _dragOnGround, _acceleration);
      
        ApplyJump();
        EditMovementFields();
    }

    //private methods
    private void ApplyJump()
    {
        if (_isJumping && _characterController.isGrounded)
        {
            _jump = true;
            
            _isJumping = false;
        }
        else
        {
            _jump = false;
        }
    }

    private void EditCharactControllerHeightCenter(bool controlObject)
    {
        // edit the the charactercontroller collider 
        if (controlObject)
        {
            _characterController.height = 1.3f;
            _characterController.center = new Vector3(0, .7f, 0);
        }
        else //use the default params
        {
            _characterController.height = _currentHeight;
            _characterController.center = _currentCenter;
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
            if (_isCrouch) //you are in the crouch statement
            {
                _acceleration = 15;
                _dragOnGround = 10;
            }
            else 
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
                    _acceleration = 25;
                    _dragOnGround = 5;
                }

                bool isMoveSkewForward = (_movement.normalized.x > 0 && _movement.normalized.z > 0) || (_movement.normalized.x < 0 && _movement.normalized.z > 0);
                bool isMoveSkewBackward = (_movement.normalized.x > 0 && _movement.normalized.z < 0) || (_movement.normalized.x < 0 && _movement.normalized.z < 0);

                if (isMoveSkewForward)
                {
                    _acceleration = 20;
                    _dragOnGround = 5;
                    //Debug.Log("schuin voorwaarts");
                }
                else if (isMoveSkewBackward)
                {
                    _acceleration = 12;
                    _dragOnGround = 5;
                    //Debug.Log("schuin achterwaart");
                }
            }
        }
    }

    private void ApplyDeath()
    {
        if (_hudHealth.Health <= 0)
        {
            IsDead = true;
            _swordCollider.enabled = false;
            _deathTimer += Time.deltaTime;

            if (_deathTimer >= 5f)
            {
                IsDead = false;
                _swordCollider.enabled = true;
                this.transform.position = _respawnPoint.position;
                _hudHealth.Health = _hudHealth.StartHealth;
            }
        }
        else
        {
            _deathTimer = 0;
        }
    }

    private float GetJumpDistanceToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 100, _mapLayerMask))
        {
            return (hit.point - this.transform.position).magnitude;
        }

        return 0;
    }

    private void ApplyButtonsInput()
    {
        // use to jump
        if (_ic.IsButtonAPressed() && !_isCrouch)
        {
            _isJumping = true;
        }
        //use to crouch
        if (_ic.IsLeftJoystickButtonPressed() && !_headTrigger.IsInTunnel)
        {
            _isCrouch = !_isCrouch;
            EditCharactControllerHeightCenter(_isCrouch);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            _hudHealth.Health--;
        }
    }
}
