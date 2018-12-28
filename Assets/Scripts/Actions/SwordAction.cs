using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : MonoBehaviour {

    [SerializeField]
    private Transform _posPlayerForAnimation;
    [SerializeField]
    private Transform _test;

    private bool _isCharacterInTriggerBox = false;
    private int _playerLayer = 9;

    private Animator _animator;

    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    [SerializeField]
    private Transform _char;

    private static SwordAction _instance;

    //properties

    //public bool IsSwordInHand { get; set; }

    public static SwordAction Instance()
    {
        if (_instance == null)
        {
            _instance = new SwordAction();
        }

        return _instance;
    }
    

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        if (_ic.IsButtonXPressed())
        {
            Debug.Log(_test.rotation);
            _char.position = _posPlayerForAnimation.position;
            _char.GetChild(2).rotation = Quaternion.Euler(Vector3.zero);
            _test.localRotation = Quaternion.Euler(Vector3.zero);
            Debug.Log(_test.rotation);
            //Vector3 direction = _test.position - _char.position;
            //Debug.Log(direction);
            //Quaternion rotation = Quaternion.LookRotation(direction);

            //_char.rotation = Quaternion.Euler(_char.rotation.x, rotation.y, _char.rotation.z);
            //_char.rotation = Quaternion.Lerp(_char.rotation, rotation, 5);
        }

        if (_isCharacterInTriggerBox && _ic.IsButtonXPressed())
        {
            
            _char.position = _posPlayerForAnimation.position;
           
           
            //_char.rotation = Quaternion.Lerp(_char.rotation, rotation, 5);
            
            _ac.PickupObjectAnimation(true);
           // _char.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            
            if (_ac != null)
            {
                _ac.PickupObjectAnimation(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            _animator = _char.GetComponent<Animator>();
            _ac = new AnimationController(_animator);
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
