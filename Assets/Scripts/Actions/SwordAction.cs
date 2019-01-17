using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : MonoBehaviour {

    [SerializeField]
    private Transform _posPlayerForAnimation;
    [SerializeField]
    private Transform _rightHand;
    [SerializeField]
    private Transform _char;
    [SerializeField]
    private GameObject _sword;
    [SerializeField]
    private HUDPanelTriggers _hudpt;
    [SerializeField]
    private string _actionText;

    private bool _isCharacterInTriggerBox = false;
    private int _playerLayer = 9;
    private Animator _animator;

    //externe scripts
    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    
    // Use this for initialization
    void Start () {
        _animator = _char.GetComponent<Animator>();
        _ac = new AnimationController(_animator);
        _animator.GetBehaviour<PickUpSword>().SetBehaviourFields(_rightHand, _sword, _hudpt);
    }
	
	// Update is called once per frame
	void Update () {

        if (_isCharacterInTriggerBox && _ic.IsButtonXPressed())
        {
            _char.position = _posPlayerForAnimation.position;
            _char.rotation = Quaternion.Euler(Vector3.zero);

            _ac.PickupObjectAnimation(true);

            _hudpt.HideTriggerPanels();
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            _isCharacterInTriggerBox = false;
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
            _hudpt.ShowActionPanel(_actionText);
            _isCharacterInTriggerBox = true;
        }
    }
}
