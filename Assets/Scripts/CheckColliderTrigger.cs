using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckColliderTrigger : MonoBehaviour {

    public bool IsTriggerActive { get; set; }

    [SerializeField]
    private int _layerPlayer = 9;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            IsTriggerActive = true;
            Debug.Log("player stand before a box");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            IsTriggerActive = false;
            Debug.Log("player stand before a box");
        }
    }
}
