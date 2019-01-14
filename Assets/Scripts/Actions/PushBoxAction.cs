using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBoxAction : MonoBehaviour {
    
    [SerializeField]
    private float _pushPower;
    [SerializeField]
    private SwordController _sword;
    [SerializeField]
    private HUDPanelTriggers _hudpt;


    private Animator _animator;
    private CharacterController _charController;
    private float _currentCharControllerRadius;

    private bool _isCharacterInTrigger;
    private bool _canBoxMove;
    private int _pushBoxLayer = 13;
    private bool _isPushing;

    private AnimationController _ac;
    private InputController _ic = InputController.Instance();

    // Use this for initialization
    void Start () {
        _animator = this.GetComponent<Animator>();
        _ac = new AnimationController(_animator);
        _charController = this.GetComponent<CharacterController>();

        _currentCharControllerRadius = _charController.radius;
    }
    

    // Update is called once per frame
    void Update () {
        if (_isCharacterInTrigger && _ic.IsButtonXPressed() && !_sword.IsSwordInHand)
        {
            _isPushing = !_isPushing;

            if (_isPushing)
            {
                _canBoxMove = true;
                _charController.radius = 0.8f;
            }
            else
            {
                _canBoxMove = false;
            }
        }

        if (!_isCharacterInTrigger)
        {
            _isPushing = false;
            _canBoxMove = false;
        }

        _ac.PushboxAnimation(_isPushing);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == _pushBoxLayer)
        {
            if (_sword.IsSwordInHand)
            {
                _hudpt.ShowHoldingSwordPanel();
            }
            else
            {
                _hudpt.ShowActionPanel();
                _isCharacterInTrigger = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _pushBoxLayer)
        {
            _isCharacterInTrigger = false;
            _charController.radius = _currentCharControllerRadius;
            _hudpt.HideTriggerPanels();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        if (_canBoxMove)
        {
            _hudpt.HideTriggerPanels();
            body.constraints = RigidbodyConstraints.FreezeRotation;
            //body.velocity = pushDir * 2.0f;
            body.AddForce(pushDir * _pushPower, ForceMode.Impulse);
        }

    }

}
