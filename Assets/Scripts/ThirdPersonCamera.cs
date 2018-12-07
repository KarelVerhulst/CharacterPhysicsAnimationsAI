using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    [SerializeField]
    private float _smooth = 3f;
    [SerializeField]
    private float _cameraSpeed = 50f;
    
    public Transform CharacterPos;
    public Transform Character;

    private Vector3 _v3Rotate = Vector3.zero;
    private Vector2 _rotationLimit = new Vector2(-30, 20);

    //Input Controller
    private InputController _ic = new InputController();

    // Use this for initialization
    void Start () {}

    // Update is called once per frame
    void Update () {
        _v3Rotate.y += _ic.GetRightJoystickInput().x * _cameraSpeed * Time.deltaTime;
        Character.transform.Rotate(Vector3.up, _ic.GetRightJoystickInput().x * _cameraSpeed * Time.deltaTime, Space.Self);

        //rotate camera vertical with a limit
        _v3Rotate.x -= _ic.GetRightJoystickInput().y * _cameraSpeed * Time.deltaTime;
        _v3Rotate.x = Mathf.Clamp(_v3Rotate.x, _rotationLimit.x, _rotationLimit.y);

        CharacterPos.transform.localEulerAngles = _v3Rotate;
    }
}
