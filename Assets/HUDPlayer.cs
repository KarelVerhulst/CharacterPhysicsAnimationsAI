using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPlayer : MonoBehaviour
{

    public float Health { get; set; }
   // private float _health;
    [SerializeField]
    private Image _healthImage;

	// Update is called once per frame
	public void Update () {
        _healthImage.fillAmount = (Health / 10);
	}
}
