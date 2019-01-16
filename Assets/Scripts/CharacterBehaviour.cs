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
    [SerializeField]
    private SwordController _sc;
    [SerializeField]
    private HUD _hudHealth;
    [SerializeField]
    private LayerMask _mapLayerMask;

    private CharacterController _characterController;
    
    private Vector3 _movement;
    private bool _jump;
    private bool _isJumping;
    private bool _isCrouch;
    private AnimatorStateInfo _aStateInfo;

    float currentHeight;
    Vector3 currentCenter;

   // private int _health = 15;
    [SerializeField]
    private Transform _respawnPoint;

    //externe scripts
    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    private HeadTrigger _headTrigger;
    private PhysicController _pc;
    
    private float _deathTimer = 0;
    
    // Use this for initialization
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _ac = new AnimationController(this.GetComponent<Animator>());

        currentHeight = _characterController.height;
        currentCenter = _characterController.center;

        _headTrigger = GetComponent<HeadTrigger>();
        //_hudHealth.Health = 10;
        IsGravity = true;
        IsDead = false;

        _pc = new PhysicController(_characterController, _maxRunningSpeed, _jumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
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
            //ResetVelocity();
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

                bool isMoveSkewForward = (_movement.normalized.x > 0 && _movement.normalized.z > 0) || (_movement.normalized.x < 0 && _movement.normalized.z > 0);
                bool isMoveSkewBackward = (_movement.normalized.x > 0 && _movement.normalized.z < 0) || (_movement.normalized.x < 0 && _movement.normalized.z < 0);

                if (isMoveSkewForward && _sc.IsSwordInHand)
                {
                    _acceleration = 12;
                    _dragOnGround = 5;
                    //Debug.Log("schuin voorwaarts");
                }
                else if (isMoveSkewBackward)
                {
                    _acceleration = 10;
                    _dragOnGround = 5;
                    //Debug.Log("schuin achterwaart");
                }
            }
            else //you are in the crouch statement
            {
                _acceleration = 15;
                _dragOnGround = 10;
            }
        }
    }

    private void ApplyDeath()
    {
        if (_hudHealth.Health <= 0)
        {
            IsDead = true;
            _deathTimer += Time.deltaTime;

            if (_deathTimer >= 5f)
            {
                _ac.DeathAnimation(false);
                IsDead = false;
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
            EditCharactControllerParams(_isCrouch);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            //Debug.Log("hit npc");
            //_health--;
            _hudHealth.Health--;
        }
    }
}
