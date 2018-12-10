using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrigger : MonoBehaviour {

    public bool IsInTunnel { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            IsInTunnel = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
        {
            Debug.Log("should i stay our should i go");
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
