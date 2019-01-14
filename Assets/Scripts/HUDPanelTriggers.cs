using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDPanelTriggers : MonoBehaviour {

    [SerializeField]
    private GameObject _actionPanel;
    [SerializeField]
    private GameObject _holdingSwordPanel;

    public void ShowActionPanel() {
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
