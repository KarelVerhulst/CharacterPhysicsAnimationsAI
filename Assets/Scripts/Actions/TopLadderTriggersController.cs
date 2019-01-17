using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLadderTriggersController : MonoBehaviour {

    /*
     * check if the character is at the top of the ladder
     */

    public bool CharacterIsAtTop { get; set; }

    [SerializeField]
    private int _characterMaskIndex;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _characterMaskIndex)
        {
            CharacterIsAtTop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _characterMaskIndex)
        {
            CharacterIsAtTop = false;
        }
    }
}
