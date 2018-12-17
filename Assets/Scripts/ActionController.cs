using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {

    private Animator _animator;
    
    private InputController _ic = new InputController();
    private AnimationController _ac;

    private bool _isCharacterInTriggerBox = false;
    
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _ac = new AnimationController(_animator);

        
	}
	
	// Update is called once per frame
	void Update () {
        
        if (_isCharacterInTriggerBox && _ic.IsButtonXPressed())
        {
            Debug.Log("char is in triggerbox and button x is pressed");
            _ac.PickupObjectAnimation(true);
        }
        else
        {
            _ac.PickupObjectAnimation(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Weapon") || other.tag.Equals("Ladder") || other.tag.Equals("Switch"))
        {
            _isCharacterInTriggerBox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Weapon"))
        {
            _isCharacterInTriggerBox = false;
        }
    }

}
