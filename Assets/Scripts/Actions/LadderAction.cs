using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderAction : MonoBehaviour {

    public  bool IsCharacterReadyToClimb { get; set; }

    [SerializeField]
    private GameObject _char;
    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private Vector3 _FaceToLadderRotation;
    [SerializeField]
    private Transform _characterTopPosition;

    private InputController _ic = InputController.Instance();
    private AnimationController _ac;

    private int _playerLayer = 9;
    private bool _isCharacterInTriggerBox;
    private string _currentName;


    // Use this for initialization
    void Start () {
        IsCharacterReadyToClimb = false;
        _ac = new AnimationController(_char.GetComponent<Animator>());
	}
	
	// Update is called once per frame
	void Update () {
        
        if (_ic.IsButtonXPressed() && _isCharacterInTriggerBox)
        {
           // Debug.Log("klik en in triggerbox");
            //IsCharacterReadyToClimb = !IsCharacterReadyToClimb;
            IsCharacterReadyToClimb = true;
            RotateAndPositionCharacterToLadder();
        }

        if (IsCharacterReadyToClimb)
        {
            //Debug.Log("idle ");
            _ac.ClimbAnimation(true, 0);
            _char.GetComponent<CharacterBehaviour>().IsGravity = false;
        }
        //else
        //{
        //    _ac.ClimbAnimation(false, 0);
        //    _char.GetComponent<CharacterBehaviour>().IsGravity = true;
        //}

        if (IsCharacterReadyToClimb && _ic.GetLeftJoystickInput().z > .5f)
        {
            //Debug.Log("climb up");
            _ac.ClimbAnimation(true,1);
            
            _char.transform.position += new Vector3(0, 0.01f, 0);
        }
        
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            //Debug.Log("trigger enter");
            _isCharacterInTriggerBox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerLayer && !_char.GetComponent<CharacterController>().isGrounded)
        {
           // Debug.Log("exit trigger and player is NOT grounded");
            _ac.EndClimbAnimation(true);
            //_char.transform.position = _characterTopPosition.position;
        }
        //else if (other.gameObject.layer == _playerLayer)
        //{
        //    Debug.Log("trigger exit");
        //    _ac.ClimbAnimation(false, 0);
        //    _char.GetComponent<CharacterBehaviour>().IsGravity = true;
        //    _isCharacterInTriggerBox = false;
        //}
    }

    private void RotateAndPositionCharacterToLadder()
    {
        _char.transform.position = _startPoint.position;
        _char.transform.rotation = Quaternion.Euler(_FaceToLadderRotation);
    }
}
