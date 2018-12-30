using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAction : MonoBehaviour {

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private List<Animation> _gates = new List<Animation>();

    private int _playerLayer = 9;
    private bool _isCharacterInTriggerBox;

    private InputController _ic = InputController.Instance();
    private AnimationController _ac;
    
    // Use this for initialization
    void Start () {
        _ac = new AnimationController(_animator);
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
    
    public void OpenCloseGates(bool isClosed)
    {
        if (isClosed)
        {
            // Debug.Log("Open all the gates");
            foreach (Animation gate in _gates)
            {
                gate.Play("GateOpen");
                gate.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        else
        {
            //Debug.Log("Close all the gates");
            foreach (Animation gate in _gates)
            {
                gate.Play("GateClose");
                gate.gameObject.GetComponent<BoxCollider>().enabled = true;
            }
        }
    }
}
