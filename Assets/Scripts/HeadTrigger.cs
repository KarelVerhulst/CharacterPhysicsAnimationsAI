using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrigger : MonoBehaviour {

    /*
     * Check if something is above the character
     */
    public bool IsInTunnel { get; set; }

    private int _tunnelLayer = 14;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == _tunnelLayer)
        {
            IsInTunnel = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsInTunnel = false;
    }
}
