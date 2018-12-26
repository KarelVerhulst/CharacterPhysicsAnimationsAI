using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    public Transform RightHand { get; set; }

    [SerializeField]
    private Transform _rightHand;
    [SerializeField]
    private Vector3 _localSwordPosition;
    [SerializeField]
    private Vector3 _localSwordrRotation;

    private Transform _transform;

	// Use this for initialization
	void Start () {
        _transform = this.transform;
        RightHand = _rightHand;
	}
	
	// Update is called once per frame
	void Update () {
        if (_transform.parent)
        {
            _transform.localPosition = _localSwordPosition;
            _transform.localEulerAngles = _localSwordrRotation;
        }
    }

    public void TakeSword(Transform parent)
    {
        _transform.parent = parent;
        _transform.localPosition = _localSwordPosition;
        _transform.localEulerAngles = _localSwordrRotation;
    }
}
