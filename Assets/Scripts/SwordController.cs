using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    public Transform RightHand { get; set; }
    public bool IsSwordInHand { get; set; }

    [SerializeField]
    private Transform _rightHand;
    [SerializeField]
    private Vector3 _localSwordPosition;
    [SerializeField]
    private Vector3 _localSwordRotation;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Vector3 _localWeaponHolderPosition;
    [SerializeField]
    private Vector3 _localWeaponHolderRotation;
    [SerializeField]
    private Transform _weaponHolder;
    [SerializeField]
    private Transform _handR;

    private Transform _transform;
    private bool _isWeaponArmed = true;
    private bool _characterHaveSword;
    private bool _isSwordAtBack;

    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    
    private float _timer;
    private bool _isDisArmTrue;
    private bool _setMeleeActive;

    // Use this for initialization
    void Start () {
        _transform = this.transform;
        RightHand = _rightHand;

        _ac = new AnimationController(_animator);
	}
	
	// Update is called once per frame
	void Update () {

        if (IsSwordInHand) //if this is true you can put your weapon at your back
        {
            SetSwordAtCharactersBack();

            if (_isDisArmTrue)
            {
                _timer += Time.deltaTime;

                if (_timer > 0.92f)
                {
                    _isWeaponArmed = false;
                    IsSwordInHand = false;
                }
            }
        }
        else //there is no sword in hand
        {
            //check if there is a character has a sword
            if (_characterHaveSword)
            {
                //you have a sword you can take it from you back (you know it is NOT in your hand)
                TakeSwordFromBack();

                if (!_isDisArmTrue)
                {
                    _timer += Time.deltaTime;

                    if (_timer > 0.92f)
                    {
                        _isWeaponArmed = true;
                        IsSwordInHand = true;
                        _isSwordAtBack = false;
                    }
                }
            }
            else
            {
                //the character has no sword so pick up a sword if you are in the trigger zone
            }
        }

        //give sword the correct position and rotation on the character
        if (!_isWeaponArmed)
        {
            //Debug.Log("false isweapon armed");
            _ac.SetWeaponAtBack(false);
            _ac.UseSwordLocomotionAnimation(false);

            _transform.parent = _weaponHolder;
            _transform.localPosition = _localWeaponHolderPosition;
            _transform.localEulerAngles = _localWeaponHolderRotation;

        }
        else if (!_isSwordAtBack && _characterHaveSword)
        {
            //Debug.Log("take weapon animation + use sword locomotion");
            _ac.TakeWeaponFromBack(false);
            _ac.UseSwordLocomotionAnimation(true);

            _transform.parent = _handR;
            _transform.localPosition = _localSwordPosition;
            _transform.localEulerAngles = _localSwordRotation;
        }


        if (IsSwordInHand && _ic.IsButtonBPressed())
        {
            _setMeleeActive = true;
        }
        else
        {
            _setMeleeActive = false;
        }

        _ac.AttackMeleeAnimation(_setMeleeActive);
    }

    public void TakeSword(Transform parent)
    {
        _transform.parent = parent;
        _transform.localPosition = _localSwordPosition;
        _transform.localEulerAngles = _localSwordRotation;
        IsSwordInHand = true;
        _characterHaveSword = true;
    }

    //private mehtods

    private void SetSwordAtCharactersBack()
    {
        if (_ic.IsButtonYPressed())
        {
            _timer = 0;
            _ac.SetWeaponAtBack(true);
            _isDisArmTrue = true;
        }
    }

    private void TakeSwordFromBack()
    {
        if (_ic.IsButtonYPressed())
        {
            _timer = 0;
            _ac.TakeWeaponFromBack(true);
            _isDisArmTrue = false;
        }
    }
}
