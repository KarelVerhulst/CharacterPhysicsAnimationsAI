using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomLadderTriggersController : MonoBehaviour {

    /*
     * check if the character is at the bottom of the ladder
     * show the correct panel
     */

    public bool CharacterIsAtGroundLadder { get; set; }

    [SerializeField]
    private int _characterMaskIndex;
    [SerializeField]
    private HUDPanelTriggers _hudpt;
    [SerializeField]
    private SwordController _sword;
    [SerializeField]
    private string _actionText;

    void Update()
    {
        if (CharacterIsAtGroundLadder)
        {
            if (_sword.IsSwordInHand)
            {
                _hudpt.ShowHoldingSwordPanel();
            }
            else
            {
                _hudpt.ShowActionPanel(_actionText);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _characterMaskIndex)
        {
            if (_sword.IsSwordInHand)
            {
                _hudpt.ShowHoldingSwordPanel();
            }
            else
            {
                _hudpt.ShowActionPanel(_actionText);
                CharacterIsAtGroundLadder = true;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _characterMaskIndex)
        {
            CharacterIsAtGroundLadder = false;
            _hudpt.HideTriggerPanels();
        }
    }
}
