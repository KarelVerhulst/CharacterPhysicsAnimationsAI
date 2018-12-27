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
    private Transform CharacterPos;
    [SerializeField]
    private Transform Character;

    private Vector3 _v3Rotate = Vector3.zero;
    private Vector2 _rotationLimit = new Vector2(-20, 40);

    //Input Controller
    private InputController _ic = InputController.Instance();
    private AnimationController _ac;

    // Use this for initialization
    void Start () {
        _ac = new AnimationController(this.GetComponentInParent<Animator>());
    }

    // Update is called once per frame
    void Update () {
        //rotate camera horizontal
        _v3Rotate.y += _ic.GetRightJoystickInput().x * _cameraSpeed * Time.deltaTime;
        Character.transform.Rotate(Vector3.up, _ic.GetRightJoystickInput().x * _cameraSpeed * Time.deltaTime);

        //rotate camera vertical with a limit
        _v3Rotate.x += _ic.GetRightJoystickInput().y * _cameraSpeed * Time.deltaTime;
        _v3Rotate.x = Mathf.Clamp(_v3Rotate.x, _rotationLimit.x, _rotationLimit.y);

        CharacterPos.transform.localEulerAngles = _v3Rotate;

        //animation
        /*
         * todo 
         *      - if rotating in idle you use the turn animation
         *      
         *     problem: rotatie animatie gaat veel te traag in vergelijking met de snelheid van de camera
         *              de snelheid van de animatie wordt in de blend tree niet aangepast ook al is de snelheid meer dan 1
         */
        //_ac.RotateCameraAnimation(_v3Rotate.y);
    }
}
