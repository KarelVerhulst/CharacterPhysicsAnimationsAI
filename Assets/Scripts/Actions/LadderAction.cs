using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderAction : MonoBehaviour {

    public  bool IsCharacterReadyToClimb { get; set; }

    [SerializeField]
    private GameObject _char;

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
            Debug.Log("klik en in triggerbox");
            IsCharacterReadyToClimb = !IsCharacterReadyToClimb;
        }

        if (IsCharacterReadyToClimb)
        {
            //Debug.Log("idle ");
            _ac.ClimbAnimation(true, 0);
            _char.GetComponent<CharacterBehaviour>().IsGravity = false;
        }
        else
        {
            _ac.ClimbAnimation(false, 0);
            _char.GetComponent<CharacterBehaviour>().IsGravity = true;
        }

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
            _isCharacterInTriggerBox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            _isCharacterInTriggerBox = false;
        }
    }
}
