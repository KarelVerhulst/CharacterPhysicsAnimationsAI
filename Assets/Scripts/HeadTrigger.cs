using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrigger : MonoBehaviour {

    /*
     * Check if something is something above
     */
    public bool IsInTunnel { get; set; }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
        {
            IsInTunnel = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            IsInTunnel = false;
        }
    }
}
