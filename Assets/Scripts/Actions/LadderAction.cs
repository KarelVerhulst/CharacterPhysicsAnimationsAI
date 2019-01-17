using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderAction : MonoBehaviour {

    /*
     *  the main of the climbing a ladder
     */

    public  bool IsCharacterReadyToClimb { get; set; }

    [SerializeField]
    private GameObject _char;
    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private Vector3 _FaceToLadderRotation;
    [SerializeField]
    private Transform _rightHandLadder;
    [SerializeField]
    private BottomLadderTriggersController _bltc;
    [SerializeField]
    private TopLadderTriggersController _tltc;
    [SerializeField]
    private SwordController _sword;

    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    private Animator _charAnimator;

    // Use this for initialization
    void Start () {
        IsCharacterReadyToClimb = false;
        _charAnimator = _char.GetComponent<Animator>();
        _ac = new AnimationController(_charAnimator);
    }
	
	// Update is called once per frame
	void Update () {
        if (_ic.IsButtonXPressed() && !_sword.IsSwordInHand && _bltc.CharacterIsAtGroundLadder)
        {
            _charAnimator.GetBehaviour<TopLadderBehaviour>().SetBehaviourFields(_rightHandLadder, _char, this.GetComponent<LadderAction>());
            IsCharacterReadyToClimb = !IsCharacterReadyToClimb;

            RotateAndPositionCharacterToLadder();

            if (!IsCharacterReadyToClimb)
            {
                _ac.ClimbAnimation(false, 0);
                _char.GetComponent<CharacterBehaviour>().IsGravity = true;
            }
        }

        if (IsCharacterReadyToClimb)
        {
            ClimbingState();
            AtTopOfTheLadder();
        }
    }
    
    private void RotateAndPositionCharacterToLadder()
    {
        _char.transform.position = _startPoint.position;
        _char.transform.rotation = Quaternion.Euler(_FaceToLadderRotation);
    }

    private void ClimbingState()
    {
        if (!_tltc.CharacterIsAtTop && IsCharacterReadyToClimb && _ic.GetLeftJoystickInput().z > .5f)
        {
            _ac.ClimbAnimation(true, 1);

            _char.transform.position += new Vector3(0, 0.01f, 0);
        }
        else if (!_bltc.CharacterIsAtGroundLadder && IsCharacterReadyToClimb && _ic.GetLeftJoystickInput().z < -.5f)
        {
            _ac.ClimbAnimation(true, -1);
            _char.transform.position -= new Vector3(0, 0.01f, 0);
        }
        else
        {
            _ac.ClimbAnimation(true, 0);
            _char.GetComponent<CharacterBehaviour>().IsGravity = false;
        }
    }

    private void AtTopOfTheLadder()
    {
        if (_tltc.CharacterIsAtTop)
        {
            _ac.EndClimbAnimation(true);
        }
    }
}
