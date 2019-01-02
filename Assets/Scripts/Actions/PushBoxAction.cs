using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBoxAction : MonoBehaviour {

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private CharacterController _charController;

    private AnimationController _ac;
    private InputController _ic = InputController.Instance();

    private bool _isPushing;
    [SerializeField]
    private float _pushPower = 2.5f;

    private Vector3 _currentCharControllerCenter;
    private bool _isCharacterInTrigger;
    private bool _canBoxMove;
    private int _pushBoxLayer = 10;

    // Use this for initialization
    void Start () {
        _ac = new AnimationController(_animator);
        _currentCharControllerCenter = _charController.center;
    }
    

    // Update is called once per frame
    void Update () {
        if (_isCharacterInTrigger && _ic.IsButtonXPressed())
        {
            _isPushing = true;
            _canBoxMove = true;
            _charController.center = new Vector3(0,1,0.6f);
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
            _isCharacterInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _pushBoxLayer)
        {
            _isCharacterInTrigger = false;
            _charController.center = _currentCharControllerCenter;
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
            //body.velocity = pushDir * 2.0f;
            body.AddForce(pushDir * _pushPower, ForceMode.Impulse);
        }

    }

}
