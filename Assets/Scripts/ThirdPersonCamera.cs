using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    /*
     * What does the camera do?
     *  rotate horizontal
     *  rotate vertical with a limit
     */

    //[SerializeField]
    //private float _smooth = 3f;
    [SerializeField]
    private Transform _standardPos;
    [SerializeField]
    private float _cameraSpeed = 50f;
    [SerializeField]
    private Transform _characterPos;
    [SerializeField]
    private GameObject _character;
    
    private Vector3 _v3Rotate = Vector3.zero;
    private Vector2 _rotationLimit = new Vector2(-20, 40);
    
    //externe scripts
    private CharacterController _characterController;
    private InputController _ic = InputController.Instance();
    private AnimationController _ac;

    // Use this for initialization
    void Start () {
        _characterController = _character.GetComponent<CharacterController>();
        
        _ac = new AnimationController(_character.GetComponent<Animator>());
    }

    // Update is called once per frame
    void Update () {

        //this.transform.position = Vector3.Lerp(this.transform.position, _standardPos.position, Time.deltaTime * _smooth);
        //this.transform.forward = Vector3.Lerp(this.transform.forward, _standardPos.forward, Time.deltaTime * _smooth);

        //rotate camera vertical with a limit
        _v3Rotate.x += _ic.GetRightJoystickInput().y * _cameraSpeed * Time.deltaTime;
        _v3Rotate.x = Mathf.Clamp(_v3Rotate.x, _rotationLimit.x, _rotationLimit.y);

        _characterPos.transform.localEulerAngles = _v3Rotate;

        //when animation is playing you can't rotate horizontal, only vertical
        if (_ac.CheckIfAnimationIsPlaying(0,"PushAtSwitch") || _ac.CheckIfAnimationIsPlaying(0, "PickUpObject") || _ac.CheckIfAnimationIsPlaying(0, "Blend Tree PushBox") || _ac.CheckIfAnimationIsPlaying(4, "Climb Layer.BlendTreeClimb"))
            return;

        //rotate camera and character horizontal
        _characterController.transform.eulerAngles += new Vector3(0, _ic.GetRightJoystickInput().x,0) * _cameraSpeed * Time.deltaTime;
        
        //animation
        _ac.RotateCameraAnimation(_ic.GetRightJoystickInput().x);
    }
}
