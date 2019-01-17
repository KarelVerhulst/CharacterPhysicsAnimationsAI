using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanelTriggers : MonoBehaviour {

    /*
     * show the correct panel for the player if he can do an action or not
     */

    [SerializeField]
    private GameObject _actionPanel;
    [SerializeField]
    private GameObject _holdingSwordPanel;
    [SerializeField]
    private Text _txtAction;

    public void ShowActionPanel(string actionText) {
        _txtAction.text = actionText;
        _actionPanel.SetActive(true);
        _holdingSwordPanel.SetActive(false);
    }

    public void ShowHoldingSwordPanel()
    {
        _actionPanel.SetActive(false);
        _holdingSwordPanel.SetActive(true);
    }

    public void HideTriggerPanels()
    {
        _actionPanel.SetActive(false);
        _holdingSwordPanel.SetActive(false);
    }
}
