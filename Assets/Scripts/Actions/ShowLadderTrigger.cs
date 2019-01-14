using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLadderTrigger : MonoBehaviour {

    [SerializeField]
    private GameObject _ladder;
    [SerializeField]
    private int _boxLayerMask;

    void Start()
    {
        _ladder.SetActive(false);    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _boxLayerMask)
        {
            _ladder.SetActive(true);
        }
    }
}
