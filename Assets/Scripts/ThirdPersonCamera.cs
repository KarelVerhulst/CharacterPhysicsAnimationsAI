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
    private float _cameraSpeed = 50f;
    [SerializeField]
    private Transform _characterPos;
    [SerializeField]
    private CharacterController _characterController;

    private Vector3 _v3Rotate = Vector3.zero;
    private Vector2 _rotationLimit = new Vector2(-20, 40);
    private InputController _ic = InputController.Instance();
    private AnimationController _ac;

    // Use this for initialization
    void Start () {
        _ac = new AnimationController(this.GetComponentInParent<Animator>());
    }

    // Update is called once per frame
    void Update () {


        //rotate camera vertical with a limit
        _v3Rotate.x += _ic.GetRightJoystickInput().y * _cameraSpeed * Time.deltaTime;
        _v3Rotate.x = Mathf.Clamp(_v3Rotate.x, _rotationLimit.x, _rotationLimit.y);

        _characterPos.transform.localEulerAngles = _v3Rotate;

        if (_ac.CheckIfAnimationIsPlaying("PushAtSwitch") || _ac.CheckIfAnimationIsPlaying("PickUpObject") || _ac.CheckIfAnimationIsPlaying("BlendTreeClimb"))
            return;

        //rotate camera and character horizontal
        _characterController.transform.eulerAngles += new Vector3(0, _ic.GetRightJoystickInput().x,0) * _cameraSpeed * Time.deltaTime;


        

        

        //animation
        /*
         * todo 
         *      - if rotating in idle you use the turn animation
         *      
         *     problem: rotatie animatie gaat veel te traag in vergelijking met de snelheid van de camera
         *              de snelheid van de animatie wordt in de blend tree niet aangepast ook al is de snelheid meer dan 1
         */
            _ac.RotateCameraAnimation(_ic.GetRightJoystickInput().x);
    }
}
