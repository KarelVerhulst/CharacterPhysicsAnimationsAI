using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {

    private Animator _animator;
    [SerializeField]
    private Transform _rightHandObj;
    [SerializeField]
    private Transform _rightHandCharacter;

    private InputController _ic = new InputController();
    private AnimationController _ac;

    private bool _isCharacterInTriggerBox = false;
    private string _setTypeAction;

    private Transform _weapon;


    private bool _test = false;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _ac = new AnimationController(_animator);
	}
	
	// Update is called once per frame
	void Update () {
        
        if (_isCharacterInTriggerBox && _ic.IsButtonXPressed())
        {
            //DoTheCorrectAction();
            _test = true;
        }

        if (_test)
        {
            _rightHandObj.transform.position = _rightHandCharacter.transform.position;
            _rightHandObj.transform.rotation = _rightHandCharacter.transform.rotation;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Weapon"))
        {
            _isCharacterInTriggerBox = true;

            _setTypeAction = other.tag;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Weapon"))
        {
            _weapon = other.transform;
            _isCharacterInTriggerBox = false;
        }
    }

    private void DoTheCorrectAction()
    {
        switch (_setTypeAction)
        {
            case "Weapon":
                DoWeaponAction();
                break;
            default:
                break;
        }
    }

    private void DoWeaponAction()
    {
        Debug.Log("do weapon action");
        _rightHandObj.transform.position = _rightHandCharacter.transform.position;
       // _ac.PickupObjectAnimation(_isCharacterInTriggerBox);
    }
    
}
