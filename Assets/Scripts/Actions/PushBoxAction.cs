using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBoxAction : MonoBehaviour {

    [SerializeField]
    private CheckColliderTrigger _cct;
    [SerializeField]
    private Animator _animator;

    private AnimationController _ac;
    private InputController _ic = InputController.Instance();

    private bool _isPushing;

    float pushPower = 2.0f;

    // Use this for initialization
    void Start () {
        _ac = new AnimationController(_animator);
    }
    

    // Update is called once per frame
    void Update () {
        if (_cct.IsTriggerActive && _ic.IsButtonXPressed())
        {
            _isPushing = true;
        }

        if (!_cct.IsTriggerActive)
        {
            _isPushing = false;
        }
        Debug.Log(_isPushing);

        _ac.PushboxAnimation(_isPushing);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_isPushing && hit.gameObject.name == "Cube")
        {
            Debug.Log(hit.gameObject.name);

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
            body.velocity = pushDir * pushPower;
        }

    }
}
