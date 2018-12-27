using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : MonoBehaviour {

    [SerializeField]
    private Transform _posPlayerForAnimation;

    private bool _isCharacterInTriggerBox = false;
    private int _playerLayer = 9;

    private Animator _animator;

    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    
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
        
        if (_isCharacterInTriggerBox && _ic.IsButtonXPressed())
        {
            _char.position = _posPlayerForAnimation.position;
            _char.GetChild(2).rotation = Quaternion.Euler(Vector3.zero);

            

            Debug.Log(_char.GetChild(0).transform.rotation);

            _ac.PickupObjectAnimation(true);
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
            _char = other.transform;
            _animator = other.GetComponent<Animator>();
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
