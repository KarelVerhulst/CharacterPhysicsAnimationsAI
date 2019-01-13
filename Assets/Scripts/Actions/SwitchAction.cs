using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAction : MonoBehaviour {

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private List<Animation> _gates = new List<Animation>();
    [SerializeField]
    private Transform _button;
    [SerializeField]
    private HUDPanelTriggers _hudpt;
    [SerializeField]
    private SwordController _sword;

    private int _playerLayer = 9;
    private bool _isCharacterInTriggerBox;

    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    
    // Use this for initialization
    void Start () {
        _ac = new AnimationController(_animator);
        _animator.GetBehaviour<PushButton>().SetBehaviourFields(_gates,_button);
	}
	
	// Update is called once per frame
	void Update () {
        
        if (_isCharacterInTriggerBox && _ic.IsButtonXPressed())
        {
            _ac.PushButtonAnimation(true);
        }
        else
        {
            _ac.PushButtonAnimation(false);
        }

        if (_isCharacterInTriggerBox)
        {
            if (_sword.GetComponent<SwordController>().IsSwordInHand)
            {
                _hudpt.ShowHoldingSwordPanel();
            }
            else
            {
                _hudpt.ShowActionPanel();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            if (_sword.IsSwordInHand)
            {
                _hudpt.ShowHoldingSwordPanel();
            }
            else
            {
                _hudpt.ShowActionPanel();
                _isCharacterInTriggerBox = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            _isCharacterInTriggerBox = false;
            _hudpt.HideTriggerPanels();
        }
    }


}
