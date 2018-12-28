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

    private Transform _transform;
    private bool _isWeaponArmed = true;

    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    
    private float _timer;
    private bool _isDisArmTrue;

    // Use this for initialization
    void Start () {
        _transform = this.transform;
        RightHand = _rightHand;

        _ac = new AnimationController(_animator);
	}
	
	// Update is called once per frame
	void Update () {
        

        if (_isDisArmTrue)
        {
            _timer += Time.deltaTime;

            if (_timer > 0.92f)
            {
                IsSwordInHand = false;
                _isWeaponArmed = false;
            }
        }

        if (IsSwordInHand)
        {
            if (_ic.IsButtonYPressed())
            {
                _ac.DisArmWeaponWeapon(true);
                _isDisArmTrue = true;
            }
        }
        else if(!_isWeaponArmed)
        {
            _ac.DisArmWeaponWeapon(false);
            _ac.UseSwordLocomotionAnimation(IsSwordInHand);

            _transform.parent = _weaponHolder;
            _transform.localPosition = _localWeaponHolderPosition;
            _transform.localEulerAngles = _localWeaponHolderRotation;
        }
        
        
        if (_transform.parent && _isWeaponArmed)
        {
            _transform.localPosition = _localSwordPosition;
            _transform.localEulerAngles = _localSwordRotation;
        }
    }

    public void TakeSword(Transform parent)
    {
        _transform.parent = parent;
        _transform.localPosition = _localSwordPosition;
        _transform.localEulerAngles = _localSwordRotation;
        IsSwordInHand = true;
    }
}
