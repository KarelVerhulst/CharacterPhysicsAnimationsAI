using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomLadderTriggersController : MonoBehaviour {

    public bool CharacterIsAtGround { get; set; }

    [SerializeField]
    private int _characterMaskIndex;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _characterMaskIndex)
        {
            CharacterIsAtGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _characterMaskIndex)
        {
            CharacterIsAtGround = false;
        }
    }
}
