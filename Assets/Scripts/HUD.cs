using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    /*
     * https://www.youtube.com/watch?v=g03gH-uDuqU
     * for now this hud is only showing the health bar
     * later you can add for example mana bar or stamina bar ...
     */
    public float Health { get; set; }

    [SerializeField]
    private float _startHealth;

    public float StartHealth
    {
        get { return _startHealth; }
        set { _startHealth = value; }
    }
    
    [SerializeField]
    private Image _healthImage;

    void Start()
    {
        Health = _startHealth;
    }

    // Update is called once per frame
    void Update () {
        _healthImage.fillAmount = (Health / _startHealth);
	}
}
