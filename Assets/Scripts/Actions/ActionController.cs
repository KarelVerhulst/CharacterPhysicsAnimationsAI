using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {

    /*
     * --- Action ---
     * This script show the different Actions
     * What must the character do?
     * 
     * The actions are 
     *      - pick up weapon
     *      - climb a ladder
     *      - push a box
     * 
     * todo
     *  add the other methods for the actions
     */

    private Animator _animator;
    
    private InputController _ic = InputController.Instance();
    private AnimationController _ac;

    private bool _isCharacterInTriggerBox = false;

    //properties
   
    public bool IsSwordInHand { get; set; }

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        _ac = new AnimationController(_animator);

        IsSwordInHand = false;
	}
	
	// Update is called once per frame
	void Update () {
        //check if the character is in the trigger area and button x is pressed
        /*
         * todo 
         *      switch which action the character must do and do the correct animation (that you get from the AnimationController.cs) 
         */
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
        if (other.tag.Equals("Weapon") || other.tag.Equals("Ladder") || other.tag.Equals("Switch"))
        {
            _isCharacterInTriggerBox = false;
        }
    }

}
